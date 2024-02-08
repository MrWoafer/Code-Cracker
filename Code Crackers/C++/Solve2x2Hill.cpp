#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include "Annealing.h"
#include <stdio.h>
#include <streambuf>
#include <algorithm>


int Mod26(int Num)
{
	/*while (Num >= 26){
		Num -= 26;
	}
	while (Num < 0){
		Num += 26;
	}
	return Num;*/
	if (Num > 25){
		return Mod26(Num-26);
	}
	else if (Num < 0){
		return Mod26(Num+26);
	}
}


//void CopyKey(int KeyToCopy[4]; int (&KeyTopCopyTo)[4])
//void CopyKey(int KeyToCopy[4], int (&KeyTopCopyTo)[4])
void CopyKey(int KeyToCopy[4], int (&KeyToCopyTo)[4])
{
	int x;
	for (x = 0; x < 4; x++){
		KeyToCopyTo[x] = KeyToCopy[x];
	}
}


//void InvertKey(int (&Key)[4], int Det);
//void InvertKey(int *Key, int Det);
void InvertKey(int *Key, int Det)
{
	int Temp;
	Temp = Key[0];
	/**//*Key[0] = Key[3];
	Key[3] = Temp;
	Key[1] = -Key[1];
	Key[2] = -Key[2];*/
	
	Key[0] = Mod26(Det*Key[3]);
	Key[3] = Mod26(Det*Temp);
	Key[1] = Mod26(-Key[1]*Det);
	Key[2] = Mod26(-Key[2]*Det);
	
	//cout << Key[0] << "  " << Key[1] << "\n" << Key[2] << "  " << Key[3] << "\n";
	//cout << Key[0] << "  " << Key[1] << "\n" << Key[2] << "  " << Key[3] << "\n\n";
	//Key[0] = Det*Key[3];
	//Key[3] = Det*Temp;
	//Key[1] = -Key[1]*Det;
	//Key[2] = -Key[2]*Det;
	//Key[1] = Mod26(-Key[1]*Det);
	///Key[2] = Mod26(-Key[2]*Det);
}


int Determinant(int Key[4])
{
	//int Determinant;
	//return Key[0]*Key[3]-Key[1][*Key[2];
	//return Key[0]*Key[3]-Key[1][*Key[2]];
	return Key[0]*Key[3]-Key[1]*Key[2];
}


int Mod26Inverse(int Num)
{
	Num = Mod26(Num);
	switch (Num){
		case 1: return 1;
		case 3: return 9;
		case 5: return 21;
		case 7: return 15;
		case 9: return 3;
		case 11: return 19;
		case 15: return 7;
		case 17: return 23;
		case 19: return 11;
		case 21: return 5;
		case 23: return 17;
		case 25: return 25;
		default: return 0;
	}
}


/*int Mod26(int Num)
{
	while (Num >= 26){
		Num -= 26;
	}
	while (Num < 0){
		Num += 26;
	}
	return Num;
}*/


//void MultiplyMatrix(int Key[4]; int (&Digram)[2])
void MultiplyMatrix(int Key[4], int (&Digram)[2])
{
	Digram[0] -= 97;
	Digram[1] -= 97;
	int Temp;
	Temp = Key[0]*Digram[0]+Key[1]*Digram[1];
	Digram[1] = Key[2]*Digram[0]+Key[3]*Digram[1];
	Digram[0] = Temp;
}


//string Decode2x2Hill(string Msg, int Key[4]);
string Decode2x2Hill(string Msg, int Key[4])
{
	string Alphabet = "abcdefghijklmnopqrstuvwxyz";
	string NewMsg;
	NewMsg = "";
	
	int Det;
	Det = Determinant(Key);
	//cout << "Mod Inverse: " << Det << " -> ";
	Det = Mod26Inverse(Det);
	//cout << Det << "\n";
	if ((Det == 0) || (Det == 13) || (Det%2 == 0)){
		return Msg;
	}
	else {
		//cout << "Mod Inverse: " << Det << " -> ";
		//Det = Mod26Inverse(Det);
		//cout << Det << "\n";
		//int InverseKey[4];
		//CopyKey(Key, InverseKey)
		InvertKey(Key, Det);
		
		//cout << Key[0] << "  " << Key[1] << "\n" << Key[2] << "  " << Key[3] << "\n\n";
		
		int i;
		int Digram[2];
		for (i = 0; i < Msg.length()-1; i += 2){
			Digram[0] = Msg[i];
			Digram[1] = Msg[i+1];
			MultiplyMatrix(Key, Digram);
			//cout << "\n" << Digram[0] << " " << Digram[1] << "\n\n";
			//NewMsg += alphabet.at(Mod26(Digram[0]));
			//NewMsg += alphabet.at(Mod26(Digram[1]));
			NewMsg += Alphabet.at(Mod26(Digram[0]));
			NewMsg += Alphabet.at(Mod26(Digram[1]));
		}
	}
	
	return NewMsg;
}


