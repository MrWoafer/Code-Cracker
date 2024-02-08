using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpPollux
{
    class Program
    {
        const int numOfTrials = 10000;
        //const int numOfTrials = 100000;

        static void Main(string[] args)
        {
            Console.Write("args: ");
            foreach (string i in args)
            {
                Console.Write(i + " ");
            }
            Console.Write("\n\n");

            Console.Write("-- C# Pollux Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string ciphertext = System.IO.File.ReadAllText("--PolluxMessage.txt");
            Console.Write("Ciphertext:\n");
            Console.Write("-----------\n");
            Console.Write(ciphertext);
            Console.Write("\n\n-----------------------\n\n");

            string alphabet;
            int numDash;
            int numDot;
            int numSep;
            if (args.Length > 0)
            {
                alphabet = args[0];
                numDash = Int32.Parse(args[1]);
                numDot = Int32.Parse(args[2]);
                numSep = Int32.Parse(args[3]);
            }
            else
            {
                //if (true)
                if (false)
                {
                    alphabet = "abcdefghijklmnopqrstuvwxyz0123456789";
                    numDash = 11;
                    //numDash = 12;
                    numDot = 14;
                    //numDot = 11;
                    //numDot = 12;
                    numSep = 11;
                    //numSep = 14;
                    //numSep = 12;
                }
                else
                {
                    alphabet = "0123456789";
                    numDash = 3;
                    numDot = 4;
                    numSep = 3;
                }
            }

            Console.Write("Alphabet: " + alphabet);
            Console.Write("\n\n");
            Console.Write("Num Of Dashes: " + numDash);
            Console.Write("\n\n");
            Console.Write("Num Of Dots: " + numDot);
            Console.Write("\n\n");
            Console.Write("Num Of Separators: " + numSep);
            Console.Write("\n\n");
            Console.Write("-----------------------\n\n");

            //char[] testDashes = "145BCGJNRTW".ToLower().ToCharArray();
            //char[] testDots = "0378AEFMOPQXYZ".ToLower().ToCharArray();
            //char[] testSeps = "269DHIKLSUV".ToLower().ToCharArray();
            //Console.Write(CipherLib.Morse.DecodePollux(ciphertext, testDashes, testDots, testSeps));

            /// key[0] is dashes; key[1] is dots; key[2] is separators (seps).
            List<char>[] currentKey = new List<char>[3];
            for (int i = 0; i < currentKey.Length; i++)
            {
                currentKey[i] = new List<char>();
            }
            List<char>[] bestKey = new List<char>[3];
            for (int i = 0; i < bestKey.Length; i++)
            {
                bestKey[i] = new List<char>();
            }

            /*for (int i = 0; i < alphabet.Length; i++)
            {
                currentKey[i % 3].Add(alphabet[i]);
                bestKey[i % 3].Add(alphabet[i]);
            }*/

            if (numDash + numDot + numSep != alphabet.Length)
            {
                Console.Write("ERROR: The provided total number of morse symbols does not match the length of the provided alphabet.");
            }
            else
            {
                //if (false)
                if (true)
                {
                    int outerIndex;
                    int outerIndex2;
                    int innerIndex;
                    int innerIndex2;

                    char temp;

                    for (int i = 0; i < numDash; i++)
                    {
                        currentKey[0].Add(alphabet[i]);
                        bestKey[0].Add(alphabet[i]);
                    }
                    for (int i = 0; i < numDot; i++)
                    {
                        currentKey[1].Add(alphabet[numDash + i]);
                        bestKey[1].Add(alphabet[numDash + i]);
                    }
                    for (int i = 0; i < numSep; i++)
                    {
                        currentKey[2].Add(alphabet[numDash + numDot + i]);
                        bestKey[2].Add(alphabet[numDash + numDot + i]);
                    }

                    for (int i = 0; i < 1000; i++)
                    {
                        outerIndex = CipherLib.Utils.rand.Next() % 3;
                        while (currentKey[outerIndex].Count <= 1)
                        {
                            outerIndex = CipherLib.Utils.rand.Next() % 3;
                        }
                        outerIndex2 = CipherLib.Utils.rand.Next() % 3;
                        while (outerIndex == outerIndex2)
                        {
                            outerIndex2 = CipherLib.Utils.rand.Next() % 3;
                        }

                        innerIndex = CipherLib.Utils.rand.Next() % currentKey[outerIndex].Count;
                        innerIndex2 = CipherLib.Utils.rand.Next() % currentKey[outerIndex2].Count;

                        temp = currentKey[outerIndex][innerIndex];
                        //currentKey[outerIndex].RemoveAt(innerIndex);
                        //currentKey[outerIndex2].Add(temp);
                        currentKey[outerIndex][innerIndex] = currentKey[outerIndex2][innerIndex2];
                        currentKey[outerIndex2][innerIndex2] = temp;
                    }

                    //currentKey = new List<char>[] { new List<char>("145BCGJNRTW".ToLower().ToCharArray()), new List<char>("0378AEFMOPQXYZ".ToLower().ToCharArray()), new List<char>("269DHIKLSUV".ToLower().ToCharArray()) };
                    //bestKey = new List<char>[] { new List<char>("145BCGJNRTW".ToLower().ToCharArray()), new List<char>("0378AEFMOPQXYZ".ToLower().ToCharArray()), new List<char>("269DHIKLSUV".ToLower().ToCharArray()) };

                    Console.Write("Starting Key:\n");
                    DisplayKey(currentKey);
                    Console.Write("\n\n");

                    //float currentScore = float.NegativeInfinity;
                    float currentScore = float.PositiveInfinity;
                    float bestScore = currentScore;

                    if (Decode(ciphertext, currentKey) != null)
                    {
                        //currentScore = CipherLib.Annealing.QuadgramScore(Decode(ciphertext, currentKey));
                        //currentScore = -CipherLib.Annealing.ChiSquared(Decode(ciphertext, currentKey));
                        //currentScore = Score(ciphertext, currentKey);
                        currentScore = CipherLib.Annealing.ChiSquared(Decode(ciphertext, currentKey));
                        bestScore = currentScore;
                    }

                    string plaintext;
                    for (int trial = 0; trial < numOfTrials; trial++)
                    {
                        //if (trial % 10 == 0)
                        if (trial % 100 == 0)
                        {
                            //Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\bTrial: " + trial);
                            Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bTrial: " + trial.ToString() + " / " + numOfTrials.ToString());
                        }
                        for (int i = 0; i < 5; i++)
                        {
                            outerIndex = CipherLib.Utils.rand.Next() % 3;
                            while (currentKey[outerIndex].Count <= 1)
                            {
                                outerIndex = CipherLib.Utils.rand.Next() % 3;
                            }
                            outerIndex2 = CipherLib.Utils.rand.Next() % 3;
                            while (outerIndex == outerIndex2)
                            {
                                outerIndex2 = CipherLib.Utils.rand.Next() % 3;
                            }

                            innerIndex = CipherLib.Utils.rand.Next() % currentKey[outerIndex].Count;
                            innerIndex2 = CipherLib.Utils.rand.Next() % currentKey[outerIndex2].Count;

                            temp = currentKey[outerIndex][innerIndex];
                            //currentKey[outerIndex].RemoveAt(innerIndex);
                            //currentKey[outerIndex2].Add(temp);
                            currentKey[outerIndex][innerIndex] = currentKey[outerIndex2][innerIndex2];
                            currentKey[outerIndex2][innerIndex2] = temp;
                        }

                        //DisplayKey(currentKey);
                        //Console.Write("\n\n");

                        plaintext = Decode(ciphertext, currentKey);
                        //plaintext = "";
                        if (plaintext != null)
                        {
                            //currentScore = CipherLib.Annealing.QuadgramScore(plaintext);
                            //currentScore = -CipherLib.Annealing.ChiSquared(plaintext);
                            //currentScore = Score(ciphertext, currentKey);
                            currentScore = CipherLib.Annealing.ChiSquared(plaintext);

                            //if (currentScore > bestScore)
                            if (currentScore < bestScore)
                            {
                                bestScore = currentScore;
                                bestKey = new List<char>[3];
                                for (int i = 0; i < currentKey.Length; i++)
                                {
                                    bestKey[i] = new List<char>();
                                    for (int j = 0; j < currentKey[i].Count; j++)
                                    {
                                        bestKey[i].Add(currentKey[i][j]);
                                    }
                                    //bestKey[i] = new List<char>(currentKey[i]);
                                }
                            }
                            else if (currentScore > bestScore)
                            {
                                bool ReplaceAnyway;

                                ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(bestScore - currentScore, numOfTrials - trial);
                                //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(bestScore - currentScore, 20f);
                                //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(bestScore - currentScore, 20f * (trial - numOfTrials) / numOfTrials);
                                //ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(bestScore - currentScore, 20f * (numOfTrials - trial) / numOfTrials);

                                if (ReplaceAnyway == true)
                                {
                                    bestScore = currentScore;
                                    bestKey = new List<char>[3];
                                    for (int i = 0; i < currentKey.Length; i++)
                                    {
                                        bestKey[i] = new List<char>();
                                        for (int j = 0; j < currentKey[i].Count; j++)
                                        {
                                            bestKey[i].Add(currentKey[i][j]);
                                        }
                                    }
                                }
                                else
                                {
                                    currentKey = new List<char>[3];
                                    for (int i = 0; i < bestKey.Length; i++)
                                    {
                                        currentKey[i] = new List<char>();
                                        for (int j = 0; j < bestKey[i].Count; j++)
                                        {
                                            currentKey[i].Add(bestKey[i][j]);
                                        }
                                    }
                                }
                            }
                            else
                            {
                                currentKey = new List<char>[3];
                                //for (int i = 0; i < currentKey.Length; i++)
                                for (int i = 0; i < bestKey.Length; i++)
                                {
                                    //currentKey = new List<char>[3];
                                    //currentKey[i] = new List<char>(bestKey[i]);
                                    currentKey[i] = new List<char>();
                                    for (int j = 0; j < bestKey[i].Count; j++)
                                    {
                                        currentKey[i].Add(bestKey[i][j]);
                                    }
                                }
                            }
                        }
                        else if (plaintext == null && float.IsNegativeInfinity(bestScore))
                        {
                            bestKey = new List<char>[3];
                            for (int i = 0; i < currentKey.Length; i++)
                            {
                                //bestKey[i] = new List<char>();
                                /*for (int j = 0; j < currentKey[i].Count; j++)
                                {
                                    bestKey[i].Add(currentKey[i][j]);
                                }*/
                                bestKey[i] = new List<char>(currentKey[i]);
                            }
                        }
                        else
                        {
                            currentKey = new List<char>[3];
                            for (int i = 0; i < bestKey.Length; i++)
                            {
                                currentKey[i] = new List<char>();
                                for (int j = 0; j < bestKey[i].Count; j++)
                                {
                                    currentKey[i].Add(bestKey[i][j]);
                                }
                            }
                        }
                    }
                    //Console.Write("\b\b\b\b\b\b\b\b\b\\b\b\b\b\b\b\b\b");
                    Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\b\bTrial: " + numOfTrials.ToString() + " / " + numOfTrials.ToString());
                    Console.Write("\n\n");
                    //Console.Write("BestKey:\n\n");
                    Console.Write("Best Key:\n\n");
                    DisplayKey(bestKey);
                    Console.Write("\n\n");
                    Console.Write(Decode(ciphertext, bestKey));
                }

                //if (true)
                if (false)
                {
                    float currentScore = float.PositiveInfinity;
                    float bestScore = currentScore;

                    int outerIndex;
                    int outerIndex2;
                    int innerIndex;

                    char temp;
                    string plaintext;
                    for (int trial = 0; trial < numOfTrials; trial++)
                    {
                        //if (trial % 100 == 0)
                        if (trial % 1000 == 0)
                        {
                            Console.Write("\b\b\b\b\b\b\b\b\b\b\b\b\b\bTrial: " + trial);
                        }
                        outerIndex = CipherLib.Utils.rand.Next() % 3;
                        while (currentKey[outerIndex].Count <= 1)
                        {
                            outerIndex = CipherLib.Utils.rand.Next() % 3;
                        }
                        outerIndex2 = CipherLib.Utils.rand.Next() % 3;
                        while (outerIndex == outerIndex2)
                        {
                            outerIndex2 = CipherLib.Utils.rand.Next() % 3;
                        }

                        innerIndex = CipherLib.Utils.rand.Next() % currentKey[outerIndex].Count;

                        temp = currentKey[outerIndex][innerIndex];
                        currentKey[outerIndex].RemoveAt(innerIndex);
                        currentKey[outerIndex2].Add(temp);

                        currentScore = Score(ciphertext, currentKey);
                        if (currentScore < bestScore)
                        {
                            bestScore = currentScore;
                            bestKey = new List<char>[3];
                            for (int i = 0; i < currentKey.Length; i++)
                            {
                                //bestKey[i] = new List<char>();
                                /*for (int j = 0; j < currentKey[i].Count; j++)
                                {
                                    bestKey[i].Add(currentKey[i][j]);
                                }*/
                                bestKey[i] = new List<char>(currentKey[i]);
                            }
                        }
                        else
                        {
                            currentKey = new List<char>[3];
                            for (int i = 0; i < currentKey.Length; i++)
                            {
                                //currentKey = new List<char>[3];
                                currentKey[i] = new List<char>(bestKey[i]);
                            }
                        }
                    }
                    Console.Write("\n\n");
                    Console.Write("Best Key:\n\n");
                    DisplayKey(bestKey);
                    Console.Write("\n\n");
                    Console.Write(Decode(ciphertext, bestKey));
                }
            }

            Console.Write("\n\n-----------------------\n\nPress ENTER to close...");
            Console.ReadLine();
        }

        public static void DisplayKey(List<char>[] key)
        {
            //Console.Write("Dashes: ");
            Console.Write("Dashes:\t\t");
            for (int i = 0; i < key[0].Count; i++)
            {
                Console.Write(key[0][i]);
                if (i < key[0].Count - 1)
                {
                    Console.Write(" ");
                }
            }
            Console.Write("\n");
            //Console.Write("Dots: ");
            Console.Write("Dots:\t\t");
            for (int i = 0; i < key[1].Count; i++)
            {
                Console.Write(key[1][i]);
                if (i < key[1].Count - 1)
                {
                    Console.Write(" ");
                }
            }
            Console.Write("\n");
            //Console.Write("Separators: ");
            Console.Write("Separators:\t");
            for (int i = 0; i < key[2].Count; i++)
            {
                Console.Write(key[2][i]);
                if (i < key[2].Count - 1)
                {
                    Console.Write(" ");
                }
            }
        }

        public static string Decode(string ciphertext, List<char>[] key)
        {
            return CipherLib.Morse.DecodePollux(ciphertext, key[0].ToArray(), key[1].ToArray(), key[2].ToArray());
        }

        public static string PolluxToMorse(string ciphertext, List<char>[] key)
        {
            return CipherLib.Morse.SubPollux(ciphertext, key[0].ToArray(), key[1].ToArray(), key[2].ToArray());
        }

        public static float Score(string ciphertext, List<char>[] key)
        {
            int score = 0;

            //StringBuilder decoded = new StringBuilder();
            
            ciphertext = PolluxToMorse(ciphertext, key);

            foreach (string i in ciphertext.Split('/'))
            {
                if (i == "")
                {
                }
                else if (CipherLib.Morse.MorseToCharSuperRestricted(i) != null)
                {
                    //decoded.Append(CipherLib.Morse.MorseToCharSuperRestricted(i));
                }
                else
                {
                    score++;
                }
            }
            return score;
        }
    }
}
