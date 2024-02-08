using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpNicodemus
{
    class Program
    {
        //const int numOfTrials = 1000;
        const int numOfTrials = 10000;
        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Nicodemus Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--NicodemusMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            int columnNum;
            //int period;
            int chunkSize;
            if (args.Length > 0)
            {
                alphabet = args[0];
                columnNum = Int32.Parse(args[1]);
                //period = Int32.Parse(args[2]);
                //chunkSize = Int32.Parse(args[3]);
                chunkSize = Int32.Parse(args[2]);
            }
            else
            {
                alphabet = "abcdefghijklmnopqrstuvwxyz";
                columnNum = 7;
                //period = 7;
                chunkSize = 5;
            }

            Console.Write("Using Alphabet: " + alphabet);
            Console.Write("\n\nNumber Of Columns: ");
            Console.Write(columnNum);
            Console.Write("\n\nVigenère Period: ");
            //Console.Write(period);
            Console.Write(columnNum);
            Console.Write(" (Must be same as number of columns)");
            Console.Write("\n\n-----------------------\n\n");

            Console.Write("Phase 1...");

            //Console.Write(CipherLib.Annealing.IncompleteColumnarTranspoReadOffInChunks(CipherLib.Annealing.DecodeVigenere(ciphertext, "hillary"), new int[] { 1, 2, 3, 4, 0, 5, 6 }, 5));
            //Console.Write(CipherLib.Annealing.DecryptNicodemus(ciphertext, "hillary", new int[] { 1, 2, 3, 4, 0, 5, 6 }, 5, alphabet));
            //Console.Write(CipherLib.Annealing.DecryptNicodemus(ciphertext, "ahillry", new int[] { 1, 2, 3, 4, 0, 5, 6 }, 5, alphabet));
            //Console.Write(CipherLib.Annealing.DecryptNicodemus(ciphertext, "ahillry", new int[] { 4, 0, 1, 2, 3, 5, 6 }, 5, alphabet));
            //Console.ReadLine();

            float bestScore = Int32.MinValue;

            string bestVigenereKey = "";
            
            int[] newTranspoKey = new int[columnNum];
            string newVigenereKey = new string('a', columnNum);

            for (int i = 0; i < columnNum; i++)
            {
                newTranspoKey[i] = i;
            }
            /*for (int i = 0; i < 100; i++)
            {
                newTranspoKey = CipherLib.Annealing.MessWithIntKey(newTranspoKey);
            }*/

            int[] bestTranspoKey = new int[columnNum];

            Array.Copy(newTranspoKey, bestTranspoKey, columnNum);

            float newScore = CipherLib.Annealing.QuadgramScore(CipherLib.Annealing.DecryptNicodemus(ciphertext, newVigenereKey, newTranspoKey, chunkSize, alphabet));

            string decodedMsg = "";

            int operation;

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                operation = CipherLib.Utils.rand.Next() % 10;
                //if (operation < 5)
                if (operation < 0)
                //if (operation < 9)
                //if (operation < 7)
                {
                    CipherLib.Utils.CopyArray(CipherLib.Annealing.BlockMessWithIntKey(newTranspoKey), newTranspoKey);
                }
                else
                {
                    newVigenereKey = CipherLib.Annealing.SetChar(alphabet[CipherLib.Utils.rand.Next() % alphabet.Length], CipherLib.Utils.rand.Next() % newVigenereKey.Length, newVigenereKey);
                }

                decodedMsg = CipherLib.Annealing.DecryptNicodemus(ciphertext, newVigenereKey, newTranspoKey, chunkSize, alphabet);

                //newScore = CipherLib.Annealing.QuadgramScore(decodedMsg);
                newScore = -CipherLib.Annealing.ChiSquared(decodedMsg);

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                    bestVigenereKey = newVigenereKey;
                }
                else if (newScore < bestScore)
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, 20f * (numOfTrials - trial) / numOfTrials);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = newScore;
                        CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                        bestVigenereKey = newVigenereKey;
                    }
                    else
                    {
                        CipherLib.Utils.CopyArray(bestTranspoKey, newTranspoKey);
                        newVigenereKey = bestVigenereKey;
                    }
                }
                else
                {
                    CipherLib.Utils.CopyArray(bestTranspoKey, newTranspoKey);
                    newVigenereKey = bestVigenereKey;
                }
            }

            //Console.Write("\b\b\b\b\b\b\b\b\b\b\b");
            Console.Write("\n\n");
            Console.Write("Best Vigenère key:\n\n");
            Console.Write(bestVigenereKey.ToUpper());
            Console.Write(" or rearranged as ");
            Console.Write(CipherLib.Annealing.DecodeRowTranspo(bestVigenereKey, bestTranspoKey).ToUpper());
            Console.Write("\n\n");
            Console.Write("Best transposition key:\n\n");
            CipherLib.Utils.DisplayArray(bestTranspoKey);
            //Console.Write(" or, generated from the best Vigenere key ");
            //Console.Write("\nOr generated from the best Vigenere key:\n");
            //CipherLib.Utils.DisplayArray(CipherLib.Annealing.IntKeyFromKeyWord(bestVigenereKey, alphabet));
            Console.Write("\n\n");
            Console.Write("Best decryption:\n\n");
            Console.Write(CipherLib.Annealing.DecryptNicodemus(ciphertext, bestVigenereKey, bestTranspoKey, chunkSize, alphabet));
            //Console.Write("\n\nor\n\n");
            //Console.Write(CipherLib.Annealing.DecryptNicodemus(ciphertext, bestVigenereKey, CipherLib.Annealing.IntKeyFromKeyWord(bestVigenereKey, alphabet), chunkSize, alphabet));

            Console.Write("\n\n-----------------------\n\n");

            Console.Write("Entering phase 2...");
            //Console.Write("\n\n");

            /// Phase two: from my testing, it appears it gets the Vigenere key pretty much always correct after phase 1, but not the transposition key. So, in phase 2 it tries to then sovle for the transpo key, keeping the
            /// vigenere key constant. In Phase 3, it brute forces the transpo keys, keeping the Vigenere key constant.

            bestScore = CipherLib.Annealing.QuadgramScore(CipherLib.Annealing.DecryptNicodemus(ciphertext, bestVigenereKey, bestTranspoKey, chunkSize, alphabet));

            for (int i = 0; i < 100; i++)
            {
                newTranspoKey = CipherLib.Annealing.MessWithIntKey(newTranspoKey);
            }

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                operation = CipherLib.Utils.rand.Next() % 10;
                if (operation >= 0)
                {
                    CipherLib.Utils.CopyArray(CipherLib.Annealing.BlockMessWithIntKey(newTranspoKey), newTranspoKey);
                    //CipherLib.Utils.CopyArray(CipherLib.Annealing.MessWithIntKey(newTranspoKey), newTranspoKey);
                }
                else
                {
                    newVigenereKey = CipherLib.Annealing.SetChar(alphabet[CipherLib.Utils.rand.Next() % alphabet.Length], CipherLib.Utils.rand.Next() % newVigenereKey.Length, newVigenereKey);
                }

                decodedMsg = CipherLib.Annealing.DecryptNicodemus(ciphertext, newVigenereKey, newTranspoKey, chunkSize, alphabet);

                newScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                    bestVigenereKey = newVigenereKey;
                }
                else if (newScore < bestScore)
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, 20f * (numOfTrials - trial) / numOfTrials);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = newScore;
                        CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                        bestVigenereKey = newVigenereKey;
                    }
                    else
                    {
                        CipherLib.Utils.CopyArray(bestTranspoKey, newTranspoKey);
                        newVigenereKey = bestVigenereKey;
                    }
                }
                else
                {
                    CipherLib.Utils.CopyArray(bestTranspoKey, newTranspoKey);
                    newVigenereKey = bestVigenereKey;
                }
            }

            //Console.Write("\b\b\b\b\b\b\b\b\b\b\b");
            Console.Write("\n\n");
            Console.Write("Best Vigenère key:\n\n");
            Console.Write(bestVigenereKey.ToUpper());
            Console.Write(" or rearranged as ");
            Console.Write(CipherLib.Annealing.DecodeRowTranspo(bestVigenereKey, bestTranspoKey).ToUpper());
            Console.Write("\n\n");
            Console.Write("Best transposition key:\n\n");
            CipherLib.Utils.DisplayArray(bestTranspoKey);
            //Console.Write(" or, generated from the best Vigenere key ");
            //Console.Write("\nOr generated from the best Vigenere key:\n");
            //CipherLib.Utils.DisplayArray(CipherLib.Annealing.IntKeyFromKeyWord(bestVigenereKey, alphabet));
            Console.Write("\n\n");
            Console.Write("Best decryption:\n\n");
            Console.Write(CipherLib.Annealing.DecryptNicodemus(ciphertext, bestVigenereKey, bestTranspoKey, chunkSize, alphabet));
            //Console.Write("\n\nor\n\n");
            //Console.Write(CipherLib.Annealing.DecryptNicodemus(ciphertext, bestVigenereKey, CipherLib.Annealing.IntKeyFromKeyWord(bestVigenereKey, alphabet), chunkSize, alphabet));

            Console.Write("\n\n-----------------------\n\n");
            
            Console.Write("Entering phase 3...");
            Console.Write("\n\n");

            /// Phase two - now three: from my testing, it appears it gets the Vigenere key pretty much always correct after phase 1, but not the transposition key. So, in phase 2 it tries to then sovle for the transpo key, keeping the
            /// vigenere key constant. In Phase 3, it brute forces the transpo keys, keeping the Vigenere key constant.

            int[][] perms = CipherLib.Annealing.Permutations(columnNum);
            int displayPeriod = CipherLib.Annealing.Factorial(columnNum - 2);

            bestScore = CipherLib.Annealing.QuadgramScore(CipherLib.Annealing.DecryptNicodemus(ciphertext, bestVigenereKey, bestTranspoKey, chunkSize, alphabet));

            Console.Write("Searching " + perms.Length.ToString() + " keys...");
            Console.Write("\n\n");

            for (int i = 0; i < perms.Length; i++)
            {
                if ((i + 1) % displayPeriod == 0 || i == 0)
                {
                    CipherLib.Utils.ClearLine();
                    Console.Write("Searched " + (i + 1).ToString() + " / " + perms.Length.ToString() + " keys...");
                }

                decodedMsg = CipherLib.Annealing.DecryptNicodemus(ciphertext, bestVigenereKey, perms[i], chunkSize, alphabet);

                newScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    CipherLib.Utils.CopyArray(perms[i], bestTranspoKey);
                }
            }

            Console.Write("\n\n");
            Console.Write("Best Vigenère key:\n\n");
            Console.Write(bestVigenereKey.ToUpper());
            Console.Write(" or rearranged as ");
            Console.Write(CipherLib.Annealing.DecodeRowTranspo(bestVigenereKey, bestTranspoKey).ToUpper());
            Console.Write("\n\n");
            Console.Write("Best transposition key:\n\n");
            CipherLib.Utils.DisplayArray(bestTranspoKey);
            Console.Write("\n\n");
            Console.Write("Best decryption:\n\n");
            Console.Write(CipherLib.Annealing.DecryptNicodemus(ciphertext, bestVigenereKey, bestTranspoKey, chunkSize, alphabet));

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
