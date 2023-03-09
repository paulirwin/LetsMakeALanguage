namespace LetsMakeALanguageDemo;

public static class RuntimeFunctions
{
    public static object Print(object[] args)
    {
        switch (args.Length)
        {
            case 0:
                throw new InvalidOperationException("Too few arguments for print");
            case 1:
                Console.Write(args[0]);
                return VoidObject.Instance;
            default:
                throw new InvalidOperationException("Too many arguments for print");
        }
    }
    
    public static object PrintLine(object[] args)
    {
        switch (args.Length)
        {
            case 0:
                Console.WriteLine();
                return VoidObject.Instance;
            case 1:
                Console.WriteLine(args[0]);
                return VoidObject.Instance;
            default:
                throw new InvalidOperationException("Too many arguments for print_line");
        }
    }
}