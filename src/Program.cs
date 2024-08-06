
Console.WriteLine("Start");

string input =  "{ \"name\": \"John\", \"age\": 30, \"isMarried\": true }";
string jsonString2 =  "{ \"name\": \"Bob\", \"age\": 32.5, \"isMarried\": false }";

string jsonString3 = @"{
        ""name"": ""John"",
        ""age"": 30,
        ""isMarried"": true,
        ""children"": [
            {
                ""name"": ""Anna"",
                ""age"": 5
            }
        ],
        ""address"": {
            ""street"": ""123 Main St"",
            ""city"": ""Anytown"",
            ""zipcode"": ""12345""
        },
    ""pets"": null
}";

if (input[0] != '{' || input[input.Length - 1] != '}')
{
    Console.WriteLine("Invalid JSON");
    Environment.Exit(1);
}

JsonLexer lexer = new JsonLexer(jsonString3);
List<JsonToken> tokens = lexer.Tokenize();

for (int i = 0; i< tokens.Count; i++)
{
    Console.WriteLine(tokens[i].Type + ":" + tokens[i].Value);    
}


var tt = new JsonParser(tokens);

tt.ParseJsonTokens();