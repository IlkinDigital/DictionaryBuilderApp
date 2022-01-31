using static DictionaryBuilderApp.ConsoleUtils;
using System;

namespace DictionaryBuilderApp
{
    public class Program
    {
        static void Main()
        {
            string? srcLang, trLang, filepath;

            Console.WriteLine("Welcome to Dictionary Builder\n" +
                              "Please enter");

            srcLang = Ask("Source language: ");
            trLang = Ask("Translations language: ");
            filepath = Ask("Enter filepath of an existing or new file: ");


            DictionaryBuilder db = new(srcLang, trLang, filepath);
            bool exit = false;

            while (!exit)
            {
                Console.Clear();

                Console.WriteLine("Dictionary Builder");

                int actionChoice = Convert.ToInt32(Ask(
                    "1. Display dictionary\n" +
                    "2. Add word and translations to dictionary\n" +
                    "3. Edit a source word\n" +
                    "4. Edit translations\n" +
                    "5. Delete word pair\n" +
                    "6. Delete translation\n" +
                    "7. Search word translations\n" +
                    "8. Exit\n" +
                    "\n" +
                    "Choice: "));

                Console.Clear();

                switch (actionChoice)
                {
                    case 1:
                        try
                        {
                            Console.WriteLine(db);
                        } 
                        catch (Exception ex)
                        {
                            Console.WriteLine("Dictionary is empty.");
                            Console.WriteLine($"Possible error: {ex.Message}");
                        }
                        break;
                    case 2:
                        {
                            while (true)
                            {
                                List<string> translations = new List<string>(); 

                                string srcWord = Ask("Enter Source Word: ");

                                // Get Translations
                                while (true)
                                {
                                    string translation = Ask($"Enter a translation for {srcWord}: ");
                                    translations.Add(translation);

                                    string trRes = AskOptions($"Do you want to add more translations to {srcWord} (y/n): ", 
                                        new string[]{ "y", "n" });

                                    if (trRes == "n")
                                        break;
                                    else
                                        Console.Clear();
                                }

                                db.Submit(srcWord, translations.ToArray());

                                string wpRes = AskOptions($"Do you want to add more word pairs (y/n): ",
                                    new string[] { "y", "n" });

                                if (wpRes == "n")
                                    break;
                                else
                                    Console.Clear();
                            }

                            db.Flush();
                        }
                        break;
                    case 3:
                        {
                            try
                            {
                                db.EditSourceWord(Ask("Enter old source word to edit: "), Ask("Enter new source word: "));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        break;
                    case 4:
                        {
                            string srcWord = Ask("Enter source word: ");

                            List<string> newTranslations = new List<string>();
                            
                            while (true)
                            {
                                string translation = Ask($"Enter a new translation for {srcWord}: ");
                                newTranslations.Add(translation);

                                string trRes = AskOptions($"Do you want to add more translations to {srcWord} (y/n): ",
                                    new string[] { "y", "n" });

                                if (trRes == "n")
                                    break;
                                else
                                    Console.Clear();
                            }

                            try
                            {
                                db.EditTranslations(srcWord, newTranslations);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        break;
                    case 5:
                        {
                            try
                            {
                                db.DeleteTranslationPair(Ask("Enter source word of pair you want to delete: "));
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        break;
                    case 6:
                        {
                            try
                            {
                                db.DeleteTranslation(Ask("Enter source word of a translation to delete: "),
                                                     Ask("Enter a translation to delete: "));
                            }
                            catch(Exception ex)
                            {
                                Console.WriteLine(ex.Message);
                            }
                        }
                        break;
                    case 7:
                        {
                            string srcWord = Ask("Enter a word to translate: ");

                            var dict = db.GetAsList();

                            bool found = false;
                            foreach (var tp in dict)
                            {
                                if (srcWord == tp.SourceWord)
                                {
                                    Console.WriteLine(tp);
                                    found = true;
                                    break;
                                }
                            }

                            if (!found)
                                Console.WriteLine("Specified word is not in the dictionary!");
                        }
                        break;
                    case 8:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice!");
                        break;

                }

                Console.Write("Press enter to continue...");
                Console.ReadLine();
            }
        }
    }
}
