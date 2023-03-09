using System.Text;

namespace LetsMakeALanguageDemo.Tests;

public class DemoTests
{
    public DemoTests()
    {
        // prevents "variable has already been declared" when running multiple tests
        Interpreter.ResetGlobalState();
    }
    
    [Fact]
    public void SimpleAssignmentTest()
    {
        const string input = """
x := 4
x % 3
""";

        var ast = Parser.Parse(input);

        var result = Interpreter.Interpret(ast);
        
        Assert.Equal(1, result);
    }
    
    [Fact]
    public void SimplePrintTest()
    {
        var sb = new StringBuilder();
        using var tw = new StringWriter(sb);
        
        const string input = """
print_line("foo")
print_line("bar")
""";

        var ast = Parser.Parse(input);

        var result = Interpreter.Interpret(ast, tw);
        
        Assert.Equal(VoidObject.Instance, result);
        Assert.Equal($"foo{Environment.NewLine}bar{Environment.NewLine}", sb.ToString());
    }
    
    [Fact]
    public void SimpleWhileLoopTest()
    {
        const string input = """
x := 4
while x < 10 do
begin
    x = x + 1
end
x
""";

        var ast = Parser.Parse(input);

        var result = Interpreter.Interpret(ast);
        
        Assert.Equal(10, result);
    }
    
    [Fact(Skip = "Not yet implemented")]
    //[Fact]
    public void SimpleTernaryTest()
    {
        const string input = """
x := 4
"fizz" if x % 3 == 0 else x
""";

        var ast = Parser.Parse(input);

        var result = Interpreter.Interpret(ast);
        
        Assert.Equal(4, result);
    }
}