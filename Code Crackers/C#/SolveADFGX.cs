using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using CipherLib;

namespace CSharpADFGX
{
    class Program
    {
        //public const string ADFGXALPHABET = "abcdefghiklmnopqrstuvwxyz";
        public static string ADFGXALPHABET = "abcdefghijklmnopqrstuvwxyz";
        public const float LOWERIOCLIMIT = 0.06f;
        //public const float LOWERIOCLIMIT = 0.064f;
        //public const float LOWERIOCLIMIT = 0.061f;
        //public const float UPPERIOCLIMIT = 0.073f;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# ADFGX Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--ADFGXMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n-----------------------\n\n");

            //Console.Write("Correct Score: " + Score(msg, new int[] { 4, 2, 1, 0, 3 }).ToString() + " " + IOCADFGX(msg, new int[] { 4, 2, 1, 0, 3 }).ToString() + "\n\n");

            //Console.ReadLine();

            //Console.Write(DecodeADFGX(msg, new int[] { 5, 3, 2, 1, 4 }));
            //Console.Write(DecodeADFGX(msg, new int[] { 4, 3, 2, 5, 1 }));
            //Console.Write(DecodeADFGX(msg, new int[] { 4, 2, 1, 0, 3}));
            //Console.Write("\n\n");

            //Console.Write(DecodeADFGX(msg, new int[] { 0, 2, 4, 3, 1 }));
            //Console.Write(Score(msg, new int[] { 0, 2, 4, 3, 1 }));
            //Console.Write("\n\n");

            //Console.ReadLine();

            int trial = 0;

            int[] currentKey;
            char missingLetter;
            if (args.Length > 0)
            {
                currentKey = new int[Int32.Parse(args[0])];
                missingLetter = args[1][0];
            }
            else
            {
                //currentKey = new int[5];
                currentKey = new int[8];
                //currentKey = new int[15];
                //currentKey = new int[12];
                missingLetter = 'j';
                //missingLetter = 'h';
            }

            ADFGXALPHABET = ADFGXALPHABET.Replace(missingLetter.ToString(), string.Empty);

            Console.Write("Using Alphabet:\n" + ADFGXALPHABET);
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

            float currentScore = Score(msg, currentKey);
            float bestScore = currentScore;

            Console.Write("Starting with key: ");
            DisplayKey(currentKey);
            Console.Write("\n\n");
            Console.Write("Starting with score: " + bestScore.ToString() + "\n\n");
            Console.Write("-----------------------\n\n");

            //Console.ReadLine();

            Tuple<int[], float> result;
            string decipherment;
            string adfgxKey;

