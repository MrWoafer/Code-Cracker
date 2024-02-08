using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class Transposition
    {
        
    }

    class Cadenus
    {
        public const string Default_Cadenus_Alphabet = "azyxvutsrqponmlkjihgfedcb";

        //public static string Decrypt(string ciphertext, string key, string vertKey = Default_Cadenus_Alphabet, string alphabet = "abcdefghijklmnopqrstuvwxyz")
        //public static string Decrypt(string ciphertext, string key, int[] transpoKey, string vertKey = Default_Cadenus_Alphabet, string alphabet = "abcdefghijklmnopqrstuvwxyz")
        public static string Decrypt(string ciphertext, string key, int[] transpoKey, string vertKey = Default_Cadenus_Alphabet)
        {
            StringBuilder chunk = new StringBuilder();
            string plaintext = "";

            for (int i = 0; i < ciphertext.Length; i++)
            {
                chunk.Append(ciphertext[i]);

                //if (chunk.Length == 25)
                if (chunk.Length == 25 * key.Length)
                {
                    //plaintext += DecryptChunk(chunk.ToString(), key, vertKey, alphabet);
                    //plaintext += DecryptChunk(chunk.ToString(), key, transpoKey, vertKey, alphabet);
                    plaintext += DecryptChunk(chunk.ToString(), key, transpoKey, vertKey);
                    chunk = new StringBuilder();
                }
            }
            if (chunk.Length > 0)
            {
                //plaintext += DecryptChunk(chunk.ToString(), key, vertKey, alphabet);
                //plaintext += DecryptChunk(chunk.ToString(), key, transpoKey, vertKey, alphabet);
                plaintext += DecryptChunk(chunk.ToString(), key, transpoKey, vertKey);
            }

            return plaintext;
        }

        //public static string DecryptFirstChunk(string ciphertext, string key, int[] transpoKey, string vertKey = Default_Cadenus_Alphabet, string alphabet = "abcdefghijklmnopqrstuvwxyz")
        public static string DecryptFirstChunk(string ciphertext, string key, int[] transpoKey, string vertKey = Default_Cadenus_Alphabet)
        {
            StringBuilder chunk = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                chunk.Append(ciphertext[i]);

                if (chunk.Length == 25 * key.Length)
                {
                    //return DecryptChunk(chunk.ToString(), key, transpoKey, vertKey, alphabet);
                    return DecryptChunk(chunk.ToString(), key, transpoKey, vertKey);
                }
            }
            return null;
        }

        public static string DecryptSameKeyForWordAndTranspo(string ciphertext, string key, string vertKey = Default_Cadenus_Alphabet, string alphabet = "abcdefghijklmnopqrstuvwxyz")
        {
            //return Decrypt(ciphertext, key, CipherLib.Annealing.InvertKey(CipherLib.Annealing.IntKeyFromKeyWord(key, alphabet)), vertKey, alphabet);
            return Decrypt(ciphertext, key, CipherLib.Annealing.InvertKey(CipherLib.Annealing.IntKeyFromKeyWord(key, alphabet)), vertKey);
        }

        /// vertKey == vertical Key. I.e. the alphabet that goes vertically next to the grid
        //public static string DecryptChunk(string ciphertext, string key, string vertKey, string alphabet)
        //public static string DecryptChunk(string ciphertext, string key, int[] transpoKey, string vertKey, string alphabet)
        public static string DecryptChunk(string ciphertext, string key, int[] transpoKey, string vertKey)
        {
            List<char>[] grid = new List<char>[key.Length];
            for (int i = 0; i < grid.Length; i++)
            {
                grid[i] = new List<char>();
            }

            int rowNum = ciphertext.Length / key.Length;

            /// Add the ciphertext to the grid, starting with the leftmost column, then the one to its right, etc. ...
            int index = 0;
            /*for (int i = 0; i < key.Length; i++)
            {
                for (int j = 0; j < rowNum; j++)
                {
                    grid[i].Add(ciphertext[index]);
                    index++;
                    
                    /// Vs and Ws are equivalent in a Cadenus
                    if (grid[i][grid[i].Count - 1] == 'w')
                    {
                        grid[i][grid[i].Count - 1] = 'v';
                    }
                }
            }*/
            for (int i = 0; i < rowNum; i++)
            {
                for (int j = 0; j < key.Length; j++)
                {
                    grid[j].Add(ciphertext[index]);
                    index++;

                    /// Vs and Ws are equivalent in a Cadenus
                    if (grid[j][grid[j].Count - 1] == 'w')
                    {
                        grid[j][grid[j].Count - 1] = 'v';
                    }
                }
            }

            //CipherLib.Annealing.DisplayTranspoGrid(grid, rowNum);
            //Console.Write("\n\n");

            /// Get the column permutation from the string key
            //int[] columnPerm = CipherLib.Annealing.InvertKey(CipherLib.Annealing.IntKeyFromKeyWord(key, alphabet));
            //int[] columnPerm = new int[key.Length];
            //CipherLib.Utils.CopyArray(transpoKey, columnPerm);

            /// Shift the columns vertically to undo the vertical transposition.
            /*for (int i = 0; i < key.Length; i++)
            {
                //Console.Write(i + " " + key[columnPerm[i]] + (Default_Cadenus_Alphabet.Length - Default_Cadenus_Alphabet.IndexOf(key[columnPerm[i]])) + "\n");
                for (int j = 0; j < vertKey.Length - vertKey.IndexOf(key[columnPerm[i]]); j++)
                {
                    grid[i].Add(grid[i][0]);
                    grid[i].RemoveAt(0);
                }
            }*/

            //CipherLib.Annealing.DisplayTranspoGrid(grid, rowNum);
            //Console.Write("\n\n");

            //return new string('a', 1400);
            //return "";

            /// Read off the plaintext
            StringBuilder transpoed = new StringBuilder();

            /// Go down each row
            for (int i = 0; i < rowNum; i++)
            {
                /// j is the next number we are looking for in the key column headings
                //for (int j = 0; j < columnPerm.Length; j++)
                for (int j = 0; j < transpoKey.Length; j++)
                {
                    /// Go through each key column
                    //for (int k = 0; k < columnPerm.Length; k++)
                    for (int k = 0; k < transpoKey.Length; k++)
                    {
                        //if (columnPerm[k] == j && i < grid[k].Count)
                        if (transpoKey[k] == j)
                        {
                            //transpoed.Append(grid[k][i]);
                            //transpoed.Append(grid[k][CipherLib.Annealing.Mod(vertKey.Length - vertKey.IndexOf(key[transpoKey[k]]) - i, grid[k].Count)]);
                            transpoed.Append(grid[k][CipherLib.Annealing.Mod(vertKey.Length - vertKey.IndexOf(key[transpoKey[k]]) + i, grid[k].Count)]);
                            break;
                        }
                    }
                }
            }

            /// Return plaintext
            return transpoed.ToString();
        }

        //public static string CrackWordKey(string ciphertext, int[] transpoKey, string vertKey, string alphabet)
        //public static Tuple<string, string> CrackWordKeyReturnKey(string ciphertext, int[] transpoKey, string vertKey, string alphabet)
        public static Tuple<string, string> CrackWordKeyReturnKey(string ciphertext, int[] transpoKey, string vertKey)
        //public static Tuple<string, string> CrackWordKeyReturnKey2(string ciphertext, int[] transpoKey, string vertKey, string alphabet)
        {
            string newKey = new string('a', transpoKey.Length);
            string bestKey = newKey;

            //float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, vertKey, alphabet));
            float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, vertKey));
            //float newScore = CipherLib.Annealing.QuadgramScore(DecryptFirstChunk(ciphertext, newKey, transpoKey, vertKey, alphabet));
            float bestScore = newScore;

            //for (int trial = 0; trial < 10; trial++)
            for (int trial = 0; trial < 1; trial++)
            {
                for (int i = 0; i < transpoKey.Length; i++)
                {
                    //for (int j = 0; j < alphabet.Length; j++)
                    for (int j = 0; j < vertKey.Length; j++)
                    {
                        //newKey = CipherLib.Annealing.SetChar(alphabet[j], i, bestKey);
                        newKey = CipherLib.Annealing.SetChar(vertKey[j], i, bestKey);

                        //newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, vertKey, alphabet));
                        newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, vertKey));
                        //newScore = CipherLib.Annealing.QuadgramScore(DecryptFirstChunk(ciphertext, newKey, transpoKey, vertKey, alphabet));

                        if (newScore > bestScore)
                        {
                            bestScore = newScore;
                            bestKey = newKey;
                        }
                    }
                }
            }

            /// The above code gets very recognisable English, but it is often segmented. So, you get ful, sentences, but they cut off after a line or two and continue elsewhere in the plaintext.
            /// It's fixable by hand, you just need to jigsaw the sentences together.
            /// But, I realised that the fact it's still readable means the shifts for each column are the same as the correct ones, but they're all off by a constant amount.
            /// The code below goes through all 25 shifts of the key to get the one that puts the columns in the right order.
            //newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, vertKey, alphabet));
            //for (int i = 0; i < alphabet.Length; i++)
            for (int i = 0; i < vertKey.Length; i++)
            {
                newKey = bestKey;
                //for (int j = 0; j < newKey.Length; i++)
                for (int j = 0; j < newKey.Length; j++)
                {
                    //newKey = CipherLib.Annealing.SetChar(alphabet[CipherLib.Annealing.Mod(alphabet.IndexOf(newKey[j]) + i, alphabet.Length)], j, newKey);
                    newKey = CipherLib.Annealing.SetChar(vertKey[CipherLib.Annealing.Mod(vertKey.IndexOf(newKey[j]) + i, vertKey.Length)], j, newKey);
                }

                //newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, vertKey, alphabet));
                newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, vertKey));
                //newScore = CipherLib.Annealing.QuadgramScore(DecryptFirstChunk(ciphertext, newKey, transpoKey, vertKey, alphabet));

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestKey = newKey;
                }
            }

            //Console.Write(bestKey);
            //Console.Write("\n\n");
            //return Decrypt(ciphertext, bestKey, transpoKey, vertKey, alphabet);
            //return new Tuple<string, string>(Decrypt(ciphertext, bestKey, transpoKey, vertKey, alphabet), bestKey);
            return new Tuple<string, string>(Decrypt(ciphertext, bestKey, transpoKey, vertKey), bestKey);
        }

        /*public static Tuple<string, string> CrackWordKeyReturnKey(string ciphertext, int[] transpoKey, string vertKey, string alphabet)
        {
            string newKey = new string('a', transpoKey.Length);
            string bestKey = newKey;

            float newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, vertKey, alphabet));
            float bestScore = newScore;

            //for (int trial = 0; trial < 10; trial++)
            //for (int trial = 0; trial < 1000; trial++)
            for (int trial = 0; trial < 100; trial++)
            {
                newKey = CipherLib.Annealing.SetChar(vertKey[CipherLib.Utils.rand.Next() % vertKey.Length], CipherLib.Utils.rand.Next() % newKey.Length, bestKey);

                newScore = CipherLib.Annealing.QuadgramScore(Decrypt(ciphertext, newKey, transpoKey, vertKey, alphabet));

                if (newScore > bestScore)
                {
                    bestScore = newScore;
                    bestKey = newKey;
                }
            }
            return new Tuple<string, string>(Decrypt(ciphertext, bestKey, transpoKey, vertKey, alphabet), bestKey);
        }*/
    }
}
