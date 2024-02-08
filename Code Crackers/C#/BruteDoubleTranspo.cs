using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBruteDoubleTranspo
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

            Console.Write("-- C# Double Transposition Bruter --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--BruteDoubleTranspoMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");

            int trial = 0;

            int[] bestKey1;
            int key2Length;
            bool row1;
            bool row2;

            if (args.Length > 0)
            {
                bestKey1 = new int[Int32.Parse(args[0])];
                key2Length = Int32.Parse(args[1]);

                row1 = Boolean.Parse(args[2]);
                row2 = Boolean.Parse(args[3]);
            }
            else
            {
                bestKey1 = new int[7];
                key2Length = 5;

                row1 = false;
                row2 = false;
            }

            for (int i = 0; i < bestKey1.Length; i++)
            {
                bestKey1[i] = i;
            }

            float currentScore = Score(msg, bestKey1, key2Length, row1, row2);
            float bestScore = currentScore;

            Console.Write("Starting with key: ");
            DisplayKey(bestKey1);
            Console.Write("\n\n");
            Console.Write("Starting with score: " + bestScore.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");
            Console.Write("Searching " + CipherLib.Annealing.Factorial(bestKey1.Length).ToString() + " keys...\n\n");
            
            string decipherment;

            int[][] perms = CipherLib.Annealing.Permutations(bestKey1.Length);

            Tuple<string, int[]> result = new Tuple<string, int[]>("", new int[1]);

            for (int i = 0; i < perms.Length; i++)
            {
                //if (i % 10 == 0)
                if ((i + 1) % 10 == 0 || i == 0)
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bKeys Searched: " + (i + 1) + " / " + perms.Length.ToString());
                }              

                currentScore = Score(msg, perms[i], key2Length, row1, row2);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;

                    Array.Copy(perms[i], bestKey1, bestKey1.Length);

                    result = DecodeDoubleTranspo(msg, bestKey1, key2Length, row1, row2);
                    decipherment = result.Item1;

                    Console.Write("\n\n\b\b\b\b\b\b\b\b\b");
                    Console.Write("New best key:\n\n");
                    Console.Write("Score: " + bestScore + "\n");
                    Console.Write("\n");
                    Console.Write("Key:\n");
                    DisplayKey(result.Item2);
                    Console.Write("\n");
                    DisplayKey(bestKey1);
                    Console.Write("\n\n");
                    //decipherment = DecodeDoubleTranspo(msg, bestKey1, key2Length, row1, row2);
                    Console.Write("Decipherment: " + decipherment);
                    Console.Write("\n\n");
                }

                while (Console.KeyAvailable)
                {
                    ProducePlaintext(msg, bestKey1, key2Length, row1, row2);
                }
            }


            Console.Write("\n\n-----------------------\n\nPress ENTER to close...");
            Console.ReadLine();
        }

        static float Score(string msg, int[] key1, int key2Length, bool row1, bool row2)
        {
            string decodedMsg = "";
            float score = 0;

            if (true)
            {
                //decodedMsg = DecodeDoubleTranspo(msg, key1, key2Length, row1, row2);
                decodedMsg = DecodeDoubleTranspo(msg, key1, key2Length, row1, row2).Item1;

                score = CipherLib.Annealing.QuadgramScore(decodedMsg);
            }

            return score;
        }

        //static string DecodeDoubleTranspo(string msg, int[] key1, int key2Length, bool row1, bool row2, int trialNum = 1000)
        static Tuple<string, int[]> DecodeDoubleTranspo(string msg, int[] key1, int key2Length, bool row1, bool row2, int trialNum = 1000)
        {
            string intermediateMsg;

            if (row1 == true)
            {
                intermediateMsg = CipherLib.Annealing.DecodeRowTranspo(msg, key1);
            }
            else
            {
                //intermediateMsg = CipherLib.Annealing.DecodeColumnTranspo(msg, key1);
                intermediateMsg = CipherLib.Annealing.IncompleteColumnarTranspo(msg, key1);
            }

            if (row2 == true)
            {
                //return CipherLib.Annealing.CrackRowTranspo(intermediateMsg, key2Length, trialNum);
                return CipherLib.Annealing.CrackRowTranspoReturnKey(intermediateMsg, key2Length, trialNum);
            }
            else
            {
                //return CipherLib.Annealing.CrackColumnTranspo(intermediateMsg, key2Length, trialNum);
                return CipherLib.Annealing.CrackColumnTranspoReturnKey(intermediateMsg, key2Length, trialNum);
            }
        }

        static void DisplayKey(int[] key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                Console.Write(key[i]);

                if (i < key.Length - 1)
                {
                    Console.Write(", ");
                }
            }
        }

        static void ProducePlaintext(string msg, int[] key1, int key2Length, bool row1, bool row2)
        {
            Console.Write("\n\n---------------\n");
            Console.Write("Refined output from best key so far:\n\n");
            //string decodedMsg = DecodeDoubleTranspo(msg, key1, key2Length, row1, row2, 10000);
            string decodedMsg = DecodeDoubleTranspo(msg, key1, key2Length, row1, row2, 10000).Item1;
            Console.Write(decodedMsg);
            //Console.Write("\n");
            Console.Write("\n");
            Console.ReadLine();
            Console.Write("---------------\n\n");
        }
    }
}