            //for (trial = 0; trial < 1; trial++)
            //for (trial = 0; trial < Math.Ceiling((decimal)currentKey.Length / 4); trial++)
            for (trial = 0; trial >= 0; trial++)
            {
                //Console.Write("Trial: " + (trial + 1).ToString("n1") + "\n\n");
                Console.Write("Trial: " + (trial + 1) + "\n\n");

                Array.Copy(bestKey, currentKey, bestKey.Length);

                //result = AnnealingADFGX(msg, currentKey, 10, 1f, 1000);
                //result = AnnealingADFGX(msg, currentKey, 20, 0.2f, 1000);
                result = AnnealingADFGX(msg, currentKey, 20, 1f, 1000, bestKey);
                //result = AnnealingADFGX(msg, currentKey, 10, 1f, 10);
                //result = AnnealingADFGX(msg, currentKey, 10, 1f, 100);
                //result = AnnealingADFGX(msg, currentKey, 10, 1f, 10);
                //result = AnnealingADFGX(msg, currentKey, 10, 2f, 100);

                currentKey = result.Item1;
                currentScore = result.Item2;

                if (currentScore > bestScore)
                //if (currentScore < bestScore)
                {
                    bestScore = currentScore;

                    Array.Copy(currentKey, bestKey, currentKey.Length);

                    Console.Write("\b\b\b\b\b\b\b\b\b");
                    Console.Write("New best key:\n\n");
                    Console.Write("Score: " + bestScore + "\n");
                    Console.Write("\n");
                    Console.Write("Key:\n");
                    DisplayKey(currentKey);
                    Console.Write("\n\n");
                    //decipherment = DecodeADFGX(msg, bestKey);
                    decipherment = DecodeADFGX(msg, bestKey, 5000);
                    adfgxKey = CipherLib.Annealing.GetKeyFromPlaintext(decipherment, TransposeADFGX(msg, bestKey), ADFGXALPHABET);
                    CipherLib.Annealing.DisplayPolybiusSquare("ADFGX", "ADFGX", adfgxKey);
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

        static float Score(string msg, int[] key)
        {
            string decodedMsg = "";
            float score = 0;

            if (true)
            {                 
                decodedMsg = DecodeADFGX(msg, key);

                //Console.Write("\n\n");
                //Console.Write(decodedMsg);
                //Console.Write("\n\n");

                score = CipherLib.Annealing.QuadgramScore(decodedMsg);
                //score = CipherLib.Annealing.ChiSquared(decodedMsg);

                //Console.Write("\n\n");
                //Console.Write(score);
                //Console.Write("\n\n");
            }
            else
            {
                score = IOCADFGX(msg, key);
            }

            return score;
        }

        static string TransposeADFGX(string msg, int[] key)
        {
            //string decodedMsg = "";
            StringBuilder decodedMsg = new StringBuilder();
            float columnLength = msg.Length / key.Length;

            for (int i = 0; i < columnLength; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    for (int k = 0; k < key.Length; k++)
                    {
                        if (key[k] == j)
                        {
                            //decodedMsg += msg[k * i + I];
                            //decodedMsg += msg[k * key.Length + i];
                            //decodedMsg += msg[(int)(k * columnLength + i)];
                            decodedMsg.Append(msg[(int)(k * columnLength + i)]);
                        }
                    }
                }
            }

            //Console.Write("Transposed:\n");
            //Console.Write(decodedMsg);

            //string replacedMsg = "";
            StringBuilder replacedMsg = new StringBuilder();

            for (int i = 0; i < decodedMsg.Length; i += 2)
            {
                //replacedMsg += DigramToMonogramADFGX(decodedMsg[i], decodedMsg[i + 1]);
                replacedMsg.Append(DigramToMonogramADFGX(decodedMsg[i], decodedMsg[i + 1]));
            }

            //Console.Write("\n\nReplaved:");
            //Console.Write(replacedMsg);
            //Console.Write("\n\n");

            //return CipherLib.Annealing.CrackMonoSub(replacedMsg, 1000);
            //return replacedMsg;
            //return CipherLib.Annealing.CrackMonoSub(replacedMsg, 100);
            //return CipherLib.Annealing.CrackMonoSub(replacedMsg.Remove(100), 1000);
            //return replacedMsg;
            return replacedMsg.ToString();
        }

        //static string DecodeADFGX(string msg, int[] key)
        static string DecodeADFGX(string msg, int[] key, int trials = 1000)
        {
            //return CipherLib.Annealing.CrackMonoSub(TransposeADFGX(msg, key), 1000);
            //return CipherLib.Annealing.CrackMonoSub(TransposeADFGX(msg, key), 10000);
            //return CipherLib.Annealing.CrackMonoSub(TransposeADFGX(msg, key), trials);
            //return CipherLib.Annealing.CrackMonoSub(TransposeADFGX(msg, key), trials, CipherLib.SolutionType.ChiSquared);
            return CipherLib.Annealing.CrackMonoSub(TransposeADFGX(msg, key), trials, CipherLib.SolutionType.QuadgramScore);
        }

        static float IOCADFGX(string msg, int[] key)
        {
            //DisplayKey(key);
            //Console.Write(CipherLib.Annealing.IOC(TransposeADFGX(msg, key)));

            return CipherLib.Annealing.IOC(TransposeADFGX(msg, key));
        }

        static Tuple<int[], float> AnnealingADFGX(string msg, int[] key, float temperature, float step, int count, int[] overallBestKey)
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

                //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" + zeroes + t);
                Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b" + zeroes + t.ToString("n1"));

                for (int trial = 0; trial < count; trial++)
                {
                    /*Console.Write("\n");
                    DisplayKey(bestKey);
                    Console.Write("\n");
                    DisplayKey(currentKey);*/

                    //for (int i = 0; i < CipherLib.Annealing.rand.Next() % 2 + 1; i++)
                    //for (int i = 0; i < CipherLib.Annealing.rand.Next() % 3 + 1; i++)
                    for (int i = 0; i < CipherLib.Annealing.rand.Next() % 6 + 1; i++)
                    //for (int i = 0; i < 1; i++)
                    {
                        //Array.Copy(MessAroundWithKey(currentKey), currentKey, currentKey.Length);
                        Array.Copy(CipherLib.Annealing.MessWithIntKey(currentKey), currentKey, currentKey.Length);
                    }

                    /*Console.Write("\n");
                    DisplayKey(bestKey);
                    Console.Write("\n");
                    DisplayKey(currentKey);
                    Console.Write("\n");*/

                    //if (IOCADFGX(msg, key) > LOWERIOCLIMIT)
                    //Console.Write("\n\n");
                    //DisplayKey(currentKey);
                    //Console.Write("\n");

                    ioc = IOCADFGX(msg, currentKey);

                    //if (IOCADFGX(msg, currentKey) > LOWERIOCLIMIT)
                    if (ioc > LOWERIOCLIMIT)
                    //if (ioc > LOWERIOCLIMIT && ioc < UPPERIOCLIMIT)
                    {
                        //Console.Write("\nI've got one!\n");
                        //Console.Write("Substitution!\n");
                        currentScore = Score(msg, currentKey);

                        if (currentScore > bestScore)
                        //if (currentScore < bestScore)
                        {
                            //Console.Write("Better!\n");
                            bestScore = currentScore;
                            Array.Copy(currentKey, bestKey, currentKey.Length);
                        }
                        else if (currentScore < bestScore)
                        //else if (currentScore > bestScore)
                        {

                            bool ReplaceAnyway;
                            ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, t);
                            //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(bestScore - currentScore, t);

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
                    //else if (currentScore < bestScore)
                    /*else
                    {

                        bool ReplaceAnyway;
                        //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(currentScore - bestScore, t);
                        ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(ioc - LOWERIOCLIMIT, t);

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            Array.Copy(currentKey, bestKey, currentKey.Length);
                        }
                        else
                        {
                            Array.Copy(bestKey, currentKey, bestKey.Length);
                        }
                    }*/

                    while (Console.KeyAvailable)
                    {
                        Pause(msg, overallBestKey);
                    }
                }               
            }

            return new Tuple<int[], float>(bestKey, bestScore);
        }

