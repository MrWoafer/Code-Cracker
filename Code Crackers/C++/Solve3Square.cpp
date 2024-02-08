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

void DisplayKeys(string Key1, string Key2, string Key3)
{
	int y;
	int x;
	
	//cout << "          ";
	//cout << "   ";
	
	for (y = 0; y < 5; y++){
		cout << "          ";
		cout << "   ";
		for (x = 0; x < 5; x++){
			cout << " " << Key2[5*y+x];
		}
		cout << "\n";
	}
		
	cout << "\n";
	
	for (y = 0; y < 5; y++){
		for (x = 0; x < 5; x++){
			cout << " " << Key1[5*y+x];
		}
		cout << "   ";
		for (x = 0; x < 5; x++){
			cout << " " << Key3[5*y+x];
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


string Decode3Square(string Msg, string Key1, string Key2, string Key3, string Order)
{
	string NewMsg;
	NewMsg = "";
   
	int Index1;
	int Index2;
	int Index3;
   
	int i;
	/////i = 0;
	int j;
	for (i = 0; i < Msg.length()-2; i += 3){
		
		for (j = 0; j<3; j++){
			if (Order[j] == '1'){
				//Index1 = Key1.find(Msg[i+j])
				Index1 = Key1.find(Msg[i+j]);
			}
			else if (Order[j] == '2'){
				//Index2 = Key2.find(Msg[i+j])
				Index2 = Key2.find(Msg[i+j]);
			}
			else if (Order[j] == '3'){
				//Index3 = Key3.find(Msg[i+j])
				Index3 = Key3.find(Msg[i+j]);
			}
		}
		
	//cout << Index1 << " " << Index2 << " " << Index3 << " " << Msg[i] << " " << Msg[i+1] << " " << Msg[i+2] << "\n";
		
		/*for (j = 0; j<3; j++){
			if (Order[j] == '1'){
				NewMsg += Key1[5*floor(Index3/5)+Index1%5];
			}
			else if (Order[j] == '2'){
				NewMsg += Key1[5*floor(Index3/5)+Index1%5];
			}
			else if (Order[j] == '3'){
				NewMsg += Key1[5*floor(Index3/5)+Index1%5];
			}
		}*/
		NewMsg += Key1[5*floor(Index3/5)+Index1%5];
		NewMsg += Key2[5*floor(Index2/5)+Index3%5];
	}
   return NewMsg;
}


double Score(string Msg, string Key1, string Key2, string Key3, string Order)
{
	string DecodedMsg;
	DecodedMsg = Decode3Square(Msg, Key1, Key2, Key3, Order);
	
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


//void SwapALetter(string &Key1, string &Key2)
void SwapALetter(string &Key1, string &Key2, string &Key3)
{
	int RandKey;
	
	RandKey = rand()%3;
		
	if (RandKey == 0){
		SwapLetters(Key1);
	}
	else if (RandKey == 1){
		SwapLetters(Key2);
	}
	else if (RandKey == 2){
		SwapLetters(Key3);
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


void MessAroundWithTheKeys(string &Key1, string &Key2, string &Key3)
{
	int Manoeuvre;
	Manoeuvre = rand()%75;
	switch (Manoeuvre){
		case 0: SwapRows(Key1); break;
		case 1: SwapRows(Key2); break;
		case 2: SwapRows(Key3); break;
		case 3: SwapColumns(Key1); break;
		case 4: SwapColumns(Key2); break;
		case 5: SwapColumns(Key3); break;
		case 6: ReverseKey(Key1); break;
		case 7: ReverseKey(Key2); break;
		case 8: ReverseKey(Key3); break;
		case 9: SwapAllRows(Key1); break;
		case 10: SwapAllRows(Key2); break;
		case 11: SwapAllRows(Key3); break;
		case 12: SwapAllColumns(Key1); break;
		case 13: SwapAllColumns(Key2); break;
		case 14: SwapAllColumns(Key3); break;
		default: SwapALetter(Key1, Key2, Key3); break;
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


double TwoSquareAnnealing(string Msg, string &OutputKey1, string &OutputKey2, string &OutputKey3, string Order, float Temperature, float Step, int Count)
{
	double BestScore;
	
	string BestKey1;
	string BestKey2;
	string BestKey3;
	
	BestKey1 = OutputKey1;
	BestKey2 = OutputKey2;
	//BestKey2 = OutputKey3;
	BestKey3 = OutputKey3;
	
	BestScore = Score(Msg, BestKey1, BestKey2, BestKey3, Order);
	
	string CurrentKey1;
	string CurrentKey2;
	string CurrentKey3;
	CurrentKey1 = BestKey1;
	CurrentKey2 = BestKey2;
	CurrentKey3 = BestKey3;
	
	double CurrentScore;
	
	float T;
	int Trial;
	for (T = Temperature; T >= 0; T -= Step){
		cout << T << "\n";
		for (Trial = 0; Trial < Count; Trial++){
			if (true){
				MessAroundWithTheKeys(CurrentKey1, CurrentKey2, CurrentKey3);
				CurrentScore = Score(Msg, CurrentKey1, CurrentKey2, CurrentKey3, Order);
				
				if (CurrentScore > BestScore){
					BestScore = CurrentScore;
					BestKey1 = CurrentKey1;
					BestKey2 = CurrentKey2;
					BestKey3 = CurrentKey3;
				}
				else if (CurrentScore < BestScore){
					
					bool ReplaceAnyway;
					ReplaceAnyway = AnnealingProbability(CurrentScore-BestScore, T);
					
					if (ReplaceAnyway == true){
						BestScore = CurrentScore;
						BestKey1 = CurrentKey1;
						BestKey2 = CurrentKey2;
						BestKey3 = CurrentKey3;
					}
					else {
						CurrentKey1 = BestKey1;
						CurrentKey2 = BestKey2;
						CurrentKey3 = BestKey3;
					}
				}
				else {
					CurrentKey1 = BestKey1;
					CurrentKey2 = BestKey2;
					CurrentKey3 = BestKey3;
				}
			}
		}
	}
	OutputKey1 = BestKey1;
	OutputKey2 = BestKey2;
	OutputKey3 = BestKey3;
	return BestScore;
}


void Solve2Square(string Msg, string Order)
{
	string AlphabetForKey;
	AlphabetForKey = "abcdefghiklmnopqrstuvwxyz";
    
    string BestKey1 = AlphabetForKey;
	string BestKey2 = AlphabetForKey;
	string BestKey3 = AlphabetForKey;
	
	int i;
	for(i = 0; i < 100; i++){
		SwapALetter(BestKey1, BestKey2, BestKey3);
	}
	
	double BestScore = Score(Msg, BestKey1, BestKey2, BestKey3, Order);
	
	cout.precision(17);
	
	DisplayKeys(BestKey1, BestKey2, BestKey3);
	
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
		
		NewScore = TwoSquareAnnealing(Msg, NewKey1, NewKey2, BestKey3, Order, 20, 0.2, 10000);
		
		if (NewScore > BestScore){
			BestScore = NewScore;
			
			BestKey1 = NewKey1;
			BestKey2 = NewKey2;
			
			cout << "\n";
			cout << "New Best Key!\n\n";
			cout << "Score: " << fixed << BestScore << "\n";
			cout << "\n";
			cout << "Key:\n";
			DisplayKeys(BestKey1, BestKey2, BestKey3);
			cout << "\n";
			//string Decipherment = Decode2Square(Msg, BestKey1, BestKey2, BestKey3, Order);
			string Decipherment = Decode3Square(Msg, BestKey1, BestKey2, BestKey3, Order);
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
	
    string ORDER;
    string MESSAGE;
	
	//ifstream File("2-Square Message.txt");
	ifstream File("3-Square Message.txt");
	
	File.seekg(0, ios::end);
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);

	ORDER = argv[1];
    
    string Alphabet;
    string AlphabetForKey;
    Alphabet = "abcdefghijklmnopqrstuvwxyz";
    AlphabetForKey = "abcdefghiklmnopqrstuvwxyz";
    
    string Key1;
	string Key2;
	string Key3;
	
	Key1 = AlphabetForKey;
	Key2 = AlphabetForKey;
	Key3 = AlphabetForKey;
	
	/*Key1 = "helomyatbcdfgiknpqrsuvwxz";
	Key2 = "posieabcdfghklmnqrtuvwxyz";
	Key3 = "chekisabdfglmnopqrtuvwxyz";
	
	DisplayKeys(Key1, Key2, Key3);
	
	cout << Decode3Square(MESSAGE, Key1, Key2, Key3, ORDER);*/
	
	cout << "Starting Solve!\n\n";
	
	Solve2Square(MESSAGE, ORDER);
	
	cin.get();/**/
    
    return 1;
}
