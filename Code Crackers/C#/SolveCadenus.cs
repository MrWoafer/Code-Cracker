using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpCadenus
{
    class Program
    {
        //const int numOfTrials = 10000;
        /*const int numOfTrials = 1000;

        const float temperature = 20f;
        const float step = 2f;
        const int count = 10000;*/

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Cadenus Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--CadenusMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            //string alphabet;
            string vertKey;
            int columnNum;
            if (args.Length > 0)
            {
                columnNum = Int32.Parse(args[0]);
                //alphabet = args[1];
                vertKey = args[1];
            }
            else
            {
                //columnNum = 5;
                columnNum = 7;
                //alphabet = "abcdefghijklmnopqrtsuvwxyz";
                vertKey = CipherLib.Cadenus.Default_Cadenus_Alphabet;
            }

            //Console.Write("Alphabet: " + alphabet);
            Console.Write("Vertical Alphabet: " + vertKey);
            Console.Write("\n\nNumber Of Columns: ");
            Console.Write(columnNum);
            Console.Write("\n\n-----------------------\n\n");

            //Console.Write(CipherLib.Cadenus.Decrypt(ciphertext, "harry", alphabet: alphabet));
            //Console.Write(CipherLib.Cadenus.Decrypt(ciphertext, "finalty", alphabet: alphabet));
            //Console.ReadLine();

            /*Console.Write(CipherLib.Cadenus.Decrypt(ciphertext, "finalty", CipherLib.Annealing.InvertKey(new int[] { 1, 2, 4, 0, 3, 5, 6 }), CipherLib.Cadenus.Default_Cadenus_Alphabet, alphabet));
            Console.Write("\n\n");
            //Console.Write(CipherLib.Cadenus.CrackWordKey(ciphertext, new int[] { 1, 2, 4, 0, 3, 5, 6 }, CipherLib.Cadenus.Default_Cadenus_Alphabet, alphabet));
            //Console.Write(CipherLib.Cadenus.CrackWordKey(ciphertext, CipherLib.Annealing.InvertKey(new int[] { 1, 2, 4, 0, 3, 5, 6 }), CipherLib.Cadenus.Default_Cadenus_Alphabet, alphabet));
            Console.ReadLine();*/

            //string newKey = new string('a', columnNum);
            /*string newKey = "";
            for (int i = 0; i < columnNum; i++)
            {
                newKey += alphabet[CipherLib.Utils.rand.Next() % alphabet.Length];
            }*/
            //newKey = "finalty";
            //newKey = "finatty";
            //string bestKey = newKey;

            //string newVertKey = CipherLib.Cadenus.Default_Cadenus_Alphabet;
            //string bestVertKey = newVertKey;

            /*int[] newTranspoKey = new int[columnNum];
            for (int i = 0; i < columnNum; i++)
            {
                newTranspoKey[i] = i;
            }
            for (int i = 0; i < 1000; i++)
            {
                CipherLib.Utils.CopyArray(CipherLib.Annealing.BlockMessWithIntKey(newTranspoKey), newTranspoKey);
            }*/

            //int[] bestTranspoKey = new int[columnNum];
            //CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);

            //float newScore = CipherLib.Annealing.QuadgramScore(CipherLib.Cadenus.Decrypt(ciphertext, newKey, newVertKey, alphabet));
            //float newScore = CipherLib.Annealing.QuadgramScore(CipherLib.Cadenus.Decrypt(ciphertext, newKey, newTranspoKey, newVertKey, alphabet));
            //float bestScore = newScore;

            //string decodedMsg = "";

            //int operation;

            //for (int trial = 0; trial < numOfTrials; trial++)
            /*for (float T = temperature; T >= 0; T -= step)
            {
                for (int trial  = 0; trial < count; trial++)
                {
                    if (trial % (count / 10) == 0)
                    {
                        CipherLib.Utils.ClearLine();
                        Console.Write("Countdown: " + (10 * (1 + temperature / step) - (10 * trial / count + 10 * (temperature - T) / step)).ToString("n1"));
                        Console.Write("      ");
                    }

                    operation = CipherLib.Utils.rand.Next() % 10;

                    if (operation < 5)
                    {
                        newKey = CipherLib.Annealing.SetChar(alphabet[CipherLib.Utils.rand.Next() % alphabet.Length], CipherLib.Utils.rand.Next() % newKey.Length, newKey);
                    }
                    else
                    {
                        CipherLib.Utils.CopyArray(CipherLib.Annealing.BlockMessWithIntKey(newTranspoKey), newTranspoKey);
                    }

                    /*for (int i = 0; i < 2; i++)
                    //for (int i = 0; i < 1; i++)
                    //for (int i = 0; i < 1 + temperature / 10; i++)
                    {
                        newKey = CipherLib.Annealing.SetChar(alphabet[CipherLib.Utils.rand.Next() % alphabet.Length], CipherLib.Utils.rand.Next() % newKey.Length, newKey);
                    }*/

            //decodedMsg = CipherLib.Cadenus.Decrypt(ciphertext, newKey, newVertKey, alphabet);
            /*decodedMsg = CipherLib.Cadenus.Decrypt(ciphertext, newKey, newTranspoKey, newVertKey, alphabet);

            newScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

            if (newScore > bestScore)
            {
                bestScore = newScore;
                bestKey = newKey;
                CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
            }
            else if (newScore < bestScore)
            {
                bool ReplaceAnyway;
                //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, 20f * (numOfTrials - trial) / numOfTrials);
                ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, T);

                if (ReplaceAnyway == true)
                {
                    bestScore = newScore;
                    bestKey = newKey;
                    CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                }
                else
                {
                    newKey = bestKey;
                    CipherLib.Utils.CopyArray(bestTranspoKey, newTranspoKey);
                }
            }
            else
            {
                newKey = bestKey;
                CipherLib.Utils.CopyArray(bestTranspoKey, newTranspoKey);
            }
        }
    }*/

            /*newKey = new string('a', columnNum);
            bestKey = newKey;

            decodedMsg = CipherLib.Cadenus.DecryptSameKeyForWordAndTranspo(ciphertext, newKey, newVertKey, alphabet);
            newScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

            //for (int trial = 0; trial < numOfTrials; trial++)
            for (int trial = 0; trial < 1; trial++)
            {
                for (int i = 0; i < columnNum; i++)
                {
                    //bestScore = CipherLib.Annealing.QuadgramScore(CipherLib.Cadenus.DecryptSameKeyForWordAndTranspo())
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        newKey = CipherLib.Annealing.SetChar(alphabet[j], i, bestKey);
                    }

                    //decodedMsg = CipherLib.Cadenus.Decrypt(ciphertext, newKey, newVertKey, alphabet);
                    decodedMsg = CipherLib.Cadenus.DecryptSameKeyForWordAndTranspo(ciphertext, newKey, newVertKey, alphabet);

                    newScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                    if (newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestKey = newKey;
                    }
                }
            }*/

            /*for (int trial = 0; trial < numOfTrials; trial++)
            {
                //if (trial % (count / 10) == 0)
                if (trial % 10 == 0)
                {
                    CipherLib.Utils.ClearLine();
                    //Console.Write("Countdown: " + (10 * (1 + temperature / step) - (10 * trial / count + 10 * (temperature - T) / step)).ToString("n1"));
                    Console.Write("Countdown: " + trial.ToString() + " / " + numOfTrials.ToString());
                    //Console.Write("      ");
                }

                CipherLib.Utils.CopyArray(CipherLib.Annealing.BlockMessWithIntKey(newTranspoKey), newTranspoKey);

                decodedMsg = CipherLib.Cadenus.CrackWordKeyReturnKey(ciphertext, newTranspoKey, newVertKey, alphabet).Item1;

                newScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                }
                else if (newScore < bestScore)
                {
                    bool ReplaceAnyway;

                    //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, T);
                    ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, numOfTrials - trial);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = newScore;
                        CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                    }
                    else
                    {
                        CipherLib.Utils.CopyArray(bestTranspoKey, newTranspoKey);
                    }
                }
                else
                {
                    CipherLib.Utils.CopyArray(bestTranspoKey, newTranspoKey);
                }
            }*/

            float newScore = float.MinValue;
            float bestScore = newScore;

            string decodedMsg = "";

            bool justFoundBetterKey = false;

            int[][] perms = CipherLib.Annealing.Permutations(columnNum);
            int displayPeriod;
            if (columnNum > 2)
            {
                displayPeriod = CipherLib.Annealing.Factorial(columnNum - 2);
            }
            else
            {
                displayPeriod = 1;
            }

            for (int i = 0; i < perms.Length; i++)
            {
                if ((i + 1) % displayPeriod == 0 || i == 0 || justFoundBetterKey)
                {
                    CipherLib.Utils.ClearLine();
                    Console.Write("Searched " + (i + 1).ToString() + " / " + perms.Length.ToString() + " keys...");
                    justFoundBetterKey = false;
                }

                //decodedMsg = CipherLib.Cadenus.CrackWordKeyReturnKey(ciphertext, perms[i], newVertKey, alphabet).Item1;
                decodedMsg = CipherLib.Cadenus.CrackWordKeyReturnKey(ciphertext, perms[i], vertKey).Item1;

                newScore = CipherLib.Annealing.QuadgramScore(decodedMsg);

                if (newScore > bestScore)
                {
                    bestScore = newScore;

                    Console.Write("\n\n");
                    Console.Write("Best key:\n\n");
                    //Console.Write(CipherLib.Cadenus.CrackWordKeyReturnKey(ciphertext, perms[i], bestVertKey, alphabet).Item2.ToUpper());
                    Console.Write(CipherLib.Cadenus.CrackWordKeyReturnKey(ciphertext, perms[i], vertKey).Item2.ToUpper());
                    Console.Write("\n\n");
                    Console.Write("Best transposition key:\n\n");
                    CipherLib.Utils.DisplayArray(perms[i], true);
                    Console.Write("\nOr its inverse\n");
                    CipherLib.Utils.DisplayArray(CipherLib.Annealing.InvertKey(perms[i]), true);
                    Console.Write("\n\n");
                    Console.Write("Best decryption:\n\n");
                    //Console.Write(CipherLib.Cadenus.CrackWordKeyReturnKey(ciphertext, perms[i], bestVertKey, alphabet).Item1);
                    Console.Write(CipherLib.Cadenus.CrackWordKeyReturnKey(ciphertext, perms[i], vertKey).Item1);
                    Console.Write("\n\n-----------------------\n\n");

                    justFoundBetterKey = true;
                }
            }

            /*CipherLib.Utils.ClearLine();
            //Console.Write("Best key:\n\n");
            Console.Write("Best key:                                            \n\n");
            //Console.Write(bestKey.ToUpper());
            Console.Write(CipherLib.Cadenus.CrackWordKeyReturnKey(ciphertext, bestTranspoKey, bestVertKey, alphabet).Item2.ToUpper());
            Console.Write("\n\n");
            Console.Write("Best transposition key:\n\n");
            CipherLib.Utils.DisplayArray(bestTranspoKey, true);
            Console.Write("\n\n");
            Console.Write("Best decryption:\n\n");
            //Console.Write(CipherLib.Cadenus.Decrypt(ciphertext, bestKey, bestVertKey, alphabet));
            //Console.Write(CipherLib.Cadenus.Crack(ciphertext, bestKey, bestTranspoKey, bestVertKey, alphabet));
            Console.Write(CipherLib.Cadenus.CrackWordKeyReturnKey(ciphertext, bestTranspoKey, bestVertKey, alphabet).Item1);*/

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }   
    }
}
