using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    class Pollux
    {
        public static string DecryptSimplePollux(string ciphertext, char[] ciphertextAlphabet, char[] key)
        {
            string morse = PolluxSubstitution(ciphertext, ciphertextAlphabet, key);
            //Console.Write(morse);

            StringBuilder plaintext = new StringBuilder();

            //char morseChar;
            string morseChar;

            foreach (string i in morse.Split('/'))
            {
                if (i != "")
                {
                    //morseChar = CipherLib.Morse.MorseToCharSuperRestricted(i)[0];
                    morseChar = CipherLib.Morse.MorseToCharSuperRestricted(i);

                    if (morseChar == null)
                    {
                        return null;
                    }
                    else
                    {
                        plaintext.Append(morseChar);
                    }
                }
            }
            return plaintext.ToString();
        }

        public static string PolluxSubstitution(string ciphertext, char[] ciphertextAlphabet, char[] key)
        {
            StringBuilder morse = new StringBuilder();

            for (int i = 0; i < ciphertext.Length; i++)
            {
                for (int k = 0; k < key.Length; k++)
                {
                    //if (key[k] == morse[i])
                    //if (key[k] == ciphertext[i])
                    if (ciphertextAlphabet[k] == ciphertext[i])
                    {
                        //morse.Append(ciphertextAlphabet[k]);
                        morse.Append(key[k]);
                        break;
                    }
                }
            }
            return morse.ToString();
        }
    }
}
