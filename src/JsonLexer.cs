using System.IO.IsolatedStorage;
using System.Text.Json.Serialization;
using System.Xml.XPath;

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

                case '"':
                    tokens.Add(ReadString());
                break;
                
                case ' ' or '\t' or '\n' or '\r' or '\b' or '\f':
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
        var nn = input[position];

        while (position < input.Length && input[position] != ',') 
        {
            if (input[position] == ']') break; // remove braket close from last digint like this: 2] 
            if (!char.IsDigit(input[position])) throw new Exception($"Invalid number format: expected digits after exponent at position {position}");
            position++;
        }

        return new JsonToken(TokenType.Number, input.Substring(numStart, position - numStart));
    }

    private JsonToken ReadString()
    {
        position++; 
        var result = new System.Text.StringBuilder();

        while (position < input.Length)
        {
            char currentChar = input[position];

            if (currentChar == '"')
            {
                // Closing quote
                position++;
                break;
            }

            if (currentChar == '\\')
            {
                // Handle escaped characters
                position++;
                if (position >= input.Length)
                    throw new Exception("Unexpected end of input after backslash");

                currentChar = input[position];

                switch (currentChar)
                {
                    case '"':
                    case '\\':
                    case '/':
                        result.Append(currentChar);
                        break;
                    case 'b':
                        result.Append('\b');
                        break;
                    case 'f':
                        result.Append('\f');
                        break;
                    case 'n':
                        result.Append('\n');
                        break;
                    case 'r':
                        result.Append('\r');
                        break;
                    case 't':
                        result.Append('\t');
                        break;
                    case 'u':
                        result.Append(ParseHexChar());
                        break;
                    default:
                        throw new Exception($"Invalid escape character '\\{currentChar}' at position {position}");
                }
            }
            else if (currentChar == '\t')
            {
                // Directly handle the tab character
                throw new Exception($"Invalid control character '{currentChar}' in string at position {position}");
            }
            else if (currentChar == '\n' || currentChar == '\r')
            {
                // JSON does not allow unescaped line breaks within strings
                throw new Exception($"Invalid line break in string at position {position}");
            }
            else
            {
                result.Append(currentChar);
            }

            position++;
        }

        if (position >= input.Length || input[position - 1] != '"')
        {
            throw new Exception("Unterminated string literal");
        }

        return new JsonToken(TokenType.String, result.ToString());
    }

    private string ParseHexChar()
    {
        // Handle Unicode escape sequence \uXXXX
        if (position + 4 >= input.Length)
            throw new Exception("Unexpected end of input in Unicode escape sequence");

        string hex = input.Substring(position + 1, 4);
        
        var isHex = IsHex(hex.ToCharArray(0,4));

        if (!isHex) throw new Exception($"Invalid Unicode escape sequence: \\u{hex}");
        
        return hex;
    }

    private bool IsHex(char[] chars)
    {
        bool isHex; 
        foreach(var c in chars)
        {
            isHex = ((c >= '0' && c <= '9') || 
                    (c >= 'a' && c <= 'f') || 
                    (c >= 'A' && c <= 'F'));

            if(!isHex) return false;
        }
        return true;
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