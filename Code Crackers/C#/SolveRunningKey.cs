using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpRunningKey
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

            Console.Write("-- C# Running Key Solver --");
            Console.Write("\n");
            Console.Write("-----------------------\n");
            Console.Write("\n");

            string msg = System.IO.File.ReadAllText("--RunningKeyMessage.txt");
            string crib = System.IO.File.ReadAllText("--RunningKeyCrib.txt");
            Console.Write("Ciphertext:\n");          
            Console.Write("-----------\n");
            Console.Write(msg);
            Console.Write("\n\n");
            Console.Write("Crib:\n");
            Console.Write("-----\n");
            Console.Write(crib);

            int aIndex;
            CipherLib.DecryptionType decryptType;
            if (args.Length > 0)
            {
                aIndex = Int32.Parse(args[0]);
                decryptType = (CipherLib.DecryptionType)Int32.Parse(args[1]);
            }
            else
            {
                aIndex = 0;
                decryptType = CipherLib.DecryptionType.subtraction;
            }

            string key = CipherLib.RunningKey.DeduceKey(crib, msg, decryptType, aIndex);

            /*for (int i = key.Length; i < msg.Length; i++)
            {
                key += '.';
            }*/

            Console.Write("\n\n");
            Console.Write("Starting Key:\n");
            Console.Write("-------------\n");
            Console.Write(key);

            //Console.Write("\n\n-----------------------\n\n");

            Console.Write("\n\n");

            Console.Write("Using Alphabet: " + CipherLib.RunningKey.ALPHABET);
            Console.Write("\n\nIndex Of 'A': " + aIndex.ToString());
            Console.Write("\n\n-----------------------\n\n");

            /*string dictionaryText = System.IO.File.ReadAllText("==FullDictionary.txt");

            int numOfWords = 0;

            for (int i = 0; i < dictionaryText.Length; i++)
            {
                if (dictionaryText[i] == '\n')
                {
                    numOfWords += 1;
                }
            }
            numOfWords += 1;

            string[] dictionary = new string[numOfWords];*/
            //int j;
            //for (int i = 0; i < dictionary.Length; i++)
            //{
            //j = i;
            //Console.Write("\n" + i);
            /*
            for (int j = 0; j < dictionaryText.Length; j++)
            {
                if (dictionaryText[j] == '\n')
                {
                    dictionary[i] = "";

                    for (int k = 0; k < j; k++)
                    {
                        dictionary[i] += dictionaryText[0];
                        //dictionaryText = dictionaryText.Remove(0, 1);
                        dictionaryText = dictionaryText.Substring(1);
                    }                    
                    //dictionaryText = dictionaryText.Remove(0, 1);
                    dictionaryText = dictionaryText.Substring(1);
                    dictionary[i] = dictionary[i].ToLower();
                    j = 0;
                    Console.Write("\n" + dictionary[j]);
                }
            }*/
            //}
            //dictionary[dictionary.Length - 1] = dictionaryText;

            //string[] dictionary = (string[])System.IO.File.ReadLines("==FullDictionary.txt");
            //string[] dictionary = System.IO.File.ReadAllLines("==FullDictionary.txt");
            string[] dictionary = System.IO.File.ReadAllLines("==LargeDictionary.txt");

            for (int i = 0; i < dictionary.Length; i++)
            {
                dictionary[i] = dictionary[i].ToLower();
                //Console.Write("\n" + dictionary[i]);
            }

            Console.Write("Beginning solution...");

            Console.Write("\n\n-----------------------\n\n");

            string newKey;
            string bestKey = key;

            float newScore;
            float bestScore = -9999999;

            string overallBestKey = key;
            float overallBestScore = -99999999999;

            overallBestScore = Score(msg, overallBestKey, decryptType, aIndex);

            //string keyWithoutChanges;

            //string newSection = "";
            //bool inEnglishWord;
            string decipherment;

            int[][] perms;

            //perms = CipherLib.Annealing.NToTheMArrangements(26, 5);
            perms = CipherLib.Annealing.NToTheMArrangements(26, 4);

            List<string> bestKeys = new List<string>();

            Queue<string> keyQueue = new Queue<string>();

            keyQueue.Enqueue(key);

            //while (key)
            //for (int word = 0; word < 5; word++)
            for (int trial = 0; trial < 26; trial++)
            {
                key = keyQueue.Dequeue();
                //keyWithoutChanges = key;

                //for (int i = 0; i < dictionary.Length; i++)
                for (int i = 0; i < perms.Length; i++)
                {
                    //Console.Write("\n" + dictionary[i]);

                    //newKey = key + dictionary[i];
                    //newKey = keyWithoutChanges;

                    newKey = key;

                    for (int j = 0; j < perms[i].Length; j++)
                    {
                        //newKey += perms[i][j];
                        newKey += CipherLib.Annealing.ALPHABET[perms[i][j]];
                    }

                    /*for (int j = 0; j < newKey.Length; j++)
                    {
                        if (newKey[j] == '.')
                        {
                            newKey = CipherLib.Annealing.OverlayString(dictionary[i], j, newKey);
                            break;
                        }
                    }*/

                    if (i == 0)
                    {
                        bestKey = newKey;
                        //bestScore = Score(msg, bestKey, dictionary[i].Length, decryptType, aIndex);
                        bestScore = Score(msg, bestKey, decryptType, aIndex);

                        bestKeys.Add(bestKey);
                    }
                    else
                    {
                        //inEnglishWord = false;
                        //newSection = "";
                        decipherment = CipherLib.RunningKey.DecryptRunningKeyPartial(msg, newKey, decryptType, aIndex);

                        /*for (int j = key.Length; j < newKey.Length; j++)
                        {
                            newSection += decipherment[j];
                        }*/
                        /*for (int j = 0; j < dictionary.Length; j++)
                        {
                            //if (dictionary[j].Contains(newSection))
                            //if (false)
                            if (true)
                            {
                                inEnglishWord = true;
                            }
                        }*/

                        //newScore = Score(msg, newKey, dictionary[i].Length, decryptType, aIndex);
                        newScore = Score(msg, newKey, decryptType, aIndex);

                        /*if (dictionary[i] == "memo")
                        {
                            Console.Write("\nMemo;");
                        }*/

                        if (newScore > bestScore)
                        //if (newScore < bestScore)
                        {
                            bestScore = newScore;
                            bestKey = newKey;

                            //Console.Write("\nI think this is better: " + bestKey);

                            bestKeys.Insert(0, bestKey);

                            /*if (dictionary[i] == "memo")
                            {
                                Console.Write("\nMemo chosen;");
                            }*/

                            if (newScore > overallBestScore)
                            {
                                overallBestScore = newScore;
                                overallBestKey = newKey;

                                Console.Write("\nNew best overall key: " + overallBestKey);
                            }
                        }
                    }                    

                    /*if (inEnglishWord)
                    {

                    }*/

                    //Console.Write("\n" + dictionary[i]);                    
                }

                Console.Write("\nBest extensions for key: " + key + " :\n");

                //for (int j = 0; j < 5 && j < bestKeys.Count(); j++)
                for (int j = 0; j < 10 && j < bestKeys.Count(); j++)
                {
                    keyQueue.Enqueue(bestKeys[j]);
                    Console.Write("\n" + bestKeys[j].Remove(0, bestKeys[j].Length-4));
                }

                bestKeys.Clear();
                //}
                //}

                /*perms = CipherLib.Annealing.NToTheMArrangements(26, 5);

                for (int i = 0; i < perms.Length; i++)
                {
                    for (int j = 0; j < perms[i].Length; j++)
                    {
                        newSection += (char)(perms[i][j] + 97);
                    }
                }*/

                key = bestKey;

                //Console.Write("\n\n");
                //Console.Write("Next pieced-together key: " + key);
                Console.Write("\n");
                Console.Write("\n");
                Console.Write("Next pieced-together key:\n");
                Console.Write("-------------------------\n");
                Console.Write(key);
                Console.Write("\n\n");
                Console.Write("Corresponding plaintext:\n");
                Console.Write("------------------------\n");
                Console.Write(CipherLib.RunningKey.DecryptRunningKeyPartial(msg, key, decryptType, aIndex));
                Console.Write("\n\n-----------------------\n\n");
            }

            Console.Write("\n\n-----------------------\n\n");
            Console.Write("Program finished.");
            Console.Write("\n\nPress ENTER to exit...");
            Console.Read();
        }

        //private static float Score(string msg, string key, int lengthOfWord, CipherLib.DecryptionType decryptType, int aIndex)
        private static float Score(string msg, string key, CipherLib.DecryptionType decryptType, int aIndex)
        {
            /*for (int i = 0; i < msg.Length; i++)
            {
                if (key[i] == '.')
                {
                    key = CipherLib.Annealing.SetChar('a', i, key);
                }
            }*/

            string decipherment = CipherLib.RunningKey.DecryptRunningKeyPartial(msg, key, decryptType, aIndex);

            float score = CipherLib.Annealing.QuadgramScore(decipherment);

            score += CipherLib.Annealing.QuadgramScore(key);

            //score /= msg.Length;
            //score /= lengthOfWord;
            //score *= lengthOfWord;
            //score *= msg.Length;
            //float score = CipherLib.Annealing.ChiSquared(decipherment);

            //score /= 8*key.Length + CipherLib.Annealing.QuadgramScore(decipherment);
            //score /= CipherLib.Annealing.ChiSquared(decipherment);

            //score += lengthOfWord * 7;
            //score += lengthOfWord * 8;
            //score += lengthOfWord * 4;
            //score += lengthOfWord * 5;
            //score += lengthOfWord * 3;

            //score += lengthOfWord / msg.Length;
            //score += lengthOfWord;
            //score += lengthOfWord / 10;
            //score += lengthOfWord / 100;

            //return CipherLib.Annealing.QuadgramScore(decipherment);
            return score;
        }
    }
}
