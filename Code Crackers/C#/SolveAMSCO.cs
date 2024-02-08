using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpAMSCO
{
    class Program
    {
        //private const int trialNum = 1000;
        private const int trialNum = 10000;

        private const int refinedTrialNum = 10000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# AMSCO Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--AMSCOMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int columnNum;
            int maxChunkSize;
            if (args.Length > 0)
            {
                columnNum = Int32.Parse(args[0]);
                maxChunkSize = Int32.Parse(args[1]);
            }
            else
            {
                columnNum = 6;
                maxChunkSize = 2;
            }

            //Console.Write("\n\n");
            //Console.Write(CipherLib.Annealing.DecodeAMSCO(ciphertext, 2, new int[] { 4, 5, 0, 2, 1, 3 }, new int[] { 1, 2 }));
            //Console.Write("\n\n");
            //Console.Write(CipherLib.Annealing.DecodeAMSCO(ciphertext, 2, new int[] { 2, 4, 3, 5, 0, 1 }, new int[] { 1, 2 }));
            //Console.Write(CipherLib.Annealing.DecodeAMSCO(ciphertext, 3, new int[] { 4, 5, 0, 2, 1, 3 }, new int[] { 1, 2, 3 }));
            //Console.Write(CipherLib.Annealing.DecodeAMSCO(ciphertext, 2, new int[] { 4, 5, 0, 2, 1, 3 }, new int[] { 2, 1 }));
            //Console.Write(CipherLib.Annealing.DecodeAMSCO(ciphertext, 2, new int[] { 2, 4, 3, 5, 0, 1 }, new int[] { 2, 1 }));
            //Console.Write("\n\n");
            //Console.Write(CipherLib.Annealing.QuadgramScore(CipherLib.Annealing.DecodeMyszkowskiTranspo(ciphertext, new int[] { 0, 3, 0, 0, 2, 1, 1, 4 })));
            //Console.Write(CipherLib.Annealing.DecodeAMSCO(ciphertext, 2, new int[] { 4, 5, 0, 2, 1, 3 }, new int[] { 1, 2 }));
            //Console.Write("\n\n");
            //Console.Write(CipherLib.Annealing.QuadgramScore(CipherLib.Annealing.DecodeAMSCO(ciphertext, 2, new int[] { 4, 5, 0, 2, 1, 3 }, new int[] { 1, 2 })));
            //Console.Write("\n\n");


            float currentScore = float.MinValue;
            float bestScore = currentScore;

            int[] bestKey = new int[columnNum];
            int[] bestPattern = new int[maxChunkSize];

            Tuple<string, int[]> result = new Tuple<string, int[]>("", new int[columnNum]);
            string plaintext;

            int[][] perms = CipherLib.Annealing.Permutations(maxChunkSize);
            int displayPeriod = CipherLib.Annealing.Factorial(maxChunkSize - 1);

            for (int i = 0; i < perms.Length; i++)
            {
                if ((i + 1) % displayPeriod == 0 || i == 0)
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bSearched " + (i + 1).ToString() + " / " + perms.Length + " patterns...");
                }

                for (int j = 0; j < maxChunkSize; j++)
                {
                    perms[i][j]++;
                }

                result = CipherLib.Annealing.CrackAMSCOReturnKey(ciphertext, columnNum, perms[i], trialNum);

                plaintext = result.Item1;

                currentScore = CipherLib.Annealing.QuadgramScore(plaintext);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;

                    Array.Copy(result.Item2, bestKey, columnNum);
                    Array.Copy(perms[i], bestPattern, maxChunkSize);

                    Console.Write("\n\n");
                    Console.Write("New best key:\n\n");
                    Console.Write("Permutation:\n\n");
                    DisplayKey(bestKey);
                    Console.Write("\n\n");
                    Console.Write("Pattern:\n\n");
                    DisplayKey(bestPattern);
                    Console.Write("\n\n");
                    Console.Write("Score: " + bestScore + "\n\n");
                    Console.Write("Decipherment: " + plaintext);
                    Console.Write("\n\n");
                    Console.Write("-----------------------\n\n");
                }
            }

            Console.Write("\n\n-----------------------\n\n");
            //Console.Write("Program fnished.");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
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
    }
}
