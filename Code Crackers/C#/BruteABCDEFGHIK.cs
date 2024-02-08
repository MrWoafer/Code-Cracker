using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBruteABCDEFGHIK
{
    class Program
    {
        public const float LOWERIOCLIMIT = 0.06f;
        public const float UPPERIOCLIMIT = 0.073f;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# ABCDEFGHIK Bruter --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--BruteABCDEFGHIKMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");

            int trial = 0;

            string alphabet;
            int keyLength;
            CipherLib.TranspositionType transpoType;
            int ngramLength;
            if (args.Length > 0)
            {
                keyLength = Int32.Parse(args[0]);
                transpoType = (CipherLib.TranspositionType)Int32.Parse(args[1]);
                alphabet = args[2];
                ngramLength = Int32.Parse(args[3]);
            }
            else
            {
                keyLength = 7;
                transpoType = CipherLib.TranspositionType.column;
                //transpoType = CipherLib.TranspositionType.myszkowski;
                alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
                ngramLength = 3;
            }

            Console.Write("Using Alphabet:\n" + alphabet);
            Console.Write("\n\nNum Of Columns: " + keyLength);
            Console.Write("\n\nTransposition Type: ");
            if (transpoType == CipherLib.TranspositionType.column)
            {
                Console.Write("Column");
            }
            //else
            else if (transpoType == CipherLib.TranspositionType.row)
            {
                Console.Write("Row");
            }
            else if (transpoType == CipherLib.TranspositionType.myszkowski)
            {
                Console.Write("Myszkowski");
            }
            else if (transpoType == CipherLib.TranspositionType.amsco)
            {
                Console.Write("AMSCO");
            }
            else
            {
                Console.Write("Unknown (I'm probably about to crash)");
            }
            Console.Write("\n\n-----------------------\n\n");

            //Console.Write(CipherLib.ABCDEFGHIK.IncompleteColumnarTranspoABCDEFGHIK(msg, new int[] { 5, 4, 2, 6, 0, 1, 3 }));
            //Console.Read();
            
            string decipherment;
            Dictionary<string, char> adfgvxKey;

            int[][] perms = CipherLib.Annealing.Permutations(keyLength);

            int displayPeriod = CipherLib.Annealing.Factorial(keyLength - 1);

            Console.Write("Searching all " + perms.Length.ToString() + " keys...");
            Console.Write("\n\n");

            List<int[]> possibleKeys = new List<int[]>();
            float ioc;

            int[] bestKey = new int[keyLength];

            float currentScore = float.MinValue;
            float bestScore = currentScore;

            bool justGotNewBestKey = false;

            for (trial = 0; trial < perms.Length; trial++)
            {
                if ((trial + 1) % displayPeriod == 0 || trial == 0 || justGotNewBestKey)
                {
                    //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bSearched: " + (trial + 1) + " keys");
                    CipherLib.Utils.ClearLine();
                    Console.Write("Searched: " + (trial + 1).ToString() + " / " + perms.Length.ToString() + " keys...");
                    justGotNewBestKey = false;
                }

                //ioc = CipherLib.ABCDEFGHIK.IOCABCDEFGHIK(msg, perms[trial], transpoType, alphabet);
                ioc = CipherLib.ABCDEFGHIK.IOCABCDEFGHIK(msg, perms[trial], transpoType, ngramLength, alphabet);

                if (ioc > 0 && (ioc > LOWERIOCLIMIT && ioc < UPPERIOCLIMIT))
                {
                    possibleKeys.Add(perms[trial]);

                    //decipherment = CipherLib.ABCDEFGHIK.DecodeABCDEFGHIK(msg, perms[trial], transpoType, alphabet, 10000);
                    decipherment = CipherLib.ABCDEFGHIK.DecodeABCDEFGHIK(msg, perms[trial], transpoType, ngramLength, alphabet, 10000);

                    currentScore = CipherLib.Annealing.QuadgramScore(decipherment);

                    if (possibleKeys.Count == 1 || currentScore > bestScore)
                    {
                        bestScore = currentScore;

                        Array.Copy(perms[trial], bestKey, keyLength);

                        Console.Write("\n\n--------------------------------------");
                        Console.Write("\n\n");
                        Console.Write("New best key:\n\n");
                        Console.Write("Score: " + bestScore + "\n");
                        Console.Write("\n");
                        Console.Write("Key:\n");
                        //CipherLib.Utils.DisplayArray(perms[trial]);
                        CipherLib.Utils.DisplayArray(perms[trial], true);
                        Console.Write("\n\n");
                        //adfgvxKey = CipherLib.ABCDEFGHIK.DeduceKey(decipherment, CipherLib.ABCDEFGHIK.TransposeABCDEFGHIK(msg, bestKey, transpoType, alphabet, false), alphabet);
                        adfgvxKey = CipherLib.ABCDEFGHIK.DeduceKey(decipherment, CipherLib.ABCDEFGHIK.TransposeABCDEFGHIK(msg, bestKey, transpoType, ngramLength, alphabet, false), ngramLength, alphabet);
                        //CipherLib.ABCDEFGHIK.DisplayKey(adfgvxKey);
                        CipherLib.ABCDEFGHIK.DisplayKey(adfgvxKey, ngramLength);
                        Console.Write("\n\n");
                        //Console.Write("Decipherment: " + decipherment);
                        Console.Write(decipherment);
                        Console.Write("\n\n");
                        Console.Write("--------------------------------------\n\n");

                        justGotNewBestKey = true;
                    }
                }
            }

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("All keys checked.");
            Console.Write("\n\n");
            Console.Write("I identified " + possibleKeys.Count() + " possible keys:");
            Console.Write("\n\n");

            for (int i = 0; i < possibleKeys.Count(); i++)
            {
                //CipherLib.Utils.DisplayArray(possibleKeys[i]);
                CipherLib.Utils.DisplayArray(possibleKeys[i], true);
                Console.Write("\n");
            }

            Console.Write("\n--------------------------------------\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        //static float Score(string msg, int[] key, CipherLib.TranspositionType transpoType, string alphabet)
        /*static float Score(string msg, int[] key, CipherLib.TranspositionType transpoType, int ngramLength, string alphabet)
        {
            string decodedMsg = "";
            float score = 0;

            //decodedMsg = CipherLib.ABCDEFGHIK.DecodeABCDEFGHIK(msg, key, transpoType, alphabet, trials: 1000);
            decodedMsg = CipherLib.ABCDEFGHIK.DecodeABCDEFGHIK(msg, key, transpoType, ngramLength, alphabet, trials: 1000);

            score = CipherLib.Annealing.QuadgramScore(decodedMsg);

            return score;
        }*/
    }
}
