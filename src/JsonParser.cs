public class JsonParser
{
    private List<JsonToken> tokens { get; set; }
    private int position { get; set; }


    public JsonParser(List<JsonToken> _tokens)
    {
        this.tokens = _tokens;
        this.position = 0;
    }

    public void ParseJsonTokens()
    {
        ParseObject();
        ParseString();
        ParseColon();
        ParseValue();
    }

    private void ParseObject()
    {
        if (tokens[position].Value != "{" && tokens[tokens.Count-1].Value != "}") throw new Exception("Expected JSON object!");
        position++;
    }

    private void ParseString()
    {   
        if (!tokens[position].Value.StartsWith("\"") || !tokens[position].Value.EndsWith("\"")) throw new Exception("Expected a string!");
        position++;
    }

    private void ParseColon()
    {
        if (tokens[position].Value != ":") throw new Exception("Expected a colon!");
        position++;
    }

    private void ParseValue()
    {
        var currentTokenType = tokens[position].Type;

        switch (currentTokenType)
        {
            case TokenType.BraceOpen:
                ParseObject();
                break;
            case TokenType.String:
                ParseString();
                break;
            case TokenType.Number:
            case TokenType.True:
            case TokenType.False:
            case TokenType.Null:
                position++;
                break;
            default:
                throw new Exception("Unexpected token");
        }


    }

    
}