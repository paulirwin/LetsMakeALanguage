namespace LetsMakeALanguageDemo.Syntax;

public class AssignmentExpression : Expression
{
    public AssignmentExpression(Identifier identifier, Expression expression)
    {
        Identifier = identifier;
        Expression = expression;
    }

    public Identifier Identifier { get; }
    
    public Expression Expression { get; }

    public override string ToString() => $"{Identifier} = {Expression}";
}