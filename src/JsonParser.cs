using System.Text.Json.Nodes;

public class JsonParser
{
    private List<JsonToken> tokens { get; set; }
    private int currentTokenIndex { get; set; }


    public JsonParser(List<JsonToken> _tokens)
    {
        this.tokens = _tokens;
        this.currentTokenIndex = 0;
    }

    private JsonToken CurrentToken => currentTokenIndex < tokens.Count ? tokens[currentTokenIndex] : null;

    private JsonToken NextToken() => tokens[currentTokenIndex++];

    public void ParseJsonTokens()
    {
        ParseJsonValue();
    }

    private void ParseObject()
    {
        if (tokens[currentTokenIndex].Value != "{" && tokens[tokens.Count-1].Value != "}") throw new Exception("Expected JSON object!");
        currentTokenIndex++;

        while (true)
        {
            ParseJsonPair();
            if (CurrentToken is null) return;

            if (CurrentToken.Type == TokenType.BraceClose)
            {
                NextToken();
                return;
            }
            if (NextToken().Type != TokenType.Comma) throw new Exception("Expected Comma!");
        }

    }

    private void ParseJsonPair()
    {
        JsonString();
        JsonColon();
        ParseJsonValue();
    }

    private void JsonColon()
    {
        if (tokens[currentTokenIndex].Value != ":") throw new Exception("Expected a colon!");
        currentTokenIndex++;
    }

    private void JsonString()
    {   
        if (NextToken().Type != TokenType.String) throw new Exception("Expected string");
    }

    private void ParseJsonArray()
    {
        if (tokens[currentTokenIndex].Type != TokenType.BracketOpen) throw new Exception("Expected open bracket!");
        currentTokenIndex++;

        // keep parsing values 
        while (true)
        {
            ParseJsonValue();
            if (CurrentToken is null) return;

            if (CurrentToken.Type == TokenType.BracketClose)
            {
                NextToken();
                return;
            }

            // there should be comma after each value
            if (NextToken().Type != TokenType.Comma) throw new Exception("Expected Comma!");
            
        }
    }

    private void ParseJsonValue()
    {
        var currentTokenType = tokens[currentTokenIndex].Type;
        var tt = tokens[currentTokenIndex];
        
        switch (currentTokenType)
        {
            case TokenType.BraceOpen:
                ParseObject();
                break;
            case TokenType.String:
                JsonString();
                break;
            case TokenType.BracketOpen:
                ParseJsonArray();
                break;
            case TokenType.Number:
            case TokenType.True:
            case TokenType.False:
            case TokenType.Null:
                currentTokenIndex++;
                break;
            default:
                throw new Exception("Unexpected token!");
        }
    }

    
}