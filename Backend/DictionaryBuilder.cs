using System;
using System.Text.Json;

namespace DictionaryBuilderApp
{
    public class DictionaryBuilder
    {
        static void Flush(string filepath, List<TranslationPair> buffer)
        {
            using (FileStream fs = new(filepath, FileMode.Open, FileAccess.Read))
            {
                try
                {
                    var result = JsonSerializer.Deserialize<List<TranslationPair>>(fs);

                    if (result != null)
                        buffer.AddRange(result);
                }
                catch (JsonException) { } // Ignore Json Exception
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            }

            using (FileStream fs = new(filepath, FileMode.Create, FileAccess.Write))
            {
                JsonSerializer.Serialize(fs, buffer);
            }
        }

        public string SourceLang { get; private set; }
        public string TranslationLang { get; private set; }
        private List<TranslationPair> WordBatch { get; init; } = new List<TranslationPair>();

        private string _filepath = "";
        public string Filepath 
        {
            get => _filepath;
            set
            {
                _filepath = value;
                // Insures that file will be opened or created
                using FileStream fs = new(_filepath, FileMode.OpenOrCreate);
            }
        }

        
        public DictionaryBuilder(string firstLang, string secondLang, string filepath)
        {
            SourceLang = firstLang;
            TranslationLang = secondLang;
            Filepath = filepath;
        }

        public void Add(string source, string[] translations)
        {
            Submit(source, translations);
            Flush();
        }
        public void Add(List<TranslationPair> pairs)
        {
            foreach (var pair in pairs)
            {
                Submit(pair);
            }

            Flush();
        }

        public void Submit(TranslationPair pair)
        {
            WordBatch.Add(pair);
        }
        public void Submit(string source, string[] translations)
        {
            WordBatch.Add(new TranslationPair(source, translations.ToList()));
        }

        // Clears Word Batch and appends it into a json file 
        public void Flush()
        {
            Flush(Filepath, WordBatch);
            WordBatch.Clear();
        }

        public List<TranslationPair> GetAsList()
        {
            using FileStream fs = new(Filepath, FileMode.Open, FileAccess.Read);

            var dict = JsonSerializer.Deserialize<List<TranslationPair>>(fs);

            if (dict != null)
                return new List<TranslationPair>(dict);
            
            return new List<TranslationPair>();
        }
        List<string> GetTranslations(string sourceWord)
        {
            var dict = GetAsList();

            foreach (var item in dict)
            {
                if (sourceWord == item.SourceWord)
                    return item.Translations;
            }

            throw new Exception("Specified source word doesn't exist in the dictionary!");
        }
        
        public void DeleteTranslationPair(string sourceWord)
        {
            var dict = GetAsList();
            List<TranslationPair> removed = new();

            bool found = false;
            foreach (var item in dict)
            {
                if (item.SourceWord == sourceWord)
                {
                    if (!found)
                        removed = new(dict);
                    removed.Remove(item);
                    found = true;
                }
            }

            if (!found)
                throw new Exception("Translation pair with specified source doesn't exist in the dictionary!");

            Clear();
            Flush(Filepath, found ? removed : dict);
        }
        public void DeleteTranslation(string sourceWord, string translation)
        {
            var dict = GetAsList();

            bool srcFound = false;
            bool translationFound = false;
            for (int i = 0; i < dict.Count; i++)
            {
                if (dict[i].SourceWord == sourceWord)
                {
                    if (dict[i].Translations.Count > 1)
                    {
                        List<string> modTranslations = new();
                        foreach (var word in dict[i].Translations)
                        {
                            if (word == translation)
                            {
                                if (!translationFound)
                                    modTranslations = new(dict[i].Translations);

                                modTranslations.Remove(word);
                                translationFound = true;
                            }
                        }

                        dict[i].Translations = modTranslations;
                    }

                    srcFound = true;
                }
            }

            if (!srcFound)
                throw new Exception("Specified source word doesn't exist in the dictionary!");
            if (!translationFound)
                throw new Exception("Specified translation doesn't exist or is only one!");

            Clear();
            Flush(Filepath, dict);
        }

        public void EditSourceWord(string originalSrcWord, string newSrcWord)
        {
            var dict = GetAsList();

            bool found = false;
            foreach (var item in dict)
            {
                if (item.SourceWord == originalSrcWord)
                {
                    item.SourceWord = newSrcWord;
                    found = true;
                }
            }

            if (!found)
                throw new Exception("Specified original source word doesn't exist in the dictionary!");

            Clear();
            Flush(Filepath, dict);
        }
        public void EditTranslations(string sourceWord, List<string> newTranslations)
        {
            var dict = GetAsList();

            bool found = false;
            foreach (var item in dict)
            {
                if (item.SourceWord == sourceWord)
                {
                    item.Translations = newTranslations;
                    found = true;
                }
            }

            if (!found)
                throw new Exception("Specified source word doesn't exist in the dictionary!");

            Clear();
            Flush(Filepath, dict);
        }

        public void Clear()
        {
            using FileStream fs = new(Filepath, FileMode.Create);
        }

        public override string ToString()
        {
            string result = $"'{SourceLang}' to '{TranslationLang}' Dictionary:\n";
            var dict = GetAsList();

            foreach (var item in dict)
            {
                result += $"\t{item}\n";
            }

            return result;
        }
    }
}
