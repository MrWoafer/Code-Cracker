using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRunningKeyGibbs
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("-- C# Running Key (Gibbs Sampling) Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--RunningKeyGibbsMessage.txt");

            //msg = "WERGATERYBVIEDOW".ToLower();
            //msg = "NWYULTWLAIJMWSCQ".ToLower();

            int aIndex;
            CipherLib.DecryptionType decryptType;
            if (args.Length > 0)
            {
                aIndex = Int32.Parse(args[0]);
                decryptType = (CipherLib.DecryptionType)Int32.Parse(args[1]);
            }
            else
            {
                aIndex = 0;
                decryptType = CipherLib.DecryptionType.subtraction;
            }

            string plaintext = "";
            for (int i = 0; i < msg.Length; i++)
            {
                plaintext += CipherLib.Annealing.ALPHABET[CipherLib.Annealing.rand.Next() % CipherLib.Annealing.ALPHABET.Length];
            }
            //plaintext = "WERGATERYBVIEDOW".ToLower();
            //plaintext = "hellothereyouarelookingverysharptoday";
            string key = CipherLib.RunningKey.DeduceKey(plaintext, msg, decryptType, aIndex);

            Console.Write("\n\n");

            Console.Write("Starting Plaintext:\n");
            Console.Write("-------------\n");
            Console.Write(plaintext);

            Console.Write("\n\n");

            Console.Write("Starting Key:\n");
            Console.Write("-------------\n");
            Console.Write(key);

            Console.Write("\n\n");

            Console.Write("Using Alphabet: " + CipherLib.RunningKey.ALPHABET);
            Console.Write("\n\nIndex Of 'A': " + aIndex.ToString());
            Console.Write("\n\n-----------------------\n\n");

            CipherLib.LanguageModel languageModel = new CipherLib.LanguageModel(3, "==LargeDictionary.txt");

            languageModel.Analyse();
            //languageModel.Analyse(true);
            //languageModel.Analyse(true, false);
            //languageModel.Analyse(false, true);

            /*Console.Write("\n\n");
            Console.Write(Pr("WERGAT ER YB VIEDOW".ToLower(), languageModel));
            Console.Write("\n\n");
            /*Console.Write(Pr("Hello there sweety pie".ToLower(), languageModel));
            Console.Write("\n\n");
            Console.Write(Pr("And as his back over his".ToLower(), languageModel));
            Console.Write("\n\n");
            Console.Write(Pr("There was not the white figure".ToLower(), languageModel));
            Console.Write("\n\n");*/
            /*Console.Write(Pr("WE RG ATER YBVIE DOW".ToLower(), languageModel));
            Console.Write("\n\n");
            Console.Write(Pr("WER GATERY BVIEDOW".ToLower(), languageModel));
            Console.Write("\n\n");
            //Console.Write(Pr("WERGATER YBVIEDOW".ToLower(), languageModel));
            //Console.Write(Pr("WERGATERYBVIEDOW".ToLower(), languageModel));
            Console.Write(Pr("WERGA TERYBVIED OW".ToLower(), languageModel));*/
            //Console.Write("\n" + Pr(msg, languageModel));
            /*Console.Write(Pr("WERGATER YBVIEDOW".ToLower(), languageModel));
            Console.Write("\n");
            Console.Write(Pr("WERGATERYBVIEDOW".ToLower(), languageModel));
            Console.Write("\n");
            Console.Write(Pr("WERGA TERYBVIED OW".ToLower(), languageModel));
            Console.Write("\n");
            Console.Write(Pr("WERGAT ER YB VIEDOW".ToLower(), languageModel));
            Console.Write("\n");
            Console.Write(Pr("WE RG A T ER YB VIEDOW".ToLower(), languageModel));
            Console.Write("\n");*/

            string bestPlaintext = plaintext;
            //double bestScore = Score(msg, languageModel, bestPlaintext, key, decryptType, aIndex);
            double bestScore = Int32.MinValue;

            double newScore;

            Console.Write("Sampling...\n\n");

            int trialNum = 10000;

            for (int i = 0; i < trialNum; i++)
            {
                /*if (i % 1000 == 0)
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bSample: " + i + " / " + trialNum);
                    //Console.Write("\n" + SampleWordBoundaries(msg, languageModel) + "\n");
                    //Console.Write("\n" + SampleWordBoundaries(plaintext, languageModel) + "\n");
                    /*plaintext = SampleWordBoundaries(plaintext, languageModel);
                    key = SampleWordBoundaries(key, languageModel);
                    Console.Write("\n" + plaintext + "\n");
                    Console.Write("\n" + key + "\n");

                    plaintext = SampleWords(plaintext, languageModel);
                    key = OverlayNewKey(msg, plaintext, key, decryptType, aIndex);

                    Console.Write("\n" + plaintext + "\n");
                    Console.Write("\n" + key + "\n");

                    key = SampleWords(key, languageModel);
                    plaintext = OverlayNewKey(msg, key, plaintext, decryptType, aIndex);

                    Console.Write("\n" + plaintext + "\n");
                    Console.Write("\n" + key + "\n");

                    plaintext = plaintext.Replace(" ", "");
                    key = key.Replace(" ", "");*/
                //}
                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bSample: " + i + " / " + trialNum);

                plaintext = SampleWordBoundaries(plaintext, languageModel);
                key = SampleWordBoundaries(key, languageModel);

                plaintext = SampleWords(plaintext, languageModel);
                key = OverlayNewKey(msg, plaintext, key, decryptType, aIndex);

                key = SampleWords(key, languageModel);
                plaintext = OverlayNewKey(msg, key, plaintext, decryptType, aIndex);

                plaintext = plaintext.Replace(" ", "");
                key = key.Replace(" ", "");

                newScore = Score(msg, languageModel, plaintext, key, decryptType, aIndex);

                //if (newScore > bestScore)
                if (true)
                {
                    bestPlaintext = plaintext;
                    bestScore = newScore;
                }

                while (Console.KeyAvailable)
                {
                    Pause(bestPlaintext, msg, decryptType, aIndex);
                }
            }
            Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bSample: " + trialNum + " / " + trialNum);

            Console.Write("\n\n");
            Console.Write("Plaintext:\n\n");
            Console.Write(bestPlaintext);
            Console.Write("\n\n");
            Console.Write("Key:\n\n");
            Console.Write(CipherLib.RunningKey.DeduceKey(bestPlaintext, msg, decryptType, aIndex));


            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\nPress ENTER to exit...");
            Console.Read();
        }


        public static double Pr(string text, CipherLib.LanguageModel lang, double lambda = 0.7)
        {
            //return lambda * WordTrigramProb(text) + (1 - lambda) * LetterTrigramProb(text);
            //return 0d;
            return lambda * PrWord(text, lang) + (1 - lambda) * PrLetter(text, lang);
        }

        private static double PrWord(string text, CipherLib.LanguageModel lang)
        {
            double p = 1;
            string[] words = text.Split(null);
            if (words.Length < 3)
            {
                //return 0.0000000000001;
                return 1d / lang.NGramsAnalysedCount(3);
            }
            string ngram;
            string ngram2;

            for (int i = 0; i < words.Length - 2; i++)
            {
                ngram = words[i] + " " + words[i+1] + " " + words[i+2];

                ngram2 = words[i] + " " + words[i + 1];

                //Console.Write("\n" + ngram + " " + lang.WordNGramProb(ngram) + "\n");

                //p *= lang.WordNGramProb(ngram);
                p *= lang.WordNGramProb(ngram, 3) / lang.WordNGramProb(ngram2, 2);
            }

            return p;
        }

        private static double PrLetter(string text, CipherLib.LanguageModel lang)
        {
            double p = 1;
            //text = text.Replace(null, "");
            //text = text.Replace(" ", "");
            //Console.Write(text);
            string ngram;

            for (int i = 0; i < text.Length - 2; i++)
            {
                //ngram = text[i] + " " + text[i + 1] + " " + text[i + 2];
                ngram = text[i].ToString() + text[i + 1] + text[i + 2];

                //Console.Write("\n" + ngram + " " + lang.LetterNGramProb(ngram) + "\n");

                p *= lang.LetterNGramProb(ngram);
            }

            return p;
        }

        //private static double Score(string ciphertext, CipherLib.LanguageModel lang, string plaintext, CipherLib.DecryptionType decryptType, int aIndex)
        private static double Score(string ciphertext, CipherLib.LanguageModel lang, string plaintext, string key, CipherLib.DecryptionType decryptType, int aIndex)
        {
            //string key = CipherLib.RunningKey.DeduceKey(plaintext, ciphertext, decryptType, aIndex);

            return Math.Log(Pr(plaintext, lang)) + Math.Log(Pr(key, lang));
        }

        private static string SampleWordBoundaries(string text, CipherLib.LanguageModel lang)
        {
            string keptBoundaries = RandomWordBoundaries(text);
            string newBoundaries;
            //for (int i = 0; i < 10000; i++)
            for (int i = 0; i < 1000; i++)
            {
                newBoundaries = RandomWordBoundaries(text);

                //Console.Write("\n" + newBoundaries);

                if (CipherLib.Annealing.RandFloat() <= Pr(newBoundaries, lang) / Pr(keptBoundaries, lang))
                {
                    keptBoundaries = newBoundaries;
                }
            }
            return keptBoundaries;
        }

        private static string RandomWordBoundaries(string text)
        {
            int countdown = RandomWordLength();
            //for (int i = 0; i < 1000; i++)
            for (int i = 0; i < text.Length; i++)
            {
                if (countdown <= 0)
                {
                    text = text.Insert(i, " ");
                    countdown = RandomWordLength(countdown);
                }
                else
                {
                    countdown -= 1;
                }
            }
            return text;
        }

        private static int RandomWordLength(int previousLength = 6)
        {
            //return 1 + CipherLib.Annealing.WeightedRand(new int[] { 1, 1, 6, 26, 52, 85, 122, 140, 140, 126, 101, 75, 52, 32, 20, 10, 6, 3, 2, 1, 1 });
            //return CipherLib.Annealing.rand.Next() % 15 + 1;
            //return CipherLib.Annealing.rand.Next() % 6 + 1;
            //return
            return 1 + CipherLib.Annealing.WeightedRand(new int[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 4, 4, 4, 4, 4, 2, 1, 1});
        }

        private static string SampleWords(string text, CipherLib.LanguageModel lang)
        {
            string keptWords = FillWithRandomWords(text, lang);
            string newWords;
            for (int i = 0; i < 1000; i++)
            {
                newWords = FillWithRandomWords(text, lang);

                if (CipherLib.Annealing.RandFloat() <= Pr(newWords, lang) / Pr(keptWords, lang))
                {
                    keptWords = newWords;
                }
            }
            return keptWords;
        }

        private static string FillWithRandomWords(string text, CipherLib.LanguageModel lang)
        {
            string newText = "";
            string[] words = text.Split(null);

            for (int i = 0; i < words.Length; i++)
            {
                newText += lang.RandomWord(words[i].Length);

                if (i < words.Length - 1)
                {
                    newText += " ";
                }
            }

            return newText;
        }

        private static string OverlayNewKey(string ciphertext, string plaintext, string key, CipherLib.DecryptionType decryptType, int aIndex)
        {
            string newKey = CipherLib.RunningKey.DeduceKey(plaintext.Replace(" ", ""), ciphertext, decryptType, aIndex);

            string resultKey = "";
            int index = 0;

            for (int i = 0; i < key.Length; i++)
            {
                if (key[i] == ' ')
                {
                    resultKey += ' ';
                }
                else
                {
                    resultKey += newKey[index];
                    index += 1;
                }
            }
            return resultKey;
        }

        private static void Pause(string plaintext, string msg, CipherLib.DecryptionType decryptType, int aIndex)
        {
            Console.Write("\n---------------\n");
            Console.Write("Refined output from best key so far:\n\n");
            Console.Write("Plaintext:\n\n");
            Console.Write(plaintext);
            Console.Write("\n\n");
            Console.Write("Key:\n\n");
            Console.Write(CipherLib.RunningKey.DeduceKey(plaintext, msg, decryptType, aIndex));
            Console.ReadLine();
            Console.Write("\n---------------\n");
        }
    }
}
