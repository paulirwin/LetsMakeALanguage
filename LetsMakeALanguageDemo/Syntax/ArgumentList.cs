namespace LetsMakeALanguageDemo.Syntax;

public class ArgumentList : SyntaxNode
{
    public ArgumentList(IReadOnlyList<Expression> arguments)
    {
        Arguments = arguments;
    }

    public IReadOnlyList<Expression> Arguments { get; }

    public override string ToString() => string.Join(", ", Arguments);
}