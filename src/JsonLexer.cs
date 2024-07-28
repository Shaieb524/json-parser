using System.Text.Json.Serialization;

public class JsonLexer
{

    public static List<JsonToken> JsonTokenizer(string jsonString)
    {
        List<JsonToken> tokens = new List<JsonToken>();

        int i = 0;

        while (i <jsonString.Length)
        {
            var tt = jsonString[i];
            switch (jsonString[i])
            {
                case '{':
                    tokens.Add(new JsonToken(TokenType.BraceOpen, "{"));
                    i++;
                break;

                case '}':
                    tokens.Add(new JsonToken(TokenType.BraceClose, "}"));   
                    i++;
                break;

                case '[':
                    tokens.Add(new JsonToken(TokenType.BracketOpen, "["));
                    i++;
                break;

                case ']':
                    tokens.Add(new JsonToken(TokenType.BracketClose, "]"));
                    i++;
                break;

                case ':':
                    tokens.Add(new JsonToken(TokenType.Colon, ":"));
                    i++;
                break;

                case ',':
                    tokens.Add(new JsonToken(TokenType.Comma, ","));
                    i++;
                break;

                case '\"':
                    int strStart = i;
                    i++;
                    while (i < jsonString.Length && jsonString[i] != '\"')
                    {
                        i++;
                    }
                    i++;
                    tokens.Add(new JsonToken(TokenType.String, jsonString.Substring(strStart, i - strStart)));
                break;
                
                case ' ' or '\t' or '\n' or '\r':
                    i++;
                continue;

                default:
                    // check digits 
                    if (char.IsDigit(jsonString[i]) || jsonString[i] == '-' || jsonString[i] == '+')
                    {
                        int numStart = i;
                        // TODO check this clause
                        while (i < jsonString.Length && jsonString[i] != ',') 
                        {
                            i++;
                        }
                        tokens.Add(new JsonToken(TokenType.Number, jsonString.Substring(numStart, i - numStart)));  
                    }
                break;

            }

        }

        return tokens;
    }
}