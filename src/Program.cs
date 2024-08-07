class Program
{
    public static void Main()
    {
        string projectRoot = Directory.GetCurrentDirectory();
        string testsDirPath = $"{projectRoot}\\tests";
        string[] jsonFiles = Directory.GetFiles(testsDirPath, "*.json");

        foreach (string file in jsonFiles)
        {
            try
            {
                string fileName = Path.GetFileName(file);
                Console.WriteLine($"====== Processing file {fileName} ======");

                string jsonContent = File.ReadAllText(file);
                ParseJsonString(jsonContent);

                Console.WriteLine(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file {file}: {ex.Message}");
            }
        }
    }

    private static void ParseJsonString(string jsonString)
    {
        JsonLexer lexer = new JsonLexer(jsonString);
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
    }
}