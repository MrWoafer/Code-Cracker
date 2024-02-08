using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpBruteADFGVX
{
    class Program
    {
        //public const string ADFGVXALPHABET = "abcdefghijklmnopqrstuvwxyz0123456789";
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

            //Console.Write("-- C# ADFGVX Solver --");
            Console.Write("-- C# ADFGVX Bruter --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--BruteADFGVXMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");

            int trial = 0;

            int keyLength;
            CipherLib.TranspositionType transpoType;
            if (args.Length > 0)
            {
                keyLength = Int32.Parse(args[0]);
                transpoType = (CipherLib.TranspositionType)Int32.Parse(args[1]);
            }
            else
            {
                //keyLength = 3;
                //keyLength = 5;
                //keyLength = 7;
                keyLength = 6;
                transpoType = CipherLib.TranspositionType.column;
            }

            Console.Write("Using Alphabet:\n" + CipherLib.ADFGVX.ADFGVXALPHABET);
            Console.Write("\n\nTransposition Type: ");
            if (transpoType == CipherLib.TranspositionType.column)
            {
                Console.Write("Column");
            }
            else
            {
                Console.Write("Row");
            }
            Console.Write("\n\n-----------------------\n\n");

            //Tuple<int[], float> result;
            string decipherment;
            string adfgvxKey;

            int[][] perms = CipherLib.Annealing.Permutations(keyLength);

            int displayPeriod = CipherLib.Annealing.Factorial(keyLength - 1);

            Console.Write("Searching all " + perms.Length.ToString() + " keys...");
            Console.Write("\n\n");

            List<int[]> possibleKeys = new List<int[]>();
            float ioc;

            //int[][] tryingKeys = possibleKeys.ToArray();

            int[] bestKey = new int[keyLength];
            //Array.Copy(tryingKeys[0], bestKey, keyLength);

            //float currentScore = Score(msg, tryingKeys[0], transpoType);
            float currentScore = float.MinValue;
            float bestScore = currentScore;

            bool justGotNewBestKey = false;

            for (trial = 0; trial < perms.Length; trial++)
            {
                if ((trial + 1) % displayPeriod == 0 || trial == 0 || justGotNewBestKey)
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bSearched: " + (trial + 1) + " keys");
                    justGotNewBestKey = false;
                }

                ioc = CipherLib.ADFGVX.IOCADFGVX(msg, perms[trial], transpoType);

                if (ioc > LOWERIOCLIMIT && ioc < UPPERIOCLIMIT)
                {
                    possibleKeys.Add(perms[trial]);

                    /*if (tryingKeys.Length < 4)
                    {
                        decipherment = CipherLib.ADFGVX.DecodeADFGVX(msg, tryingKeys[trial], transpoType, 10000);
                    }
                    else if (tryingKeys.Length < 10)
                    {
                        decipherment = CipherLib.ADFGVX.DecodeADFGVX(msg, tryingKeys[trial], transpoType, 5000);
                    }
                    else
                    {
                        //decipherment = DecodeADFGVX(msg, tryingKeys[trial], 2500);
                        decipherment = CipherLib.ADFGVX.DecodeADFGVX(msg, tryingKeys[trial], transpoType, 5000);
                    }*/
                    decipherment = CipherLib.ADFGVX.DecodeADFGVX(msg, perms[trial], transpoType, 10000);
                    //decipherment = DecodeADFGVX(msg, tryingKeys[trial], 2500);
                    //decipherment = DecodeADFGVX(msg, tryingKeys[trial], 5000);
                    //decipherment = DecodeADFGVX(msg, tryingKeys[trial], 10000);

                    //currentScore = Score(msg, tryingKeys[trial]);
                    //currentScore = CipherLib.Annealing.QuadgramScore(msg);
                    currentScore = CipherLib.Annealing.QuadgramScore(decipherment);

                    if (possibleKeys.Count == 1 || currentScore > bestScore)
                    {
                        bestScore = currentScore;

                        Array.Copy(perms[trial], bestKey, keyLength);

                        //Console.Write("\b\b\b\b\b\b\b\b\b");
                        Console.Write("\n\n--------------------------------------");
                        Console.Write("\n\n");
                        Console.Write("New best key:\n\n");
                        Console.Write("Score: " + bestScore + "\n");
                        Console.Write("\n");
                        Console.Write("Key:\n");
                        CipherLib.ADFGVX.DisplayKey(perms[trial]);
                        Console.Write("\n\n");
                        adfgvxKey = CipherLib.Annealing.GetKeyFromPlaintext(decipherment, CipherLib.ADFGVX.TransposeADFGVX(msg, bestKey, transpoType), CipherLib.ADFGVX.ADFGVXALPHABET);
                        CipherLib.Annealing.DisplayPolybiusSquare("ADFGVX", "ADFGVX", adfgvxKey);
                        Console.Write("\n\n");
                        Console.Write("Decipherment: " + decipherment);
                        Console.Write("\n\n");
                        Console.Write("--------------------------------------\n\n");

                        justGotNewBestKey = true;
                    }
                }                
            }

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("All keys checked.");
            //Console.Write("I have identified " + possibleKeys.Count() + " possible keys.");
            //Console.Write("I have identified " + possibleKeys.Count() + " possible keys:");
            Console.Write("\n\n");
            Console.Write("I identified " + possibleKeys.Count() + " possible keys:");
            //Console.Write("\n\n-----------------------");
            Console.Write("\n\n");

            for (int i = 0; i < possibleKeys.Count(); i++)
            {
                CipherLib.ADFGVX.DisplayKey(possibleKeys[i]);
                Console.Write("\n");
            }

            //Console.Write("\n\n-----------------------");

            /*if (possibleKeys.Count() > 0)
            {
                //Console.Write("\n\nI will now try each one...");
                Console.Write("\nI will now try each one...");
                Console.Write("\n\n-----------------------\n\n");

                for (trial = 0; trial < tryingKeys.Length; trial++)
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bTrying key " + (trial + 1) + " / " + tryingKeys.Length);

                    

                    while (Console.KeyAvailable)
                    {
                        Pause(msg, bestKey, transpoType);
                    }
                }
            }*/

            //Console.Write("--------------------------------------\n\n");
            Console.Write("\n--------------------------------------\n\n");
            //Console.Write("\n\n");
            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        static float Score(string msg, int[] key, CipherLib.TranspositionType transpoType)
        {
            string decodedMsg = "";
            float score = 0;

            decodedMsg = CipherLib.ADFGVX.DecodeADFGVX(msg, key, transpoType, trials: 1000);

            score = CipherLib.Annealing.QuadgramScore(decodedMsg);

            return score;
        }

        /*static string TransposeADFGVX(string msg, int[] key)
        {
            string decodedMsg = "";
            float columnLength = msg.Length / key.Length;

            for (int i = 0; i < columnLength; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    for (int k = 0; k < key.Length; k++)
                    {
                        if (key[k] == j)
                        {
                            decodedMsg += msg[(int)(k * columnLength + i)];
                        }
                    }
                }
            }

            string replacedMsg = "";

            for (int i = 0; i < decodedMsg.Length; i += 2)
            {
                replacedMsg += DigramToMonogramADFGVX(decodedMsg[i], decodedMsg[i + 1]);
            }

            return replacedMsg;
        }*/

        /*static string DecodeADFGVX(string msg, int[] key, int trials = 2500)
        {
            return CipherLib.Annealing.CrackCustomMonoSub(TransposeADFGVX(msg, key), ADFGVXALPHABET, trials, CipherLib.SolutionType.QuadgramScore);
        }*/

        /*static string GetKeyFromPlaintext(string plaintext, string ciphertext)
        {
            string key = ADFGVXALPHABET;

            for (int i = 0; i < ciphertext.Length; i++)
            {
                key = CipherLib.Annealing.Swap(key, ADFGVXALPHABET.IndexOf(ciphertext[i]), key.IndexOf(plaintext[i]));
            }

            return key;
        }*/

        /*static float IOCADFGVX(string msg, int[] key)
        {
            return CipherLib.Annealing.IOCAlphanumeric(TransposeADFGVX(msg, key));
        }*/

        /*static Tuple<int[], float> AnnealingADFGVX(string msg, int[] key, CipherLib.TranspositionType transpoType, float temperature, float step, int count, int[] overallBestKey)
        {
            int[] currentKey = new int[key.Length];
            Array.Copy(key, currentKey, key.Length);

            int[] bestKey = new int[key.Length];
            Array.Copy(key, bestKey, key.Length);

            float currentScore = Score(msg, currentKey);
            float bestScore = currentScore;

            float ioc;

            for (float t = temperature; t >= 0; t -= step)
            {
                string zeroes = "";

                for (int i = 0; i < 2 - t.ToString().Length; i++)
                {
                    zeroes += "0";
                }

                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" + zeroes + t.ToString("n1"));

                for (int trial = 0; trial < count; trial++)
                {
                    //Console.Write("Trial: " + trial + "\n");
                    //Console.Write("\nTrial: " + trial + " | "); DisplayKey(bestKey);
                    for (int i = 0; i < CipherLib.Annealing.rand.Next() % 6 + 1; i++)
                    {
                        Array.Copy(CipherLib.Annealing.MessWithIntKey(currentKey), currentKey, currentKey.Length);
                    }

                    ioc = CipherLib.ADFGVX.IOCADFGVX(msg, currentKey);

                    if (ioc > LOWERIOCLIMIT)
                    //if (ioc > LOWERIOCLIMIT && ioc < UPPERIOCLIMIT)
                    {
                        //Console.Write("\nI've got one!\n");

                        currentScore = Score(msg, currentKey);

                        if (currentScore > bestScore)
                        {
                            bestScore = currentScore;
                            Array.Copy(currentKey, bestKey, currentKey.Length);
                        }
                        else if (currentScore < bestScore)
                        {

                            bool ReplaceAnyway;
                            ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, t);

                            if (ReplaceAnyway == true)
                            {
                                bestScore = currentScore;
                                Array.Copy(currentKey, bestKey, currentKey.Length);
                            }
                            else
                            {
                                Array.Copy(bestKey, currentKey, bestKey.Length);
                            }
                        }
                        else
                        {
                            Array.Copy(bestKey, currentKey, bestKey.Length);
                        }
                    }

                    while (Console.KeyAvailable)
                    {
                        Pause(msg, overallBestKey);
                    }
                }
            }

            return new Tuple<int[], float>(bestKey, bestScore);
        }*/

        /*static int[] MessAroundWithKey(int[] key)
        {
            int index1 = CipherLib.Annealing.rand.Next() % key.Length;
            int index2 = CipherLib.Annealing.rand.Next() % key.Length;

            int temp = key[index1];

            key[index1] = key[index2];
            key[index2] = temp;

            return key;
        }

        static int[] MessAroundWithKey2(int[] key)
        {
            int length = CipherLib.Annealing.rand.Next() % (key.Length - 2) + 1;

            int index1 = CipherLib.Annealing.rand.Next() % (key.Length - length);
            int index2 = CipherLib.Annealing.rand.Next() % (key.Length - length);

            int temp;

            for (int i = 0; i < length; i++)
            {
                temp = key[index1 + i];
                key[index1 + i] = key[index2 + i];
                key[index2 + i] = temp;
            }

            return key;
        }*/

        /*static void DisplayKey(int[] key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                Console.Write(key[i]);

                if (i < key.Length - 1)
                {
                    Console.Write(", ");
                }
            }
        }*/

        /*static char DigramToMonogramADFGVX(char char1, char char2)
        {
            int index = 0;

            // Row coord
            if (char1 == 'a')
            {
                index += 0;
            }
            else if (char1 == 'd')
            {
                index += 6;
            }
            else if (char1 == 'f')
            {
                index += 12;
            }
            else if (char1 == 'g')
            {
                index += 18;
            }
            else if (char1 == 'v')
            {
                index += 24;
            }
            else if (char1 == 'x')
            {
                index += 30;
            }

            // Column co-ord
            if (char2 == 'a')
            {
                index += 0;
            }
            else if (char2 == 'd')
            {
                index += 1;
            }
            else if (char2 == 'f')
            {
                index += 2;
            }
            else if (char2 == 'g')
            {
                index += 3;
            }
            else if (char2 == 'v')
            {
                index += 4;
            }
            else if (char2 == 'x')
            {
                index += 5;
            }

            return ADFGVXALPHABET[index];
        }*/

        static void Pause(string msg, int[] bestKey, CipherLib.TranspositionType transpoType)
        {
            Console.Write("\n");
            string input = Console.ReadLine();

            if (input == "r")
            {
                Console.Write("\n\n-----------------------\n\n");
                Console.Write("Refined output of best key so far:\n\n");
                Console.Write("Best key: ");
                CipherLib.ADFGVX.DisplayKey(bestKey);
                Console.Write("\n\n");

                string decipherment = CipherLib.ADFGVX.DecodeADFGVX(msg, bestKey, transpoType, 10000);

                string adfgvxKey = CipherLib.Annealing.GetKeyFromPlaintext(decipherment, CipherLib.ADFGVX.TransposeADFGVX(msg, bestKey, transpoType), CipherLib.ADFGVX.ADFGVXALPHABET);
                CipherLib.Annealing.DisplayPolybiusSquare("ADFGVX", "ADFGVX", adfgvxKey);
                Console.Write("\n\n");

                Console.Write("Decipherment:\n\n");
                Console.Write(decipherment);
                Console.Write("\n\n");
                Console.ReadLine();
                Console.Write("-----------------------\n\n");
            }
        }
    }
}
