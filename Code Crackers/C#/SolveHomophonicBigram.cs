using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpHomophonic
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Homophonic Substitution Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string[] ciphertext = System.IO.File.ReadAllText("--HomophonicMessage.txt").Split(null);
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            //Console.Write(ciphertext);
            for (int i = 0; i < ciphertext.Length; i++)
            {
                Console.Write(ciphertext[i]);
                if (i < ciphertext.Length - 1)
                {
                    Console.Write(" ");
                }
            }
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            if (args.Length > 0)
            {
                alphabet = args[0];
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }

            HashSet<string> symbols = new HashSet<string>();

            foreach (string i in ciphertext)
            {
                symbols.Add(i);
            }

            float[] alphabetExpectedFrequencies = CipherLib.Annealing.averageletterfrequencies;
            for (int i = 0; i < alphabetExpectedFrequencies.Length; i++)
            {
                alphabetExpectedFrequencies[i] /= 100f;
            }

            CipherLib.Homophonic currentKey = new CipherLib.Homophonic(symbols.ToArray(), alphabet, alphabetExpectedFrequencies);
            currentKey.RandomiseKey();

            /// Testing decryption
            ///
            //if (true)
            if (false)
            {
                currentKey.ChangeHomophone("00", 'a');
                currentKey.ChangeHomophone("01", 'a');
                currentKey.ChangeHomophone("02", 'a');
                currentKey.ChangeHomophone("03", 'a');
                currentKey.ChangeHomophone("04", 'b');
                currentKey.ChangeHomophone("05", 'c');
                currentKey.ChangeHomophone("06", 'c');
                currentKey.ChangeHomophone("07", 'd');
                currentKey.ChangeHomophone("08", 'd');
                currentKey.ChangeHomophone("09", 'e');
                currentKey.ChangeHomophone("10", 'e');
                currentKey.ChangeHomophone("11", 'e');
                currentKey.ChangeHomophone("12", 'e');
                currentKey.ChangeHomophone("13", 'e');
                currentKey.ChangeHomophone("14", 'e');
                currentKey.ChangeHomophone("15", 'f');
                currentKey.ChangeHomophone("16", 'g');
                currentKey.ChangeHomophone("17", 'h');
                currentKey.ChangeHomophone("18", 'h');
                currentKey.ChangeHomophone("19", 'h');
                currentKey.ChangeHomophone("20", 'i');
                currentKey.ChangeHomophone("21", 'i');
                currentKey.ChangeHomophone("22", 'i');
                currentKey.ChangeHomophone("23", 'i');
                currentKey.ChangeHomophone("24", 'j');
                currentKey.ChangeHomophone("25", 'k');
                currentKey.ChangeHomophone("26", 'l');
                currentKey.ChangeHomophone("27", 'l');
                currentKey.ChangeHomophone("28", 'm');
                currentKey.ChangeHomophone("29", 'n');
                currentKey.ChangeHomophone("30", 'n');
                currentKey.ChangeHomophone("31", 'n');
                currentKey.ChangeHomophone("32", 'o');
                currentKey.ChangeHomophone("33", 'o');
                currentKey.ChangeHomophone("34", 'o');
                currentKey.ChangeHomophone("35", 'o');
                currentKey.ChangeHomophone("36", 'p');
                currentKey.ChangeHomophone("37", 'q');
                currentKey.ChangeHomophone("38", 'r');
                currentKey.ChangeHomophone("39", 'r');
                currentKey.ChangeHomophone("40", 'r');
                currentKey.ChangeHomophone("41", 's');
                currentKey.ChangeHomophone("42", 's');
                currentKey.ChangeHomophone("43", 's');
                currentKey.ChangeHomophone("44", 't');
                currentKey.ChangeHomophone("45", 't');
                currentKey.ChangeHomophone("46", 't');
                currentKey.ChangeHomophone("47", 't');
                currentKey.ChangeHomophone("48", 't');
                currentKey.ChangeHomophone("49", 'u');
                currentKey.ChangeHomophone("50", 'v');
                currentKey.ChangeHomophone("51", 'w');
                currentKey.ChangeHomophone("52", 'x');
                currentKey.ChangeHomophone("53", 'y');
                currentKey.ChangeHomophone("54", 'z');

                /*Console.Write(currentKey.Decrypt(ciphertext));
                Console.Write("\n\n" + Score(ciphertext, currentKey));
                Console.Write("\n\n");*/
            }

            ///

            Console.Write(currentKey.Decrypt(ciphertext));
            Console.Write("\n\n" + Score(ciphertext, currentKey));
            Console.Write("\n\n");

            CipherLib.Homophonic bestKey = new CipherLib.Homophonic(currentKey);

            float currentScore = Score(ciphertext, currentKey);
            float bestScore = currentScore;

            Console.Write("Using alphabet: " + alphabet + "\n\n");
            Console.Write("Number of symbols: " + symbols.Count.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");

            Tuple<CipherLib.Homophonic, float> result;
            string decipherment;

            int trial = 0;

            for (trial = 0; trial < 1; trial++)
            {
                Console.Write("Trial: " + (trial + 1).ToString() + "\n\n");

                //currentKey = bestKey;
                currentKey = new CipherLib.Homophonic(bestKey);

                //result = AnnealingHomophonic(ciphertext, currentKey, symbols.ToArray(), alphabet, 10f, 1f, 1000);
                //result = AnnealingHomophonic(ciphertext, currentKey, symbols.ToArray(), alphabet, 20f, 1f, 10000);
                result = AnnealingHomophonic(ciphertext, currentKey, symbols.ToArray(), alphabet, 20f, 0.2f, 10000);

                //currentKey = result.Item1;
                currentKey = new CipherLib.Homophonic(result.Item1);
                currentScore = result.Item2;

                if (currentScore > bestScore)
                //if (currentScore < bestScore)
                {
                    bestScore = currentScore;

                    //bestKey = currentKey;
                    bestKey = new CipherLib.Homophonic(currentKey);

                    Console.Write("\b\b");
                    Console.Write("New best key:\n\n");
                    Console.Write("Score: " + bestScore + "\n");
                    Console.Write("\n");
                    Console.Write("Key:\n");
                    Console.Write(currentKey);
                    Console.Write("\n\n");
                    decipherment = bestKey.Decrypt(ciphertext);
                    Console.Write("Decipherment: " + decipherment);
                    Console.Write("\n\n");
                }

                else
                {
                    Console.Write("\b\b");
                    Console.Write("Didn't find a better key...");
                    Console.Write("\n\n");
                }
                Console.Write("--------------------------------------\n\n");
            }


            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        static float Score(string[] ciphertext, CipherLib.Homophonic key)
        {
            string plaintext = "";
            plaintext = key.Decrypt(ciphertext);

            float score = 0;
            score = CipherLib.Annealing.QuadgramScore(plaintext);
            //score = CipherLib.Annealing.ChiSquared(plaintext);

            return score;
        }

        static Tuple<CipherLib.Homophonic, float> AnnealingHomophonic(string[] ciphertext, CipherLib.Homophonic key, string[] symbols, string alphabet, float temperature, float step, int count)
        {
            CipherLib.Homophonic currentKey = key;
            CipherLib.Homophonic bestKey = key;

            float currentScore = Score(ciphertext, currentKey);
            float bestScore = currentScore;

            for (float t = temperature; t >= 0; t -= step)
            {
                string zeroes = "";

                for (int i = 0; i < 2 - t.ToString().Length; i++)
                {
                    zeroes += "0";
                }

                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" + zeroes + t);

                for (int trial = 0; trial < count; trial++)
                {
                    //currentKey = MessAroundWithKey(currentKey, symbols, alphabet);
                    //for (int i = 0; i < 5; i++)
                    for (int i = 0; i < 1; i++)
                    {
                        currentKey = new CipherLib.Homophonic(MessAroundWithKey(currentKey, symbols, alphabet));
                    }

                    currentScore = Score(ciphertext, currentKey);

                    if (currentScore > bestScore)
                    //if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        //bestKey = currentKey;
                        bestKey = new CipherLib.Homophonic(currentKey);
                    }
                    else if (currentScore < bestScore)
                    //else if (currentScore > bestScore)
                    {

                        bool ReplaceAnyway;
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, t);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            //bestKey = currentKey;
                            bestKey = new CipherLib.Homophonic(currentKey);
                        }
                        else
                        {
                            //currentKey = bestKey;
                            currentKey = new CipherLib.Homophonic(bestKey);
                        }
                    }
                    else
                    {
                        //currentKey = bestKey;
                        currentKey = new CipherLib.Homophonic(bestKey);
                    }
                }
            }

            return new Tuple<CipherLib.Homophonic, float>(bestKey, bestScore);
        }

        static CipherLib.Homophonic MessAroundWithKey(CipherLib.Homophonic key, string[] symbols, string alphabet)
        {
            string symbol = CipherLib.Utils.PickRandom(symbols);

            key.ChangeHomophone(symbol, alphabet[CipherLib.Utils.rand.Next() % alphabet.Length]);

            return key;
        }
    }
}