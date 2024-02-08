using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpMyszkowski
{
    class Program
    {
        private const int trialNum = 1000;
        //private const int trialNum = 10000;

        private const int refinedTrialNum = 10000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Myszkowski Transposition Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--MyszkowskiMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int columnNum;
            if (args.Length > 0)
            {
                columnNum = Int32.Parse(args[0]);
            }
            else
            {
                columnNum = 8;
            }

            /*Console.Write("\n\n");
            Console.Write(CipherLib.Annealing.DecodeMyszkowskiTranspo(ciphertext, new int[] { 0, 3, 0, 0, 2, 1, 1, 4 }));
            Console.Write("\n\n");*/
            //Console.Write(CipherLib.Annealing.QuadgramScore(CipherLib.Annealing.DecodeMyszkowskiTranspo(ciphertext, new int[] { 0, 3, 0, 0, 2, 1, 1, 4 })));

            float currentScore = float.MinValue;
            float bestScore = currentScore;

            int[] bestKey = new int[columnNum];

            Tuple<string, int[]> result = new Tuple<string, int[]>("", new int[columnNum]);
            string plaintext;

            int trial = 0;
            while (true)
            {

                Console.Write("-----------------------\n\n");
                Console.Write("Trial: " + (trial + 1).ToString() + "\n\n");

                result = CipherLib.Annealing.CrackMyszkowskiTranspoReturnKey(ciphertext, columnNum, trialNum);

                plaintext = result.Item1;

                currentScore = CipherLib.Annealing.QuadgramScore(plaintext);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;

                    Array.Copy(result.Item2, bestKey, columnNum);

                    Console.Write("New best key:\n\n");
                    DisplayKey(bestKey);
                    Console.Write("\n\n");
                    Console.Write("Score: " + bestScore + "\n\n");
                    Console.Write("Decipherment: " + plaintext);
                    Console.Write("\n\n");
                }
                else
                {
                    Console.Write("Didn't find a better key...");
                    Console.Write("\n\n");
                }

                trial++;
            }


            Console.Write("\n\n-----------------------\n\nPress ENTER to close...");
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
