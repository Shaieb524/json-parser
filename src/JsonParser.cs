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


    private bool isJsonObj()
    {
        return tokens[0].Value == "{" && tokens[tokens.Count-1].Value == "}";
    }

    private bool isJsonArr()
    {
        return tokens[0].Value == "[" && tokens[tokens.Count-1].Value == "]";
    }

    public bool ParseJsonTokens()
    {
        try 
        {
            if (!isJsonObj() && !isJsonArr()) throw new Exception("Can't parse JSON start or end!");

            ParseJsonValue();
            return currentTokenIndex == tokens.Count; // Ensure all tokens are consumed
        } 
        catch (Exception ex)
        {
            Console.WriteLine($"Parsing error: {ex.Message}");
            return false;
        }
    }

    private void ParseObject()
    {
        if (tokens[currentTokenIndex].Value != "{" && tokens[tokens.Count-1].Value != "}") throw new Exception("Expected JSON object!");
        currentTokenIndex++;
        if (CurrentToken.Type == TokenType.BraceClose) { NextToken(); return; } // Empty object
        
        while (true)
        {
            // TODO this clause check was done here to skip the { token for arrays with multiple object
            // TBH it should be refined 
            if (CurrentToken.Type == TokenType.BraceOpen) NextToken();

            ParseJsonPair();
            if (CurrentToken is null) return;

            if (CurrentToken.Type == TokenType.BraceClose)
            {
                NextToken();
                return;
            }

            if (NextToken().Type != TokenType.Comma)
                throw new Exception("Expected Comma!");
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
        
        if (CurrentToken.Type == TokenType.BracketClose) { NextToken(); return; } // Empty array
       
        // keep parsing values 
        while (true)
        {
            ParseJsonValue();

            // hande the case where an obj has array and string key after we face : after the str process
            // so here we have : after the str we have to skip
            // TODO fix dis shit 
            if (CurrentToken.Type == TokenType.Colon)
            {
                NextToken();
                return;
            } 

            if (CurrentToken is null) return;

            if (CurrentToken.Type == TokenType.BracketClose)
            {
                NextToken();
                return;
            }

            if (CurrentToken.Type == TokenType.BraceClose)
            {
                NextToken();
            }

            // there should be comma after each value
            if (CurrentToken.Type != TokenType.Comma) throw new Exception("Expected Comma!");
            
            NextToken();
        }
    }

    private void ParseJsonValue()
    {
        var currentTokenType = tokens[currentTokenIndex].Type;
        
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