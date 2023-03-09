namespace LetsMakeALanguageDemo.Syntax;

public class ExpressionStatement : Statement
{
    public ExpressionStatement(Expression expression)
    {
        Expression = expression;
    }

    public Expression Expression { get; }

    public override string ToString() => Expression.ToString();
}