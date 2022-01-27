using System;
using System.Text.Json;

namespace DictionaryBuilderApp
{
    public class DictionaryBuilder
    {
        public string FirstLang { get; private set; }
        public string SecondLang { get; private set; }

        private string _filepath = "";
        public string Filepath 
        {
            get => _filepath;
            set
            {
                _filepath = value;
                using FileStream fs = new(_filepath, FileMode.OpenOrCreate);
            }
        }

        private List<KeyValuePair<string, string>> WordPairBuffer { get; init; } = new List<KeyValuePair<string, string>>();

        public DictionaryBuilder(string firstLang, string secondLang, string filepath)
        {
            FirstLang = firstLang;
            SecondLang = secondLang;
            Filepath = filepath;
        }

        public void Add(KeyValuePair<string, string> wordPair)
        {
            WordPairBuffer.Add(wordPair);
        }

        public void Flush()
        {
            using FileStream fs = new(Filepath, FileMode.Create);

            try
            {
                var result = JsonSerializer.Deserialize<List<KeyValuePair<string, string>>>(fs);
                WordPairBuffer.AddRange(result);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            JsonSerializer.Serialize(fs, WordPairBuffer);
            WordPairBuffer.Clear();
        }

    }
}
