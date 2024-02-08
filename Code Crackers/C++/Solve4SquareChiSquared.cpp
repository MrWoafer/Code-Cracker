#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include "Annealing.h"
#include <stdio.h>
#include <streambuf>
#include <algorithm>
#include <time.h>
#include "ChiSquare.h"

using namespace std;

void DisplayKeys(string Key1, string Key2, string Key3, string Key4)
{
    int y;
    int x;
    for (y = 0; y < 5; y++){
        for (x = 0; x < 5; x++){
            cout << " " << Key1[5*y+x];
            }
		cout << "   ";
		for (x = 0; x < 5; x++){
            cout << " " << Key2[5*y+x];
            }
        cout << "\n";
        }
		
	cout << "\n\n";
	
	for (y = 0; y < 5; y++){
        for (x = 0; x < 5; x++){
            cout << " " << Key3[5*y+x];
            }
		cout << "   ";
		for (x = 0; x < 5; x++){
            cout << " " << Key4[5*y+x];
            }
        cout << "\n";
        }
    return;
}


int CycleRound(int Num, int Min, int Max)
{
    while (Num > Max){
          Num -= Max-Min+1;
          }
    while (Num < Min){
          Num += Max-Min+1;
          }
    return Num;
}


int FindInKey(char Char, string Key)
{
	int i;
	for (i = 0; i < 25; i++){
		if (Key[i] == Char){
			return i;
		}
	}
	return 25;
}


//string DecodePlayfair(string Msg, string Key1, string Key2, string Key3, string Key4)
string Decode4Square(string Msg, string Key1, string Key2, string Key3, string Key4)
{
       string NewMsg;
       NewMsg = "";
	   
	   int Index1;
	   int Index2;
	   
       int i;
       i = 0;
		for (i = 0; i < Msg.length()-1; i += 2){
			 //Index1 = FindInKey(Msg[i], Key1);
			 //Index2 = FindInKey(Msg[i+1], Key4);
			 Index1 = FindInKey(Msg[i], Key2);
			 Index2 = FindInKey(Msg[i+1], Key3);
			 
			 //NewMsg += Key2[5*floor(Index1/5)+Index2%5];
			 //NewMsg += Key3[5*floor(Index2/5)+Index1%5];
			 NewMsg += Key1[5*floor(Index1/5)+Index2%5];
			 NewMsg += Key4[5*floor(Index2/5)+Index1%5];
             }
       return NewMsg;
}


double Score(string Msg, string Key1, string Key2, string Key3, string Key4)
{
	string DecodedMsg;
	DecodedMsg = Decode4Square(Msg, Key1, Key2, Key3, Key4);
	
	//double QuadScore;
	//QuadScore = QuadgramScore(DecodedMsg);
	double ChiScore;
	ChiScore = ChiSquare(DecodedMsg);
	
	/*cout << "MSG: " << DecodedMsg << "\n";
	cout << "SCORE: " << ChiScore << "\n\n";*/
	
	//return QuadScore;
	return ChiScore;
}


void SwapLetters(string &Key)
{
	//cout << "Swapped Two Letters!\n";
	int x1;
	int x2;
	
	x1 = rand()%25;
	x2 = rand()%25;
	while (x1 == x2){
		x2 = rand()%25;
	}
	
	char Temp;
	Temp = Key[x1];
	Key[x1] = Key[x2];
	Key[x2] = Temp;
}


//void SwapALetter(string Key1, string Key2, string Key3, string Key4, bool NormalAlphabetSquares)
void SwapALetter(string &Key1, string &Key2, string &Key3, string &Key4, bool NormalAlphabetSquares)
{
	//cout << "Swapping Letters!\n";
	int RandKey;
	
	if (NormalAlphabetSquares == false){
		//cout << "Any Key!\n";
		RandKey = rand()%4;
		
		if (RandKey == 0){
			SwapLetters(Key1);
		}
		else if (RandKey == 1){
			SwapLetters(Key2);
		}
		else if (RandKey == 2){
			SwapLetters(Key3);
		}
		else if (RandKey == 3){
			SwapLetters(Key4);
		}
	}
	
	else {
		//cout << "Two Keys!\n";
		RandKey = rand()%2;
		
		if (RandKey == 0){
			SwapLetters(Key2);
		}
		else if (RandKey == 1){
			SwapLetters(Key3);
		}
	}
}


