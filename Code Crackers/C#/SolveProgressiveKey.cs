using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpProgressiveKey
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

            //Console.Write("-- C# ProgressiveKey Solver --");
            Console.Write("-- C# Progressive Key Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--ProgressiveKeyMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int period;
            int progressionIndex;
            string alphabet;
            if (args.Length > 0)
            {
                period = Int32.Parse(args[0]);
                progressionIndex = Int32.Parse(args[1]);
                alphabet = args[2];
            }
            else
            {
                period = 7;
                //progressionIndex = 4;
                progressionIndex = -1;
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }

            Console.Write("Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("Period: " + period);
            Console.Write("\n\n");
            Console.Write("Progression Index: " + progressionIndex);
            if (progressionIndex == -1)
            {
                Console.Write(" (will search all from 0 to the length of the alphabet)");
            }
            Console.Write("\n\n-----------------------\n\n");

            //Console.Write(CipherLib.ProgressiveKey.Decrypt(ciphertext, "amazing", 4, alphabet));
            //Console.Write("\n\n");

            Tuple<string, string> result = new Tuple<string, string>("", "");

            if (progressionIndex != -1)
            {
                result = CipherLib.ProgressiveKey.CrackReturnKey(ciphertext, period, progressionIndex, alphabet);

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

                int bestProgressionIndex = 0;

                for (progressionIndex = 0; progressionIndex < alphabet.Length; progressionIndex++)
                {
                    result = CipherLib.ProgressiveKey.CrackReturnKey(ciphertext, period, progressionIndex, alphabet);

                    newScore = CipherLib.Annealing.QuadgramScore(result.Item1);

                    if (progressionIndex == 0 || newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestKey = result.Item2;
                        bestProgressionIndex = progressionIndex;
                    }
                }

                Console.Write("Best key:");
                Console.Write("\n\n");
                Console.Write(bestKey.ToUpper());
                Console.Write("\n\n");
                Console.Write("Best progression index:");
                Console.Write("\n\n");
                Console.Write(bestProgressionIndex);
                Console.Write("\n\n");
                Console.Write(CipherLib.ProgressiveKey.Decrypt(ciphertext, bestKey, bestProgressionIndex, alphabet));
            }

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
