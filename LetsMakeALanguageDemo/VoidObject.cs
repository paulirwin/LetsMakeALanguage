namespace LetsMakeALanguageDemo;

public sealed class VoidObject
{
    public static readonly VoidObject Instance = new();

    private VoidObject()
    {
    }
    
    public override string ToString() => "";
}