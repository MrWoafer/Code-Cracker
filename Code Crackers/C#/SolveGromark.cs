using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpGromark
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

            Console.Write("-- C# Gromark Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--GromarkMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int primerLength;
            string alphabet;
            if (args.Length > 0)
            {
                primerLength = Int32.Parse(args[0]);
                alphabet = args[1];
            }
            else
            {
                primerLength = 6;
                alphabet = "abcdefghijklmnopqrstuvwxyz";
            }

            Console.Write("Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("Primer Length: " + primerLength);
            if (primerLength == -1)
            {
                Console.Write(" (will search all from 0 to 7)");
            }
            Console.Write("\n\n-----------------------\n\n");


            //Console.Write(CipherLib.Gromark.Decrypt(ciphertext, "amxbqzcpydrgftieslkwnhuojv", alphabet, "542345"));

            /*Console.Write(CipherLib.Gromark.CrackKeyReturnKey(ciphertext, alphabet, "542345").Item1);
            Console.Write("\n\n");
            Console.Write(alphabet);
            Console.Write("\n");
            Console.Write(CipherLib.Gromark.CrackKeyReturnKey(ciphertext, alphabet, "542345").Item2);*/

            Tuple<string, string, string> result = new Tuple<string, string, string>("", "", "");

            //result = CipherLib.Gromark.CrackReturnKey(ciphertext, alphabet, primerLength);
            //result = CipherLib.Gromark.CrackReturnKey(ciphertext, alphabet, primerLength, 10000);
            result = CipherLib.Gromark.CrackReturnKey(ciphertext, alphabet, primerLength, true);

            Console.Write("\n\n");
            Console.Write(result.Item1);
            Console.Write("\n\n");
            Console.Write(alphabet);
            Console.Write("\n");
            Console.Write(result.Item2);
            Console.Write("\n\n");
            Console.Write(result.Item3);

            /*Tuple<string, string> result = new Tuple<string, string>("", "");

            string bestKey = "";
            string bestPrimer = "";

            float newScore = float.MinValue;
            float bestScore = newScore;

            for (int primer = (int)Math.Pow(10, primerLength - 1); primer < (int)Math.Pow(10, primerLength); primer++)
            {
                if (primer % 100 == 0)
                {
                    CipherLib.Utils.ClearLine();
                    Console.Write("Search is now up to " + primer.ToString() + "...");
                }

                //result = CipherLib.Gromark.CrackKeyReturnKey(ciphertext, alphabet, primer.ToString());
                result = CipherLib.Gromark.CrackKeyReturnKey(ciphertext, alphabet, primer.ToString(), 100);

                newScore = CipherLib.Annealing.QuadgramScore(result.Item1);

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestKey = result.Item2;
                    bestPrimer = primer.ToString();

                    Console.Write("\n\n");
                    Console.Write(bestPrimer);
                    Console.Write("\n\n");
                    Console.Write(alphabet);
                    Console.Write("\n");
                    Console.Write(bestKey);
                    Console.Write("\n\n");
                    Console.Write(CipherLib.Gromark.Decrypt(ciphertext, bestKey, alphabet, bestPrimer));
                    Console.Write("\n\n-----------------------\n\n");
                }
            }*/

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
