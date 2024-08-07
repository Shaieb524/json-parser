Console.WriteLine("Start");

string input =  "{ \"name\": \"John\", \"age\": 30, \"isMarried\": true }";

string jsonString3 = @"{
        ""name"": ""John"",
        ""age"": 30,
        ""isMarried"": true,
        ""children"": [
            {
                ""name"": ""Anna"",
                ""age"": 5
            },
            {
                ""name"": ""Bob"",
                ""age"": 7
            }
        ],
        ""address"": {
            ""street"": ""123 Main St"",
            ""city"": ""Anytown"",
            ""zipcodes"": [
                {
                    ""code1"": 1
                }, 
                {
                    ""code2"": 2
                }
            ]
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


Console.WriteLine("---------- Custom Parser -----------");

var parser = new JsonParser(tokens);

if (parser.ParseJsonTokens())
{
    Console.WriteLine("Valid JSON");
    Environment.Exit(0);
}
else
{
    Console.WriteLine("Invalid JSON");
    Environment.Exit(1);
}