Console.WriteLine("Start");

string jsonString = "{ \"key\": \"value\"}";

if (jsonString[0] != '{' || jsonString[jsonString.Length - 1] != '}')
{
    Console.WriteLine("Invalid JSON");
    Environment.Exit(1);
}

var t = JsonTokenizer(jsonString);

for (int i = 0; i< t.Count; i++)
{
    Console.WriteLine(t[i]);    
}

static List<string> JsonTokenizer(string jsonString)
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

        // check strings for keys
        else if (jsonString[i] == '\"')
        {
            
            // the key is the string value between the two quotations
            var keyStart = i;
            i++;
            while (jsonString[i] != '\"')
            {
                i++;
            }
            i++;
            tokens.Add(jsonString.Substring(keyStart, i - keyStart));
        } 
    }

    return tokens;
}

