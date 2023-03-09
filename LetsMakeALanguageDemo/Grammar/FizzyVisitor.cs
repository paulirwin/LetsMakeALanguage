using LetsMakeALanguageDemo.Syntax;

namespace LetsMakeALanguageDemo.Grammar;

public class FizzyVisitor : FizzyBaseVisitor<SyntaxNode>
{
    public override SyntaxNode VisitProgram(FizzyParser.ProgramContext context)
    {
        var statements = new List<Statement>();

        foreach (var statementContext in context.statement())
        {
            if (VisitStatement(statementContext) is not Statement statement)
            {
                throw new ParserException("Unable to parse statement", statementContext.start);
            }
            
            statements.Add(statement);
        }
        
        return new ProgramRoot(statements);
    }

    public override SyntaxNode VisitVariableDeclarationStatement(FizzyParser.VariableDeclarationStatementContext context)
    {
        if (VisitIdentifier(context.identifier()) is not Identifier identifier)
        {
            throw new ParserException("Unable to parse identifier", context.identifier().start);
        }

        if (VisitExpression(context.expression()) is not Expression expression)
        {
            throw new ParserException("Unable to parse expression", context.expression().start);
        }

        return new VariableDeclarationStatement(identifier, expression);
    }

    public override SyntaxNode VisitIdentifier(FizzyParser.IdentifierContext context)
    {
        return new Identifier(context.IDENTIFIER().GetText());
    }

    public override SyntaxNode VisitExpressionStatement(FizzyParser.ExpressionStatementContext context)
    {
        if (VisitExpression(context.expression()) is not Expression expression)
        {
            throw new ParserException("Unable to parse expression", context.expression().start);
        }

        return new ExpressionStatement(expression);
    }

    public override SyntaxNode VisitExpression(FizzyParser.ExpressionContext context)
    {
        if (context.simpleExpression() is { } simpleExpression)
        {
            return VisitSimpleExpression(simpleExpression);
        }

        // if (context.ELSE() is not null)
        // {
        //     return VisitTernaryExpression(context);
        // }

        var exprs = context.expression();
        BinaryOperator op;
        
        if (context.MODULO() is not null)
        {
            op = BinaryOperator.Modulo;
        }
        else if (context.ADDITION() is not null)
        {
            op = BinaryOperator.Addition;
        }
        else if (context.EQUALS() is not null)
        {
            op = BinaryOperator.Equal;
        }
        else if (context.LT() is not null)
        {
            op = BinaryOperator.LessThan;
        }
        else if (context.LTE() is not null)
        {
            op = BinaryOperator.LessThanEqual;
        }
        else if (context.GT() is not null)
        {
            op = BinaryOperator.GreaterThan;
        }
        else if (context.GTE() is not null)
        {
            op = BinaryOperator.GreaterThanEqual;
        }
        else
        {
            throw new NotImplementedException("Expression type not implemented");
        }

        if (exprs.Length != 2)
        {
            throw new ParserException("Unable to parse expression", context.start);
        }

        if (VisitExpression(exprs[0]) is not Expression left)
        {
            throw new ParserException("Unable to parse left side of binary expression", exprs[0].start);
        }
            
        if (VisitExpression(exprs[1]) is not Expression right)
        {
            throw new ParserException("Unable to parse right side of binary expression", exprs[1].start);
        }

        return new BinaryExpression(left, op, right);
    }

    // private SyntaxNode VisitTernaryExpression(FizzyParser.ExpressionContext context)
    // {
    //     if (context.expression().Length != 2)
    //     {
    //         throw new ParserException("Unable to parse ternary expression", context.start);
    //     }
    //
    //     if (VisitCondition(context.condition()) is not Expression condition)
    //     {
    //         throw new ParserException("Unable to parse ternary expression", context.condition().start);
    //     }
    //
    //     if (VisitExpression(context.expression()[0]) is not Expression thenExpression)
    //     {
    //         throw new ParserException("Unable to parse ternary expression", context.expression()[0].start);
    //     } 
    //     
    //     if (VisitExpression(context.expression()[1]) is not Expression elseExpression)
    //     {
    //         throw new ParserException("Unable to parse ternary expression", context.expression()[1].start);
    //     }
    //
    //     return new TernaryExpression(thenExpression, condition, elseExpression);
    // }

