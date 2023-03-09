namespace LetsMakeALanguageDemo.Syntax;

public class Identifier : SyntaxNode
{
    public Identifier(string name)
    {
        Name = name;
    }

    public string Name { get; }

    public override string ToString() => Name;
}