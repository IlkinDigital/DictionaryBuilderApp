using System.Text.Json.Serialization;
using System.Text.Json;
using System;

namespace DictionaryBuilderApp
{
    public class TranslationPair
    {
        public string SourceWord { get; set; }
        public List<string> Translations { get; set; }

        public TranslationPair(string sourceWord, string translation)
        {
            SourceWord = sourceWord;

            Translations = new List<string>();
            Translations.Add(translation);
        }

        [JsonConstructor] public TranslationPair(string sourceWord, List<string> translations)
        {
            SourceWord = sourceWord;
            Translations = translations;
        }

        public void Export(string filepath)
        {
            using FileStream fs = new(filepath, FileMode.OpenOrCreate);
            JsonSerializer.Serialize(fs, this);
        }

        public override string ToString()
        {
            string result = $"{SourceWord} - ";

            for (int i = 0; i < Translations.Count - 1; i++)
            {
                result += Translations[i] + ", ";
            }
            result += Translations[Translations.Count - 1];

            return result;
        }
    }
}
