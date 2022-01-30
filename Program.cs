using System;

namespace DictionaryBuilderApp
{
    public class Program
    {
        static void Main()
        {
            DictionaryBuilder db = new("en", "es", "dict_en_es.json");

            Console.WriteLine(db);

            db.Flush();
        }
    }
}