int main(int argc, char *argv[])
{
	cout << "Welcome To The 2x2 Hill Code Creaker!\n\nToday, We're Gonna BRUTE FORCE IT!!!\n\n";
	
	/* Get Message */
	string MESSAGE;
	ifstream File("2x2 Hill Message.txt");
	File.seekg(0, ios::end);
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);
	
	cout.precision(17);
	
	double BestScore;
	//char BestKey[4];
	int BestKey[4];
	double CurrentScore;
	//char CurrentKey[4];
	int CurrentKey[4];
	
	CurrentKey[0] = 0;
	CurrentKey[1] = 0;
	CurrentKey[2] = 0;
	CurrentKey[3] = 0;
	
	/*CurrentKey[0] = 5;
	CurrentKey[1] = 17;
	CurrentKey[2] = 14;
	CurrentKey[3] = 3;*/
	
	/*CurrentKey[0] = 25;
	CurrentKey[1] = 24;
	CurrentKey[2] = 17;
	CurrentKey[3] = 15;
	
	CurrentKey[0] = 7;
	CurrentKey[1] = 8;
	CurrentKey[2] = 11;
	CurrentKey[3] = 11;*/
	
	/*CurrentKey[0] = 25;
	CurrentKey[1] = 22;
	CurrentKey[2] = 1;
	CurrentKey[3] = 23;*/
	
	CopyKey(CurrentKey, BestKey);
	
	//*cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n";**//
	
	string DecipheredMsg;
	DecipheredMsg = Decode2x2Hill(MESSAGE, CurrentKey);
	/*cout << DecipheredMsg;*/
	CurrentScore = QuadgramScore(DecipheredMsg);
	BestScore = CurrentScore;
	/*cout << "\n" << BestScore;*/
	
	//cin.get()
	cin.get();
	
	int NumCharsUsed = 26;
	
	int a;
	int b;
	int c;
	int d;
	/*for (a = 0, a < NumCharsUsed, a++){
		for (b = 0, b < NumCharsUsed, b++){
			for (c = 0, c < NumCharsUsed, c++){
				for (d = 0, d < NumCharsUsed, d++){*/
	//a, b, c, d = 0;
	//a, b, c, d = 0, 0, 0, 0;
	/**/a = 0;
	b = 0;
	c = 0;
	d = 0;
	
	//cout "----------------\n";
	cout << "----------------\n";
	
	for (a = 0; a < NumCharsUsed; a++){
		//cout << "\nA: " << a << "!\n";
		//cout << "\nKey Number: " << a*26*26*26+b*26*26+c*26+d << "!\n";
		cout << "\nKeys Analysed: " << a*26*26*26+b*26*26+c*26+d << "!\n";
		for (b = 0; b < NumCharsUsed; b++){
			for (c = 0; c < NumCharsUsed; c++){
				for (d = 0; d < NumCharsUsed; d++){
					//cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n\n";
					CurrentKey[0] = a;
					CurrentKey[1] = b;
					CurrentKey[2] = c;
					CurrentKey[3] = d;
					DecipheredMsg = Decode2x2Hill(MESSAGE, CurrentKey);
					//DecipheredMsg = Decode2x2Hill(MESSAGE, a, b, c, d);
					CurrentScore = QuadgramScore(DecipheredMsg);
					//cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n\n";
					
					if (CurrentScore > BestScore){
						BestScore = CurrentScore;
						//CopyKey(CurrentKey, BestKey);
						//BestKey[0] = CurrentKey[0];
						//BestKey[1] = CurrentKey[1];
						//BestKey[2] = CurrentKey[2];
						//BestKey[3] = CurrentKey[3];
						BestKey[0] = a;
						BestKey[1] = b;
						BestKey[2] = c;
						BestKey[3] = d;
						/*cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n";
						cout << "Score: " << BestScore << "\n";
						//cout << "Decipherment:\n" << BestDecipherment << "\n\n";
						cout << "Decipherment:\n" << DecipheredMsg << "\n\n";*/
						/*cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n";
						cout << CurrentKey[0] << "  " << CurrentKey[1] << "\n" << CurrentKey[2] << "  " << CurrentKey[3] << "\n\n";
						cout << a << " " << b << " " << c << " " << d << "\n\n";*/
					}
				}
			}
		}
		/*cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n\n";
		cout << CurrentKey[0] << "  " << CurrentKey[1] << "\n" << CurrentKey[2] << "  " << CurrentKey[3] << "\n\n";*/
	}
	
	/*cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n\n";*/
	
	//#DecipheredMsg = Decode2x2Hill(MESSAGE, BestKey);
	//string BestDecipherment = Decoded2x2Hill(MESSAGE, BestKey);
	//string BestDecipherment = Decode2x2Hill(MESSAGE, BestKey);
	
	int WoahKey[4];
	
	WoahKey[0] = BestKey[0];
	WoahKey[1] = BestKey[1];
	WoahKey[2] = BestKey[2];
	WoahKey[3] = BestKey[3];
	
	//cout.precision(17);
	
	string BestDecipherment = Decode2x2Hill(MESSAGE, WoahKey);
	
	//cout << "Best Key:\n";
	cout << "\n\n----------------\n\nBest Key:\n";
	//cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n";
	cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n\n";
	//cout << "Score: " << BestScore;
	//cout << "Score: " << BestScore << "\n";
	cout << "Score: " << BestScore << "\n\n";
	//cout << Key[0] << "  " << Key[1] << "\n" << Key[2] << "  " << Key[3] << "\n";
	//cout << BestKey[0] << "  " << BestKey[1] << "\n" << BestKey[2] << "  " << BestKey[3] << "\n";
	//cout << "Decipherment:\n" << DecipheredMsg;
	cout << "Decipherment:\n\n" << BestDecipherment << "\n\n";
	
	cin.get();
	
	return 1;
}