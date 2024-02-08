#include <iostream>
#include <fstream>
#include <stdlib.h>
#include <math.h>
#include <string.h>
#include "Annealing.h"
#include <stdio.h>
#include <streambuf>
#include <algorithm>

using namespace std;


//DisplayKey(char Key){
DisplayKey(char Key[10][3]){
	int i;
	int j;
	//for (i = 0; i < 10; i++){
	for (j = 0; j < 3; j++){
		//for (j = 0; j < 3){
		//for (j = 0; j < 3; j++){
		for (i = 0; i < 10; i++){
			cout << " " << Key[i][j];
		}
		cout << "\n";
	}
}


void FindLetterInKey(char Letter, char Key[10][3], int * OutputArray)
{
    OutputArray[0], OutputArray[1] = 10, 5;
    int y;
    int x;
    for (y = 0; y < 5; y++){
        for (x = 0; x < 5; x++){
            if (Key[x][y] == Letter){
                OutputArray[0] = x;
                OutputArray[1] = y;
            }
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


string DecodeStraddleCheckerboard(string Msg, char Key[10][3])
{
	string NewMsg;
	NewMsg = "";
	//int PosX;
	//int PosY;
	int Pos1;
	int Pos2;	
   
	int i;
	
	int Row1;
	int Row2;
	
	for (i = 0; i < 10; i++){
		if (Key[i][0] == '.'){
			Row2 = i;
		}
	}
	
	for (i = 0; i < Row2; i++){
		if (Key[i][0] == '.'){
			Row1 = i;
		}
	}
	
	i = 0;
	//for (i = 0; i < Msg.length(); i ++){
	i = 0;
	while (i < Msg.length()){
		
		//PosY = atof(Msg[i])
		//PosX = atof(Msg[i+1])
		//Pos1 = atof(Msg[i]);
		//Pos2 = atof(Msg[i+1]);
		//Pos1 = Msg[i]-97;
		//Pos2 = Msg[i+1]-97;
		Pos1 = Msg[i]-'0';
		Pos2 = Msg[i+1]-'0';
		
		//if (Key[PosX][PosY] == '.'){
		//	
		//}
		/*if (Key[Pos1][0] == '.'){
			if Key[]
			NewMsg += Key[]
		}
		else {
			
		}*/
		///////////
		///////////cout << Pos1 << " " << Pos2 << "\n";
		if (Pos1 == Row1){
			//NewMsg += Key[Pos2][Row1];
			if (Key[Pos2][1] == '.'){
				NewMsg += 'x';
			}
			else {
				NewMsg += Key[Pos2][1];
			}
			i += 2;
		}
		else if (Pos1 == Row2){
			//NewMsg += Key[Pos2][Row2];
			if (Key[Pos2][2] == '.'){
				NewMsg += 'x';
			}
			else {
				NewMsg += Key[Pos2][2];
			}
			i += 2;
		}
		else {
			//NewMsg += Key[Pos2][0];
			NewMsg += Key[Pos1][0];
			i += 1;
		}
		
	}
	return NewMsg;
}


double Score(string Msg, char Key[10][3])
{
	
	string DecodedMsg;
	DecodedMsg = DecodeStraddleCheckerboard(Msg, Key);
	
	double QuadScore;
	QuadScore = QuadgramScore(DecodedMsg);
	
	return QuadScore;
}


void SwapLetters(char (&Key)[10][3])
{
	int x1;
	int y1;
	int x2;
	int y2;
	
	x1 = rand()%10;
	y1 = rand()%3;
	x2 = x1;
	y2 = y1;
	/*//*while ((x1 == x2) && (y1 == y2)){
		x2 = rand()%10;
		y2 = rand()%3;//
		/*if (y1 == 0){
			y2 = 0;
		}
		else {
			y2 = rand()%2+1;
		}*/
	//}*/
	if (y1 == 0){
		if (Key[x1][y1] == '.'){
			//cout << "Boof!\n";
			y2 = 0;
			//while ((y2 != 0) || (Key[x2][y2] == '.') || ((x1 == x2) && (y1 == y2)))
			while ((Key[x2][y2] == '.') || (x1 == x2)){
				x2 = rand()%10;
				//y2 = 0;
			//cout << "1: " << Key[x1][y1] << " " << Key[x2][y2] << "\n";
			}
		}
		else {
			while ((Key[x2][y2] == '.') || ((x1 == x2) && (y1 == y2))){
				x2 = rand()%10;
				y2 = rand()%3;
			}
		}
	}
	else {
		if (Key[x1][y1] == '.'){
			y2 = rand()%2+1;
			//while ((y2 == 0) || (Key[x2][y2] == '.') || ((x1 == x2) && (y1 == y2)))
			while ((Key[x2][y2] == '.') || ((x1 == x2) && (y1 == y2))){
				x2 = rand()%10;
				//y2 = rand()%2+1;
				//y2 = rand()%2+1;
			}
		}
		else {
			while ((Key[x2][y2] == '.') || ((x1 == x2) && (y1 == y2))){
				x2 = rand()%10;
				y2 = rand()%3;
			}
		}
	}
	
	char Temp;
	Temp = Key[x1][y1];
	Key[x1][y1] = Key[x2][y2];
	Key[x2][y2] = Temp;
}


void SwapRows(char (&Key)[10][3])
{
	int y1;
	int y2;
	
	y1 = rand()%3;
	y2 = y1;
	while (y1 == y2){
		y2 = rand()%3;
	}
	
	char Temp[10];
	int i;
	for (i = 0; i < 10; i++){
		Temp[i] = Key[i][y1];
	}
	
	for (i = 0; i < 10; i++){
		Key[i][y1] = Key[i][y2];
		Key[i][y2] = Temp[i];
	}
}


void SwapColumns(char (&Key)[10][3])
{
	int x1;
	int x2;
	
	x1 = rand()%10;
	x2 = x1;
	while (x1 == x2){
		x2 = rand()%10;
	}
	
	char Temp[3];
	int i;
	for (i = 0; i < 3; i++){
		Temp[i] = Key[x1][i];
	}
	
	for (i = 0; i < 3; i++){
		Key[x1][i] = Key[x2][i];
		Key[x2][i] = Temp[i];
	}
}


void SwapAllColumns(char (&Key)[10][3])
{
	char Temp[10][3];
	int x;
	int y;
	int i;
	for (x = 0; x < 10; x++){
		for (y = 0; y < 3; y++){
			Temp[x][y] = Key[x][y];
		}
	}
	
	char TempItem;
	for (x = 0; x < 5; x++){
		for (i = 0; i < 3; i++){
			TempItem = Key[x][i];
			Key[x][i] = Key[9-x][i];
			Key[9-x][i] = TempItem;
		}
	}
}


void SwapAllRows(char (&Key)[10][3])
{
	char Temp[10][3];
	int x;
	int y;
	for (x = 0; x < 10; x++){
		for (y = 0; y < 3; y++){
			Temp[x][y] = Key[x][y];
		}
	}
	
	char TempItem;
	int i;
	for (y = 0; y < 1; y++){
		for (i = 0; i < 10; i++){
			TempItem = Key[i][y];
			Key[i][y] = Key[i][2-y];
			Key[i][2-y] = TempItem;
		}
	}
}


void ReverseColumn(char (&Key)[10][3])
{
	int x1;
	
	x1 = rand()%10;
	
	char Temp[3];
	int i;
	for (i = 0; i < 3; i++){
		Temp[i] = Key[x1][i];
	}
	
	for (i = 0; i < 3; i++){
		Key[x1][i] = Temp[2-i];
	}
}


void ReverseRow(char (&Key)[10][3])
{
	int y1;
	
	y1 = rand()%3;
	
	char Temp[10];
	int i;
	for (i = 0; i < 10; i++){
		Temp[i] = Key[i][y1];
	}
	
	for (i = 0; i < 10; i++){
		Key[i][y1] = Temp[9-i];
	}
}


void ReverseKey(char (&Key)[10][3])
{
	char Temp[10][3];
	int x;
	int y;
	for (x = 0; x < 10; x++){
		for (y = 0; y < 3; y++){
			Temp[x][y] = Key[x][y];
		}
	}
	
	for (x = 0; x < 10; x++){
		for (y = 0; y < 3; y++){
			Key[x][y] = Temp[9-x][2-y];
		}
	}
}


void MessAroundWithTheKey(char (&Key)[10][3])
{
	//int Manoeuvre = rand()%50;
	int Manoeuvre = rand()%20;
	switch (Manoeuvre){
		//case 0: SwapRows(Key);
		case 1: SwapColumns(Key);
		//case 4: ReverseKey(Key);
		//case 5: SwapAllRows(Key);
		case 6: SwapAllColumns(Key);
		default: SwapLetters(Key);
	}
}


void CopyKey(char InputKey[10][3], char (&OutputKey)[10][3])
{
	int x;
	int y;
	for (x = 0; x < 10; x++){
		for (y = 0; y < 3; y++){
			OutputKey[x][y] = InputKey[x][y];
		}
	}
	return;
}


double StraddleCheckerboardAnnealing(string Msg, char (&OutputKey)[10][3], float Temperature, float Step, int Count)
{
	char BestKey[10][3];
	CopyKey(OutputKey, BestKey);
	
	double BestScore;
	BestScore = Score(Msg, BestKey);
	
	char CurrentKey[10][3];
	CopyKey(OutputKey, CurrentKey);
	
	double CurrentScore;
	
	float T;
	int Trial;
	for (T = Temperature; T >= 0; T -= Step){
		////////cout << T << "\n";
		
		for (Trial = 0; Trial < Count; Trial++){			
			MessAroundWithTheKey(CurrentKey);
			CurrentScore = Score(Msg, CurrentKey);
			
			if (CurrentScore > BestScore){				
				BestScore = CurrentScore;
				CopyKey(CurrentKey, BestKey);
				
			}
			
			else if (CurrentScore < BestScore){				
				bool ReplaceAnyway;
				ReplaceAnyway = AnnealingProbability(CurrentScore-BestScore, T);
				
				if (ReplaceAnyway == true){					
					BestScore = CurrentScore;
					CopyKey(CurrentKey, BestKey);
					
				}
				else {
					CopyKey(BestKey, CurrentKey);
					
				}
			}
			else {
				CopyKey(BestKey, CurrentKey);
				
			}
		}
	}
	CopyKey(BestKey, OutputKey);
	return BestScore;
}


void SolveStraddleCheckerboard(string Msg, string AlphabetForKey)
{
	char BestKey[10][3];
	
	int i;
	for (i = 0; i < 30; i++){
		//BestKey[i%10][int(floor(i/3))] = AlphabetForKey.at(i);
		BestKey[i%10][int(floor(i/10))] = AlphabetForKey.at(i);
    }
	
	//for (i = 0; i < 100; i++){
	for (i = 0; i < 1000; i++){
		SwapLetters(BestKey);
	}
	
	double BestScore = Score(Msg, BestKey);
	
	cout.precision(17);
	
	char NewKey[10][3];
	double NewScore;
	
	DisplayKey(BestKey);
	cout << "\nScore: " << BestScore << "\n\n";
	cout << "--------------------------------------\n\n";
	
	///////////cin.get();
	
	int anneal;
	anneal = 0;
	while (true){
		anneal += 1;
		cout << "Trial: " << anneal << "\n\n";
		CopyKey(BestKey, NewKey);
		//NewScore = StraddleCheckerboardAnnealing(Msg, NewKey, 20, 0.2, 10000);
		//NewScore = StraddleCheckerboardAnnealing(Msg, NewKey, 0, 1, 10000);
		NewScore = StraddleCheckerboardAnnealing(Msg, NewKey, 1, 1, 10000);
		
		if (NewScore > BestScore){
			BestScore = NewScore;
			CopyKey(NewKey, BestKey);
			cout << "New Best Key!\n\n";
			cout << "Score: " << fixed << BestScore << "\n";
			cout << "\n";
			cout << "Key:\n";
			DisplayKey(BestKey);
			cout << "\n";
			string Decipherment = DecodeStraddleCheckerboard(Msg, BestKey);
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
    string MESSAGE;
	
	ifstream File("Straddle Checkerboard Message.txt");
	
	File.seekg(0, ios::end);
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);       
    
    string Alphabet;
    string AlphabetForKey;
    Alphabet = "abcdefghijklmnopqrstuvwxyz";
    //AlphabetForKey = "abcdefghijklmnopqrstuvwxyz    ";
	//AlphabetForKey = "abcdefghijklmnopqrstuvwxyz....";
	AlphabetForKey = "..abcdefghijklmnopqrstuvwxyz..";
	
	char TestKey[10][3];
	
	string TestKeyString;
	TestKeyString = ".nl.ptvjsckwfdehiagoxybzmqru..";
	TestKeyString = "gsz..torhjwyciuakxvdplbqmefn..";
	
	//int j;
	/*int i;
	for (i = 0; i < 30; i++){
		TestKey[i%10][int(floor(i/10))] = TestKeyString[i];
	}
	
	cout << "Key!:\n\n";
	DisplayKey(TestKey);
	
	string TestDecode;
	
	TestDecode = DecodeStraddleCheckerboard(MESSAGE, TestKey);
	
	cin.get();
	
	double TestScore;
	TestScore = 0;
	TestScore = Score(MESSAGE, TestKey);/**/
	//
	/*cout << "Score: " << TestScore << "\nDecipherment: " << TestDecode << "\n\n";
	
	cin.get();/**/
	
	cout << "Starting Solve!\n\n";
	
	SolveStraddleCheckerboard(MESSAGE, AlphabetForKey);
	
	cin.get();
    
    return 1;
}
