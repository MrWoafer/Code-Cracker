using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    public static class ADFGVX
    {
        public const string ADFGVXALPHABET = "abcdefghijklmnopqrstuvwxyz0123456789";

        public static string TransposeADFGVX(string msg, int[] key, CipherLib.TranspositionType transpoType)
        {
            /*string decodedMsg = "";
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
            }*/
            string decodedMsg;
            if (transpoType == CipherLib.TranspositionType.column)
            {
                //decodedMsg = CipherLib.Annealing.DecodeColumnTranspo(msg, key);
                decodedMsg = CipherLib.Annealing.IncompleteColumnarTranspo(msg, key);
            }
            else
            {
                decodedMsg = CipherLib.Annealing.DecodeRowTranspo(msg, key);
            }

            //string replacedMsg = "";
            StringBuilder replacedMsg = new StringBuilder();

            for (int i = 0; i < decodedMsg.Length; i += 2)
            {
                //replacedMsg += DigramToMonogramADFGVX(decodedMsg[i], decodedMsg[i + 1]);
                replacedMsg.Append(DigramToMonogramADFGVX(decodedMsg[i], decodedMsg[i + 1]));
            }

            //return replacedMsg;
            return replacedMsg.ToString();
        }

        public static float IOCADFGVX(string msg, int[] key, CipherLib.TranspositionType transpoType)
        {
            return CipherLib.Annealing.IOCAlphanumeric(TransposeADFGVX(msg, key, transpoType));
        }

        public static void DisplayKey(int[] key)
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

        public static char DigramToMonogramADFGVX(char char1, char char2)
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
        }

        public static string DecodeADFGVX(string msg, int[] key, CipherLib.TranspositionType transpoType, int trials = 2500)
        {
            return CipherLib.Annealing.CrackCustomMonoSub(TransposeADFGVX(msg, key, transpoType), ADFGVXALPHABET, trials, CipherLib.SolutionType.QuadgramScore);
        }
    }

    static class ADFGVXRecombined
    {
        public static string Decrypt(string ciphertext, string gridKey, int[] transpoKey, TranspositionType transpoType)
        {
            /// Use two strings to perform each decipherment stage on. We only need two to save memory and time.
            StringBuilder string1 = new StringBuilder();
            string string2;

            /// Undo Polybius
            int index;
            for (int i = 0; i < ciphertext.Length; i++)
            {
                index = gridKey.IndexOf(ciphertext[i]);
                //string1.Append("adfgvx"[index / 6] + "adfgvx"[index % 6]);
                string1.Append("adfgvx"[index / 6].ToString() + "adfgvx"[index % 6].ToString());
            }

            /// transposition stage
            string2 = CipherLib.Annealing.DecodeTranspo(string1.ToString(), transpoKey, transpoType);
            string1 = new StringBuilder();

            /// Put back with Polybius grid
            //for (int i = 0; i < string2.Length; i += 2)
            for (int i = 0; i + 1 < string2.Length; i += 2)
            {
                string1.Append(gridKey[6 * "adfgvx".IndexOf(string2[i]) + "adfgvx".IndexOf(string2[i + 1])]);
            }

            return string1.ToString();
        }

        public static Tuple<string, string> CrackGridReturnKey(string ciphertext, int[] transpoKey, string alphabet, TranspositionType transpoType, int numOfTrials = 1000)
        {
            string newKey = alphabet;
            for (int i = 0; i < 100; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(newKey);
            }
            string bestKey = newKey;

            float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, transpoType));
            float bestScore = newScore;

            for (int i = 0; i < numOfTrials; i++)
            {
                newKey = CipherLib.Annealing.MessWithStringKey(bestKey);

                newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, transpoType));

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestKey = newKey;
                }
            }

            return new Tuple<string, string>(Decrypt(ciphertext, bestKey, transpoKey, transpoType), bestKey);
        }


        //public static Tuple<string, string, int[]> CrackReturnKey(string ciphertext, string alphabet, int columnNum, TranspositionType transpoType, int numOfTrials = 1000)
        private static Tuple<string, string, int[]> CrackReturnKey2(string ciphertext, string alphabet, int columnNum, TranspositionType transpoType, int numOfTrials = 1000)
        {
            string newGridKey = alphabet;
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

            float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newGridKey, newTranspoKey, transpoType));
            float bestScore = newScore;

            int operation;

            for (int i = 0; i < numOfTrials; i++)
            {
                operation = CipherLib.Utils.rand.Next() % 10;

                if (operation < 7)
                {
                    newGridKey = CipherLib.Annealing.MessWithStringKey(newGridKey);
                }
                else
                {
                    CipherLib.Utils.CopyArray(CipherLib.Annealing.BlockMessWithIntKey(newTranspoKey), newTranspoKey);
                }

                newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newGridKey, newTranspoKey, transpoType));

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestGridKey = newGridKey;
                    CipherLib.Utils.CopyArray(newTranspoKey, bestTranspoKey);
                }
                else
                {
                    bool ReplaceAnyway;
                    ReplaceAnyway = CipherLib.Annealing.AnnealingProbability(newScore - bestScore, numOfTrials - i);

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

            return new Tuple<string, string, int[]>(Decrypt(ciphertext, bestGridKey, bestTranspoKey, transpoType), bestGridKey, bestTranspoKey);
        }

        //public static Tuple<string, string, int[]> CrackReturnKey(string ciphertext, string alphabet, int columnNum, TranspositionType transpoType, int numOfTrials = 1000, bool displayCountdown = false)
        private static Tuple<string, string, int[]> CrackReturnKey(string ciphertext, string alphabet, int columnNum, TranspositionType transpoType, int numOfTrials = 1000, bool displayCountdown = false)
        {
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

            Tuple<string, string> result;

            for (int i = 0; i < perms.Length; i++)
            {
                if (displayCountdown && ((i + 1) % displayPeriod == 0 || i == 0))
                {
                    CipherLib.Utils.ClearLine();
                    Console.Write("Searched " + (i + 1).ToString() + " / " + perms.Length.ToString() + " transposition keys...");
                }

                result = CrackGridReturnKey(ciphertext, perms[i], alphabet, transpoType, numOfTrials);

                newScore = CipherLib.Annealing.QuadgramScore(result.Item1);

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestGridKey = result.Item2;
                    CipherLib.Utils.CopyArray(perms[i], bestTranspoKey);
                }
            }

            //return new Tuple<string, string, int[]>(Decrypt(ciphertext, bestGridKey, bestTranspoKey, transpoType), bestGridKey, bestTranspoKey);
            return new Tuple<string, string, int[]>(CrackGridReturnKey(ciphertext, bestTranspoKey, alphabet, transpoType, 10000).Item1, bestGridKey, bestTranspoKey);
        }

        public static string UndoPolybius(string ciphertext, string gridKey, int[] transpoKey, TranspositionType transpoType)
        {
            StringBuilder undone = new StringBuilder();

            /// Undo Polybius
            int index;
            for (int i = 0; i < ciphertext.Length; i++)
            {
                index = gridKey.IndexOf(ciphertext[i]);
                undone.Append("adfgvx"[index / 6].ToString() + "adfgvx"[index % 6].ToString());
            }

            return undone.ToString();
        }
    }
}
