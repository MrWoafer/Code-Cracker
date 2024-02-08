using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVigenere
{
    class Program
    {
        private static Random rand = new Random();

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Vigenere Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--VigenereMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");

            int trial = 0;

            int keyLength = 1;
            if (args.Length > 0)
            {
                keyLength = Int32.Parse(args[0]);
            }
            else
            {
                //keyLength = 2;
                //keyLength = 7;
                //keyLength = 6;
                keyLength = 8;
            }

            string currentKey = "";
            for (int i = 0; i < keyLength; i++)
            {
                currentKey += "a";
            }
            string bestKey = currentKey;

            float currentScore = Score(msg, currentKey);
            float bestScore = currentScore;

            Console.Write("Starting with key: " + bestKey + "\n\n");
            Console.Write("Starting with score: " + bestScore.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");

            Tuple<string, float> result;
            string decipherment;

            //for (trial = 0; trial < 2; trial++)
            for (trial = 0; trial < 1; trial++)
            {
                Console.Write("Trial: " + (trial + 1).ToString() + "\n\n");

                currentKey = bestKey;

                result = AnnealingVigenere(msg, currentKey, 10, 1f, 1000);

                currentKey = result.Item1;
                currentScore = result.Item2;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;

                    bestKey = currentKey;

                    Console.Write("\b\b");
                    Console.Write("New best key:\n\n");
                    Console.Write("Score: " + bestScore + "\n");
                    Console.Write("\n");
                    Console.Write("Key:\n");
                    Console.Write(currentKey);
                    Console.Write("\n\n");
                    decipherment = DecodeVigenere(msg, bestKey);
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

                //trial++;
            }


            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        static float Score(string msg, string key)
        {
            string decodedMsg = "";
            decodedMsg = DecodeVigenere(msg, key);

            float score = 0;
            score = CipherLib.Annealing.QuadgramScore(decodedMsg);

            return score;
        }

        static string DecodeVigenere(string msg, string key)
        {
            //string decodedMsg = "";
            StringBuilder decodedMsg = new StringBuilder();
            for (int i = 0; i < msg.Length; i++)
            {
                //decodedMsg += CipherLib.Annealing.ALPHABET[(int)CipherLib.Annealing.Mod((msg[i] - key[i % key.Length]), 26)];
                decodedMsg.Append(CipherLib.Annealing.ALPHABET[(int)CipherLib.Annealing.Mod((msg[i] - key[i % key.Length]), 26)]);
            }
            //return decodedMsg;
            return decodedMsg.ToString();
        }

        static Tuple<string, float> AnnealingVigenere(string msg, string key, float temperature, float step, int count)
        {
            string currentKey = key;
            string bestKey = key;

            float currentScore = Score(msg, currentKey);
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
                    currentKey = MessAroundWithKey(currentKey);

                    currentScore = Score(msg, currentKey);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestKey = currentKey;
                    }
                    else if (currentScore < bestScore)
                    {

                        bool ReplaceAnyway;
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, t);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            bestKey = currentKey;
                        }
                        else
                        {
                            currentKey = bestKey;
                        }
                    }
                    else
                    {
                        currentKey = bestKey;
                    }
                }
            }

            return new Tuple<string, float>(bestKey, bestScore);
        }

        static string MessAroundWithKey(string key)
        {
            int index = rand.Next() % key.Length;

            key = key.Remove(index, 1);
            key = key.Insert(index, CipherLib.Annealing.ALPHABET[rand.Next() % 26].ToString());

            return key;
        }
    }
}