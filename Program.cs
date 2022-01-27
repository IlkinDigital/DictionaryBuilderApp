using System;

namespace DictionaryBuilderApp
{
    public class Program
    {
        static void Main()
        {
            DictionaryBuilder db = new("en", "es", "dict_en_es.json");

            db.Add(new KeyValuePair<string, string>("jerk", "pendejo"));

            db.Flush();
        }
    }
}