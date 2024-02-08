using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherLib
{
    public static class Enigma
    {
        public static readonly string[] ENIGMA_ROTORS = new string[] {"ekmflgdqvzntowyhxuspaibrcj", "ajdksiruxblhwtmcqgznpyfvoe", "bdfhjlcprtxvznyeiwgakmusqo", "esovpzjayquirhxlnftgkdcmwb",
            "vzbrgityupsdnhlxawmjqofeck", "jpgvoumfyqbenhzrdkasxlictw", "nzjhgrcxmyswboufaivlpekqdt", "fkqhtlxocbjspdzramewniuygv", "leyjvcnixwpbqmdrtakzgfuhos", "fsokanuerhmbtiycwlqpzxvgjd"};

        public static readonly char[][] ENIGMA_NOTCHES = new char[][] { new char[] { 'r' }, new char[] { 'f' }, new char[] { 'w' }, new char[] { 'k' }, new char[] { 'a' }, new char[] { 'a', 'n' },
            new char[] { 'a', 'n' }, new char[] { 'a', 'n' }, new char[] { }, new char[] { } };

        public static readonly string[] ENIGMA_REFLECTORS = {"ejmzalyxvbwfcrquontspikhgd", "yruhqsldpxngokmiebfzcwvjat", "enkqauywjicopblmdxzvfthrgs", "fvpjiaoyedrzxwgctkuqsbnmhl",
            "rdobjntkvehmlfcwzaxgyipsuq" };


        public static char EnigmaPlugboard(char chr, char[][] plugboard)
        {
            char newChr = chr;

            for (int j = 0; j < plugboard.Length; j++)
            {
                if (chr == plugboard[j][0])
                {
                    newChr = plugboard[j][1];
                }
                else if (chr == plugboard[j][1])
                {
                    newChr = plugboard[j][0];
                }
            }

            return newChr;
        }

        public static string EncryptEnigma(string msg, int[] rotorOrder, int[] rotorPositions, int[] ring, int reflector, char[][] plugboard)
        {
            return DecryptEnigma(msg, rotorOrder, rotorPositions, ring, reflector, plugboard);
        }

        public static string DecryptEnigma(string msg, int[] rotorOrder, int[] rotorPositions, int[] ring, int reflector, char[][] plugboard)
        {
            string newMsg = "";
            int[] rotations = new int[3] { 0, 0, 0 };
            char chr;

            for (int i = 0; i < msg.Length; i++)
            {
                if (!Annealing.IsEnglishLetter(msg[i]))
                {
                    newMsg += msg[i];
                }
                else
                {
                    // Rotate fastest rotor
                    rotations[2] += 1;

                    // Check for double step of middle rotor
                    //if ((rotations[1] + 1) % 26 == ((ENIGMA_NOTCHES[rotorOrder[1]][0] - 97) - rotorPositions[1]) % 26)
                    //if ((rotations[1] + 1) % 26 == ((ENIGMA_NOTCHES[rotorOrder[1]][0] - 96) - rotorPositions[1]) % 26)
                    if ((rotations[1] + 1) % 26 == Annealing.Mod((ENIGMA_NOTCHES[rotorOrder[1]][0] - 97) - rotorPositions[1], 26))
                    {
                        rotations[1] += 1;
                        rotations[0] += 1;
                    }

                    // Increment middle rotor if fast rotor hits its notch
                    //if (rotations[2] % 26 == ((ENIGMA_NOTCHES[rotorOrder[0]][0] - 97) - rotorPositions[0]) % 26)
                    //if (rotations[2] % 26 == ((ENIGMA_NOTCHES[rotorOrder[0]][0] - 96) - rotorPositions[0]) % 26)
                    //if (rotations[2] % 26 == Annealing.Mod((ENIGMA_NOTCHES[rotorOrder[0]][0] - 97) - rotorPositions[0], 26))
                    if (rotations[2] % 26 == Annealing.Mod((ENIGMA_NOTCHES[rotorOrder[2]][0] - 97) - rotorPositions[2], 26))
                    {
                        rotations[1] += 1;
                    }
                }

                // Begin encryption
                chr = msg[i];

                chr = EnigmaPlugboard(chr, plugboard);

                for (int j = 2; j >= 0; j--)
                {
                    //Console.Write(ENIGMA_ROTORS[rotorOrder[j]]);
                    //Console.Write("\n");
                    //Console.Write(ENIGMA_ROTORS[rotorOrder[j]][(rotations[j] - ring[j] + (chr - 97) + rotorPositions[j]) % 26]);
                    //Console.Write("\n");
                    //chr = Annealing.ALPHABET[(ENIGMA_ROTORS[rotorOrder[j]][(rotations[j] - ring[j] + (chr - 97) + rotorPositions[j]) % 26] - 97 - rotations[j] + ring[j] - rotorPositions[j]) % 26];
                    chr = Annealing.ALPHABET[Annealing.Mod(ENIGMA_ROTORS[rotorOrder[j]][Annealing.Mod(rotations[j] - ring[j] + (chr - 97) + rotorPositions[j], 26)] - 97 - rotations[j] + ring[j] - rotorPositions[j], 26)];
                }

                chr = ENIGMA_REFLECTORS[reflector][chr - 97];

                for (int j = 0; j <= 2; j++)
                {
                    chr = Annealing.ALPHABET[Annealing.Mod(ENIGMA_ROTORS[rotorOrder[j]].IndexOf(Annealing.ALPHABET[Annealing.Mod((chr - 97) + rotations[j] - ring[j] + rotorPositions[j], 26)]) - rotations[j] + ring[j] - rotorPositions[j], 26)];
                }

                chr = EnigmaPlugboard(chr, plugboard);

                newMsg += chr;
            }

            return newMsg;
        }
    }
}
