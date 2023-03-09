namespace LetsMakeALanguageDemo.Syntax;

public class LiteralExpression : Expression
{
    public LiteralExpression(object value)
    {
        Value = value;
    }

    public object Value { get; }

    public override string ToString() => Value switch
    {
        string s => $"\"{s.Replace("\"", "\\\"")}\"",
        _ => Value.ToString() ?? throw new InvalidOperationException("Value.ToString() returned null")
    };
}