![Capture 21](https://github.com/MrWoafer/Code-Cracker/assets/159387325/3fab6620-6f50-4766-87c6-e6aa223b8c59)

# Code Cracker

A tool for automated solving of messages encrypted with classical ciphers. It primarily uses the approach of Simulated Annealing. Also includes statistical tests to help identify ciphers and can suggest possible cipher types for you.

GUI written in Python with tkinter; cipher crackers written in C#, and a few in C++.

# Cipher Types

The program can solve the following cipher types:
* Letter substitutions
  * Monoalphabetic substitution (mono sub)
  * Polyalphabetic substitution (e.g. Vigenere)
  * Homophonic substitution
  * Caesar
  * Affine
  * Polybius
  * Bazeries
  * Straddle Checkerboard
* N-gram substitutions
  * Playfair
  * 2-square
  * 3-square
  * NxN Hill
  * Bifid
* Running keys
  * Autokey
  * Progressive key
  * Interrupted key
  * Ragbaby
* Transpositions
  * Row
  * Column
  * Double transposition
  * Myszkowski
  * AMSCO
  * Rail fence
  * Cadenus
* Combinations
  * ADFGX
  * ADFGVX
  * ABCDEFGHIK (ADFGVX but can substitute 3-grams or 4-grams etc)
  * Mono sub + transposition
  * Polybius + Vigenere
  * Vigenere + Transposition
  * Nicodemus
