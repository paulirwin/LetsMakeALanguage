using Antlr4.Runtime;

namespace LetsMakeALanguageDemo;

public class ParserException : Exception
{
    public ParserException(string message, IToken token, Exception? innerException = null) 
        : base(message, innerException)
    {
        Token = token;
    }
    
    public IToken Token { get; }
}