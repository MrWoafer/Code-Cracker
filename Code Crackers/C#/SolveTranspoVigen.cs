using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTranspoVigen
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

            Console.Write("-- C# Transpo-Vigen (Transposition + Vigenère) Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--TranspoVigenMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            CipherLib.TranspositionType transpoType;
            int columnNum;
            int period;
            if (args.Length > 0)
            {
                alphabet = args[0];
                transpoType = (CipherLib.TranspositionType)Int32.Parse(args[1]);
                columnNum = Int32.Parse(args[2]);
                period = Int32.Parse(args[3]);
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
                transpoType = CipherLib.TranspositionType.column;
                columnNum = 4;
                period = 6;
            }

            Console.Write("Using Alphabet: " + alphabet);
            Console.Write("\n\nTransposition Type: ");
            if (transpoType == CipherLib.TranspositionType.column)
            {
                Console.Write("Column");
            }
            else
            {
                Console.Write("Row");
            }
            //Console.Write("\n\nNumber of columns: ");
            Console.Write("\n\nNumber Of Columns: ");
            Console.Write(columnNum);
            //Console.Write("\n\nPeriod: ");
            Console.Write("\n\nVigenère Period: ");
            Console.Write(period);
            Console.Write("\n\n-----------------------\n\n");

            float bestScore = Int32.MinValue;
            string bestDecryption = "";

            string bestVigenereKey = "";
            int[] bestTranspoKey = new int[columnNum];

            float newScore;
            int[] newTranspoKey = new int[columnNum];
            string newVigenereKey = "";

            int countdown = 10;
            //int countdown = 1;

            Tuple<string, int[]> result = new Tuple<string, int[]>("", new int[columnNum]);
            Tuple<string, string> result2 = new Tuple<string, string>("", "");

            for (int i = 0; i < countdown; i++)
            {
                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" + (countdown - i).ToString() + new String(' ', countdown.ToString().Length - (countdown - i).ToString().Length));

                //string initialKey = CipherLib.Annealing.OrderedFrequencyKey(ciphertext);

                result2 = CipherLib.Annealing.CrackVigenereReturnKey(ciphertext, period);

                string decryption = result2.Item1;

                newVigenereKey = result2.Item2;

                if (transpoType == CipherLib.TranspositionType.row)
                {
                    result = CipherLib.Annealing.CrackRowTranspoReturnKey(decryption, columnNum, 1000);
                }
                else if (transpoType == CipherLib.TranspositionType.column)
                {
                    result = CipherLib.Annealing.CrackColumnTranspoReturnKey(decryption, columnNum, 1000);
                }

                decryption = result.Item1;
                Array.Copy(result.Item2, newTranspoKey, columnNum);

                newScore = CipherLib.Annealing.QuadgramScore(decryption);

                if (i == 0 || newScore > bestScore)
                {
                    bestScore = newScore;
                    bestDecryption = decryption;

                    Array.Copy(newTranspoKey, bestTranspoKey, columnNum);
                    bestVigenereKey = newVigenereKey;
                }
            }

            Console.Write("\b\b\b\b\b\b\b\b\b\b\b");
            Console.Write("Best Vigenère key:\n\n");
            Console.Write(bestVigenereKey.ToUpper());
            Console.Write("\n\n");
            Console.Write("Best transposition key:\n\n");
            CipherLib.Utils.DisplayArray(bestTranspoKey);
            Console.Write("\n\n");
            Console.Write("Best decryption:\n\n");
            Console.Write(bestDecryption);
            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
