namespace LetsMakeALanguageDemo.Syntax;

public class CallExpression : Expression
{
    public CallExpression(Identifier identifier, ArgumentList argumentList)
    {
        Identifier = identifier;
        ArgumentList = argumentList;
    }

    public Identifier Identifier { get; }

    public ArgumentList ArgumentList { get; }

    public override string ToString() => $"{Identifier}({ArgumentList})";
}