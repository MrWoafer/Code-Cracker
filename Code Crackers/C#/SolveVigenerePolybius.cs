using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpVigenerePolybius
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

            Console.Write("-- C# Polybius + Vigenère Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--PolybiusVigenMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int period;
            string alphabet;
            int ngramLength;
            //bool exactMatch;
            if (args.Length > 0)
            {
                period = Int32.Parse(args[0]);
                alphabet = args[1];
                ngramLength = Int32.Parse(args[2]);
                //exactMatch = bool.Parse(args[3]);
            }
            else
            {
                period = 7;
                //period = 8;
                alphabet = "abcdefghijklmnopqrstuvwxyz";
                ngramLength = 2;
                //ngramLength = 3;
                //exactMatch = true;
            }

            Console.Write("Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("Period: " + period);
            if (period == -1)
            {
                Console.Write(" (will search all from 1 to 10)");
            }
            Console.Write("\n\n-----------------------\n\n");

            /*Tuple<string, string> result = new Tuple<string, string>("", "");

            result = CipherLib.PolybiusVigenere.CrackPolybiusGrid(ciphertext, 2, "lunatic", alphabet);
            Console.Write(result.Item2);
            Console.Write("\n\n");
            Console.Write(result.Item1);*/

            Tuple<string, string, string> result = new Tuple<string, string, string>("", "", "");

            //result = CipherLib.PolybiusVigenere.CrackReturnKey(ciphertext, ngramLength, period, alphabet);
            //result = CipherLib.PolybiusVigenere.CrackReturnKey(ciphertext, ngramLength, period, alphabet, 10000);
            //result = CipherLib.PolybiusVigenere.CrackReturnKey(ciphertext, ngramLength, period, alphabet, exactMatch);
            for (int numAllowedIncorrectChars = 0; numAllowedIncorrectChars < 5; numAllowedIncorrectChars++)
            {
                Console.Write("Num Of Allowed Incorrect Characters: " + numAllowedIncorrectChars.ToString());
                Console.Write("\n\n");

                result = CipherLib.PolybiusVigenere.CrackReturnKey(ciphertext, ngramLength, period, alphabet, numAllowedIncorrectChars);

                if (result != null)
                {
                    Console.Write(result.Item2);
                    Console.Write("\n\n");
                    Console.Write(result.Item3);
                    Console.Write("\n\n");
                    Console.Write(result.Item1);
                }
                else
                {
                    Console.Write("Nothing for " + numAllowedIncorrectChars.ToString() + " allowed incorrect chars.");
                }
                Console.Write("\n\n-----------------------\n\n");
            }

            //Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
