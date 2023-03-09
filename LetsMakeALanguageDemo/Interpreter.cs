using LetsMakeALanguageDemo.Syntax;

namespace LetsMakeALanguageDemo;

public class Interpreter
{
    private static readonly IDictionary<string, object> _globalState = new Dictionary<string, object>();

    private static readonly IReadOnlyDictionary<string, Func<object[], object>> _runtimeFunctions =
        new Dictionary<string, Func<object[], object>>
        {
            ["print"] = RuntimeFunctions.Print,
            ["print_line"] = RuntimeFunctions.PrintLine
        };

    public static object Interpret(ProgramRoot ast, TextWriter? stdOut = null)
    {
        TextWriter? originalStdOut = null;
        
        if (stdOut != null)
        {
            originalStdOut = Console.Out;
            Console.SetOut(stdOut);
        }
        
        object result = VoidObject.Instance;
        
        foreach (var statement in ast.Statements)
        {
            result = InterpretStatement(statement);
        }

        if (originalStdOut != null)
        {
            Console.SetOut(originalStdOut);
        }

        return result;
    }

    private static object InterpretStatement(Statement statement)
    {
        return statement switch
        {
            VariableDeclarationStatement v => InterpretVariableDeclaration(v),
            ExpressionStatement e => InterpretExpression(e.Expression),
            WhileStatement w => InterpretWhileStatement(w),
            _ => throw new NotImplementedException($"Statement type {statement.GetType().Name} not yet implemented in interpreter")
        };
    }

    private static object InterpretWhileStatement(WhileStatement whileStatement)
    {
        while (true)
        {
            var conditionResult = InterpretExpression(whileStatement.Condition);

            if (conditionResult is not bool conditionValue)
            {
                throw new InvalidOperationException("Condition of a while statement must result in a boolean value");
            }

            if (!conditionValue)
            {
                break;
            }

            InterpretBlockStatement(whileStatement.Block);
        }

        return VoidObject.Instance;
    }

    private static void InterpretBlockStatement(BlockStatement blockStatement)
    {
        // TODO: should blocks yield their last expression value?
        foreach (var statement in blockStatement.Statements)
        {
            InterpretStatement(statement);
        }
    }

    private static object InterpretExpression(Expression expression)
    {
        return expression switch
        {
            LiteralExpression l => l.Value,
            IdentifierExpression i => _globalState.TryGetValue(i.Identifier.Name, out var value) ? value : throw new InvalidOperationException($"Identifier {i.Identifier.Name} is not defined"),
            BinaryExpression b => InterpretBinaryExpression(b),
            AssignmentExpression a => InterpretAssignmentExpression(a),
            ParenthesizedExpression p => InterpretExpression(p.Expression),
            CallExpression c => InterpretCallExpression(c),
            TernaryExpression t => InterpretTernaryExpression(t),
            _ => throw new NotImplementedException($"Expression type {expression.GetType().Name} not yet implemented in interpreter")
        };
    }

    private static object InterpretTernaryExpression(TernaryExpression ternaryExpression)
    {
        var conditionResult = InterpretExpression(ternaryExpression.Condition);
    
        if (conditionResult is not bool conditionValue)
        {
            throw new InvalidOperationException("Ternary operator (if/else) condition must result in a boolean value");
        }
    
        return conditionValue
            ? InterpretExpression(ternaryExpression.ThenExpression)
            : InterpretExpression(ternaryExpression.ElseExpression);
    }

    private static object InterpretCallExpression(CallExpression callExpression)
    {
        if (!_runtimeFunctions.TryGetValue(callExpression.Identifier.Name, out var runtimeFunction))
        {
            throw new InvalidOperationException($"Runtime function with name {callExpression.Identifier.Name} could not be found");
        }

        var args = callExpression.ArgumentList.Arguments.Select(InterpretExpression).ToArray();

        return runtimeFunction(args);
    }

    private static object InterpretAssignmentExpression(AssignmentExpression assignmentExpression)
    {
        var value = InterpretExpression(assignmentExpression.Expression);

        var variableName = assignmentExpression.Identifier.Name;

        if (!_globalState.ContainsKey(variableName))
        {
            throw new InvalidOperationException($"Attempt to assign undeclared variable {variableName}");
        }
        
        _globalState[variableName] = value;
        
        return value;
    }

    private static object InterpretBinaryExpression(BinaryExpression binaryExpression)
    {
        var left = InterpretExpression(binaryExpression.Left);
        var right = InterpretExpression(binaryExpression.Right);

        if (binaryExpression.Operator is BinaryOperator.Equal)
        {
            return left.Equals(right);
        }
        
        if (binaryExpression.Operator is BinaryOperator.Addition
            && left is string leftStr)
        {
            return leftStr + right;
        }
        
        // remainder of binary expressions currently require integers
        
        if (left is not int leftInt)
        {
            throw new InvalidOperationException($"Left side of {binaryExpression.Operator} expression must be an integer");
        }

        if (right is not int rightInt)
        {
            throw new InvalidOperationException($"Right side of {binaryExpression.Operator} expression must be an integer");
        }

        return binaryExpression.Operator switch
        {
            BinaryOperator.Modulo => leftInt % rightInt,
            BinaryOperator.Addition => leftInt + rightInt,
            BinaryOperator.LessThan => leftInt < rightInt,
            BinaryOperator.LessThanEqual => leftInt <= rightInt,
            BinaryOperator.GreaterThan => leftInt > rightInt,
            BinaryOperator.GreaterThanEqual => leftInt >= rightInt,
            _ => throw new NotImplementedException(
                $"Binary operator {binaryExpression.Operator} not yet implemented in interpreter")
        };
    }

    private static VoidObject InterpretVariableDeclaration(VariableDeclarationStatement variableDeclarationStatement)
    {
        var value = InterpretExpression(variableDeclarationStatement.Expression);

        var variableName = variableDeclarationStatement.Identifier.Name;

        if (_globalState.ContainsKey(variableName))
        {
            throw new InvalidOperationException($"The variable {variableName} has already been declared");
        }
        
        _globalState[variableName] = value;
        
        return VoidObject.Instance;
    }

    public static void ResetGlobalState() => _globalState.Clear();
}