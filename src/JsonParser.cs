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
        JsonString();
    }

    private void ParseObject()
    {
        if (tokens[position].Value != "{" && tokens[tokens.Count-1].Value != "}") throw new Exception("Expected Json Object");

        position++;
    }

    private void JsonString()
    {   
        if (!tokens[position].Value.StartsWith("\"") || !tokens[position].Value.EndsWith("\"")) throw new Exception("Expected a string");

        position++;

    }

    
}