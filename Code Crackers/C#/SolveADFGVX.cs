using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpADFGVX
{
    class Program
    {        
        public const float LOWERIOCLIMIT = 0.06f;
        //public const float UPPERIOCLIMIT = 0.073f;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# ADFGVX Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--ADFGVXMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");

            int trial = 0;

            int[] currentKey;
            CipherLib.TranspositionType transpoType;
            if (args.Length > 0)
            {
                currentKey = new int[Int32.Parse(args[0])];
                transpoType = (CipherLib.TranspositionType)Int32.Parse(args[1]);
            }
            else
            {
                //currentKey = new int[12];
                currentKey = new int[7];
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

            for (int i = 0; i < currentKey.Length; i++)
            {
                currentKey[i] = i;
            }

            for (int i = 0; i < 100; i++)
            {
                currentKey = MessAroundWithKey(currentKey);
            }

            int[] bestKey = new int[currentKey.Length];
            Array.Copy(currentKey, bestKey, currentKey.Length);

            float currentScore = Score(msg, currentKey, transpoType);
            float bestScore = currentScore;

            Console.Write("Starting with key: ");
            CipherLib.ADFGVX.DisplayKey(currentKey);
            Console.Write("\n\n");
            Console.Write("Starting with score: " + bestScore.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");

            Tuple<int[], float> result;
            string decipherment;
            string adfgvxKey;

            for (trial = 0; trial >= 0; trial++)
            {
                Console.Write("Trial: " + (trial + 1) + "\n\n");

                Array.Copy(bestKey, currentKey, bestKey.Length);

                result = AnnealingADFGVX(msg, currentKey, transpoType, 20, 1f, 1000, bestKey);

                currentKey = result.Item1;
                currentScore = result.Item2;

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;

                    Array.Copy(currentKey, bestKey, currentKey.Length);

                    Console.Write("\b\b\b\b\b\b\b\b\b");
                    Console.Write("New best key:\n\n");
                    Console.Write("Score: " + bestScore + "\n");
                    Console.Write("\n");
                    Console.Write("Key:\n");
                    CipherLib.ADFGVX.DisplayKey(currentKey);
                    Console.Write("\n\n");
                    decipherment = CipherLib.ADFGVX.DecodeADFGVX(msg, bestKey, transpoType, 5000);
                    //adfgvxKey = GetKeyFromPlaintext(decipherment, TransposeADFGVX(msg, bestKey));
                    adfgvxKey = CipherLib.Annealing.GetKeyFromPlaintext(decipherment, CipherLib.ADFGVX.TransposeADFGVX(msg, bestKey, transpoType), CipherLib.ADFGVX.ADFGVXALPHABET);
                    CipherLib.Annealing.DisplayPolybiusSquare("ADFGVX", "ADFGVX", adfgvxKey);
                    Console.Write("\n\n");
                    Console.Write("Decipherment: " + decipherment);
                    Console.Write("\n\n");
                }

                else
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b");
                    Console.Write("Didn't find a better key...");
                    Console.Write("\n\n");
                }
                Console.Write("--------------------------------------\n\n");
            }


            Console.Write("Press ENTER to close...");
            Console.ReadLine();
        }

        static float Score(string msg, int[] key, CipherLib.TranspositionType transpoType)
        {
            string decodedMsg = "";
            float score = 0;

            decodedMsg = CipherLib.ADFGVX.DecodeADFGVX(msg, key, transpoType);

            score = CipherLib.Annealing.QuadgramScore(decodedMsg);

            return score;
        }        

        /*static string GetKeyFromPlaintext(string plaintext, string ciphertext)
        {
            string key = ADFGVXALPHABET;

            for (int i = 0; i < ciphertext.Length; i++)
            {
                key = CipherLib.Annealing.Swap(key, ADFGVXALPHABET.IndexOf(ciphertext[i]), key.IndexOf(plaintext[i]));
            }

            return key;
        }*/

        static Tuple<int[], float> AnnealingADFGVX(string msg, int[] key, CipherLib.TranspositionType transpoType, float temperature, float step, int count, int[] overallBestKey)
        {
            int[] currentKey = new int[key.Length];
            Array.Copy(key, currentKey, key.Length);

            int[] bestKey = new int[key.Length];
            Array.Copy(key, bestKey, key.Length);

            float currentScore = Score(msg, currentKey, transpoType);
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

                    ioc = CipherLib.ADFGVX.IOCADFGVX(msg, currentKey, transpoType);

                    if (ioc > LOWERIOCLIMIT)
                    //if (ioc > LOWERIOCLIMIT && ioc < UPPERIOCLIMIT)
                    {
                        //Console.Write("\nI've got one!\n");

                        currentScore = Score(msg, currentKey, transpoType);

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
                        Pause(msg, overallBestKey, transpoType);
                    }
                }
            }

            return new Tuple<int[], float>(bestKey, bestScore);
        }

        static int[] MessAroundWithKey(int[] key)
        {
            int index1 = CipherLib.Annealing.rand.Next() % key.Length;
            int index2 = CipherLib.Annealing.rand.Next() % key.Length;

            int temp = key[index1];

            key[index1] = key[index2];
            key[index2] = temp;

            return key;
        }

        /*static int[] MessAroundWithKey2(int[] key)
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