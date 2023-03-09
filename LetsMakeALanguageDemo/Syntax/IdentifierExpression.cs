namespace LetsMakeALanguageDemo.Syntax;

public class IdentifierExpression : Expression
{
    public IdentifierExpression(Identifier identifier)
    {
        Identifier = identifier;
    }

    public Identifier Identifier { get; }

    public override string ToString() => Identifier.ToString();
}