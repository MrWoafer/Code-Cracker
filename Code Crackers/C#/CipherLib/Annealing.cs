using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    public enum SolutionType
    {
        QuadgramScore = 0,
        ChiSquared = 1
    }

    public enum TranspositionType
    {
        row = 0,
        column = 1,
        myszkowski = 2,
        amsco = 3
    }

    public static class Annealing
    {
        public const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";
        public static Random rand = new Random();
        //public const string orderedlettersonavgfreq = "etaoinsrhdlucmfywgpbvkxqjz0123456789";
        //public const string orderedlettersonavgfreq = "etaoinsrhdlcumfwgypbvkxjqz0123456789";
        public const string orderedlettersonavgfreq = "etaoinsrhdlcumfwgypbvkxjqz0123456789#/";
        //public const string orderedlettersonavgfreq = "abcdefghijklmnopqrstuvwxyz";
        public static readonly float[] averageletterfrequencies = new float[26]
        {8.167f,
        1.492f,
        2.782f,
        4.253f,
        12.702f,
        2.228f,
        2.015f,
        6.094f,
        6.966f,
        0.153f,
        0.772f,
        4.025f,
        2.406f,
        6.749f,
        7.507f,
        1.929f,
        0.095f,
        5.987f,
        6.327f,
        9.056f,
        2.758f,
        0.978f,
        2.360f,
        0.150f,
        1.974f,
        0.074f};

        public static float QuadgramScore(string msg)
        {
            float Score = 0;
            int index = 0;
            for (int i = 0; i < msg.Length - 3; i++)
            {
                index = (msg[i + 0] - 97) * 17576 + (msg[i + 1] - 97) * 676 + (msg[i + 2] - 97) * 26 + (msg[i + 3] - 97);
                //if (index >= 0)
                //if (index >= 0 && index < QuadScores.QUADGRAMSCORES.Length)
                if (index >= 0 && index < 456976)
                {
                    Score += QuadScores.QUADGRAMSCORES[index];
                }
                else
                {
                    Score += -8;
                }
            }
            return Score;
        }

        public static int AlphabetIndex(char letter)
        {
            char newLetter;
            for (int i = 0; i < 26; i++)
            {
                newLetter = ALPHABET[i];
                if (newLetter == letter)
                {
                    return i;
                }
            }
            return -1;
        }

        public static float Mod(float x, float y)
        {
            return (x % y + y) % y;
        }

        public static int Mod(int x, int y)
        {
            return (x % y + y) % y;
        }

        public static bool AnnealingProbability(float difference, float temperature)
        {
            //double Probability = Math.Pow(Math.E, difference / temperature);
            double Probability = Math.Exp(difference / temperature);
            double ProbabilityNum = new Random().NextDouble();

            if (Probability >= ProbabilityNum)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string CrackMonoSub(string msg, int numOfTrials, SolutionType solutionType = SolutionType.QuadgramScore)
        {
            string currentKey = OrderedFrequencyKey(msg);
            //string currentKey = "abcdefghijklmnopqrstuvwxyz";
            string bestKey = currentKey;

            float currentScore;

            bool higherIsBetter;

            if (solutionType == SolutionType.QuadgramScore)
            {
                higherIsBetter = true;
            }
            else if (solutionType == SolutionType.ChiSquared)
            {
                higherIsBetter = false;
            }
            else
            {
                higherIsBetter = true;
            }

            if (solutionType == SolutionType.QuadgramScore)
            {
                 currentScore = QuadgramScore(DecodeMonoSub(msg, currentKey));
            }
            else if (solutionType == SolutionType.ChiSquared)
            {
                currentScore = ChiSquared(DecodeMonoSub(msg, currentKey));
            }
            else
            {
                currentScore = QuadgramScore(DecodeMonoSub(msg, currentKey));
            }
            
            float bestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                currentKey = MessWithStringKey(currentKey);

                decodedMsg = DecodeMonoSub(msg, currentKey);

                if (solutionType == SolutionType.QuadgramScore)
                {
                    currentScore = QuadgramScore(decodedMsg);
                }
                else if (solutionType == SolutionType.ChiSquared)
                {
                    currentScore = ChiSquared(decodedMsg);
                }

                if (higherIsBetter)
                {
                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestKey = currentKey;
                    }
                    else
                    {
                        currentKey = bestKey;
                    }
                }
                else
                {
                    if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        bestKey = currentKey;
                    }
                    else
                    {
                        currentKey = bestKey;
                    }
                }
                
            }

            return DecodeMonoSub(msg, bestKey);
        }

        public static string DecodeMonoSub(string msg, string key)
        {
            //string newMsg = "";
            StringBuilder newMsg = new StringBuilder();

            for (int i = 0; i < msg.Length; i++)
            {
                //newMsg += key[msg[i] - 97];
                newMsg.Append(key[msg[i] - 97]);
            }

            //return newMsg;
            return newMsg.ToString();
        }

        public static string MessWithStringKey(string key)
        {
            int index1 = rand.Next() % key.Length;
            int index2 = rand.Next() % key.Length;

            char temp = key[index1];

            key = key.Insert(index1, key[index2].ToString());
            key = key.Remove(index1 + 1, 1);

            key = key.Remove(index2, 1);
            key = key.Insert(index2, temp.ToString());

            return key;
        }

        public static string OrderedFrequencyKey(string msg)
        {
            int[] counts = new int[26];
            string key = "abcdefghijklmnopqrstuvwxyz";

            for (int i = 0; i < 26; i++)
            {
                counts[i] = 0;
            }

            for (int i = 0; i < msg.Length; i++)
            {
                counts[msg[i] - 97] += 1;
            }

            int bestIndex;

            for (int i = 0; i < 26; i++)
            {
                bestIndex = 0;
                for (int j = 1; j < 26 - 0; j++)
                {
                    if (counts[j] > counts[bestIndex])
                    {
                        bestIndex = j;
                    }
                }

                key = key.Remove(bestIndex, 1);
                key = key.Insert(bestIndex, orderedlettersonavgfreq[i].ToString());

                counts[bestIndex] = -1;
            }

            return key;
        }

        public static float IOC(string msg)
        {
            int[] counts = new int[26];
            int total = 0;

            for (int i = 0; i < msg.Length; i++)
            {
                /*if (IsEnglishLetter(msg[i]))
                {
                    counts[msg[i] - 97] += 1;
                    total += 1;
                } */
                counts[msg[i] - 97] += 1;
                total += 1;
            }

            /*Console.Write("\n\n");
            for (int i = 0; i < counts.Length; i++)
            {
                Console.Write(counts[i] + " ");
            }
            Console.Write("\n\n");*/

            double ioc = 0;

            for (int i = 0; i < 26; i++)
            {
                //Console.Write(counts[i] * (counts[i] - 1) + " " + total * (total - 1) + "  ");
                //ioc += (counts[i] * (counts[i] - 1) / (total * (total - 1)));
                //Console.Write(ioc + " ");

                //Console.Write(counts[i] * (counts[i] - 1) / (total * (total - 1)));

                ioc = ioc + (double)counts[i] * ((double)counts[i] - 1) / ((double)total * ((double)total - 1));
            }

            return (float)ioc;
        }

        public static float IOCAlphanumeric(string msg)
        {
            int[] counts = new int[36];
            int total = 0;

            for (int i = 0; i < msg.Length; i++)
            {
                if (msg[i] >= 97)
                {
                    counts[msg[i] - 97] += 1;
                }
                else
                {
                    counts[msg[i] - 22] += 1;
                    
                }
                total += 1;
            }

            double ioc = 0;

            for (int i = 0; i < 36; i++)
            {
                ioc = ioc + (double)counts[i] * ((double)counts[i] - 1) / ((double)total * ((double)total - 1));
            }

            return (float)ioc;
        }

        public static string CrackColumnTranspo(string msg, int keyLength, int numOfTrials)
        {
            int[] currentKey = new int[keyLength];

            for (int i = 0; i < keyLength; i++)
            {
                currentKey[i] = i;
            }
            for (int i = 0; i < 100; i++)
            {
                currentKey = MessWithIntKey(currentKey);
            }
            /*for (int i = 0; i < keyLength; i++)
            {
                Console.Write(currentKey[i] + " ");
            }
            Console.Write("\n");*/

            int[] bestKey = new int[keyLength];

            Array.Copy(currentKey, bestKey, keyLength);

            //float currentScore = QuadgramScore(DecodeColumnTranspo(msg, currentKey));
            float currentScore = QuadgramScore(IncompleteColumnarTranspo(msg, currentKey));
            float bestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                //currentKey = MessWithIntKey(currentKey);
                Array.Copy(MessWithIntKey(currentKey), currentKey, keyLength);
                //Array.Copy(BlockMessWithIntKey(currentKey), currentKey, keyLength);

                //decodedMsg = DecodeColumnTranspo(msg, currentKey);
                decodedMsg = IncompleteColumnarTranspo(msg, currentKey);
                //decodedMsg = msg;

                currentScore = QuadgramScore(decodedMsg);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    Array.Copy(currentKey, bestKey, keyLength);
                }
                else if (currentScore < bestScore)
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = AnnealingProbability(currentScore - bestScore, numOfTrials - trial);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestKey, keyLength);
                    }
                    else
                    {
                        Array.Copy(bestKey, currentKey, keyLength);
                    }
                }
                else
                {
                    Array.Copy(bestKey, currentKey, keyLength);
                }
            }

            /*Console.Write("\n");
            for (int i = 0; i < keyLength; i++)
            {
                Console.Write(currentKey[i] + " ");
            }
            Console.Write("\n");*/

            //return DecodeColumnTranspo(msg, bestKey);
            return IncompleteColumnarTranspo(msg, bestKey);
        }

        public static int[] MessWithIntKey(int[] key)
        {
            int index1 = rand.Next() % key.Length;
            int index2 = rand.Next() % key.Length;

            int temp = key[index1];

            key[index1] = key[index2];
            key[index2] = temp;

            return key;
        }

        public static string DecodeColumnTranspo(string msg, int[] key)
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
                        if (key[k] == j && k * columnLength + i < msg.Length)
                        {
                            //decodedMsg += msg[(int)(k * columnLength + i)];
                            decodedMsg.Append(msg[(int)(k * columnLength + i)]);
                            break;
                        }
                    }
                }
                //decodedMsg += "aaaaaaa";
            }

            //return decodedMsg;
            return decodedMsg.ToString();
        }

        /*public static string DecodeColumnTranspo2(string msg, int[] key)
        {
            string decodedMsg = "";
            for (int i = 0; i < msg.Length; i++)
            {
                decodedMsg += " ";
            }

            int columnLength = (int)((float)msg.Length / (float)key.Length);

            for (int i = 0; i < msg.Length; i++)
            {
                decodedMsg = decodedMsg.Remove(key[i / columnLength] + (i % columnLength) * key.Length, 1);
                decodedMsg = decodedMsg.Insert(key[i / columnLength] + (i % columnLength) * key.Length, msg[i].ToString());
            }

            return decodedMsg;
        }

        public static string DecodeColumnTranspo3(string msg, int[] key)
        {
            string decodedMsg = "";
            float columnLength = msg.Length / key.Length;

            for (int i = 0; i < columnLength; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    decodedMsg += msg[(int)(key[j] * columnLength + i)];
                }               
            }

            return decodedMsg;
        }

        public static string DecodeColumnTranspo4(string msg, int[] key)
        {
            int rowNum;
            if (msg.Length % key.Length == 0)
            {
                rowNum = msg.Length / key.Length;
            }
            else
            {
                rowNum = (int)Math.Ceiling((decimal)(1 + (msg.Length) - (msg.Length % key.Length)) / key.Length);
            }
            string[] rows = new string[rowNum];
            int i;
            int j;

            string row;       
            for (i = 0; i < rowNum; i++)
            {
                row = "";
                for (j = 0; j < key.Length; j++)
                {
                    if (j * rowNum + i < msg.Length)
                    {
                        row += msg[j * rowNum + i];
                    }
                }
                rows[i] = row;
            }

            string newMsg = "";

            for (i = 0; i < rowNum; i++)
            {
                for (j = 0; j < key.Length; j++)
                {
                    if (key[j] < rows[i].Length)
                    {
                        newMsg += rows[i][key[j]];
                    }
                }
            }

            return newMsg;
        }*/

        public static string DecodeRowTranspo(string msg, int[] key)
        {
            //string decodedMsg = "";
            StringBuilder decodedMsg = new StringBuilder();

            for (int i = 0; i < msg.Length; i += key.Length)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    for (int k = 0; k < key.Length; k++)
                    {
                        if (key[k] == j && i + k < msg.Length)
                        {
                            //decodedMsg += msg[(int)(i + k)];
                            decodedMsg.Append(msg[(int)(i + k)]);
                        }
                    }
                }
            }

            //return decodedMsg;
            return decodedMsg.ToString();
        }

        public static string CrackRowTranspo(string msg, int keyLength, int numOfTrials)
        {
            int[] currentKey = new int[keyLength];

            for (int i = 0; i < keyLength; i++)
            {
                currentKey[i] = i;
            }
            for (int i = 0; i < 100; i++)
            {
                currentKey = MessWithIntKey(currentKey);
            }

            int[] bestKey = new int[keyLength];

            Array.Copy(currentKey, bestKey, keyLength);

            float currentScore = QuadgramScore(DecodeRowTranspo(msg, currentKey));
            float bestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                Array.Copy(MessWithIntKey(currentKey), currentKey, keyLength);

                decodedMsg = DecodeRowTranspo(msg, currentKey);

                currentScore = QuadgramScore(decodedMsg);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    Array.Copy(currentKey, bestKey, keyLength);
                }
                else if (currentScore < bestScore)
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = AnnealingProbability(currentScore - bestScore, numOfTrials - trial);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestKey, keyLength);
                    }
                    else
                    {
                        Array.Copy(bestKey, currentKey, keyLength);
                    }
                }
                else
                {
                    Array.Copy(bestKey, currentKey, keyLength);
                }
            }

            return DecodeRowTranspo(msg, bestKey);
        }

        public static int Factorial(int n)
        {
            if (n == 0 || n == 1)
            {
                return 1;
            }
            else
            {
                return n * Factorial(n - 1);
            }
        }

        public static int[][] Permutations(int n)
        {
            if ( n > 1)
            {
                List<int[]> perms = new List<int[]>();

                int[][] permsRecur = Permutations(n - 1);

                int[] newPerm = new int[n];

                for (int x = 0; x < n; x++)
                {
                    for (int i = 0; i < permsRecur.Length; i++)
                    {
                        newPerm = new int[n];
                        //newPerm[0] = i;
                        newPerm[0] = x;

                        for (int j = 0; j < permsRecur[i].Length; j++)
                        {
                            //if (permsRecur[i][j] >= i)
                            //if (permsRecur[i][j] > i)
                            if (permsRecur[i][j] >= x)
                            {
                                //permsRecur[i][j] += 1;
                                newPerm[j + 1] = permsRecur[i][j] + 1;
                            }
                            else
                            {
                                newPerm[j + 1] = permsRecur[i][j];
                            }
                        }                        

                        /*for (int j = 0; j < permsRecur[i].Length; j++)
                        {
                            newPerm[j + 1] = permsRecur[i][j];
                        }*/

                        perms.Add(newPerm);
                    }
                }
                
                return perms.ToArray();
            }
            else
            {
                //return new int[][] { new int[1] { 1 } };
                return new int[][] { new int[1] { 0 } };
            }
        }

        public static void DisplayPermutations(int[][] perms)
        {
            for (int i = 0; i < perms.Length; i++)
            {
                for (int j = 0; j < perms[i].Length; j++)
                {
                    Console.Write(perms[i][j] + " ");
                }
                Console.Write("\n");
            }
        }

        /*public static int[][] NToTheNArrangements(int n)
        {
            int[][] arrs = new int[(int)Math.Pow(n, n)][];
            int[] arr = new int[n];

            for (int i = 0; i < Math.Pow(n, n); i++)
            {
                arr = new int[n];

                for (int j = 0; j < n; j++)
                {
                    //arr[j] = i % (int)Math.Pow(n, n - j + 1);
                    //arr[j] = i % (int)Math.Pow(n, n - j);
                    //arr[j] = i % (int)Math.Pow(n - 1, n - j);
                    //arr[j] = (i / (int)Math.Pow(n, j)) % (int)Math.Pow(n, n - j);
                    //arr[j] = (i / (int)Math.Pow(n, n - j)) % (int)Math.Pow(n, n - j);
                    //arr[j] = (i / (int)Math.Pow(n, n - j - 1)) % (int)Math.Pow(n, n - j);
                    arr[j] = (i / (int)Math.Pow(n, n - j - 1)) % n;
                }

                arrs[i] = arr;
            }

            return arrs;
        }*/

        public static int[][] NToTheMArrangements(int n, int m)
        {
            int[][] arrs = new int[(int)Math.Pow(n, m)][];
            //int[] arr = new int[n];
            int[] arr = new int[m];

            for (int i = 0; i < Math.Pow(n, m); i++)
            {
                //arr = new int[n];
                arr = new int[m];

                for (int j = 0; j < m; j++)
                {
                    arr[j] = (i / (int)Math.Pow(n, m - j - 1)) % n;
                }

                arrs[i] = arr;
            }

            return arrs;
        }

        public static float ChiSquared(string msg)
        {
            //float[] freqs = LetterFreqs(msg);
            int[] counts = LetterCounts(msg);

            float chiSquared = 0;

            //for (int i = 0; i < freqs.Length; i++)
            for (int i = 0; i < counts.Length; i++)
            {
                //chiSquared += (freqs[i] * 100 - averageletterfrequencies[i]) * (freqs[i] * 100 - averageletterfrequencies[i]) / averageletterfrequencies[i];
                //chiSquared += (freqs[i] - msg.Length * averageletterfrequencies[i] / 100) * (freqs[i] - msg.Length * averageletterfrequencies[i] / 100) / averageletterfrequencies[i];
                //chiSquared += (float)Math.Pow(freqs[i] * 100 - averageletterfrequencies[i], 2) / averageletterfrequencies[i];
                //chiSquared += (counts[i] - msg.Length * averageletterfrequencies[i] / 100) * (counts[i] - msg.Length * averageletterfrequencies[i] / 100) / averageletterfrequencies[i];
                chiSquared += (counts[i] - msg.Length * averageletterfrequencies[i] / 100) * (counts[i] - msg.Length * averageletterfrequencies[i] / 100) / (msg.Length * averageletterfrequencies[i] / 100);
            }

            return chiSquared;
        }

        public static int[] LetterCounts(string msg)
        {
            int[] counts = new int[26];

            for (int i = 0; i < msg.Length; i++)
            {
                counts[msg[i] - 97] += 1;
            }

            /*int total = 0;

            for (int i = 0; i < counts.Length; i++)
            {
                total += counts[i];
            }

            foreach (int i in counts)
            {
                Console.Write(i + " ");
            }
            Console.Write(total + " ");*/

            return counts;
        }

        public static float[] LetterFreqs(string msg)
        {
            int[] counts = LetterCounts(msg);
            float[] freqs = new float[26];

            int total = 0;

            for (int i = 0; i < counts.Length; i++)
            {
                total += counts[i];
            }

            for (int i = 0; i < counts.Length; i++)
            {
                freqs[i] = (float)counts[i] / (float)total;
            }

            return freqs;
        }

        public static bool IsEnglishLetter(char c)
        {
            return (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z');
        }



        public static string CustomOrderedFrequencyKey(string msg, string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789")
        {
            int[] counts = new int[alphabet.Length];
            string key = alphabet;

            for (int i = 0; i < counts.Length; i++)
            {
                counts[i] = 0;
            }

            for (int i = 0; i < msg.Length; i++)
            {
                if (alphabet.Contains(msg[i]))
                {
                    if (msg[i] >= 97)
                    {
                        counts[msg[i] - 97] += 1;
                    }
                    else
                    {
                        counts[msg[i] - 22] += 1;
                    }
                }
            }

            int bestIndex;
            int orderIndex = 0;

            for (int i = 0; i < alphabet.Length; i++)
            {
                bestIndex = 0;

                for (int j = 1; j < alphabet.Length; j++)
                {
                    if (counts[j] > counts[bestIndex])
                    {
                        bestIndex = j;
                    }
                }

                //Console.Write(bestIndex + " " + alphabet[bestIndex] + " " + orderedlettersonavgfreq[orderIndex] + "\n");

                while (!alphabet.Contains(orderedlettersonavgfreq[orderIndex]))
                {
                    orderIndex += 1;
                }

                key = key.Remove(bestIndex, 1);
                key = key.Insert(bestIndex, orderedlettersonavgfreq[orderIndex].ToString());
                //key = SetChar(orderedlettersonavgfreq[orderIndex], bestIndex, key);
                //Console.Write(key + "\n");

                counts[bestIndex] = -1;

                orderIndex += 1;
            }

            //return alphabet;
            return key;
        }


        public static string CrackCustomMonoSub(string msg, string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789", int numOfTrials = 1000, SolutionType solutionType = SolutionType.QuadgramScore)
        {
            string currentKey = CustomOrderedFrequencyKey(msg, alphabet);
            string bestKey = currentKey;

            float currentScore;

            bool higherIsBetter;

            if (solutionType == SolutionType.QuadgramScore)
            {
                higherIsBetter = true;
            }
            else if (solutionType == SolutionType.ChiSquared)
            {
                higherIsBetter = false;
            }
            else
            {
                higherIsBetter = true;
            }

            if (solutionType == SolutionType.QuadgramScore)
            {
                currentScore = QuadgramScore(DecodeCustomMonoSub(msg, currentKey, alphabet));
            }
            else if (solutionType == SolutionType.ChiSquared)
            {
                currentScore = ChiSquared(DecodeCustomMonoSub(msg, currentKey, alphabet));
            }
            else
            {
                currentScore = QuadgramScore(DecodeCustomMonoSub(msg, currentKey, alphabet));
            }

            float bestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                //for (int i = 0; i < rand.Next() % 10 + 1; i++)
                //{
                //currentKey = MessWithStringKey(currentKey);
                //}    
                currentKey = MessWithStringKey(currentKey);

                decodedMsg = DecodeCustomMonoSub(msg, currentKey, alphabet);

                if (solutionType == SolutionType.QuadgramScore)
                {
                    currentScore = QuadgramScore(decodedMsg);
                }
                else if (solutionType == SolutionType.ChiSquared)
                {
                    currentScore = ChiSquared(decodedMsg);
                }
                else
                {
                    currentScore = QuadgramScore(decodedMsg);
                }

                if (higherIsBetter)
                {
                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestKey = currentKey;
                    }
                    else
                    {
                        currentKey = bestKey;
                    }
                }
                else
                {
                    if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        bestKey = currentKey;
                    }
                    else
                    {
                        /*bool ReplaceAnyway;
                        ReplaceAnyway = AnnealingProbability(currentScore - bestScore, numOfTrials - trial);
                        //ReplaceAnyway = false;

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            bestKey = currentKey;
                        }
                        else
                        {
                            currentKey = bestKey;
                        }
                        /*currentKey = bestKey;*/
                        currentKey = bestKey;
                    }
                }

            }

            return DecodeCustomMonoSub(msg, bestKey, alphabet);
        }

        public static Tuple<string, string> CrackCustomMonoSubReturnKey(string msg, string alphabet = "abcdefghijklmnopqrstuvwxyz0123456789", int numOfTrials = 1000, SolutionType solutionType = SolutionType.QuadgramScore)
        {
            string currentKey = CustomOrderedFrequencyKey(msg, alphabet);
            string bestKey = currentKey;

            float currentScore;

            bool higherIsBetter;

            if (solutionType == SolutionType.QuadgramScore)
            {
                higherIsBetter = true;
            }
            else if (solutionType == SolutionType.ChiSquared)
            {
                higherIsBetter = false;
            }
            else
            {
                higherIsBetter = true;
            }

            if (solutionType == SolutionType.QuadgramScore)
            {
                currentScore = QuadgramScore(DecodeCustomMonoSub(msg, currentKey, alphabet));
            }
            else if (solutionType == SolutionType.ChiSquared)
            {
                currentScore = ChiSquared(DecodeCustomMonoSub(msg, currentKey, alphabet));
            }
            else
            {
                currentScore = QuadgramScore(DecodeCustomMonoSub(msg, currentKey, alphabet));
            }

            float bestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                //for (int i = 0; i < rand.Next() % 10 + 1; i++)
                //{
                //currentKey = MessWithStringKey(currentKey);
                //}    
                currentKey = MessWithStringKey(currentKey);

                decodedMsg = DecodeCustomMonoSub(msg, currentKey, alphabet);

                if (solutionType == SolutionType.QuadgramScore)
                {
                    currentScore = QuadgramScore(decodedMsg);
                }
                else if (solutionType == SolutionType.ChiSquared)
                {
                    currentScore = ChiSquared(decodedMsg);
                }
                else
                {
                    currentScore = QuadgramScore(decodedMsg);
                }

                if (higherIsBetter)
                {
                    if (currentScore > bestScore)
                    {
                        bestScore = currentScore;
                        bestKey = currentKey;
                    }
                    else
                    {
                        currentKey = bestKey;
                    }
                }
                else
                {
                    if (currentScore < bestScore)
                    {
                        bestScore = currentScore;
                        bestKey = currentKey;
                    }
                    else
                    {
                        /*bool ReplaceAnyway;
                        ReplaceAnyway = AnnealingProbability(currentScore - bestScore, numOfTrials - trial);
                        //ReplaceAnyway = false;

                        if (ReplaceAnyway == true)
                        {
                            bestScore = currentScore;
                            bestKey = currentKey;
                        }
                        else
                        {
                            currentKey = bestKey;
                        }
                        /*currentKey = bestKey;*/
                        currentKey = bestKey;
                    }
                }

            }

            return new Tuple<string, string>(DecodeCustomMonoSub(msg, bestKey, alphabet), bestKey);
        }

        public static string DecodeCustomMonoSub(string msg, string key, string alphabet)
        {
            //string newMsg = "";
            StringBuilder newMsg = new StringBuilder();

            for (int i = 0; i < msg.Length; i++)
            {
                //for (int j = 0; j < key.Length; j++)
                for (int j = 0; j < alphabet.Length; j++)
                {
                    //if (key[j] == msg[i])
                    if (alphabet[j] == msg[i])
                    {
                        //newMsg += alphabet[j];
                        //newMsg.Append(alphabet[j]);
                        newMsg.Append(key[j]);
                        break;
                    }
                }
                
            }

            //return newMsg;
            return newMsg.ToString();
        }

        public static string Swap(string text, int index1, int index2)
        {
            if (index1 >= 0 && index1 < text.Length && index2 >= 0 && index2 < text.Length)
            {
                char temp = text[index1];

                text = text.Insert(index1, text[index2].ToString());
                text = text.Remove(index1 + 1, 1);

                text = text.Insert(index2, temp.ToString());
                text = text.Remove(index2 + 1, 1);
            }            

            return text;
        }

        public static void DisplayPolybiusSquare(string rowAlphabet, string columnAlphabet, string contents)
        {
            int i;
            int j;
            int index = 0;
            Console.Write(" |");
            for (i = 0; i < columnAlphabet.Length; i++)
            {
                Console.Write(" " + columnAlphabet[i]);
            }
            Console.Write("\n-+");
            for (i = 0; i < columnAlphabet.Length; i++)
            {
                Console.Write("--");
            }
            for (i=0; i < rowAlphabet.Length; i++)
            {
                //Console.Write(rowAlphabet[i] + "|");
                Console.Write("\n" + rowAlphabet[i] + "|");
                for (j = 0; j < columnAlphabet.Length; j++)
                {
                    if (index < contents.Length)
                    {
                        Console.Write(" " + contents[index]);
                        index += 1;
                    }
                }
            }
        }

        public static string GetKeyFromPlaintext(string plaintext, string ciphertext, string alphabet = "abcdefghijklmnopqrstuvwxyz")
        {
            string key = alphabet;

            for (int i = 0; i < ciphertext.Length; i++)
            {
                key = Swap(key, alphabet.IndexOf(ciphertext[i]), key.IndexOf(plaintext[i]));
            }

            return key;
        }

        public static string[] GetDictionary(string dictionaryFilePath = "==FullDictionary.txt")
        {
            string[] dictionary = System.IO.File.ReadAllLines(dictionaryFilePath);

            for (int i = 0; i < dictionary.Length; i++)
            {
                dictionary[i] = dictionary[i].ToLower();
            }

            return dictionary;
        }

        public static string SetChar(char chr, int index, string text)
        {
            /*if (index < 0 || index >= text.Length)
            {
                throw new IndexOutOfRangeException("Index is outside the range of the text.");
            }*/
            if (index < 0)
            {
                throw new IndexOutOfRangeException("Index is too low.");
            }
            else if (index >= text.Length)
            {
                throw new IndexOutOfRangeException("Index is too high.");
            }

            text = text.Insert(index, chr.ToString());
            text = text.Remove(index + 1, 1);

            return text;
        }

        public static int RoundToRange(int num, int min, int max)
        {
            if (num < min)
            {
                return min;
            }
            else if (num > max)
            {
                return max;
            }
            return num;
        }

        public static string OverlayString(string overlayStr, int index, string baseText)
        {
            string newStr = baseText;

            for (int i = index; i < baseText.Length && i - index < overlayStr.Length; i++)
            {
                newStr = SetChar(overlayStr[i - index], i, newStr);
            }

            return newStr;
        }

        public static int WeightedRand(int[] weights)
        {
            int total = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                if (weights[i] >= 0)
                {
                    total += weights[i];
                }
                else
                {
                    throw new ArgumentException("Cannot have negative weights.");
                }
            }
            int index = rand.Next() % total + 1;

            total = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                total += weights[i];
                if (index <= total)
                {
                    return i;
                }
            }
            return -1;
        }

        public static float RandFloat()
        {
            return (rand.Next() % 100000) / 100000f;
        }




        public static Tuple<string, int[]> CrackColumnTranspoReturnKey(string msg, int keyLength, int numOfTrials)
        {
            int[] currentKey = new int[keyLength];

            for (int i = 0; i < keyLength; i++)
            {
                currentKey[i] = i;
            }
            for (int i = 0; i < 100; i++)
            {
                currentKey = MessWithIntKey(currentKey);
            }

            int[] bestKey = new int[keyLength];

            Array.Copy(currentKey, bestKey, keyLength);

            //float currentScore = QuadgramScore(DecodeColumnTranspo(msg, currentKey));
            float currentScore = QuadgramScore(IncompleteColumnarTranspo(msg, currentKey));
            float bestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                Array.Copy(MessWithIntKey(currentKey), currentKey, keyLength);

                //decodedMsg = DecodeColumnTranspo(msg, currentKey);
                decodedMsg = IncompleteColumnarTranspo(msg, currentKey);

                currentScore = QuadgramScore(decodedMsg);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    Array.Copy(currentKey, bestKey, keyLength);
                }
                else if (currentScore < bestScore)
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = AnnealingProbability(currentScore - bestScore, numOfTrials - trial);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestKey, keyLength);
                    }
                    else
                    {
                        Array.Copy(bestKey, currentKey, keyLength);
                    }
                }
                else
                {
                    Array.Copy(bestKey, currentKey, keyLength);
                }
            }

            //return new Tuple<string, int[]>(DecodeColumnTranspo(msg, bestKey), bestKey);
            return new Tuple<string, int[]>(IncompleteColumnarTranspo(msg, bestKey), bestKey);
        }


        public static Tuple<string, int[]> CrackRowTranspoReturnKey(string msg, int keyLength, int numOfTrials)
        {
            int[] currentKey = new int[keyLength];

            for (int i = 0; i < keyLength; i++)
            {
                currentKey[i] = i;
            }
            for (int i = 0; i < 100; i++)
            {
                currentKey = MessWithIntKey(currentKey);
            }

            int[] bestKey = new int[keyLength];

            Array.Copy(currentKey, bestKey, keyLength);

            float currentScore = QuadgramScore(DecodeRowTranspo(msg, currentKey));
            float bestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                Array.Copy(MessWithIntKey(currentKey), currentKey, keyLength);

                decodedMsg = DecodeRowTranspo(msg, currentKey);

                currentScore = QuadgramScore(decodedMsg);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    Array.Copy(currentKey, bestKey, keyLength);
                }
                else if (currentScore < bestScore)
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = AnnealingProbability(currentScore - bestScore, numOfTrials - trial);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestKey, keyLength);
                    }
                    else
                    {
                        Array.Copy(bestKey, currentKey, keyLength);
                    }
                }
                else
                {
                    Array.Copy(bestKey, currentKey, keyLength);
                }
            }

            return new Tuple<string, int[]>(DecodeRowTranspo(msg, bestKey), bestKey);
        }

        public static int[] BlockMessWithIntKey(int [] key)
        {
            //int blockLength = rand.Next() % (key.Length - 1) + 1;
            int blockLength = rand.Next() % (key.Length - 2) + 1;
            int blockTakeFromIndex = rand.Next() % (key.Length - blockLength + 1);
            int blockMoveToIndex = rand.Next() % (key.Length - blockLength + 1);

            //Console.Write(" " + blockLength + ": " + blockTakeFromIndex + " -> " + blockMoveToIndex + "  ");

            int[] newKey = new int[key.Length];

            for (int i = 0; i < blockLength; i++)
            {
                newKey[blockMoveToIndex + i] = key[blockTakeFromIndex + i];
            }

            int index = 0;
            for (int i = 0; i < key.Length; i++)
            {
                if (i < blockTakeFromIndex || i >= blockTakeFromIndex + blockLength)
                {
                    //while (index >= blockMoveToIndex || index < blockMoveToIndex + blockLength)
                    while (index >= blockMoveToIndex && index < blockMoveToIndex + blockLength)
                    {
                        index += 1;
                    }
                    newKey[index] = key[i];
                    index += 1;
                }
            }

            /*Utils.DisplayArray(key);
            Console.Write("\n");
            Utils.DisplayArray(newKey);
            Console.Write("\n\n");*/

            return newKey;
        }

        public static string CrackColumnTranspoBigram(string ciphertext, int keyLength)
        {
            float scalingParameter = 1f;

            int[] bestKey = new int[keyLength];
            float bestScore = Int32.MinValue;
            string bestPlaintext = "";

            int[] keptKey;
            float keptScore;
            string keptPlaintext;

            int[] proposedKey = new int[keyLength];
            string proposedPlaintext = "";
            float proposedScore;

            float randNum;

            //for (int trial = 0; trial < 5; trial++)
            for (int trial = 0; trial < 50; trial++)
            //for (int trial = 0; trial < 500; trial++)
            {
                Console.Write("Trial: " + trial + "\n");
                keptKey = new int[keyLength];

                for (int i = 0; i < keyLength; i++)
                {
                    keptKey[i] = i;
                }
                for (int i = 0; i < 100; i++)
                {
                    Array.Copy(MessWithIntKey(keptKey), keptKey, keyLength);
                }

                keptPlaintext = DecodeColumnTranspo(ciphertext, keptKey);
                keptScore = BigramScore(keptPlaintext);
                //keptScore = QuadgramScore(keptPlaintext);
                //keptScore = BigramScoreCounts(keptPlaintext);

                if (trial == 0)
                {
                    Array.Copy(keptKey, bestKey, keyLength);
                    bestScore = keptScore;
                    bestPlaintext = keptPlaintext;
                }
                else if (keptScore > bestScore)
                {
                    Array.Copy(keptKey, bestKey, keyLength);
                    bestScore = keptScore;
                    bestPlaintext = keptPlaintext;
                }

                //for (int i = 0; i < 10000; i++)
                for (int i = 0; i < 1000; i++)
                //for (int i = 0; i < 100000; i++)
                {
                    //Array.Copy(BlockMessWithIntKey(keptKey), proposedKey, keyLength);
                    Array.Copy(MessWithIntKey(keptKey), proposedKey, keyLength);

                    if (Enumerable.SequenceEqual(new int[] { 12, 4, 6, 5, 13, 14, 8, 1, 0, 11, 2, 3, 9, 10, 7 }, proposedKey))
                    {
                        Console.Write("Got it!");
                    }

                    proposedPlaintext = DecodeColumnTranspo(ciphertext, proposedKey);
                    proposedScore = BigramScore(proposedPlaintext);
                    //proposedScore = QuadgramScore(proposedPlaintext);
                    //proposedScore = 0;
                    //proposedScore = BigramScoreCounts(proposedPlaintext);

                    //float randNum = Utils.RandFloat();
                    //randNum = Utils.RandFloatFromRange(-10000, 0);
                    //randNum = Utils.RandFloatFromRange(Int32.MinValue, 0);
                    //randNum = Utils.RandFloat();
                    //randNum = Utils.RandFloatFromRange(float.MinValue, 0f);

                    randNum = (float)Math.Log(1.0 - Utils.rand.NextDouble());

                    //if (randNum < Math.Pow(proposedScore - keptScore, scalingParameter))
                    if (randNum < scalingParameter * (proposedScore - keptScore))
                    //if (randNum < Math.Exp(scalingParameter * (proposedScore - keptScore)))
                    {
                        Array.Copy(proposedKey, keptKey, keyLength);
                        keptScore = proposedScore;
                        keptPlaintext = proposedPlaintext;

                        if (keptScore > bestScore)
                        {
                            Array.Copy(keptKey, bestKey, keyLength);
                            bestScore = keptScore;
                            bestPlaintext = keptPlaintext;
                        }
                    }
                }

                /*if (keptScore > bestScore)
                {
                    Array.Copy(keptKey, bestKey, keyLength);
                    bestScore = keptScore;
                    bestPlaintext = keptPlaintext;
                }*/
            }

            return bestPlaintext;
        }

        public static float BigramScore(string msg)
        {
            float Score = 0;
            int index = 0;
            for (int i = 0; i < msg.Length - 1; i++)
            {
                //index = (msg[i + 0] - 97) * 26 + (msg[i + 1] - 97);
                index = (msg[i] - 97) * 26 + (msg[i + 1] - 97);
                if (index >= 0 && index < 676)
                {
                    if (BigramScores.BIGRAMSCORES[index] < -10f)
                    {
                        Score += -10;
                    }
                    else
                    {
                        Score += BigramScores.BIGRAMSCORES[index];
                    }
                }
                else
                {
                    //Score += -16;
                    Score += -10;
                }
            }
            return Score;
        }

        public static float BigramScoreCounts(string msg)
        {
            double Score = 0;
            int index = 0;
            for (int i = 0; i < msg.Length - 1; i++)
            {
                index = (msg[i] - 97) * 26 + (msg[i + 1] - 97);
                if (index >= 0 && index < 676)
                {
                    Score += Math.Log(BigramScores.BIGRAMCOUNTS[index]);
                }
                else
                {
                    Score += -10;
                }
            }
            return (float)Score;
        }

        public static string CrackVigenere(string ciphertext, int period)
        {
            string key = "";
            //StringBuilder key = new StringBuilder();

            float bestScore = float.MinValue;
            int bestKey;
            float newScore;

            for (int i = 0; i < period; i++)
            {
                //key += 'a';
                bestKey = 0;
                bestScore = ChiSquared(DecodePartialVigenere(ciphertext, 0, i, period));

                for (int j = 1; j < 26; j++)
                {
                    newScore = ChiSquared(DecodePartialVigenere(ciphertext, j, i, period));

                    if (newScore < bestScore)
                    {
                        bestScore = newScore;
                        bestKey = j;
                    }
                }

                key += ALPHABET[bestKey];
                //key.Append(ALPHABET[bestKey]);

                //Console.Write(key); Console.Write("\n");
            }

            return DecodeVigenere(ciphertext, key);
            //return DecodeVigenere(ciphertext, key.ToString());
        }

        public static string CrackVigenereUnknownPeriod(string ciphertext)
        {
            int period = CrackVigenerePeriod(ciphertext);
            //Console.Write("Period: " + period);
            if (period > 0)
            {
                return CrackVigenere(ciphertext, period);
            }
            else
            {
                return ciphertext;
            }
        }

        public static Tuple<string, string> CrackVigenereUnknownPeriodReturnKey(string ciphertext)
        {
            int period = CrackVigenerePeriod(ciphertext);
            if (period > 0)
            {
                return CrackVigenereReturnKey(ciphertext, period);
            }
            else
            {
                return new Tuple<string, string>(ciphertext, "");
            }
        }

        public static Tuple<string, string> CrackVigenereReturnKey(string ciphertext, int period)
        {
            string key = "";

            float bestScore = float.MinValue;
            int bestKey;
            float newScore;

            for (int i = 0; i < period; i++)
            {
                //key += 'a';
                bestKey = 0;
                bestScore = ChiSquared(DecodePartialVigenere(ciphertext, 0, i, period));

                for (int j = 1; j < 26; j++)
                {
                    newScore = ChiSquared(DecodePartialVigenere(ciphertext, j, i, period));

                    if (newScore < bestScore)
                    {
                        bestScore = newScore;
                        bestKey = j;
                    }
                }

                key += ALPHABET[bestKey];
            }

            return new Tuple<string, string>(DecodeVigenere(ciphertext, key), key);
        }

        //private static string DecodePartialVigenere(string ciphertext, int shift, int keyColumn, int period)
        public static string DecodePartialVigenere(string ciphertext, int shift, int keyColumn, int period)
        {
            //string decodedMsg = "";
            StringBuilder decodedMsg = new StringBuilder();
            for (int i = keyColumn; i < ciphertext.Length; i += period)
            {
                //decodedMsg += Annealing.ALPHABET[(int)Annealing.Mod((ciphertext[i] - 97 - shift), 26)];
                decodedMsg.Append(Annealing.ALPHABET[(int)Annealing.Mod((ciphertext[i] - 97 - shift), 26)]);
            }
            //return decodedMsg;
            return decodedMsg.ToString();
        }

        public static string DecodeVigenere(string msg, string key)
        {
            //string decodedMsg = "";
            StringBuilder decodedMsg = new StringBuilder();
            for (int i = 0; i < msg.Length; i++)
            {
                //decodedMsg += Annealing.ALPHABET[(int)Annealing.Mod((msg[i] - key[i % key.Length]), 26)];
                decodedMsg.Append(Annealing.ALPHABET[(int)Annealing.Mod((msg[i] - key[i % key.Length]), 26)]);
            }
            //return decodedMsg;
            return decodedMsg.ToString();
        }

        public static float SteppedIOC(string msg, int step)
        {
            int[] counts = new int[26];
            int total = 0;

            for (int i = 0; i < msg.Length; i+= step)
            {
                counts[msg[i] - 97] += 1;
                total += 1;
            }

            float ioc = 0;

            for (int i = 0; i < 26; i++)
            {
                ioc = ioc + (float)counts[i] * ((float)counts[i] - 1f) / ((float)total * ((float)total - 1f));
            }

            return (float)ioc;
        }

        public static int CrackVigenerePeriod(string ciphertext, float iocLowerBound = 0.0595f, float iocUpperBound = 100000000f, int maxPeriod = 30)
        {
            float ioc;
            for (int period = 2; period < maxPeriod + 1; period++)
            {
                ioc = SteppedIOC(ciphertext, period);

                if (iocLowerBound < ioc && ioc < iocUpperBound)
                {
                    ioc = SteppedIOC(ciphertext, period * 2);
                    if (iocLowerBound < ioc && ioc < iocUpperBound)
                    {
                        return period;
                    }
                }
            }
            return -1;
        }

        public static string DecodeTranspo(string ciphertext, int[] key, TranspositionType transpoType)
        {
            if (transpoType == TranspositionType.column)
            {
                //return DecodeColumnTranspo(ciphertext, key);
                return IncompleteColumnarTranspo(ciphertext, key);
            }
            else if (transpoType == TranspositionType.row)
            {
                return DecodeRowTranspo(ciphertext, key);
            }
            else
            {
                throw new ArgumentException("Unaccepted transposition type: " + transpoType.ToString());
            }
        }


        public static Tuple<string, int[]> CrackMyszkowskiTranspoReturnKey(string msg, int keyLength, int numOfTrials)
        {
            int[] currentKey = new int[keyLength];

            for (int i = 0; i < keyLength; i++)
            {
                currentKey[i] = i;
            }
            for (int i = 0; i < 100; i++)
            {
                currentKey = MessWithIntKeyMyszkowski(currentKey);
            }

            int[] bestKey = new int[keyLength];

            Array.Copy(currentKey, bestKey, keyLength);

            float currentScore = QuadgramScore(DecodeMyszkowskiTranspo(msg, currentKey));
            float bestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                if (rand.Next() % 5 < 3)
                {
                    Array.Copy(MessWithIntKeyMyszkowski(currentKey), currentKey, keyLength);
                }
                else
                {
                    Array.Copy(BlockMessWithIntKey(currentKey), currentKey, keyLength);
                }

                decodedMsg = DecodeMyszkowskiTranspo(msg, currentKey);

                currentScore = QuadgramScore(decodedMsg);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    Array.Copy(currentKey, bestKey, keyLength);
                }
                else if (currentScore < bestScore)
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = AnnealingProbability(currentScore - bestScore, numOfTrials - trial);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestKey, keyLength);
                    }
                    else
                    {
                        Array.Copy(bestKey, currentKey, keyLength);
                    }
                }
                else
                {
                    Array.Copy(bestKey, currentKey, keyLength);
                }
            }

            return new Tuple<string, int[]>(DecodeMyszkowskiTranspo(msg, bestKey), bestKey);
        }

        public static int[] MessWithIntKeyMyszkowski(int[] key)
        {
            int index1 = rand.Next() % key.Length;

            key[index1] = rand.Next() % key.Length;

            Array.Copy(FixMyszkowskiKey(key), key, key.Length);

            return key;
        }

        public static int[] FixMyszkowskiKey(int[] key)
        {
            bool found;
            for (int i = 0; i < Enumerable.Max(key); i++)
            {
                found = false;
                while (!found)
                {
                    found = false;
                    for (int j = 0; j < key.Length; j++)
                    {
                        if (key[j] == i)
                        {
                            found = true;
                        }
                    }
                    if (!found)
                    {
                        for (int j = 0; j < key.Length; j++)
                        {
                            if (key[j] > i)
                            {
                                key[j]--;
                            }
                        }
                    }
                }
            }
            return key;
        }

        public static string DecodeMyszkowskiTranspo(string ciphertext, int[] key)
        {
            StringBuilder plaintext = new StringBuilder();
            int columnLength = ciphertext.Length / key.Length;

            int[] keyCounts = new int[key.Length];

            for (int i = 0; i < key.Length; i++)
            {
                keyCounts[key[i]] += 1;
            }

            /*int[] columnPoses = new int[key.Length];

            for (int i = 0; i < key.Length; i++)
            {
                columnPoses[i] = -1;
            }
            for (int i = 0; i < key.Length; i++)
            {
                if (columnPoses[key[i]] == -1)
                {
                    columnPoses[key[i]] = i;
                }
            }*/

            int[] keyCounts2 = new int[key.Length];
            for (int i = 0; i < key.Length; i++)
            {
                int sum = 0;

                /*for (int j = 0; j < i; j++)
                {
                    sum += keyCounts[j];
                }*/
                for (int j = 0; j < key.Length; j++)
                {
                    if (key[j] < i)
                    {
                        sum++;
                    }
                }
                keyCounts2[i] = sum;
            }

            //CipherLib.Utils.DisplayArray(keyCounts); CipherLib.Utils.DisplayArray(columnPoses);

            //int count;
            int[] counts = new int[key.Length];

            /*for (int i = 0; i < columnLength; i++)
            {
                counts = new int[key.Length];

                for (int j = 0; j < key.Length; j++)
                {
                    for (int k = 0; k < key.Length; k++)
                    {
                        //if (key[k] == j && k * columnLength + i * keyCounts[j] < ciphertext.Length)
                        if (key[k] == j && keyCounts2[j] * columnPoses[j] * columnLength + i * keyCounts[j] + counts[j] < ciphertext.Length)
                        {
                            plaintext.Append(ciphertext[keyCounts2[j] * columnPoses[j] * columnLength + i * keyCounts[j] + counts[j]]);
                            counts[j] += 1;

                            /*count = 1;
                            for (int t = k + 1; t < key.Length; t++)
                            {
                                if (key[t] == j && k * columnLength + i * keyCounts[j] + count < ciphertext.Length)
                                {
                                    plaintext.Append(ciphertext[k * columnLength + i * keyCounts[j] + count]);
                                }
                                count++;
                            }*/
            //break;
            /*}
        }
        Console.Write("\n" + plaintext.ToString());
    }
}*/

                for (int i = 0; i < columnLength; i++)
                {
                    counts = new int[key.Length];

                    for (int j = 0; j < key.Length; j++)
                    {
                        if (keyCounts2[key[j]] * columnLength + i * keyCounts[key[j]] + counts[key[j]] < ciphertext.Length)
                        {
                            plaintext.Append(ciphertext[keyCounts2[key[j]] * columnLength + i * keyCounts[key[j]] + counts[key[j]]]);
                            counts[key[j]] += 1;
                        }
                        //Console.Write("\n" + plaintext.ToString());
                    }
                }

                return plaintext.ToString();
        }

        public static string DecodeAMSCO(string ciphertext, int maxChunkSize, int[] key, int[] pattern = null)
        {
            if (pattern == null)
            {
                pattern = CipherLib.Utils.IntRangeArray(0, maxChunkSize - 1);
            }
            else if (pattern.Length != maxChunkSize)
            {
                throw new ArgumentException("The length of pattern must equal maxChunkSize.");
            }

            //int columnLength = ciphertext.Length / key.Length;
            //int columnLength = ciphertext.Length / (maxChunkSize * (maxChunkSize + 1) / 2);
            //int columnLength = ciphertext.Length / (maxChunkSize * (maxChunkSize + 1) / 2) / key.Length;

            /// chunkRef stores the sizes of the chunks for each place in the ciphertext grid.
            /// Indexed by column then by row;
            //int[][] chunkRef = new int[maxChunkSize][];
            //int[][] chunkRef = new int[key.Length][];
            List<int>[] chunkRef = new List<int>[key.Length];

            for (int i = 0; i < key.Length; i++)
            {
                //chunkRef[i] = new int[columnLength];
                chunkRef[i] = new List<int>();

                /*for (int j = 0; j < columnLength; j++)
                {
                    /// Work out what the chunk size should be in this cell
                    //chunkRef[i][j] = pattern[CipherLib.Annealing.Mod(j - i, pattern.Length)];
                    chunkRef[i][j] = pattern[CipherLib.Annealing.Mod(j + i, pattern.Length)];
                }*/
            }

            int total = 0;
            //for (int row = 0; total < ciphertext.Length; total++)
            for (int row = 0; total < ciphertext.Length; row++)
            {
                for (int column = 0; column < key.Length && total < ciphertext.Length; column++)
                {
                    //chunkRef[column].Add(pattern[CipherLib.Annealing.Mod(column + row, pattern.Length)]);
                    //total += chunkRef[column][row];
                    chunkRef[column].Add(0);

                    //for (int i = 0; i < CipherLib.Annealing.Mod(column + row, pattern.Length) && total < ciphertext.Length; i++)
                    for (int i = 0; i < pattern[CipherLib.Annealing.Mod(column + row, pattern.Length)] && total < ciphertext.Length; i++)
                    {
                        chunkRef[column][row] += 1;
                        total += 1;
                    }
                }
            }


            /*/// Display chunkRef for testing
            total = 0;
            //for (int i = 0; i < columnLength; i++)
            for (int i = 0; total < ciphertext.Length; i++)
            {
                for (int j = 0; j < key.Length && total < ciphertext.Length; j++)
                {
                    Console.Write(" ");
                    Console.Write(chunkRef[j][i]);
                    total += chunkRef[j][i];
                }
                Console.Write("\n");
            }*/

            /// Decrypt the text
            StringBuilder plaintext = new StringBuilder();

            /// The grid to write the text into;
            string[][] transpoGrid = new string[key.Length][];
            for (int i = 0; i < key.Length; i++)
            {
                //transpoGrid[i] = new string[columnLength];
                transpoGrid[i] = new string[chunkRef[i].Count()];
            }

            int index = 0;

            string tempString;

            /// keyNum is the next number we are looking for in the permutation key
            for (int keyNum = 0; keyNum < key.Length; keyNum++)
            {
                /// Find the keyNum in the key
                for (int i = 0; i < key.Length; i++)
                {
                    if (key[i] == keyNum)
                    {
                        /// Go through each row in the current column and add the correct size chunk of text to the transpoGrid
                        //for (int j = 0; j < columnLength; j++)
                        //for (int j = 0; j < columnLength; j++)
                        for (int j = 0; j < transpoGrid[i].Length; j++)
                        {
                            tempString = "";
                            for (int k = 0; k < chunkRef[i][j]; k++)
                            {
                                if (index < ciphertext.Length)
                                {
                                    tempString += ciphertext[index];
                                    index++;
                                }
                            }
                            transpoGrid[i][j] = tempString;
                        }
                        break;
                    }
                }
            }

            /*/// Display transpoGrid for testing
            //for (int i = 0; i < columnLength; i++)
            //for (int i = 0; i < columnLength; i++)
            total = 0;
            for (int i = 0; total < ciphertext.Length; i++)
            {
                for (int j = 0; j < key.Length && total < ciphertext.Length; j++)
                {
                    Console.Write(transpoGrid[j][i]);
                    for (int k = 0; k < 4 - transpoGrid[j][i].ToString().Length; k++)
                    {
                        Console.Write(" ");
                    }
                    total += chunkRef[j][i];
                }
                Console.Write("\n");
            }*/

            /// Put the plaintext together
            //for (int i = 0; i < columnLength; i++)
            for (int i = 0; plaintext.Length < ciphertext.Length; i++)
            {
                for (int j = 0; j < key.Length && plaintext.Length < ciphertext.Length; j++)
                {
                    plaintext.Append(transpoGrid[j][i]);
                }
            }

            return plaintext.ToString();
        }

        public static Tuple<string, int[]> CrackAMSCOReturnKey(string msg, int keyLength, int[] pattern, int numOfTrials)
        {
            int[] currentKey = new int[keyLength];

            for (int i = 0; i < keyLength; i++)
            {
                currentKey[i] = i;
            }
            for (int i = 0; i < 100; i++)
            {
                currentKey = MessWithIntKey(currentKey);
            }

            int[] bestKey = new int[keyLength];
            int[] overallBestKey = new int[keyLength];

            Array.Copy(currentKey, bestKey, keyLength);
            Array.Copy(currentKey, overallBestKey, keyLength);

            float currentScore = QuadgramScore(DecodeAMSCO(msg, pattern.Length, currentKey, pattern));
            float bestScore = currentScore;
            float overallBestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                //Array.Copy(MessWithIntKey(bestKey), currentKey, keyLength);
                Array.Copy(BlockMessWithIntKey(bestKey), currentKey, keyLength);

                decodedMsg = DecodeAMSCO(msg, pattern.Length, currentKey, pattern);

                /*if (Enumerable.SequenceEqual(currentKey, new int[] { 4, 5, 0, 2, 1, 3 }))
                {
                    Console.Write(trial + " / " + numOfTrials);
                    Console.Write("\n\n");
                    Console.Write("Got it!" + decodedMsg);
                }*/

                currentScore = QuadgramScore(decodedMsg);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    Array.Copy(currentKey, bestKey, keyLength);

                    if (bestScore > overallBestScore)
                    {
                        overallBestScore = bestScore;
                        Array.Copy(bestKey, overallBestKey, keyLength);
                    }
                }
                else if (currentScore < bestScore)
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = AnnealingProbability(currentScore - bestScore, numOfTrials - trial);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestKey, keyLength);
                    }
                }
            }

            //return new Tuple<string, int[]>(DecodeAMSCO(msg, pattern.Length, bestKey, pattern), bestKey);
            return new Tuple<string, int[]>(DecodeAMSCO(msg, pattern.Length, overallBestKey, pattern), bestKey);
        }

        public static char[][] MessWith2DCharKey(char[][] key)
        {
            int[] index1 = new int[2] { rand.Next() % key.Length, 0 };
            index1[1] = rand.Next() % key[index1[0]].Length;

            int[] index2 = new int[2] { rand.Next() % key.Length, 0 };
            index2[1] = rand.Next() % key[index2[0]].Length;

            char temp = key[index1[0]][index1[1]];
            key[index1[0]][index1[1]] = key[index2[0]][index2[1]];
            key[index2[0]][index2[1]] = temp;

            return key;
        }

        /*public static float[] NGramFreqs(string text, int n)
        {
            float counts = new float[Math.Pow(26, n)];

            for (int i = 0; i <)
        }*/

        public static int NumOfDistinctChars(string text)
        {
            HashSet<char> usedChars = new HashSet<char>();
            int count = 0;
            for (int i = 0; i < text.Length; i++)
            {
                if (!usedChars.Contains(text[i]))
                {
                    count++;
                    usedChars.Add(text[i]);
                }
            }
            return count;
        }

        public static int[][] Permute(int[] items)
        {
            /// Get all n! arrangements
            int[][] perms = Permutations(items.Length);

            /// Go through each permutation and replace each element in perms by the element in items at the index of the item in perms
            int[] perm;
            List<int[]> mappedPerms = new List<int[]>();
            bool alreadyGot;
            for (int i = 0; i < perms.Length; i++)
            {
                perm = new int[perms[i].Length];
                for (int j = 0; j < perm.Length; j++)
                {
                    //perm[j] = items[j];
                    perm[j] = items[perms[i][j]];
                }
                alreadyGot = false;
                for (int j = 0; j < mappedPerms.Count; j++)
                {
                    if (Enumerable.SequenceEqual(mappedPerms[j], perm))
                    {
                        alreadyGot = true;
                        break;
                    }
                }
                if (!alreadyGot)
                {
                    mappedPerms.Add(perm);
                }
            }

            return mappedPerms.ToArray();
        }

        /// The key for this function is a permutation. Online encrypters I have used use the key to indicate the order in which to read columns.
        /// E.g. if the key is 2, 0, 1. This function would read column 2, then 0, then 1. The online encrypters would read column 1, then 2, then 0.
        /// To convert between these two types of keys, one can simply invert the key. I.e. 2, 0, 1 <--> 1, 2, 0.
        /// (You can simply invert a key using this rule, for all i >=0 and < key's length: invertedKey[i] = originalKey.IndexOf(i). Note, this is not valid C# code.)
        public static string IncompleteColumnarTranspo(string msg, int[] key)
        {
            List<char>[] grid = new List<char>[key.Length];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new List<char>();
            }

            int rowNum = msg.Length / key.Length;

            /// Add the ciphertext to the grid, starting with the leftmost column, then the one to its right, etc. ...
            int index = 0;
            for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < rowNum; j++)
                {
                    grid[i].Add(msg[index]);
                    index++;
                }

                /// For an incomplete transposition, check if it was an incomplete row and add another letter if it is
                if (key[i] < msg.Length % key.Length)
                {
                    grid[i].Add(msg[index]);
                    index++;
                }
            }

            /// If it was an incomplete transposition, tell the code we need to get an extra row in the plaintext
            if (msg.Length % key.Length != 0)
            {
                rowNum += 1;
            }

            /*for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    if (i < grid[j].Count)
                    {
                        Console.Write(grid[j][i] + " ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.Write("\n");
            }
            Console.Write("\n\n");*/

            StringBuilder transpoed = new StringBuilder();

            /// Go down each row
            for (int i = 0; i < rowNum; i++)
            {
                /// j is the next number we are looking for in the key column headings
                for (int j = 0; j < key.Length; j++)
                {
                    /// Go through each key column
                    for (int k = 0; k < key.Length; k++)
                    {
                        //if (key[k] == j)
                        if (key[k] == j && i < grid[k].Count)
                        {
                            transpoed.Append(grid[k][i]);
                            break;
                        }
                    }
                }
            }

            return transpoed.ToString();
        }

        public static int[] InvertKey(int[] key)
        {
            int[] invertedKey = new int[key.Length];

            /// i is the current number we are trying to find the index of.
            for (int i = 0; i < key.Length; i++)
            {
                /// j goes through each element of the key to find i.
                for (int j = 0; j < key.Length; j++)
                {
                    if (key[j] == i)
                    {
                        invertedKey[i] = j;
                        break;
                    }
                }
            }
            return invertedKey;
        }

        //public static float NGramIOCAlphanumeric(string msg, int n)
        public static float BigramIOCAlphanumeric(string msg)
        {
            int[] counts = new int[36 * 36];
            int total = 0;

            for (int i = 0; i + 1 < msg.Length; i++)
            {
                if (msg[i] >= 97)
                {
                    if (msg[i + 1] >= 97)
                    {
                        counts[(msg[i] - 97) * 36 + msg[i + 1] - 97] += 1;
                    }
                    else
                    {
                        counts[(msg[i] - 97) * 36 + msg[i + 1] - 22] += 1;

                    }
                }
                else
                {
                    if (msg[i + 1] >= 97)
                    {
                        counts[(msg[i] - 22) * 36 + msg[i + 1] - 97] += 1;
                    }
                    else
                    {
                        counts[(msg[i] - 22) * 36 + msg[i + 1] - 22] += 1;

                    }
                }
                total += 1;
            }

            double ioc = 0;

            for (int i = 0; i < 36 * 36; i++)
            {
                ioc = ioc + (double)counts[i] * ((double)counts[i] - 1) / ((double)total * ((double)total - 1));
            }

            return (float)ioc;
        }

        public const float IsMonoSub_LowerIocLimit = 0.06f;
        public const float IsMonoSub_UpperIocLimit = 0.073f;

        public const float IsMonoSub_LowerBigramIocLimit = 0.0066f;
        //public const float IsMonoSub_UpperBigramIocLimit = 0.0085f;
        public const float IsMonoSub_UpperBigramIocLimit = 0.0087f;

        /// To identify if a ciphertext is a monosub without any transpositions applied to it - made for the Bazeries solver;
        public static bool IsNormalMonoSub(string ciphertext)
        {
            /// Monograph IOC
            float ioc = IOCAlphanumeric(ciphertext);

            if (ioc > IsMonoSub_LowerIocLimit && ioc < IsMonoSub_UpperIocLimit)
            {
                /// Bigraph IOC
                //ioc = NGramIOCAlphanumeric(ciphertext, 2);
                ioc = BigramIOCAlphanumeric(ciphertext);

                if (ioc > IsMonoSub_LowerBigramIocLimit && ioc < IsMonoSub_UpperBigramIocLimit)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// For use by the Nicodemus solver. Instead of reading off entire columns, you read off a set number of characters before moving onto the next column. You then come back to first column after reading off a chunk from each column, and
        /// so on.
        public static string IncompleteColumnarTranspoReadOffInChunks(string msg, int[] key, int chunkLength)
        {
            List<char>[] grid = new List<char>[key.Length];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new List<char>();
            }

            int rowNum = msg.Length / key.Length;

            /// Add the ciphertext to the grid, in chunks, starting with the leftmost column, then the one to its right, etc. ...
            /// The first loop is for the complete chunks
            int index = 0;
            for (int i = 0; i < rowNum / chunkLength; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    for (int k = 0; k < chunkLength; k++)
                    {
                        grid[j].Add(msg[index]);
                        index++;
                    }
                }
            }
            /// Now for the incomplete chunks
            if (rowNum % chunkLength != 0)
            {
                for (int i = 0; i < key.Length; i++)
                {
                    for (int j = 0; j < (rowNum % chunkLength); j++)
                    {
                        grid[i].Add(msg[index]);
                        index++;
                    }

                    /// For an incomplete transposition, check if it was an incomplete row and add another letter if it is
                    if (key[i] < msg.Length % key.Length)
                    {
                        grid[i].Add(msg[index]);
                        index++;
                    }
                }
            }

            /// If it was an incomplete transposition, tell the code we need to get an extra row in the plaintext
            if (msg.Length % key.Length != 0)
            {
                rowNum += 1;
            }

            /// Read off the plaintext;
            StringBuilder transpoed = new StringBuilder();

            /// Go down each row
            for (int i = 0; i < rowNum; i++)
            {
                /// j is the next number we are looking for in the key column headings
                for (int j = 0; j < key.Length; j++)
                {
                    /// Go through each key column
                    for (int k = 0; k < key.Length; k++)
                    {
                        //if (key[k] == j)
                        if (key[k] == j && i < grid[k].Count)
                        {
                            transpoed.Append(grid[k][i]);
                            break;
                        }
                    }
                }
            }

            return transpoed.ToString();
        }

        public static Tuple<string, int[]> CrackChunkedColumnTranspoReturnKey(string msg, int keyLength, int chunkSize, int numOfTrials = 1000)
        {
            int[] currentKey = new int[keyLength];

            for (int i = 0; i < keyLength; i++)
            {
                currentKey[i] = i;
            }
            for (int i = 0; i < 100; i++)
            {
                currentKey = MessWithIntKey(currentKey);
            }

            int[] bestKey = new int[keyLength];

            Array.Copy(currentKey, bestKey, keyLength);
            
            float currentScore = QuadgramScore(IncompleteColumnarTranspoReadOffInChunks(msg, currentKey, chunkSize));
            float bestScore = currentScore;

            string decodedMsg = "";

            for (int trial = 0; trial < numOfTrials; trial++)
            {
                Array.Copy(MessWithIntKey(currentKey), currentKey, keyLength);
                
                decodedMsg = IncompleteColumnarTranspoReadOffInChunks(msg, currentKey, chunkSize);

                currentScore = QuadgramScore(decodedMsg);

                if (currentScore > bestScore)
                {
                    bestScore = currentScore;
                    Array.Copy(currentKey, bestKey, keyLength);
                }
                else if (currentScore < bestScore)
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = AnnealingProbability(currentScore - bestScore, numOfTrials - trial);

                    if (ReplaceAnyway == true)
                    {
                        bestScore = currentScore;
                        Array.Copy(currentKey, bestKey, keyLength);
                    }
                    else
                    {
                        Array.Copy(bestKey, currentKey, keyLength);
                    }
                }
                else
                {
                    Array.Copy(bestKey, currentKey, keyLength);
                }
            }
            
            return new Tuple<string, int[]>(IncompleteColumnarTranspoReadOffInChunks(msg, bestKey, chunkSize), bestKey);
        }

        public static string DecryptNicodemus(string msg, string vigenereKey, int[] transpoKey, int chunkLength, string alphabet)
        {
            List<char>[] grid = new List<char>[transpoKey.Length];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new List<char>();
            }

            int rowNum = msg.Length / transpoKey.Length;

            /// Add the ciphertext to the grid, in chunks, starting with the leftmost column, then the one to its right, etc. ...
            /// The first loop is for the complete chunks
            int index = 0;
            for (int i = 0; i < rowNum / chunkLength; i++)
            {
                for (int j = 0; j < transpoKey.Length; j++)
                {
                    for (int k = 0; k < chunkLength; k++)
                    {
                        grid[j].Add(msg[index]);
                        index++;
                    }
                }
            }
            /// Now for the incomplete chunks
            if (rowNum % chunkLength != 0)
            {
                for (int i = 0; i < transpoKey.Length; i++)
                {
                    for (int j = 0; j < (rowNum % chunkLength); j++)
                    {
                        grid[i].Add(msg[index]);
                        index++;
                    }

                    /// For an incomplete transposition, check if it was an incomplete row and add another letter if it is
                    if (transpoKey[i] < msg.Length % transpoKey.Length)
                    {
                        grid[i].Add(msg[index]);
                        index++;
                    }
                }
            }

            /// If it was an incomplete transposition, tell the code we need to get an extra row in the plaintext
            if (msg.Length % transpoKey.Length != 0)
            {
                rowNum += 1;
            }

            /*for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < transpoKey.Length; j++)
                {
                    if (i < grid[j].Count)
                    {
                        Console.Write(grid[j][i] + " ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.Write("\n");
            }
            Console.Write("\n\n");*/

            /// Undo the Vigenere
            //index = 0;
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < transpoKey.Length; j++)
                {
                    if (i < grid[j].Count)
                    {
                        grid[j][i] = alphabet[CipherLib.Annealing.Mod(alphabet.IndexOf(grid[j][i]) - alphabet.IndexOf(vigenereKey[j]), alphabet.Length)];
                    }
                }
            }

            /// Read off the plaintext;
            StringBuilder transpoed = new StringBuilder();

            /// Go down each row
            for (int i = 0; i < rowNum; i++)
            {
                /// j is the next number we are looking for in the key column headings
                for (int j = 0; j < transpoKey.Length; j++)
                {
                    /// Go through each key column
                    for (int k = 0; k < transpoKey.Length; k++)
                    {
                        //if (key[k] == j)
                        if (transpoKey[k] == j && i < grid[k].Count)
                        {
                            transpoed.Append(grid[k][i]);
                            break;
                        }
                    }
                }
            }

            return transpoed.ToString();
        }

        public static int[] IntKeyFromKeyWord(string keyword, string alphabet)
        {
            int[] key = new int[keyword.Length];

            int index = 0;
            for (int i = 0; i < alphabet.Length; i++)
            {
                for (int j = 0; j < keyword.Length; j++)
                {
                    if (keyword[j] == alphabet[i])
                    {
                        key[j] = index;
                        index++;
                    }
                }
            }
            return key;
        }

        public static void DisplayTranspoGrid(List<char>[] grid, int rowNum)
        {
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < grid.Length; j++)
                {
                    if (i < grid[j].Count)
                    {
                        Console.Write(grid[j][i] + " ");
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }
                Console.Write("\n");
            }
            Console.Write("\n\n");
        }
    }
}