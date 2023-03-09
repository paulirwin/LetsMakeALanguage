namespace LetsMakeALanguageDemo.Syntax;

public class BlockStatement : Statement
{
    public BlockStatement(IReadOnlyList<Statement> statements)
    {
        Statements = statements;
    }

    public IReadOnlyList<Statement> Statements { get; }

    public override string ToString() =>
        $"begin{Environment.NewLine}{string.Join(Environment.NewLine, Statements)}{Environment.NewLine}end{Environment.NewLine}";
}