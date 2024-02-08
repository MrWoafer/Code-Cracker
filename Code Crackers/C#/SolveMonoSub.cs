using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpMonoSub
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
            
            Console.Write("-- C# Mono-Sub Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--MonoSubMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            int numOfTrials;
            if (args.Length > 0)
            {
                alphabet = args[0];
                numOfTrials = Int32.Parse(args[1]);
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
                numOfTrials = 1000;
            }

            Console.Write("Using Alphabet:\n" + alphabet);
            Console.Write("\n\nNum Of Trials: " + numOfTrials);
            Console.Write("\n\n-----------------------\n\n");

            float newScore = float.MinValue;
            float bestScore = newScore;

            string bestKey = "";

            string plaintext = "";

            //Tuple<string, string> result = new Tuple<string, string>("", "");

            for (int i = 0; i < 10; i++)
            {
                CipherLib.Utils.ClearLine();
                //Console.Write(i);
                Console.Write((10 - i).ToString());
                Console.Write(" ");

                plaintext = CipherLib.Annealing.CrackCustomMonoSub(ciphertext, alphabet, numOfTrials);

                newScore = CipherLib.Annealing.QuadgramScore(plaintext);

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestKey = CipherLib.Annealing.GetKeyFromPlaintext(plaintext, ciphertext, alphabet);
                }
            }

            CipherLib.Utils.ClearLine();
            Console.Write("Best key:");
            Console.Write("\n\n");
            Console.Write("CIP: " + alphabet);
            Console.Write("\n");
            Console.Write("PLA: " + bestKey);
            Console.Write("\n\n");
            Console.Write("Or");
            Console.Write("\n\n");
            Console.Write("PLA: " + alphabet);
            Console.Write("\n");
            Console.Write("CIP: ");

            /// i is the index in the alphabet next letter we are trying to find
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < alphabet.Length; j++)
                {
                    if (bestKey[j] == alphabet[i])
                    {
                        Console.Write(alphabet[j]);
                        break;
                    }
                }
            }

            Console.Write("\n\n");
            Console.Write(CipherLib.Annealing.DecodeCustomMonoSub(ciphertext, bestKey, alphabet));

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
