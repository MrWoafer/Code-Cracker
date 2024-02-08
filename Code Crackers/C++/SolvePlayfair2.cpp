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

void DisplayKey(string Key)
{
	int y;
	int x;
	for (y = 0; y < 5; y++){
		for (x = 0; x < 5; x++){
			//cout << " " << Key1[5*y+x];
			cout << " " << Key[5*y+x];
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


//string DecodePlayfair(string Msg, string Key, DecodeAsDoubleLetter)
string DecodePlayfair(string Msg, string Key, bool DecodeAsDoubleLetter)
{
		string NewMsg;
		NewMsg = "";
	   
		int Index1;
		int Index2;
	   
		int i;
		i = 0;
		for (i = 0; i < Msg.length()-1; i += 2){
				
			Index1 = Key.find(Msg[i]);
			Index2 = Key.find(Msg[i+1]);
			 
			if (floor(Index1/5) == floor(Index2/5)){
			 
				//NewMsg += Key[5*floor(Index1/5)+CycleRound(Index1%5-1, 0, 4)];
				//NewMsg += Key[5*floor(Index1/5)+(Index1%5-1)%5];
				//NewMsg += Key[5*floor(Index1/5)+(Index1%5-1)%5];
				NewMsg += Key[5*floor(Index1/5)+CycleRound(Index1%5-1, 0, 4)];
				//NewMsg += Key[5*floor(Index2/5)+(Index2%5-1%5];
				//NewMsg += Key[5*floor(Index2/5)+(Index2%5-1)%5];
				NewMsg += Key[5*floor(Index2/5)+CycleRound(Index2%5-1, 0, 4)];
			}
			
			else if (Index1%5 == Index2%5){
				
				//NewMsg += Key[5*((floor(Index1/5)-1)%5)+Index1%5];
				//NewMsg += Key[5*(<int>(floor(Index1/5)-1)%5)+Index1%5];
				//NewMsg += Key[5*((int)(floor(Index1/5)-1)%5)+Index1%5];
				NewMsg += Key[5*CycleRound(floor(Index1/5)-1, 0, 4)+Index1%5];
				//NewMsg += Key[5*((floor(Index2/5)-1)%5)+Index2%5];
				//NewMsg += Key[5*(<int>(floor(Index2/5)-1)%5)+Index2%5];
				//NewMsg += Key[5*((int)(floor(Index2/5)-1)%5)+Index2%5];
				NewMsg += Key[5*CycleRound(floor(Index2/5)-1, 0, 4)+Index2%5];
			}
			
			else {
				
				NewMsg += Key[5*floor(Index1/5)+Index2%5];
				NewMsg += Key[5*floor(Index2/5)+Index1%5];
			}
        }
       return NewMsg;
}


double Score(string Msg, string Key, bool DecodeAsDoubleLetter)
{
	string DecodedMsg;
	DecodedMsg = DecodePlayfair(Msg, Key, DecodeAsDoubleLetter);
	
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


void MessAroundWithTheKey(string &Key)
{
	int Manoeuvre;
	Manoeuvre = rand()%50;
	switch (Manoeuvre){
		case 0: SwapRows(Key); break;
		case 1: SwapColumns(Key); break;
		case 2: ReverseKey(Key); break;
		case 3: SwapAllRows(Key); break;
		case 4: SwapAllColumns(Key); break;
		default: SwapLetters(Key); break;
	}
}


//double TwoSquareAnnealing(string Msg, string &OutputKey1, bool DecodeAsDoubleLetter, float Temperature, float Step, int Count)
double PlayfairAnnealing(string Msg, string &OutputKey, bool DecodeAsDoubleLetter, float Temperature, float Step, int Count)
{
	double BestScore;
	
	string BestKey;
	
	BestKey = OutputKey;
	
	BestScore = Score(Msg, BestKey, DecodeAsDoubleLetter);
	
	//string CurrentKey1;
	//string CurrentKey2;
	string CurrentKey;
	CurrentKey = BestKey;
	
	double CurrentScore;
	
	float T;
	int Trial;
	for (T = Temperature; T >= 0; T -= Step){
		cout << T << "\n";
		for (Trial = 0; Trial < Count; Trial++){
			if (true){
				MessAroundWithTheKey(CurrentKey);
				CurrentScore = Score(Msg, CurrentKey, DecodeAsDoubleLetter);
				
				if (CurrentScore > BestScore){
					BestScore = CurrentScore;
					BestKey = CurrentKey;
				}
				else if (CurrentScore < BestScore){
					
					bool ReplaceAnyway;
					ReplaceAnyway = AnnealingProbability(CurrentScore-BestScore, T);
					
					if (ReplaceAnyway == true){
						BestScore = CurrentScore;
						BestKey = CurrentKey;
					}
					else {
						CurrentKey = BestKey;
					}
				}
				else {
					CurrentKey = BestKey;
				}
			}
		}
	}
	OutputKey = BestKey;
	return BestScore;
}


void SolvePlayfair(string Msg, bool DecodeAsDoubleLetter, string AlphabetForKey)
{
    string BestKey = AlphabetForKey;
	
	int i;
	for(i = 0; i < 100; i++){
		SwapLetters(BestKey);
	}
	
	double BestScore = Score(Msg, BestKey, DecodeAsDoubleLetter);
	
	cout.precision(17);
	
	//DisplayKey();
	DisplayKey(BestKey);
	
	cout << "\nScore: " << BestScore << "\n\n";
	
	cout << "--------------------------------------\n\n";
	
	string NewKey;
	
	double NewScore;
	
	cout.precision(17);
	
	int anneal;
	anneal = 0;
	while (true){
		anneal += 1;
		cout << "Trial: " << anneal << "\n\n";
		
		NewKey = BestKey;
		
		NewScore = PlayfairAnnealing(Msg, NewKey, DecodeAsDoubleLetter, 20, 0.2, 10000);
		
		if (NewScore > BestScore){
			BestScore = NewScore;
			
			BestKey = NewKey;
			
			cout << "\n";
			cout << "New Best Key!\n\n";
			cout << "Score: " << fixed << BestScore << "\n";
			cout << "\n";
			cout << "Key:\n";
			DisplayKey(BestKey);
			cout << "\n";
			string Decipherment = DecodePlayfair(Msg, BestKey, DecodeAsDoubleLetter);
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
	
    bool DECODEASDOUBLELETTER;
	char MISSINGLETTER;
	//string MISSINGLETTER;
    string MESSAGE;
	
	//ifstream File("Playfair Message.txt");
	ifstream File("Other Playfair Message.txt");
	
	File.seekg(0, ios::end);
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);

	if (string(argv[1]) == "True"){
        DECODEASDOUBLELETTER = true;
    } 
	else {
		DECODEASDOUBLELETTER = false;
    }
	
	//MISSINGLETTER = argv[2];
	//MISSINGLETTER = <char>argv[2];
	//strcpy(MISSINGLETTER, argv[2].c_str());
	MISSINGLETTER = argv[2][0];
    
    string Alphabet;
    string AlphabetForKey;
    Alphabet = "abcdefghijklmnopqrstuvwxyz";
    AlphabetForKey = "";
	
	int i;
	
	for (i = 0; i < 26; i++){
		if (Alphabet[i] != MISSINGLETTER){
			AlphabetForKey += Alphabet[i];
		}
	}
    
    string Key;
	
	Key = AlphabetForKey;
	
	Key = "druhvaetqkoxzbcywmnglpisf";
	
	/*DisplayKey(Key);
	
	//cout << DecodePlayfair(MESSAGE, Key, DECODEASDOUBLELETTER)
	cout << DecodePlayfair(MESSAGE, Key, DECODEASDOUBLELETTER) << "\n";*/
	
	cout << "Starting Solve!\n\n";
	
	SolvePlayfair(MESSAGE, DECODEASDOUBLELETTER, AlphabetForKey);
	
	cin.get();/**/
    
    return 1;
}
