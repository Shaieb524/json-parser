using System.IO.IsolatedStorage;
using System.Text.Json.Serialization;

public class JsonLexer
{

    private readonly string input;
    private int position;

    public JsonLexer(string _input)
    {
        this.input = _input;
        this.position = 0;
    }

    public List<JsonToken> Tokenize()
    {
        List<JsonToken> tokens = new List<JsonToken>();


        while (position <input.Length)
        {

            char currentChar = input[position];

            if (char.IsWhiteSpace(currentChar)) 
            {
                position++;
                continue;   
            }

            switch (currentChar)
            {
                case '{':
                    tokens.Add(new JsonToken(TokenType.BraceOpen, "{"));
                    position++;
                break;

                case '}':
                    tokens.Add(new JsonToken(TokenType.BraceClose, "}"));   
                    position++;
                break;

                case '[':
                    tokens.Add(new JsonToken(TokenType.BracketOpen, "["));
                    position++;
                break;

                case ']':
                    tokens.Add(new JsonToken(TokenType.BracketClose, "]"));
                    position++;
                break;

                case ':':
                    tokens.Add(new JsonToken(TokenType.Colon, ":"));
                    position++;
                break;

                case ',':
                    tokens.Add(new JsonToken(TokenType.Comma, ","));
                    position++;
                break;

                case '\"':
                    tokens.Add(ReadString());
                break;
                
                case ' ' or '\t' or '\n' or '\r':
                    position++;
                continue;

                default:
                    if (char.IsDigit(currentChar) || currentChar == '-' || currentChar == '+')
                        tokens.Add(ReadDigit());  
                    else if (char.IsLetter(currentChar))
                        tokens.Add(ReadKeyword());
                    else
                        throw new Exception($"Unexpected character '{currentChar}' at position {position}");
                break;
            }

        }

        return tokens;
    }


    private JsonToken ReadDigit()
    {
        int numStart = position;
        position++;

        while (position < input.Length && input[position] != ',') 
        {
            position++;
        }

        return new JsonToken(TokenType.Number, input.Substring(numStart, position - numStart));
    }

    private JsonToken ReadString()
    {
        int strStart = position;
        position++;

        while (position < input.Length && input[position] != '\"')
        {
            position++;
        }
        position++;

        return new JsonToken(TokenType.String, input.Substring(strStart, position - strStart));
    }

    private JsonToken ReadKeyword()
    {
        int start = position;

        while (position < input.Length && char.IsLetter(input[position]))
        {
            position++;
        }

        string value = input.Substring(start, position - start);

        switch (value)
        {
            case "true":
                return new JsonToken(TokenType.True, value);

            case "false":
                return new JsonToken(TokenType.False, value);

            case "null":
                return new JsonToken(TokenType.Null, value);

            default:
                throw new Exception($"Unknown keyword '{value}' at position {start}");
        }
    }
}