void SwapRows(string &Key)
{
	//cout << "Swapped Two Rows!\n";
	int y1;
	int y2;
	
	y1 = rand()%5;
	y2 = y1;
	while (y1 == y2){
		y2 = rand()%5;
	}
	
	char Temp;
	int i;
	
	for (i=0;i<5;i++){
		Temp = Key[5*y1+i];
		Key[5*y1+i] = Key[5*y2+i];
		Key[5*y2+i] = Temp;
	}
}


void SwapColumns(string &Key)
{
	//cout << "Swapped Two Columns!\n";
	int x1;
	int x2;
	
	x1 = rand()%5;
	x2 = x1;
	while (x1 == x2){
		x2 = rand()%5;
	}
	
	char Temp;
	int i;
	
	for (i = 0; i < 5; i++){
		Temp = Key[5*i+x1];
		Key[5*i+x1] = Key[5*i+x2];
		Key[5*i+x2] = Temp;
	}
}


void SwapAllColumns(string &Key)
{
	//cout << "Swapped All Columns!\n";
	char Temp;
	int i;
	
	for (i = 0; i < 5; i++){
		Temp = Key[5*i+0];
		Key[5*i+0] = Key[5*i+4];
		Key[5*i+4] = Temp;
	
		Temp = Key[5*i+1];
		Key[5*i+1] = Key[5*i+3];
		Key[5*i+3] = Temp;
	}
}


void SwapAllRows(string &Key)
{
	//cout << "Swapped All Rows!\n";
	int i;
	char Temp;
	
	for (i = 0; i < 5; i++){
		Temp = Key[0+i];
		Key[0+i] = Key[20+i];
		Key[20+i] = Temp;
	
		Temp = Key[5+i];
		Key[5+i] = Key[15+i];
		Key[15+i] = Temp;
	}
}


void ReverseColumn(string &Key)
{
	//cout << "Reversed Column!\n";
	int x1;
	
	x1 = rand()%5;
	
	char Temp;
	
	Temp = Key[0+x1];
	Key[0+x1] = Key[20+x1];
	Key[20+x1] = Temp;
	
	Temp = Key[5+x1];
	Key[5+x1] = Key[15+x1];
	Key[15+x1] = Temp;
}



void ReverseRow(string &Key)
{
	//cout << "Reversed Row!\n";
	int y1;
	
	y1 = rand()%5;
	
	char Temp;
	
	Temp = Key[5*y1+0];
	Key[5*y1+0] = Key[5*y1+4];
	Key[5*y1+4] = Temp;
	
	Temp = Key[5*y1+1];
	Key[5*y1+1] = Key[5*y1+3];
	Key[5*y1+3] = Temp;
}


void ReverseKey(string &Key)
{
	//cout << "Reversed Key!\n";
	reverse(Key.begin(), Key.end());
}


void MessAroundWithTheKeys(string &Key1, string &Key2, string &Key3, string &Key4, bool NormalAlphabetSquares)
{
	//cout << "\n" << NormalAlphabetSquares << "\n";
	int Manoeuvre;
	if (NormalAlphabetSquares == false){
		//cout << "Any Key!!!\n";
		Manoeuvre = rand()%100;
		switch (Manoeuvre){
			case 0: SwapRows(Key1); break;
			case 1: SwapRows(Key2); break;
			case 2: SwapRows(Key3); break;
			case 3: SwapRows(Key4); break;
			case 4: SwapColumns(Key1); break;
			case 5: SwapColumns(Key2); break;
			case 6: SwapColumns(Key3); break;
			case 7: SwapColumns(Key4); break;
			case 8: ReverseKey(Key1); break;
			case 9: ReverseKey(Key2); break;
			case 10: ReverseKey(Key3); break;
			case 11: ReverseKey(Key4); break;
			case 12: SwapAllRows(Key1); break;
			case 13: SwapAllRows(Key2); break;
			case 14: SwapAllRows(Key3); break;
			case 15: SwapAllRows(Key4); break;
			case 16: SwapAllColumns(Key1); break;
			case 17: SwapAllColumns(Key2); break;
			case 18: SwapAllColumns(Key3); break;
			case 19: SwapAllColumns(Key4); break;
			default: SwapALetter(Key1, Key2, Key3, Key4, NormalAlphabetSquares); break;
		}
	}
	else {
		//cout << "Two Keys!!!\n";
		Manoeuvre = rand()%50;
		switch (Manoeuvre){
			case 1: SwapRows(Key2); break;
			case 2: SwapRows(Key3); break;
			case 5: SwapColumns(Key2); break;
			case 6: SwapColumns(Key3); break;
			case 9: ReverseKey(Key2); break;
			case 10: ReverseKey(Key3); break;
			case 13: SwapAllRows(Key2); break;
			case 14: SwapAllRows(Key3); break;
			case 17: SwapAllColumns(Key2); break;
			case 18: SwapAllColumns(Key3); break;
			default: SwapALetter(Key1, Key2, Key3, Key4, NormalAlphabetSquares); break;
		}
	}
}


