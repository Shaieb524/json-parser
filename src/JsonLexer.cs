public class JsonLexer
{


    public static List<string> JsonTokenizer(string jsonString)
    {
        List<string> tokens = new List<string>();

        int i = 0;

        while (i <jsonString.Length)
        {
            // skip white spaces
            if (Char.IsWhiteSpace(jsonString[i]))
            {
                i++;
                continue;
            }

            // check structural characters  {, }, [, ], :, ,, strings (keys and values), numbers, true, false, null.
            if (jsonString[i] == '{' || jsonString[i] == '}' || jsonString[i] == ':' || jsonString[i] == ',')
            {
                tokens.Add(jsonString[i].ToString());
                i++;
            }

            // check strings for keys and string values 
            else if (jsonString[i] == '\"')
            {
                // the key is the string value between the two quotations
                var strStart = i;
                i++;
                while (jsonString[i] != '\"')
                {
                    i++;
                }
                i++;
                tokens.Add(jsonString.Substring(strStart, i - strStart));
            }
            else
            {
                // non str tokens
                var start = i;
                while (!char.IsWhiteSpace(jsonString[i]) && jsonString[i] != ',' && jsonString[i] != '}' && jsonString[i] != ':')
                {
                    i++;
                }
                tokens.Add(jsonString.Substring(start, i - start));
            }
    
        }

        return tokens;
    }
}