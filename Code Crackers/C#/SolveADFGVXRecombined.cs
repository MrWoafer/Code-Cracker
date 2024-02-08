using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpADFGVXRecombined
{
    class Program
    {
        const int initialNumOfTrials = 1000;
        //const int initialNumOfTrials = 100;
        //const int initialNumOfTrials = 500;

        //const int numOfTrials = 10000;
        const int numOfTrials = 1000;

        const float temperature = 20f;
        const float step = 2f;
        const int count = 10000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# ADFGVX Recombined Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--ADFGVXRecombinedMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            int columnNum;
            string alphabet;
            CipherLib.TranspositionType transpoType;
            if (args.Length > 0)
            {
                columnNum = Int32.Parse(args[0]);
                alphabet = args[1];
                transpoType = (CipherLib.TranspositionType)Int32.Parse(args[2]);
            }
            else
            {
                //columnNum = 4;
                columnNum = 5;
                alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
                transpoType = CipherLib.TranspositionType.row;
            }

            Console.Write("Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("Num Of Columns: " + columnNum);
            Console.Write("\n\n-----------------------\n\n");

            //Console.Write(CipherLib.ADFGVXRecombined.Decrypt(ciphertext, "QEYRTU5FIJVH1N06CM3O4XZ98BW7PLKG2DSA".ToLower(), new int[] { 3, 2, 1, 0}, CipherLib.TranspositionType.row));
            //Console.Write("\n\n");

            Tuple<string, string> result = new Tuple<string, string>("", "");

            //result = CipherLib.ADFGVXRecombined.CrackGridReturnKey(ciphertext, new int[] { 3, 2, 1, 0 }, alphabet, transpoType, numOfTrials);
            //result = CipherLib.ADFGVXRecombined.CrackGridReturnKey(ciphertext, CipherLib.Utils.IntRangeArray(0, columnNum - 1).Reverse().ToArray(), alphabet, transpoType, numOfTrials);
            result = CipherLib.ADFGVXRecombined.CrackGridReturnKey(ciphertext, CipherLib.Utils.IntRangeArray(0, columnNum - 1).Reverse().ToArray(), alphabet, transpoType, 10000);

            //Console.Write("Best key:");
            Console.Write("Best grid key (with transposition key ");
            CipherLib.Utils.DisplayArray(CipherLib.Utils.IntRangeArray(0, columnNum - 1).Reverse().ToArray(), true);
            Console.Write("):");
            Console.Write("\n\n");
            Console.Write(result.Item2.ToUpper());
            Console.Write("\n\n");
            Console.Write(result.Item1);

            Console.Write("\n\n-----------------------\n\n");

            /*Tuple<string, string, int[]> result2 = new Tuple<string, string, int[]>("", "" , new int[1]);

            result2 = CipherLib.ADFGVXRecombined.CrackReturnKey(ciphertext, alphabet, columnNum, transpoType, numOfTrials);

            Console.Write("Best grid key:");
            Console.Write("\n\n");
            Console.Write(result2.Item2.ToUpper());
            Console.Write("\n\n");
            Console.Write("Best transposition key:");
            Console.Write("\n\n");
            CipherLib.Utils.DisplayArray(result2.Item3, true);
            Console.Write("\n\n");
            Console.Write(result2.Item1);*/

            Console.Write("Phase 1 done.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to progress to the brute force (of the transposition key) stage...");
            Console.ReadLine();
            Console.Write("\n-----------------------\n\n");

            string bestGridKey = alphabet;

            int[] bestTranspoKey = new int[columnNum];

            float newScore;
            float bestScore = float.MinValue;

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

            bool justGotBetterKey = true;

            for (int i = 0; i < perms.Length; i++)
            {
                //if ((i + 1) % displayPeriod == 0 || i == 0)
                if ((i + 1) % displayPeriod == 0 || justGotBetterKey)
                {
                    CipherLib.Utils.ClearLine();
                    Console.Write("Searched " + (i + 1).ToString() + " / " + perms.Length.ToString() + " transposition keys...");
                    justGotBetterKey = false;
                }

                //result = CipherLib.ADFGVXRecombined.CrackGridReturnKey(ciphertext, perms[i], alphabet, transpoType, numOfTrials);
                result = CipherLib.ADFGVXRecombined.CrackGridReturnKey(ciphertext, perms[i], alphabet, transpoType, initialNumOfTrials);

                newScore = CipherLib.Annealing.QuadgramScore(result.Item1);

                if (newScore > bestScore)
                {
                    if (numOfTrials > initialNumOfTrials)
                    {
                        result = CipherLib.ADFGVXRecombined.CrackGridReturnKey(ciphertext, perms[i], alphabet, transpoType, numOfTrials);
                    }

                    bestScore = newScore;
                    bestGridKey = result.Item2;
                    CipherLib.Utils.CopyArray(perms[i], bestTranspoKey);

                    Console.Write("\n\n");
                    Console.Write("New best grid key:");
                    Console.Write("\n\n");
                    Console.Write(result.Item2.ToUpper());
                    Console.Write("\n\n");
                    Console.Write("New best transposition key:");
                    Console.Write("\n\n");
                    CipherLib.Utils.DisplayArray(perms[i], true);
                    Console.Write("\n\n");
                    Console.Write(result.Item1);
                    Console.Write("\n\n-----------------------\n\n");

                    justGotBetterKey = true;
                }
            }

            /*string newGridKey = alphabet;
            for (int i = 0; i < 100; i++)
            {
                newGridKey = CipherLib.Annealing.MessWithStringKey(newGridKey);
            }
            string bestGridKey = newGridKey;

            int[] newTranspoKey = new int[columnNum];
            for (int i = 0; i < columnNum; i++)
            {
                newTranspoKey[i] = i;
            }
            for (int i = 0; i < 100; i++)
            {
                CipherLib.Utils.CopyArray(CipherLib.Annealing.BlockMessWithIntKey(newTranspoKey), newTranspoKey);
            }

            int[] bestTranspoKey = new int[columnNum];
            CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);

            float newScore = CipherLib.Annealing.QuadgramScore(CipherLib.ADFGVXRecombined.Decrypt(ciphertext, newGridKey, newTranspoKey, transpoType));
            float bestScore = newScore;

            int operation;

            for (float T = temperature; T >= 0; T -= step)
            {
                for (int trial = 0; trial < count; trial++)
                {
                    if (trial % (count / 10) == 0)
                    {
                        CipherLib.Utils.ClearLine();
                        Console.Write("Countdown: " + (10 * (1 + temperature / step) - (10 * trial / count + 10 * (temperature - T) / step)).ToString("n1"));
                        Console.Write("                 ");
                    }

                    operation = CipherLib.Utils.rand.Next() % 10;

                    if (operation < 7)
                    {
                        newGridKey = CipherLib.Annealing.MessWithStringKey(newGridKey);
                    }
                    else
                    {
                        CipherLib.Utils.CopyArray(CipherLib.Annealing.BlockMessWithIntKey(newTranspoKey), newTranspoKey);
                    }

                    newScore = CipherLib.Annealing.QuadgramScore(CipherLib.ADFGVXRecombined.Decrypt(ciphertext, newGridKey, newTranspoKey, transpoType));

                    if (newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestGridKey = newGridKey;
                        CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                    }
                    else
                    {
                        bool ReplaceAnyway;
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, T);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = newScore;
                            bestGridKey = newGridKey;
                            CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                        }
                        else
                        {
                            newGridKey = bestGridKey;
                            CipherLib.Utils.CopyArray(bestTranspoKey, newTranspoKey);
                        }
                    }
                }
            }*/

            /*string newGridKey = alphabet;
            for (int i = 0; i < 100; i++)
            {
                newGridKey = CipherLib.Annealing.MessWithStringKey(newGridKey);
            }
            string bestGridKey = newGridKey;

            float newScore = float.MinValue;
            float bestScore = newScore;

            int[] bestTranspoKey = new int[columnNum];

            int[][] perms = CipherLib.Annealing.Permutations(columnNum);

            for (int i = 0; i < numOfTrials; i++)
            {
                newGridKey = CipherLib.Annealing.MessWithStringKey(bestGridKey);

                for (int j = 0; j < perms.Length; j++)
                {
                    newScore = CipherLib.Annealing.IOCAlphanumeric(CipherLib.ABCDEFGHIK.DigramToMonogramABCDEFGHIK(CipherLib.ADFGVXRecombined.UndoPolybius(ciphertext, newGridKey, perms[j], transpoType), alphabet));

                    if (newScore > bestScore)
                    {
                        bestScore = newScore;
                        bestGridKey = newGridKey;
                        CipherLib.Utils.CopyArray(perms[j], bestTranspoKey);
                    }
                }
            }*/

            result = CipherLib.ADFGVXRecombined.CrackGridReturnKey(ciphertext, bestTranspoKey, alphabet, transpoType, 10000);
            Console.Write("\n\n-----------------------\n\n");
            //Console.Write("\n\n");
            Console.Write("New best grid key:");
            Console.Write("\n\n");
            Console.Write(result.Item2.ToUpper());
            Console.Write("\n\n");
            CipherLib.Annealing.DisplayPolybiusSquare("ADFGVX", "ADFGVX", result.Item2);
            Console.Write("\n\n");
            Console.Write("New best transposition key:");
            Console.Write("\n\n");
            CipherLib.Utils.DisplayArray(bestTranspoKey, true);
            Console.Write("\n\n");
            //Console.Write(result.Item1);
            Console.Write(result.Item1);
            //Console.Write("\n\n-----------------------\n\n");

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }
    }
}
