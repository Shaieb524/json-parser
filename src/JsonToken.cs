public enum TokenType
{   
    String,
    Number,
    True,
    False,
    Null,
    Colon,
    Comma,
    BraceOpen,
    BraceClose,
    BracketOpen,
    BracketClose
}

public class JsonToken
{
    public TokenType Type { get; set; }
    public string Value { get; set; }   

    public JsonToken(TokenType type, string value)
    {
        Type = type;
        Value = value;
    }
}