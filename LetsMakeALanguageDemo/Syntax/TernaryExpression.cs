namespace LetsMakeALanguageDemo.Syntax;

public class TernaryExpression : Expression
{
    public TernaryExpression(Expression thenExpression, Expression condition, Expression elseExpression)
    {
        ThenExpression = thenExpression;
        Condition = condition;
        ElseExpression = elseExpression;
    }

    public Expression ThenExpression { get; }

    public Expression Condition { get; }

    public Expression ElseExpression { get; }

    public override string ToString() => $"{ThenExpression} if {Condition} else {ElseExpression}";
}