        static int[] MessAroundWithKey(int[] key)
        {
            //key[CipherLib.Annealing.rand.Next() % key.Length] = CipherLib.Annealing.rand.Next() % key.Length;

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

            //int length = CipherLib.Annealing.rand.Next() % (key.Length - 1);

            int temp;

            for (int i = 0; i < length; i++)
            {
                temp = key[index1 + i];
                key[index1 + i] = key[index2 + i];
                key[index2 + i] = temp;
            }

            return key;
        }

        static void DisplayKey(int[] key)
        {
            for (int i = 0; i < key.Length; i++)
            {
                Console.Write(key[i]);

                if (i < key.Length - 1)
                {
                    Console.Write(", ");
                }
            }
        }

        static char DigramToMonogramADFGX(char char1, char char2)
        {
            int index = 0;

            // Row coord
            if (char1 == 'a')
            {
                index += 0;
            }
            else if (char1 == 'd')
            {
                index += 5;
            }
            else if (char1 == 'f')
            {
                index += 10;
            }
            else if (char1 == 'g')
            {
                index += 15;
            }
            else if (char1 == 'x')
            {
                index += 20;
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
            else if (char2 == 'x')
            {
                index += 4;
            }

            return ADFGXALPHABET[index];
        }

        static void Pause(string msg, int[] bestKey)
        {
            Console.Write("\n");
            string input = Console.ReadLine();

            if (input == "r")
            {
                Console.Write("\n\n-----------------------\n\n");
                Console.Write("Refined output of best key so far:\n\n");
                Console.Write("Best key: ");
                DisplayKey(bestKey);
                Console.Write("\n\n");

                string decipherment = DecodeADFGX(msg, bestKey, 10000);

                string adfgxKey = CipherLib.Annealing.GetKeyFromPlaintext(decipherment, TransposeADFGX(msg, bestKey), ADFGXALPHABET);
                CipherLib.Annealing.DisplayPolybiusSquare("ADFGX", "ADFGX", adfgxKey);
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
