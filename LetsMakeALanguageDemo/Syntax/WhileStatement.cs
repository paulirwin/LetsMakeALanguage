namespace LetsMakeALanguageDemo.Syntax;

public class WhileStatement : Statement
{
    public WhileStatement(Expression condition, BlockStatement block)
    {
        Condition = condition;
        Block = block;
    }

    public Expression Condition { get; }

    public BlockStatement Block { get; set; }

    public override string ToString() => $"while {Condition} do{Environment.NewLine}{Block}";
}