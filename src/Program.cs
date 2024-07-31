
Console.WriteLine("Start");

string jsonString =  "{ \"name\": \"John\", \"age\": 30, \"isMarried\": true }";
string jsonString2 =  "{ \"name\": \"Bob\", \"age\": 32.5, \"isMarried\": false }";

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
                ""name"": ""Ben"",
                ""age"": 3
            }
        ],
        ""address"": {
            ""street"": ""123 Main St"",
            ""city"": ""Anytown"",
            ""zipcode"": ""12345""
        },
    ""pets"": null
}";

if (jsonString[0] != '{' || jsonString[jsonString.Length - 1] != '}')
{
    Console.WriteLine("Invalid JSON");
    Environment.Exit(1);
}

var tokens = JsonLexer.JsonTokenizer(jsonString3);

for (int i = 0; i< tokens.Count; i++)
{
    Console.WriteLine(tokens[i].Type + ":" + tokens[i].Value);    
}


var tt = new JsonParser(tokens);

tt.ParseJsonTokens();