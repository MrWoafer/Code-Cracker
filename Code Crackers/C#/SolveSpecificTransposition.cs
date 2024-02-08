using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpTranspo
{
    class Program
    {
        const int trialNum = 1000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Transposition Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--TranspoMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");
            
            int columnNum;
            CipherLib.TranspositionType transpoType;
            if (args.Length > 0)
            {
                columnNum = Int32.Parse(args[0]);
                transpoType = (CipherLib.TranspositionType)Int32.Parse(args[1]);
            }
            else
            {
                columnNum = 6;
                transpoType = CipherLib.TranspositionType.column;
            }

            //Console.Write("\n\nNum Of Columns: " + columnNum);
            Console.Write("Num Of Columns: " + columnNum);
            Console.Write("\n\nTransposition Type: ");
            if (transpoType == CipherLib.TranspositionType.column)
            {
                Console.Write("Column");
            }
            else if (transpoType == CipherLib.TranspositionType.row)
            {
                Console.Write("Row");
            }
            else if (transpoType == CipherLib.TranspositionType.myszkowski)
            {
                Console.Write("Myszkowski");
            }
            /*else if (transpoType == CipherLib.TranspositionType.amsco)
            {
                Console.Write("AMSCO");
            }*/
            else
            {
                CipherLib.Utils.ClearLine();
                Console.Write("ERROR: Unknown transposition type.");
                if (transpoType == CipherLib.TranspositionType.amsco)
                {
                    Console.Write("\n\nWARNING: Do not use this program to solve AMSCO ciphers.");
                }
            }
            //Console.Write("\n\n-----------------------\n\n");
            Console.Write("\n\n");

            //Console.Write(CipherLib.Annealing.IncompleteColumnarTranspo(ciphertext, new int[] { 3, 2, 4, 5, 1, 6 }));
            //Console.Write(CipherLib.Annealing.IncompleteColumnarTranspo(ciphertext, new int[] { 2, 1, 3, 4, 0, 5 }));
            /*Console.Write(CipherLib.Annealing.IncompleteColumnarTranspo(ciphertext, CipherLib.Annealing.InvertKey(new int[] { 2, 1, 3, 4, 0, 5 })));
            Console.Write("\n\n");*/

            Tuple<string, int[]> result = new Tuple<string, int[]>("", new int[columnNum]);

            int[] bestKey = new int[columnNum];

            float currentScore = float.MinValue;
            float bestScore = currentScore;

            string plaintext;

            int trial = 0;
            while (true)
            {

                Console.Write("-----------------------\n\n");
                Console.Write("Trial: " + (trial + 1).ToString() + "\n\n");

                result = Crack(ciphertext, columnNum, transpoType, trialNum);

                plaintext = result.Item1;

                currentScore = CipherLib.Annealing.QuadgramScore(plaintext);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;

                    CipherLib.Utils.CopyArray(result.Item2, bestKey);

                    Console.Write("New best key:\n\n");
                    CipherLib.Utils.DisplayArray(bestKey, true);
                    //Console.Write("\n Or inverted:\n");
                    //Console.Write("\nOr its inverse:\n");
                    Console.Write("\n\nOr its inverse:\n\n");
                    CipherLib.Utils.DisplayArray(CipherLib.Annealing.InvertKey(bestKey), true);
                    Console.Write("\n\n");
                    Console.Write("Score: " + bestScore + "\n\n");
                    //Console.Write("Decipherment: " + plaintext);
                    Console.Write(plaintext);
                    Console.Write("\n\n");
                }
                else
                {
                    Console.Write("Didn't find a better key...");
                    Console.Write("\n\n");
                }

                trial++;
            }

            Console.Write("\n--------------------------------------\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        public static Tuple<string, int[]> Crack(string ciphertext, int columnNum, CipherLib.TranspositionType transpoType, int trialNum)
        {
            if (transpoType == CipherLib.TranspositionType.row)
            {
                return CipherLib.Annealing.CrackRowTranspoReturnKey(ciphertext, columnNum, trialNum);
            }
            else if (transpoType == CipherLib.TranspositionType.column)
            {
                return CipherLib.Annealing.CrackColumnTranspoReturnKey(ciphertext, columnNum, trialNum);
            }
            else if (transpoType == CipherLib.TranspositionType.myszkowski)
            {
                return CipherLib.Annealing.CrackMyszkowskiTranspoReturnKey(ciphertext, columnNum, trialNum);
            }
            else
            {
                return null;
            }
        }
    }
}
