using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using LetsMakeALanguageDemo.Grammar;
using LetsMakeALanguageDemo.Syntax;

namespace LetsMakeALanguageDemo;

public class Parser
{
    public static ProgramRoot Parse(string input)
    {
        if (!input.EndsWith(Environment.NewLine))
        {
            input = input + Environment.NewLine;
        }
        
        var lexer = new FizzyLexer(new AntlrInputStream(input));
        var parser = new FizzyParser(new CommonTokenStream(lexer));
        var visitor = new FizzyVisitor();

        parser.ErrorHandler = new BailErrorStrategy();

        try
        {
            var parserOutput = visitor.Visit(parser.program());

            if (parserOutput is not ProgramRoot programRoot)
            {
                throw new InvalidOperationException($"Unexpected output from parser visitor, expected {typeof(ProgramRoot)} but got {parserOutput?.GetType().ToString() ?? "null"}");
            }

            return programRoot;
        }
        catch (ParseCanceledException ex) when (ex.InnerException is NoViableAltException noViableAltEx)
        {
            throw new ParserException($"Parse error; no viable alternative rule at '{noViableAltEx.OffendingToken.Text.Replace("\r", "\\r").Replace("\n", "\\n")}' ({noViableAltEx.OffendingToken.Line}:{noViableAltEx.OffendingToken.Column})", noViableAltEx.OffendingToken, ex);
        }
        catch (ParseCanceledException ex) when (ex.InnerException is InputMismatchException inputMismatchEx)
        {
            throw new ParserException($"Parse error; input mismatch at '{inputMismatchEx.OffendingToken.Text.Replace("\r", "\\r").Replace("\n", "\\n")}' ({inputMismatchEx.OffendingToken.Line}:{inputMismatchEx.OffendingToken.Column})", inputMismatchEx.OffendingToken, ex);
        }
    }
}