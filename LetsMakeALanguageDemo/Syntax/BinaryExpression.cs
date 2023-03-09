namespace LetsMakeALanguageDemo.Syntax;

public class BinaryExpression : Expression
{
    public BinaryExpression(Expression left, BinaryOperator @operator, Expression right)
    {
        Left = left;
        Operator = @operator;
        Right = right;
    }

    public Expression Left { get; }

    public BinaryOperator Operator { get; }

    public Expression Right { get; }

    public override string ToString() => $"{Left} {OperatorString} {Right}";

    public string OperatorString => Operator switch
    {
        BinaryOperator.Modulo => "%",
        BinaryOperator.Addition => "+",
        BinaryOperator.Equal => "==",   
        BinaryOperator.LessThan => "<",
        BinaryOperator.GreaterThan => ">",
        _ => throw new InvalidOperationException("Unexpected binary operator")
    };
}