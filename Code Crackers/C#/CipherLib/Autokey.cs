using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class Autokey
    {
        public static string DecodePartial(string ciphertext, int keyColumn, int shift, int keyLength, string alphabet)
        {
            StringBuilder plaintext = new StringBuilder();

            for (int i = keyColumn; i < ciphertext.Length; i += keyLength)
            {
                plaintext.Append(alphabet[CipherLib.Utils.Mod(alphabet.IndexOf(ciphertext[i]) - shift, alphabet.Length)]);
                shift = alphabet.IndexOf(plaintext[plaintext.Length - 1]);
            }
            return plaintext.ToString();
        }

        public static string Decode(string ciphertext, string key, string alphabet)
        {
            StringBuilder plaintext = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                plaintext.Append(alphabet[CipherLib.Utils.Mod(alphabet.IndexOf(ciphertext[i]) - alphabet.IndexOf(key[i]), alphabet.Length)]);
                key += plaintext[plaintext.Length - 1];
            }
            return plaintext.ToString();
        }
    }

    class InterruptedKey
    {
        public static string Decrypt(string ciphertext, string key, string alphabet)
        {
            StringBuilder plaintext = new StringBuilder();

            string[] split = ciphertext.Split();

            for (int i = 0; i < split.Length; i++)
            {
                for (int j = 0; j < split[i].Length; j++)
                {
                    plaintext.Append(alphabet[CipherLib.Utils.Mod(alphabet.IndexOf(split[i][j]) - alphabet.IndexOf(key[j % key.Length]), alphabet.Length)]);
                }
            }

            return plaintext.ToString();
        }

        public static string DecryptPartial(string ciphertext, int shift, int keyColumn, string alphabet)
        {
            StringBuilder plaintext = new StringBuilder();

            string[] split = ciphertext.Split();

            for (int i = 0; i < split.Length; i++)
            {
                if (keyColumn < split[i].Length)
                {
                    plaintext.Append(alphabet[CipherLib.Utils.Mod(alphabet.IndexOf(split[i][keyColumn]) - shift, alphabet.Length)]);
                }
            }

            return plaintext.ToString();
        }

        public static Tuple<string, string> CrackReturnKey(string ciphertext, int keyLength, string alphabet)
        {
            string key = "";

            float bestScore = float.MinValue;
            int bestKey;
            float newScore;

            for (int i = 0; i < keyLength; i++)
            {
                bestKey = 0;
                bestScore = CipherLib.Annealing.ChiSquared(DecryptPartial(ciphertext, 0, i, alphabet));

                for (int j = 1; j < alphabet.Length; j++)
                {
                    newScore = CipherLib.Annealing.ChiSquared(DecryptPartial(ciphertext, j, i, alphabet));

                    if (newScore < bestScore)
                    {
                        bestScore = newScore;
                        bestKey = j;
                    }
                }

                key += alphabet[bestKey];
            }

            return new Tuple<string, string>(Decrypt(ciphertext, key, alphabet), key);
        }
    }
}
