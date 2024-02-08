using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPlayfair
{
    class Program
    {
        //private const int trialNum = 1000;
        //private const int trialNum = 10000;
        private const int trialNum = 100000;

        private const int refinedTrialNum = 10000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Playfair Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--PlayfairMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            int width;
            int height;
            if (args.Length > 0)
            {
                alphabet = args[0];
                height = Int32.Parse(args[1]);
                width = Int32.Parse(args[2]);
            }
            else
            {
                //alphabet = "abcdefghiklmnopqrstuvwxyz";
                alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
                //height = 5;
                height = 4;
                //height = 9;
                //width = 5;
                width = 9;
                //width = 7;
                //width = 4;
            }

            Console.Write("Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("Key Height: " + height);
            Console.Write("\n\n");
            Console.Write("Key Width: " + width);
            Console.Write("\n\n");

            if (alphabet.Length != width * height)
            {
                Console.Write("ERROR: The provided grid size does not match that of the provided alphabet.");
            }
            else
            {
                char[][] sampleKey = new char[height][];
                int index = 0;
                for (int i = 0; i < height; i++)
                {
                    sampleKey[i] = new char[width];
                    for (int j = 0; j < width; j++)
                    {
                        sampleKey[i][j] = alphabet[index];
                        index++;
                    }
                }
                Console.Write("Sample Key:\n");
                Console.Write("\n");
                CipherLib.Playfair.DisplayKey(sampleKey);
                Console.Write("\n\n");

                /*char[][] testKey = new char[][] {
                new char[] { 'm', 'y', 'f', 'a', 'c' },
                new char[] { 'e', 'i', 's', 'o', 'l' },
                new char[] { 'b', 'd', 'g', 'h', 'k' },
                new char[] { 'n', 'p', 'q', 'r', 't' },
                new char[] { 'u', 'v', 'w', 'x', 'z' } };
                Console.Write("\n\n");
                Console.Write(CipherLib.Playfair.Decrypt(ciphertext, testKey));
                Console.Write("\n\n");
                Console.Write(CipherLib.Annealing.QuadgramScore(CipherLib.Playfair.Decrypt(ciphertext, testKey)));
                Console.Write("\n\n");*/

                float currentScore = float.MinValue;
                float bestScore = currentScore;

                char[][] bestKey = new char[height][];
                for (int i = 0; i < height; i++)
                {
                    bestKey[i] = new char[width];
                }

                Tuple<string, char[][]> result = new Tuple<string, char[][]>("", new char[height][]);
                string plaintext;

                int trial = 0;
                while (true)
                {

                    Console.Write("-----------------------\n\n");
                    Console.Write("Trial: " + (trial + 1).ToString() + "\n\n");

                    //result = CipherLib.Playfair.CrackPlayfairReturnKey(ciphertext, height, width, alphabet, trialNum);
                    result = CipherLib.Playfair.CrackPlayfairReturnKey(ciphertext, height, width, alphabet, true);

                    plaintext = result.Item1;

                    currentScore = CipherLib.Annealing.QuadgramScore(plaintext);

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;

                        CipherLib.Utils.Copy2DArray(result.Item2, bestKey);

                        //Console.Write("New best key:\n\n");
                        Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bNew best key:\n\n");
                        CipherLib.Playfair.DisplayKey(bestKey);
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
            }

            Console.Write("\n\n-----------------------\n\nPress ENTER to close...");
            Console.ReadLine();
        }
    }
}
