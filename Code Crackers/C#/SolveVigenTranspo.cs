using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpSolveVigenTranspo
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

            Console.Write("-- C# Vigen-Transpo (Vigenère + Transposition) Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--VigenTranspoMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            CipherLib.TranspositionType transpoType;
            int columnNum;
            if (args.Length > 0)
            {
                alphabet = args[0];
                transpoType = (CipherLib.TranspositionType)Int32.Parse(args[1]);
                columnNum = Int32.Parse(args[2]);
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
                //transpoType = CipherLib.TranspositionType.column;
                transpoType = CipherLib.TranspositionType.row;
                //columnNum = 6;
                columnNum = 7;
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
            Console.Write("\n\nNumber Of Columns: ");
            Console.Write(columnNum); Console.Write("\n\n-----------------------\n\n");

            int[] currentKey = new int[columnNum];

            for (int i = 0; i < columnNum; i++)
            {
                currentKey[i] = i;
            }
            for (int i = 0; i < 100; i++)
            {
                currentKey = CipherLib.Annealing.MessWithIntKey(currentKey);
            }

            int[] bestTranspoKey = new int[columnNum];
            Array.Copy(currentKey, bestTranspoKey, columnNum);
            int[] overallBestTranspoKey = new int[columnNum];
            Array.Copy(currentKey, overallBestTranspoKey, columnNum);

            float currentScore = float.MinValue;
            float bestScore = currentScore;

            float overallBestScore = bestScore;

            string decodedMsg = "";

            //int numOfTrials = 1000;
            //int numOfTrials = 100;
            int numOfTrials = 10000;

            Tuple<string, string> result = new Tuple<string, string>("", "");

            //int[][] perms = CipherLib.Annealing.Permutations(columnNum);

            int displayPeriod = CipherLib.Annealing.Factorial(columnNum - 1);

            bool foundVigenere = false;

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                //if (trial % 100 == 0)
                if (trial % 1000 == 0)
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bTrial: " + trial + " / " + numOfTrials + "...");
                }

                Array.Copy(CipherLib.Annealing.BlockMessWithIntKey(bestTranspoKey), currentKey, columnNum);

                result = DecodeVigenTranspo(ciphertext, currentKey, transpoType);

                decodedMsg = result.Item1;

                if (result.Item2 != "")
                {
                    foundVigenere = true;

                    currentScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                    if (currentScore > overallBestScore)
                    {
                        overallBestScore = currentScore;
                        Array.Copy(currentKey, overallBestTranspoKey, columnNum);
                    }

                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestTranspoKey, columnNum);
                    }
                    else if (currentScore < bestScore)
                    {
                        bool ReplaceAnyway;
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, numOfTrials - trial);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            Array.Copy(currentKey, bestTranspoKey, columnNum);
                        }
                        else
                        {
                            Array.Copy(bestTranspoKey, currentKey, columnNum);
                        }
                    }
                    else
                    {
                        Array.Copy(bestTranspoKey, currentKey, columnNum);
                    }
                }
                else if (!foundVigenere)
                {
                    Array.Copy(currentKey, bestTranspoKey, columnNum);
                }
            }

            Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bTrial: " + numOfTrials + " / " + numOfTrials + "...");

            //Tuple<string, string> decipherment = DecodeVigenTranspo(ciphertext, bestTranspoKey, transpoType);
            Tuple<string, string> decipherment = DecodeVigenTranspo(ciphertext, overallBestTranspoKey, transpoType);

            Console.Write("\n\n");
            Console.Write("Best Vigenère key:\n\n");
            Console.Write(decipherment.Item2.ToUpper());
            Console.Write("\n\n");
            Console.Write("Best transposition key:\n\n");
            //CipherLib.Utils.DisplayArray(bestTranspoKey);
            CipherLib.Utils.DisplayArray(overallBestTranspoKey);
            Console.Write("\n\n");
            Console.Write("Best decryption:\n\n");
            Console.Write(decipherment.Item1);
            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        public static Tuple<string, string> DecodeVigenTranspo(string ciphertext, int[] transpoKey, CipherLib.TranspositionType transpoType)
        {
            string plaintext = "";

            if (transpoType == CipherLib.TranspositionType.column)
            {
                //plaintext = CipherLib.Annealing.DecodeColumnTranspo(ciphertext, transpoKey);
                plaintext = CipherLib.Annealing.IncompleteColumnarTranspo(ciphertext, transpoKey);
            }
            else if (transpoType == CipherLib.TranspositionType.row)
            {
                plaintext = CipherLib.Annealing.DecodeRowTranspo(ciphertext, transpoKey);
            }

            Tuple<string, string> result = CipherLib.Annealing.CrackVigenereUnknownPeriodReturnKey(plaintext);

            //return new Tuple<string, string> (result.Item1, result.Item2);
            return result;
        }
    }
}
