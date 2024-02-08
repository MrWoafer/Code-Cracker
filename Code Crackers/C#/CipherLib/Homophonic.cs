using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text;

namespace CipherLib
{
    namespace Homophonic
    {
        class HomophonicKey
        {
            private Dictionary<string, char> key;
            private string alphabet;
            private int[] expectedAlphabetFrequencies;
            private int numSymbols;

            public HomophonicKey(string[] symbols, string alphabet, float[] alphabetExpectedFrequencies)
            {
                this.alphabet = alphabet;
                this.expectedAlphabetFrequencies = new int[alphabetExpectedFrequencies.Length];

                for (int i = 0; i < alphabetExpectedFrequencies.Length; i++)
                {
                    this.expectedAlphabetFrequencies[i] = (int)Math.Floor(alphabetExpectedFrequencies[i] * symbols.Length);

                    if (this.expectedAlphabetFrequencies[i] <= 0)
                    {
                        this.expectedAlphabetFrequencies[i] = 1;
                    }
                }

                key = new Dictionary<string, char>();

                numSymbols = symbols.Length;
                for (int i = 0; i < symbols.Length; i++)
                {
                    key[symbols[i]] = 'a';
                }
            }

            public HomophonicKey(HomophonicKey homophoneKey)
            {
                alphabet = homophoneKey.alphabet;
                expectedAlphabetFrequencies = homophoneKey.expectedAlphabetFrequencies;
                numSymbols = homophoneKey.numSymbols;
                key = homophoneKey.key;
            }

            public void RandomiseKey()
            {
                List<string> unassignedSymbols = new List<string>(key.Keys);

                string newSymbol;

                for (int i = 0; i < alphabet.Length; i++)
                {
                    for (int j = 0; j < expectedAlphabetFrequencies[i]; j++)
                    {
                        newSymbol = Utils.PickRandom(unassignedSymbols.ToArray());

                        unassignedSymbols.Remove(newSymbol);
                        key[newSymbol] = alphabet[i];
                    }
                }

                while (unassignedSymbols.Count > 0)
                {
                    int i = Utils.rand.Next() % alphabet.Length;

                    newSymbol = Utils.PickRandom(unassignedSymbols.ToArray());

                    unassignedSymbols.Remove(newSymbol);
                    key[newSymbol] = alphabet[i];
                }
            }

            public string[] GetSymbols()
            {
                return key.Keys.ToArray();
            }

            public int CountHomophones(char chr)
            {
                int total = 0;

                foreach (string i in key.Keys)
                {
                    if (key[i] == chr)
                    {
                        total += 1;
                    }
                }

                return total;
            }

            public string Decrypt(string[] ciphertext)
            {
                string plaintext = "";

                for (int i = 0; i < ciphertext.Length; i++)
                {
                    plaintext += key[ciphertext[i]];
                }

                return plaintext;
            }

            public void ChangeHomophone(string symbol, char decryptsAs)
            {
                key[symbol] = decryptsAs;
            }
        }

        static class Homophonic
        {
            //public static string Decrypt(int[] ciphertext, char[] key)
            private static string Decrypt2(int[] ciphertext, char[] key)
            {
                string plaintext = "";

                for (int i = 0; i < ciphertext.Length; i++)
                {
                    /*try
                    {
                        plaintext += key[ciphertext[i]];
                    }
                    catch
                    {
                        Console.Write("\n\n");
                        DisplayKey(key);
                    }*/
                    plaintext += key[ciphertext[i]];
                }

                return plaintext;
            }

            public static string Decrypt(int[] ciphertext, char[] key)
            //private static string Decrypt2(int[] ciphertext, char[] key)
            {
                StringBuilder plaintext = new StringBuilder();

                for (int i = 0; i < ciphertext.Length; i++)
                {
                    
                    plaintext.Append(key[ciphertext[i]]);
                }

                return plaintext.ToString();
            }

            public static void DisplayKey(char[] key)
            {
                for (int i = 0; i < key.Length; i++)
                {
                    Console.Write(key[i]);
                    if (i < key.Length - 1)
                    {
                        Console.Write(" ");
                    }
                }
            }
        }
    }
}
