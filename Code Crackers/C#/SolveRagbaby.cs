using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRagbaby
{
    class Program
    {
        const int numOfTrials = 10000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");
            
            Console.Write("-- C# Ragbaby Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--RagbabyMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");
            
            int startIndex;
            string alphabet;
            if (args.Length > 0)
            {
                startIndex = Int32.Parse(args[0]);
                alphabet = args[1];
            }
            else
            {
                //startIndex = 1;
                startIndex = -1;
                alphabet = "abcdefghiklmnopqrstuvwyz";
            }

            Console.Write("Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("Start Index: " + startIndex);
            if (startIndex == -1)
            {
                Console.Write(" (will search all from 0 to the length of the alphabet)");
            }
            Console.Write("\n\n-----------------------\n\n");

            //Console.Write(CipherLib.Ragbaby.Decrypt(ciphertext, "APHRODITEBCFGKLMNQSUVWYZ".ToLower(), 1));
            //Console.Write("\n\n");

            Tuple<string, string> result = new Tuple<string, string>("", "");

            if (startIndex != -1)
            {
                result = CipherLib.Ragbaby.CrackReturnKey(ciphertext, alphabet, startIndex, numOfTrials);

                Console.Write("Best key:");
                Console.Write("\n\n");
                Console.Write(result.Item2.ToUpper());
                Console.Write("\n\n");
                Console.Write(result.Item1);
            }
            else
            {
                string bestKey = "";
                float newScore = float.MinValue;
                float bestScore = newScore;

                int bestStartIndex = 0;

                for (startIndex = 0; startIndex < alphabet.Length; startIndex++)
                {
                    result = CipherLib.Ragbaby.CrackReturnKey(ciphertext, alphabet, startIndex, numOfTrials);

                    newScore = CipherLib.Annealing.QuadgramScore(result.Item1);

                    if (startIndex == 0 || newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestKey = result.Item2;
                        bestStartIndex = startIndex;

                        Console.Write("New best key:");
                        Console.Write("\n\n");
                        Console.Write(bestKey.ToUpper());
                        Console.Write("\n\n");
                        Console.Write("New best progression index:");
                        Console.Write("\n\n");
                        Console.Write(bestStartIndex);
                        Console.Write("\n\n");
                        Console.Write(CipherLib.Ragbaby.Decrypt(ciphertext, bestKey, bestStartIndex));
                        Console.Write("\n\n-----------------------\n\n");
                    }
                }
            }

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
