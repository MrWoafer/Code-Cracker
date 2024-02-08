using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    public enum DecryptionType
    {
        subtraction = 0,
        addition = 1
    }

    public static class RunningKey
    {
        public const string ALPHABET = "abcdefghijklmnopqrstuvwxyz";

        public static string DecryptRunningKey(string msg, string key, DecryptionType decryptType = DecryptionType.subtraction, int aIndex = 0)
        {
            if (msg.Length > key.Length)
            {
                throw new ArgumentException("Length of key must be more than or equal to the length of the message.");
            }

            //string newMsg = "";
            StringBuilder newMsg = new StringBuilder();

            for (int i = 0; i < msg.Length; i++)
            {
                if (Annealing.IsEnglishLetter(msg[i]))
                {
                    if (Annealing.IsEnglishLetter(key[i]))
                    {
                        if (decryptType == DecryptionType.subtraction)
                        {
                            //newMsg += ALPHABET[Annealing.Mod(msg[i] - key[i] - aIndex, 26)];
                            newMsg.Append(ALPHABET[Annealing.Mod(msg[i] - key[i] - aIndex, 26)]);
                        }
                        else
                        {
                            //newMsg += ALPHABET[Annealing.Mod(msg[i] + key[i] + aIndex - 194, 26)];
                            newMsg.Append(ALPHABET[Annealing.Mod(msg[i] + key[i] + aIndex - 194, 26)]);
                        }
                    }
                    else
                    {
                        //newMsg += ".";
                        newMsg.Append(".");
                    }
                }
                else
                {
                    //newMsg += msg[i];
                    newMsg.Append(msg[i]);
                }
            }

            //return newMsg;
            return newMsg.ToString();
        }

        public static string DecryptRunningKeyPartial(string msg, string key, DecryptionType decryptType = DecryptionType.subtraction, int aIndex = 0)
        {
            if (msg.Length > key.Length)
            {
                msg = msg.Remove(key.Length, msg.Length - key.Length);
            }

            return DecryptRunningKey(msg, key, decryptType, aIndex);
        }

        public static string DeduceKey(string plaintext, string ciphertext, DecryptionType decryptType = DecryptionType.subtraction, int aIndex = 0)
        {
            return DecryptRunningKeyPartial(ciphertext, plaintext, decryptType, aIndex);
        }
    }
}
