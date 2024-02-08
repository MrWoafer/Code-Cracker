using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpInterruptedKey
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

            //Console.Write("-- C# Interrupted Solver --");
            Console.Write("-- C# Interrupted Key Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--InterruptedKeyMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
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
            //int keyLength = Enumerable.Max(ciphertext.Split().Select(x => x.Length));
            int maxKeyLength = Enumerable.Max(ciphertext.Split().Select(x => x.Length));

            Console.Write("Alphabet: " + alphabet + "\n\n");
            //Console.Write("Key Length: " + keyLength.ToString() + "\n\n");
            Console.Write("Longest Word Length: " + maxKeyLength.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");

            Tuple<string, string> result = new Tuple<string, string>("", "");

            float newScore;
            float bestScore = float.MinValue;

            for (int keyLength = 1; keyLength <= maxKeyLength; keyLength++)
            {
                result = CipherLib.InterruptedKey.CrackReturnKey(ciphertext, keyLength, alphabet);

                newScore = CipherLib.Annealing.QuadgramScore(result.Item1);

                if (newScore > bestScore)
                {
                    bestScore = newScore;

                    Console.Write("New best key:");
                    Console.Write("\n\n");
                    Console.Write(result.Item2.ToUpper());
                    Console.Write("\n\n");
                    Console.Write(result.Item1);
                    Console.Write("\n\n-----------------------\n\n");
                }
            }  

            //Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
