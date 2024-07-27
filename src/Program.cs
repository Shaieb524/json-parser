
Console.WriteLine("Start");

string jsonString =  "{ \"name\": \"John\", \"age\": 30, \"isMarried\": true }";
string jsonString2 =  "{ \"name\": \"Bob\", \"age\": 32.5, \"isMarried\": false }";


if (jsonString[0] != '{' || jsonString[jsonString.Length - 1] != '}')
{
    Console.WriteLine("Invalid JSON");
    Environment.Exit(1);
}

var t = JsonLexer.JsonTokenizer(jsonString2);

for (int i = 0; i< t.Count; i++)
{
    Console.WriteLine(t[i]);    
}




