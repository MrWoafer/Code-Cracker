using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpDigraph
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

            Console.Write("-- C# Digraph Substitution Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--DigraphMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");

            int trial = 0;

            string currentKey = "";
            //for (int i = 0; i < 676 * 2; i++)
            /*for (int i = 0; i < 676; i++)
            {
                //currentKey += CipherLib.Annealing.ALPHABET[(int)Math.Floor((decimal)i / 26)] + CipherLib.Annealing.ALPHABET[i % 26];
                //Console.Write(CipherLib.Annealing.ALPHABET[i / 26].ToString() + CipherLib.Annealing.ALPHABET[i % 26].ToString());
                currentKey += CipherLib.Annealing.ALPHABET[i / 26].ToString() + CipherLib.Annealing.ALPHABET[i % 26].ToString();
            }*/
            currentKey = NewRandomKey();
            string bestKey = currentKey;

            float currentScore = Score(msg, currentKey);
            float bestScore = currentScore;

            //Console.Write("Starting with key: " + bestKey + "\n\n");
            Console.Write("Starting with key:\n");
            DisplayKey(bestKey);
            Console.Write("\n\n");
            Console.Write("Starting with score: " + bestScore.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");

            Tuple<string, float> result;
            string decipherment;

            //for (trial = 0; trial < 2; trial++)
            for (trial = 0; trial >= 0; trial++)
            {
                Console.Write("Trial: " + (trial + 1).ToString() + "\n\n");

                currentKey = bestKey;
                //currentKey = NewRandomKey();

                result = AnnealingDigraph(msg, currentKey, 20, 0.2f, 10000);
                //result = AnnealingDigraph(msg, currentKey, 20, 0.2f, 1000);

                currentKey = result.Item1;
                currentScore = result.Item2;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;

                    bestKey = currentKey;

                    Console.Write("\b\b\b\b\b\b\b\b\b");
                    Console.Write("New best key:\n\n");
                    Console.Write("Score: " + bestScore + "\n");
                    Console.Write("\n");
                    Console.Write("Key:\n");
                    DisplayKey(currentKey);
                    Console.Write("\n\n");
                    decipherment = DecodeDigraph(msg, bestKey);
                    Console.Write("Decipherment: " + decipherment);
                    Console.Write("\n\n");
                }

                else
                {
                    Console.Write("\b\b\b\b\b\b\b\b");
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
            decodedMsg = DecodeDigraph(msg, key);

            float score = 0;
            score = CipherLib.Annealing.QuadgramScore(decodedMsg);

            return score;
        }

        static string DecodeDigraph(string msg, string key)
        {
            string decodedMsg = "";
            int index = 0;
            for (int i = 0; i < msg.Length-1; i += 2)
            {
                index = ((msg[i] - 97) * 26 + (msg[i + 1] - 97)) * 2;

                //Console.Write(index.ToString() + " " + msg[i] + msg[i + 1] + "\n");
                decodedMsg += key[index].ToString() + key[index+1].ToString();
            }
            return decodedMsg;
        }

        static Tuple<string, float> AnnealingDigraph(string msg, string key, float temperature, float step, int count)
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

                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" + zeroes + t.ToString("n1"));

                for (int trial = 0; trial < count; trial++)
                {
                    //for (int i = 0; i < CipherLib.Annealing.rand.Next() % 15; i++)
                    //for (int i = 0; i < CipherLib.Annealing.rand.Next() % 30; i++)
                    for (int i = 0; i < CipherLib.Annealing.rand.Next() % 676; i++)
                    {
                        currentKey = MessAroundWithKey(currentKey);
                    }

                    currentScore = Score(msg, currentKey);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestKey = currentKey;
                    }
                    else if (currentScore <= bestScore)
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
            int index1 = CipherLib.Annealing.rand.Next() % key.Length;
            int index2 = CipherLib.Annealing.rand.Next() % key.Length;

            string temp = key[index1 - index1 % 2].ToString() + key[index1 - index1 % 2 + 1].ToString();
            string temp2 = key[index2 - index2 % 2].ToString() + key[index2 - index2 % 2 + 1].ToString();

            key = key.Remove(index1 - index1 % 2, 2);
            key = key.Insert(index1 - index1 % 2, temp2);
            key = key.Remove(index2 - index2 % 2, 2);
            key = key.Insert(index2 - index2 % 2, temp);

            return key;
        }

        static void DisplayKey(string key)
        {
            Console.Write("   ");
            for (int i = 0; i < 26; i++)
            {
                Console.Write(CipherLib.Annealing.ALPHABET[i] + " ");
            }
            Console.Write("\n   ");
            for (int i = 0; i < 26 * 2; i++)
            {
                Console.Write("-");
            }
            Console.Write("\n");
            for (int i = 0; i < key.Length; i++)
            {
                if (i % (26 * 2) == 0)
                {
                    if (i != 0)
                    {
                        Console.Write("\n");
                    }
                    Console.Write(CipherLib.Annealing.ALPHABET[i / 72] + "| ");
                }
                //Console.Write(i.ToString());
                Console.Write(key[i].ToString());
            }
        }

        static string NewRandomKey()
        {
            string key = "";
            for (int i = 0; i < 676; i++)
            {
                key += CipherLib.Annealing.ALPHABET[i / 26].ToString() + CipherLib.Annealing.ALPHABET[i % 26].ToString();
            }
            for (int i = 0; i < 1000; i++)
            {
                key = MessAroundWithKey(key);
            }
            return key;
        }
    }
}
