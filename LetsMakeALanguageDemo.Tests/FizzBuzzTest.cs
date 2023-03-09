using System.Text;

namespace LetsMakeALanguageDemo.Tests;

public class FizzBuzzTest
{
    public FizzBuzzTest()
    {
        Interpreter.ResetGlobalState();
    }
    
    [Fact(Skip = "Not yet implemented")]
    //[Fact]
    public void FizzBuzz()
    {
        var sb = new StringBuilder();
        using var tw = new StringWriter(sb);
        
        const string input = """
i := 0
f := ""
b := ""
fb := ""

while (i = i + 1) <= 100 do
begin
    f = "fizz" if i % 3 == 0 else ""
    b = "buzz" if i % 5 == 0 else ""
    fb = f + b
    print_line(i if fb == "" else fb)
end
""";

        var ast = Parser.Parse(input);

        var result = Interpreter.Interpret(ast, tw);
        
        Assert.Equal(VoidObject.Instance, result);

        var lines = sb.ToString().Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
        
        Assert.Equal(100, lines.Length);

        for (int i = 0; i < 100; i++)
        {
            if ((i + 1) % 15 == 0)
            {
                Assert.Equal("fizzbuzz", lines[i]);
            }
            else if ((i + 1) % 3 == 0)
            {
                Assert.Equal("fizz", lines[i]);
            }
            else if ((i + 1) % 5 == 0)
            {
                Assert.Equal("buzz", lines[i]);
            }
            else
            {
                Assert.Equal((i + 1).ToString(), lines[i]);
            }
        }
    }
}