    public override SyntaxNode VisitIdentifierExpression(FizzyParser.IdentifierExpressionContext context)
    {
        if (VisitIdentifier(context.identifier()) is not Identifier identifier)
        {
            throw new ParserException("Unable to parse identifier", context.identifier().start);
        }

        return new IdentifierExpression(identifier);
    }

    public override SyntaxNode VisitLiteralExpression(FizzyParser.LiteralExpressionContext context)
    {
        if (context.INTEGER() is { } integer)
        {
            return new LiteralExpression(int.Parse(integer.GetText()));
        }
        
        if (context.STRING() is { } str)
        {
            var stringValue = str.GetText()[1..^1].Replace("\\\"", "\"");
            return new LiteralExpression(stringValue);
        }

        throw new NotImplementedException("Literal expression type not implemented");
    }

    public override SyntaxNode VisitAssignmentExpression(FizzyParser.AssignmentExpressionContext context)
    {
        if (VisitIdentifier(context.identifier()) is not Identifier identifier)
        {
            throw new ParserException("Unable to parse identifier", context.identifier().start);
        }

        if (VisitExpression(context.expression()) is not Expression expression)
        {
            throw new ParserException("Unable to parse expression", context.expression().start);
        }

        return new AssignmentExpression(identifier, expression);
    }

    public override SyntaxNode VisitParenthesizedExpression(FizzyParser.ParenthesizedExpressionContext context)
    {
        if (VisitExpression(context.expression()) is not Expression expression)
        {
            throw new ParserException("Unable to parse expression", context.expression().start);
        }

        return new ParenthesizedExpression(expression);
    }

    public override SyntaxNode VisitStatement(FizzyParser.StatementContext context)
    {
        return VisitStatementType(context.statementType());
    }

    public override SyntaxNode VisitStatementType(FizzyParser.StatementTypeContext context)
    {
        if (context.variableDeclarationStatement() is { } variableDeclarationStatement)
        {
            return VisitVariableDeclarationStatement(variableDeclarationStatement);
        }

        if (context.expressionStatement() is { } expressionStatement)
        {
            return VisitExpressionStatement(expressionStatement);
        }

        if (context.whileStatement() is { } whileStatement)
        {
            return VisitWhileStatement(whileStatement);
        }

        throw new NotImplementedException("Statement type not yet implemented");
    }

    public override SyntaxNode VisitArgumentList(FizzyParser.ArgumentListContext context)
    {
        var args = new List<Expression>();

        foreach (var expressionContext in context.expression())
        {
            if (VisitExpression(expressionContext) is not Expression expression)
            {
                throw new ParserException("Unable to parse argument expression", expressionContext.start);
            }
            
            args.Add(expression);
        }
        
        return new ArgumentList(args);
    }

    public override SyntaxNode VisitCallExpression(FizzyParser.CallExpressionContext context)
    {
        if (VisitIdentifier(context.identifier()) is not Identifier identifier)
        {
            throw new ParserException("Unable to parse call expression identifier", context.identifier().start);
        }

        if (VisitArgumentList(context.argumentList()) is not ArgumentList argumentList)
        {
            throw new ParserException("Unable to parse call expression arguments", context.argumentList().start);
        }

        return new CallExpression(identifier, argumentList);
    }

    public override SyntaxNode VisitWhileStatement(FizzyParser.WhileStatementContext context)
    {
        if (VisitCondition(context.condition()) is not Expression condition)
        {
            throw new ParserException("Unable to parse while statement condition", context.condition().start);
        }

        if (VisitBlockStatement(context.blockStatement()) is not BlockStatement block)
        {
            throw new ParserException("Unable to parse while statement block", context.blockStatement().start);
        }

        return new WhileStatement(condition, block);
    }

    public override SyntaxNode VisitCondition(FizzyParser.ConditionContext context)
    {
        return VisitExpression(context.expression());
    }

    public override SyntaxNode VisitBlockStatement(FizzyParser.BlockStatementContext context)
    {
        var statements = new List<Statement>();

        foreach (var statementContext in context.statement())
        {
            if (VisitStatement(statementContext) is not Statement statement)
            {
                throw new ParserException("Unable to parse statement", statementContext.start);
            }
            
            statements.Add(statement);            
        }
        
        return new BlockStatement(statements);
    }
}