void CopyKey(char InputKey[5][5], char (&OutputKey)[5][5])
{
	int x;
	int y;
	for (x = 0; x < 5; x++){
		for (y = 0; y < 5; y++){
			//OutputKey[x][y] = CopyKey[x][y];
			OutputKey[x][y] = InputKey[x][y];
		}
	}
	return;
}


double FourSquareAnnealing(string Msg, string &OutputKey1, string &OutputKey2, string &OutputKey3, string &OutputKey4, bool NormalAlphabetSquares, float Temperature, float Step, int Count)
{
	double BestScore;
	
	string BestKey1;
	string BestKey2;
	string BestKey3;
	string BestKey4;
	
	BestKey1 = OutputKey1;
	BestKey2 = OutputKey2;
	BestKey3 = OutputKey3;
	BestKey4 = OutputKey4;
	
	BestScore = Score(Msg, BestKey1, BestKey2, BestKey3, BestKey4);
	
	string CurrentKey1;
	string CurrentKey2;
	string CurrentKey3;
	string CurrentKey4;
	CurrentKey1 = BestKey1;
	CurrentKey2 = BestKey2;
	CurrentKey3 = BestKey3;
	CurrentKey4 = BestKey4;
	
	double CurrentScore;
	
	float T;
	int Trial;
	for (T = Temperature; T >= 0; T -= Step){
		cout << T << "\n";
		for (Trial = 0; Trial < Count; Trial++){
			if (true){
				MessAroundWithTheKeys(CurrentKey1, CurrentKey2, CurrentKey3, CurrentKey4, NormalAlphabetSquares);
				CurrentScore = Score(Msg, CurrentKey1, CurrentKey2, CurrentKey3, CurrentKey4);
				
				if (CurrentScore < BestScore){
					BestScore = CurrentScore;
					BestKey1 = CurrentKey1;
					BestKey2 = CurrentKey2;
					BestKey3 = CurrentKey3;
					BestKey4 = CurrentKey4;
				}
				else if (CurrentScore > BestScore){
					
					bool ReplaceAnyway;
					ReplaceAnyway = AnnealingProbability(CurrentScore-BestScore, T);
					
					if (ReplaceAnyway == true){
						BestScore = CurrentScore;
						BestKey1 = CurrentKey1;
						BestKey2 = CurrentKey2;
						BestKey3 = CurrentKey3;
						BestKey4 = CurrentKey4;
					}
					else {
						CurrentKey1 = BestKey1;
						CurrentKey2 = BestKey2;
						CurrentKey3 = BestKey3;
						CurrentKey4 = BestKey4;
					}
				}
				else {
					CurrentKey1 = BestKey1;
					CurrentKey2 = BestKey2;
					CurrentKey3 = BestKey3;
					CurrentKey4 = BestKey4;
				}
			}
		}
	}
	OutputKey1 = BestKey1;
	OutputKey4 = BestKey4;
	OutputKey2 = BestKey2;
	OutputKey3 = BestKey3;
	return BestScore;
}


