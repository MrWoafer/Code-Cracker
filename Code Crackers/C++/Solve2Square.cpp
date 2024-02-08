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

using namespace std;

void DisplayKeys(string Key1, string Key2, bool Horizontal)
{
	if (Horizontal == true){
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
	}
	
	else{
		int y;
		int x;
		for (y = 0; y < 5; y++){
			for (x = 0; x < 5; x++){
				cout << " " << Key1[5*y+x];
				}
			cout << "\n";
			}
			
		cout << "\n\n";
		
		//int y;
		//int x;
		for (y = 0; y < 5; y++){
			for (x = 0; x < 5; x++){
				cout << " " << Key2[5*y+x];
				}
			cout << "\n";
			}
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


string Decode2Square(string Msg, string Key1, string Key2, bool Horizontal, bool SwapDigrams)
{
		string NewMsg;
		NewMsg = "";
	   
		int Index1;
		int Index2;
	   
		int i;
		i = 0;
		for (i = 0; i < Msg.length()-1; i += 2){
			
			if (SwapDigrams == false){
				
				///////Index1 = FindInKey(Msg[i], Key1);
				//////Index2 = FindInKey(Msg[i+1], Key2);
				///Index1 = 5;
				///Index2 = 1;
				Index1 = Key1.find(Msg[i]);
				Index2 = Key2.find(Msg[i+1]);
				
			}
			
			else {
				
				/*Index1 = FindInKey(Msg[i+1], Key1);
				Index2 = FindInKey(Msg[i], Key2);*/
				//Index1 = FindInKey(Msg[i], Key2);
				//Index2 = FindInKey(Msg[i+1], Key1);
				Index1 = Key2.find(Msg[i]);
				Index2 = Key1.find(Msg[i+1]);
				
			}
			
			//cout << Index1 << " " << Index2 << "\n";
			//////cout << Msg[i] << " " << Msg[i+1] << " " << Index1 << " " << Index2 << "\n";
			
			//////cin.get();
			 
			if (Horizontal == true){
			 
				if (SwapDigrams == false){
					/*NewMsg += Key1[5*floor(Index2/5)+Index1%5];
					NewMsg += Key4[5*floor(Index1/5)+Index2%5];*/
					NewMsg += Key1[5*floor(Index2/5)+Index1%5];
					NewMsg += Key2[5*floor(Index1/5)+Index2%5];
				}
				 
				else {
					/*NewMsg += Key1[5*floor(Index1/5)+Index2%5];
					NewMsg += Key4[5*floor(Index2/5)+Index1%5];*/
					/*NewMsg += Key2[5*floor(Index1/5)+Index2%5];
					NewMsg += Key1[5*floor(Index2/5)+Index1%5];*/
					NewMsg += Key1[5*floor(Index1/5)+Index2%5];
					NewMsg += Key2[5*floor(Index2/5)+Index1%5];
				}
			}
			
			else {
				if (SwapDigrams == false){
					NewMsg += Key1[5*floor(Index1/5)+Index2%5];
					NewMsg += Key2[5*floor(Index2/5)+Index1%5];
				}
				 
				else {
					/*NewMsg += Key2[5*floor(Index2/5)+Index1%5];
					NewMsg += Key1[5*floor(Index1/5)+Index2%5];*/
					NewMsg += Key1[5*floor(Index2/5)+Index1%5];
					NewMsg += Key2[5*floor(Index1/5)+Index2%5];
				}
			}
        }
       return NewMsg;
}


double Score(string Msg, string Key1, string Key2, bool Horizontal, bool SwapDigrams)
{
	string DecodedMsg;
	DecodedMsg = Decode2Square(Msg, Key1, Key2, Horizontal, SwapDigrams);
	
	double QuadScore;
	QuadScore = QuadgramScore(DecodedMsg);
	
	return QuadScore;
}


void SwapLetters(string &Key)
{
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


void SwapALetter(string &Key1, string &Key2)
{
	int RandKey;
	
	RandKey = rand()%2;
		
	if (RandKey == 0){
		SwapLetters(Key1);
	}
	else if (RandKey == 1){
		SwapLetters(Key2);
	}
	
}


void SwapRows(string &Key)
{
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
	reverse(Key.begin(), Key.end());
}


void MessAroundWithTheKeys(string &Key1, string &Key2)
{
	int Manoeuvre;
	Manoeuvre = rand()%50;
	switch (Manoeuvre){
		case 0: SwapRows(Key1); break;
		case 1: SwapRows(Key2); break;
		case 4: SwapColumns(Key1); break;
		case 5: SwapColumns(Key2); break;
		case 8: ReverseKey(Key1); break;
		case 9: ReverseKey(Key2); break;
		case 12: SwapAllRows(Key1); break;
		case 13: SwapAllRows(Key2); break;
		case 16: SwapAllColumns(Key1); break;
		case 17: SwapAllColumns(Key2); break;
		default: SwapALetter(Key1, Key2); break;
	}
}


void CopyKey(char InputKey[5][5], char (&OutputKey)[5][5])
{
	int x;
	int y;
	for (x = 0; x < 5; x++){
		for (y = 0; y < 5; y++){
			OutputKey[x][y] = InputKey[x][y];
		}
	}
	return;
}


double TwoSquareAnnealing(string Msg, string &OutputKey1, string &OutputKey2, bool Horizontal, bool SwapDigrams, float Temperature, float Step, int Count)
{
	double BestScore;
	
	string BestKey1;
	string BestKey2;
	
	BestKey1 = OutputKey1;
	BestKey2 = OutputKey2;
	
	BestScore = Score(Msg, BestKey1, BestKey2, Horizontal, SwapDigrams);
	
	string CurrentKey1;
	string CurrentKey2;
	CurrentKey1 = BestKey1;
	CurrentKey2 = BestKey2;
	
	double CurrentScore;
	
	float T;
	int Trial;
	for (T = Temperature; T >= 0; T -= Step){
		cout << T << "\n";
		for (Trial = 0; Trial < Count; Trial++){
			if (true){
				MessAroundWithTheKeys(CurrentKey1, CurrentKey2);
				CurrentScore = Score(Msg, CurrentKey1, CurrentKey2, Horizontal, SwapDigrams);
				
				if (CurrentScore > BestScore){
					BestScore = CurrentScore;
					BestKey1 = CurrentKey1;
					BestKey2 = CurrentKey2;
				}
				else if (CurrentScore < BestScore){
					
					bool ReplaceAnyway;
					ReplaceAnyway = AnnealingProbability(CurrentScore-BestScore, T);
					
					if (ReplaceAnyway == true){
						BestScore = CurrentScore;
						BestKey1 = CurrentKey1;
						BestKey2 = CurrentKey2;
					}
					else {
						CurrentKey1 = BestKey1;
						CurrentKey2 = BestKey2;
					}
				}
				else {
					CurrentKey1 = BestKey1;
					CurrentKey2 = BestKey2;
				}
			}
		}
	}
	OutputKey1 = BestKey1;
	OutputKey2 = BestKey2;
	return BestScore;
}


void Solve2Square(string Msg, bool Horizontal, bool SwapDigrams)
{
	string AlphabetForKey;
	AlphabetForKey = "abcdefghiklmnopqrstuvwxyz";
    
    string BestKey1 = AlphabetForKey;
	string BestKey2 = AlphabetForKey;
	
	int i;
	for(i = 0; i < 100; i++){
		SwapALetter(BestKey1, BestKey2);
	}
	
	double BestScore = Score(Msg, BestKey1, BestKey2, Horizontal, SwapDigrams);
	
	cout.precision(17);
	
	DisplayKeys(BestKey1, BestKey2, Horizontal);
	
	cout << "\nScore: " << BestScore << "\n\n";
	
	cout << "--------------------------------------\n\n";
	
	string NewKey1;
	string NewKey2;
	
	double NewScore;
	
	cout.precision(17);
	
	int anneal;
	anneal = 0;
	while (true){
		anneal += 1;
		cout << "Trial: " << anneal << "\n\n";
		
		NewKey1 = BestKey1;
		NewKey2 = BestKey2;
		
		NewScore = TwoSquareAnnealing(Msg, NewKey1, NewKey2, Horizontal, SwapDigrams, 20, 0.2, 10000);
		
		if (NewScore > BestScore){
			BestScore = NewScore;
			
			BestKey1 = NewKey1;
			BestKey2 = NewKey2;
			
			cout << "\n";
			cout << "New Best Key!\n\n";
			cout << "Score: " << fixed << BestScore << "\n";
			cout << "\n";
			cout << "Key:\n";
			//DisplayKeys(BestKey1, BestKey2);
			DisplayKeys(BestKey1, BestKey2, Horizontal);
			cout << "\n";
			string Decipherment = Decode2Square(Msg, BestKey1, BestKey2, Horizontal, SwapDigrams);
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
	
    bool HORIZONTAL;
	bool SWAPDIGRAMS;
    string MESSAGE;
	
	ifstream File("2-Square Message.txt");
	
	File.seekg(0, ios::end);
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);

	if (string(argv[1]) == "True"){
        HORIZONTAL = true;
    } 
	else {
		HORIZONTAL = false;
    }
	
	if (string(argv[2]) == "True"){
        SWAPDIGRAMS = true;
    } 
	else {
		SWAPDIGRAMS = false;
    }
    
    string Alphabet;
    string AlphabetForKey;
    Alphabet = "abcdefghijklmnopqrstuvwxyz";
    AlphabetForKey = "abcdefghiklmnopqrstuvwxyz";
    
    string Key1;
	string Key2;
	
	Key1 = AlphabetForKey;
	Key2 = AlphabetForKey;
	
	//////Key1 = "zebrafcdghiklmnopqstuvwxy";
	//////Key2 = "alphbetcdfgikmnoqrsuvwxyz";
	Key1 = "mrscabdefghiklnopqtuvwxyz";
	Key2 = "abigftlrcdehkmnopqsuvwxyz";
	
	/*//DisplayKeys(Key1, Key2);
	//DisplayKeys(Key1, Key2, Horizontal);
	DisplayKeys(Key1, Key2, HORIZONTAL);
	cout << "\n";
	
	cin.get();
	
	//Key1 = "ghklmvnbreopwqcztyuifdsax";
	//Key2 = "tuoplmfcnvgbedhsxyaizqwrk";
	////Key1 = "zebrafcdghiklmnopqstuvwxy";
	////Key2 = "alphbetcdfgikmnoqrsuvwxyz";
	
	//cout << "\n" << Decode2Square(MESSAGE, Key1, Key2, Horizontal, SwapDigrams) << "\n";
	cout << "\n" << Decode2Square(MESSAGE, Key1, Key2, HORIZONTAL, SWAPDIGRAMS) << "\n";
	cout << "\n";
	
	//cout << "Score: " << Score(MESSAGE, Key1, Key2) << "\n\n";;
	cout << "Score: " << Score(MESSAGE, Key1, Key2, HORIZONTAL, SWAPDIGRAMS) << "\n\n";;
	
	cin.get();*/
	
	cout << "Starting Solve!\n\n";
	
	Solve2Square(MESSAGE, HORIZONTAL, SWAPDIGRAMS);
	
	cin.get();/**/
    
    return 1;
}
