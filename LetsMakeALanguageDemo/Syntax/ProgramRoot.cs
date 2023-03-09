namespace LetsMakeALanguageDemo.Syntax;

public class ProgramRoot : SyntaxNode
{
    public ProgramRoot(IReadOnlyList<Statement> statements)
    {
        Statements = statements;
    }

    public IReadOnlyList<Statement> Statements { get; }

    public override string ToString() => string.Join(Environment.NewLine, Statements);
}