void Solve4Square(string Msg, bool NormalAlphabetSquares)
{
	string AlphabetForKey;
	AlphabetForKey = "abcdefghiklmnopqrstuvwxyz";
    
    string BestKey1 = AlphabetForKey;
	string BestKey2 = AlphabetForKey;
	string BestKey3 = AlphabetForKey;
	string BestKey4 = AlphabetForKey;
	
	double BestScore = Score(Msg, BestKey1, BestKey2, BestKey3, BestKey4);
	
	string NewKey1;
	string NewKey2;
	string NewKey3;
	string NewKey4;
	
	double NewScore;
	
	cout.precision(17);
	
	int anneal;
	anneal = 0;
	while (true){
		anneal += 1;
		cout << "Trial: " << anneal << "\n\n";
		
		NewKey1 = BestKey1;
		NewKey2 = BestKey2;
		NewKey3 = BestKey3;
		NewKey4 = BestKey4;
		
		NewScore = FourSquareAnnealing(Msg, NewKey1, NewKey2, NewKey3, NewKey4, NormalAlphabetSquares, 20, 0.2, 10000);
		
		if (NewScore < BestScore){
			BestScore = NewScore;
			
			BestKey1 = NewKey1;
			BestKey2 = NewKey2;
			BestKey3 = NewKey3;
			BestKey4 = NewKey4;
			
			cout << "New Best Key!\n\n";
			cout << "Score: " << fixed << BestScore << "\n";
			cout << "\n";
			cout << "Key:\n";
			DisplayKeys(BestKey1, BestKey2, BestKey3, BestKey4);
			cout << "\n";
			string Decipherment = Decode4Square(Msg, BestKey1, BestKey2, BestKey3, BestKey4);
			cout << "Decipherment: " << Decipherment;
			cout << "\n\n";
		}
		
		else {
			cout << "Didn't find a better key...";
			cout << "\n\n";
		}
		cout << "--------------------------------------\n\n";
	}
}


int main(int argc, char *argv[])
{
	srand(time(0));
	cout.precision(17);
	
    bool NORMALALPHABETSQUARES;
    string MESSAGE;
	
	ifstream File("4-Square Message.txt");
	
	File.seekg(0, ios::end);
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);

    
    //if (argv[1] == "True"){
	if (string(argv[1]) == "True"){
		//cout << "Yes!\n";
        NORMALALPHABETSQUARES = true;
    } 
    ////if (argv[1] == "False"){
	else {
		//cout << "No!\n";
		NORMALALPHABETSQUARES = false;
    } 

	//cout << "\n" << NORMALALPHABETSQUARES << "\n";
    
    string Alphabet;
    string AlphabetForKey;
    Alphabet = "abcdefghijklmnopqrstuvwxyz";
    AlphabetForKey = "abcdefghiklmnopqrstuvwxyz";
    
    string Key1;
	string Key2;
	string Key3;
	string Key4;
	
	Key1 = AlphabetForKey;
	Key2 = AlphabetForKey;
	Key3 = AlphabetForKey;
	Key4 = AlphabetForKey;
	
	///////DisplayKeys(Key1, Key2, Key3, Key4);
	///////cout << "\n";
	
	//MessAroundWithTheKeys(key1, Key2, Key3, Key4, NORMALALPHABETSQUARES);
	////MessAroundWithTheKeys(Key1, Key2, Key3, Key4, NORMALALPHABETSQUARES);
	
	///DisplayKeys(Key1, Key2, Key3, Key4);
	///cout << "\n";
	///cout << "\n";
	
	////////cin.get();
	
	/*Key1 = AlphabetForKey;
	Key2 = "poiuytrewqlkhgfdsamnbvcxz";
	Key3 = "lkhgfdsamnbvcxzqwertyuiop";
	Key4 = AlphabetForKey;*/
	
	/*Key1 = "ghklmvnbreopwqcztyuifdsax";
	Key2 = "tuoplmfcnvgbedhsxyaizqwrk";
	Key3 = "edygciurfthoskbnmvlxzqwpa";
	Key4 = "wsdgiuehcvkrybqaftnmxzopl";*/
	
	Key1 = AlphabetForKey;
	Key2 = "kfmlugwsqepoztnrbhdavxciy";
	Key3 = "ugsvkfizmoyxpqrwthlncabed";
	Key4 = AlphabetForKey;
	
	cout << "Chi Square: " << ChiSquare(MESSAGE) << "\n\n";
	
	cout << "\n" << Decode4Square(MESSAGE, Key1, Key2, Key3, Key4) << "\n";////////////
	cout << "\n";/////////////
	
	cout << "Score: " << Score(MESSAGE, Key1, Key2, Key3, Key4) << "\n\n";;
	
	cout << "Starting Solve!\n\n";
	
	Solve4Square(MESSAGE, NORMALALPHABETSQUARES);
	
	cin.get();/**/
    
    return 1;
}
