using LetsMakeALanguageDemo;
using Spectre.Console;

if (args.Length == 1)
{
    // interpret specified file
    var fileText = File.ReadAllText(args[0]);
    Interpreter.Interpret(Parser.Parse(fileText));
    return;
}

AnsiConsole.Write(new FigletText("Fizzy REPL").Color(Color.Magenta3));
AnsiConsole.MarkupLine("[gray]Enter [silver]\\help[/] for additional commands[/]");
AnsiConsole.WriteLine();

// REPL logic
Loop(() =>
{
    var input = Read();

    var result = Eval(input);
    
    Print(result);
});

string Read() => AnsiConsole.Ask<string>("> ");

object Eval(string input)
{
    if (input == "\\help")
    {
        return new HelpText();
    }

    if (input == "\\reset")
    {
        Interpreter.ResetGlobalState();
        AnsiConsole.WriteLine("Global state reset.");
        return VoidObject.Instance;
    }

    if (input == "\\clear")
    {
        AnsiConsole.Clear();
        return VoidObject.Instance;
    }

    if (input is "\\exit" or "\\quit")
    {
        Environment.Exit(0);
        return VoidObject.Instance;
    }
    
    try
    {
        return Interpreter.Interpret(Parser.Parse(input));
    }
    catch (Exception ex)
    {
        return ex;
    }
}

void Print(object value)
{
    if (value is Exception ex)
        AnsiConsole.MarkupLine($"[red]{FormatException(ex)}[/]");
    else if (value is HelpText help) 
        AnsiConsole.MarkupLine(help.ToString());
    else if (value is not VoidObject)
        AnsiConsole.MarkupLine($"[yellow]{FormatPrintString(value)}[/]");
}

void Loop(Action action)
{
    while (true) action();
}

string FormatPrintString(object value) => value switch
{
    string s => $"\"{s.Replace("\"", "\\\"")}\"",
    _ => value.ToString() ?? ""
};

string FormatException(Exception ex) 
    => $"{ex.GetType().FullName}: {ex.Message}";

internal class HelpText
{
    public override string ToString()
    {
        return """
Commands:
\reset   [silver]Resets global state as if starting from scratch[/]
\clear   [silver]Clears the console output[/]
\exit    [silver]Exits the app[/]
\quit    [silver]Same as [/]\exit
""";
    }
}