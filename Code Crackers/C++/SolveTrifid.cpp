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


DisplayRow(int Row[], int Length){
	int i;
	for (i = 0; i < Length; i++){
		cout << " " << Row[i];
	}
}


void DisplayKey(char Key[3][3][3])
{
	//int table;
	int row;
    int column;
    //#for (table = 0; table < 3; table++){
	for (row = 0; row < 3; row++){
        //#for (x = 0; x < 3; x++){
		for (column = 0; column < 3; column++){
            //c//out << " " << Key[3][column][row];
			cout << " " << Key[0][column][row];
        }
		cout << "   ";
		//for (x = 0; x < 3; x++){
		for (column = 0; column < 3; column++){
            cout << " " << Key[1][column][row];
        }
		cout << "   ";
		//for (x = 0; x < 3; x++){
		for (column = 0; column < 3; column++){
            cout << " " << Key[2][column][row];
        }
        cout << "\n";
    }
    return;
}


/*void FindLetterInKey(char Letter, char Key[3][3][3], int * OutputArray)
{
    OutputArray[0], OutputArray[1] = 5, 5;
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
}*/


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


string DecodeTrifid(string Msg, char Key[3][3][3], int Period)
{
	string NewMsg;
	NewMsg = "";
	//int Row2[3][Period];
	int Row[3][Period];
	int Pos;
	
	float HalfPeriod = Period/2;
	int FlooredHalfPeriod = floor(Period/2);
   
	string NewKey;
	int x;
	int y;
	int z;
   
	for (z = 0; z < 3; z++){
		for (y = 0; y < 3; y++){
			for (x = 0; x < 3; x++){
				//NewKey += Key[x][y][z];
				//NewKey += Key[z][y][x];
				NewKey += Key[z][x][y];
			}
		}
	}
   
	//char * KeyString = new char [27];
   
	//strcpy(KeyString, NewKey.c_str());
   
	int i;
	int j;
	for (i = 0; i < Msg.length()+1-Period; i += Period){
		
		//cout << Msg[i] << " " << strchr(KeyString, Msg[i+j])-KeyString << "\n";
		//////cout << Msg[i] << " " << NewKey.find(Msg[i]) << "\n";
		
		for (j = 0; j < Period; j++){
			//Pos = strchr(KeyString, Msg[i+j])-KeyString;
			Pos = NewKey.find(Msg[i+j]);
			
			//Row[(j*3)%Period] = int(floor(Pos/9));
			Row[int(floor((j*3)/Period))][(j*3)%Period] = int(floor(Pos/9));
			//Row[int(floor((j*3+1)/Period))][(j*3+1)%Period] = int(floor(Pos/9));
			Row[int(floor((j*3+1)/Period))][(j*3+1)%Period] = int(floor((Pos%9)/3));
			//Row[int(floor((j*3+2)/Period))][(j*3+2)%Period] = int(floor(Pos/3)%3);
			Row[int(floor((j*3+2)/Period))][(j*3+2)%Period] = Pos%3;
		}
		
		/*//DisplayRow(Rows[0]);
		DisplayRow(Row[0], Period);
		cout << "\n";
		//DisplayRow(Rows[1]);
		DisplayRow(Row[1], Period);
		cout << "\n";
		//DisplayRow(Rows[2]);
		DisplayRow(Row[2], Period);
		cout << "\n\n";*/
		
		for (j = 0; j < Period; j++){
			//NewMsg += Key[Row[0][j]][Row[1][j]][Row[2][j]];
			if (Key[Row[0][j]][Row[2][j]][Row[1][j]] == '#') {
				NewMsg += 'x';
			}
			else {
				NewMsg += Key[Row[0][j]][Row[2][j]][Row[1][j]];
			}
		}
	}
	return NewMsg;
}


double Score(string Msg, char Key[3][3][3], int Period)
{
	
	string DecodedMsg;
	DecodedMsg = DecodeTrifid(Msg, Key, Period);
	
	double QuadScore;
	QuadScore = QuadgramScore(DecodedMsg);
	
	return QuadScore;
}


void SwapLetters(char (&Key)[3][3][3])
{
	int x1;
	int y1;
	int x2;
	int y2;
	int grid1;
	int grid2;
	
	x1 = rand()%3;
	y1 = rand()%3;
	x2 = x1;
	y2 = y1;
	grid1 = rand()%3;
	grid2 = grid1;
	while ((x1 == x2) && (y1 == y2) && (grid1 == grid2)){
		grid2 = rand()%3;
	}
	
	char Temp;
	Temp = Key[grid1][x1][y1];
	Key[grid1][x1][y1] = Key[grid2][x2][y2];
	Key[grid2][x2][y2] = Temp;
}


void SwapRows(char (&Key)[3][3][3])
{
	int grid2;
	//int grid1;]
	int grid1;
	int y1;
	int y2;
	
	//y1 = rand()%5;
	//grid1 = rand()%5;
	y1 = rand()%3;
	grid1 = rand()%3;
	y2 = y1;
	grid2 = grid1;
	//while (y1 == y2){
	while ((y1 == y2) && (grid1 == grid2)){
		y2 = rand()%3;
		grid2 = rand()%3;
	}
	
	char Temp[3];
	int i;
	for (i = 0; i < 3; i++){
		Temp[i] = Key[grid1][i][y1];
	}
	
	for (i = 0; i < 3; i++){
		Key[grid1][i][y1] = Key[grid2][i][y2];
		Key[grid2][i][y2] = Temp[i];
	}
}


void SwapColumns(char (&Key)[3][3][3])
{
	int grid1;
	int x1;
	int x2;
	int grid2;
	
	x1 = rand()%3;
	grid1 = rand()%3;
	x2 = x1;
	grid2 = grid1;
	while ((x1 == x2) && (grid1 == grid2)){
		x2 = rand()%3;
		grid2 = rand()%3;
	}
	
	char Temp[3];
	int i;
	for (i = 0; i < 3; i++){
		Temp[i] = Key[grid1][x1][i];
	}
	
	for (i = 0; i < 3; i++){
		Key[grid1][x1][i] = Key[grid2][x2][i];
		Key[grid2][x2][i] = Temp[i];
	}
}


void ReverseColumn(char (&Key)[3][3][3])
{
	int x1;
	int grid;
	
	x1 = rand()%3;
	grid = rand()%3;
	
	char Temp[3];
	int i;
	for (i = 0; i < 3; i++){
		Temp[i] = Key[grid][x1][i];
	}
	
	for (i = 0; i < 3; i++){
		Key[grid][x1][i] = Temp[2-i];
	}
}


void ReverseRow(char (&Key)[3][3][3])
{
	int y1;
	
	y1 = rand()%3;
	
	char Temp[9];
	int i;
	for (i = 0; i < 9; i++){
		Temp[i] = Key[int(floor(i/3))][i][y1];
	}
	
	for (i = 0; i < 9; i++){
		Key[int(floor(i/3))][i][y1] = Temp[9-i];
	}
}


void ReverseKey(char (&Key)[3][3][3])
{
	char Temp[3][3][3];
	int x;
	int y;
	int z;
	for (x = 0; x < 3; x++){
		for (y = 0; y < 3; y++){
			for (z = 0; z < 3; z++){
				Temp[x][y][z] = Key[x][y][z];
			}
		}
	}
	
	for (x = 0; x < 3; x++){
		for (y = 0; y < 3; y++){
			for (z = 0; z < 3; z++){
				//Key[x][y] = Temp[2-x][2-y][2-z];
				Key[x][y][z] = Temp[2-x][2-y][2-z];
			}
		}
	}
}


void MessAroundWithTheKey(char (&Key)[3][3][3])
{
	//int Manoeuvre = rand()%50;
	int Manoeuvre = rand()%30;
	switch (Manoeuvre){
		case 0: SwapRows(Key);
		case 1: SwapColumns(Key);
		case 4: ReverseKey(Key);
		//case 5: SwapAllRows(Key);
		//case 6: SwapAllColumns(Key);
		default: SwapLetters(Key);
	}
}


void CopyKey(char InputKey[3][3][3], char (&OutputKey)[3][3][3])
{
	int x;
	int y;
	int z;
	for (x = 0; x < 3; x++){
		for (y = 0; y < 3; y++){
			for (z = 0; z < 3; z++){
				OutputKey[x][y][z] = InputKey[x][y][z];
			}
		}
	}
	return;
}


double TrifidAnnealing(string Msg, char (&OutputKey)[3][3][3], int Period, float Temperature, float Step, int Count)
{
	char BestKey[3][3][3];
	CopyKey(OutputKey, BestKey);
	
	double BestScore;
	BestScore = Score(Msg, BestKey, Period);
	
	char CurrentKey[3][3][3];
	CopyKey(OutputKey, CurrentKey);
	
	double CurrentScore;
	
	float T;
	int Trial;
	for (T = Temperature; T >= 0; T -= Step){
		
		cout << T << "\n";
		
		for (Trial = 0; Trial < Count; Trial++){
			
			MessAroundWithTheKey(CurrentKey);
			CurrentScore = Score(Msg, CurrentKey, Period);
			
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


void SolveTrifid(string Msg, int Period, string AlphabetForKey)
{
	char BestKey[3][3][3];
	
	int i;
	for (i = 0; i < 27; i++){
		//BestKey[int(floor(i/9))][i%3][int(floor(i/3))] = AlphabetForKey.at(i);
		BestKey[int(floor(i/9))][i%3][int(floor((i%9)/3))] = AlphabetForKey.at(i);
    }
	
	for (i = 0; i < 100; i++){
		SwapLetters(BestKey);
	}
	
	double BestScore = Score(Msg, BestKey, Period);
	
	cout.precision(17);
	
	char NewKey[3][3][3];
	double NewScore;
	
	DisplayKey(BestKey);
	cout << "\nScore: " << BestScore << "\n\n";
	cout << "--------------------------------------\n\n";
	
	int anneal;
	anneal = 0;
	while (true){
		anneal += 1;
		cout << "Trial: " << anneal << "\n\n";
		CopyKey(BestKey, NewKey);
		NewScore = TrifidAnnealing(Msg, NewKey, Period, 20, 0.2, 10000);
		
		if (NewScore > BestScore){
			BestScore = NewScore;
			CopyKey(NewKey, BestKey);
			cout << "New Best Key!\n\n";
			cout << "Score: " << fixed << BestScore << "\n";
			cout << "\n";
			cout << "Key:\n";
			DisplayKey(BestKey);
			cout << "\n";
			string Decipherment = DecodeTrifid(Msg, BestKey, Period);
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
	
    int PERIOD;
	char ADDEDLETTER;
    string MESSAGE;
    
    PERIOD = atof(argv[1]);
	ADDEDLETTER = argv[2][0];
	
	ifstream File("Trifid Message.txt");
	
	File.seekg(0, ios::end);
	MESSAGE.reserve(File.tellg());
	File.seekg(0, ios::beg);
	
	MESSAGE.assign((istreambuf_iterator<char>(File)), istreambuf_iterator<char>());
	
	transform(MESSAGE.begin(), MESSAGE.end(), MESSAGE.begin(), ::tolower);       
    
    string Alphabet;
    string AlphabetForKey;
    Alphabet = "abcdefghijklmnopqrstuvwxyz";
    AlphabetForKey = Alphabet + ADDEDLETTER;
	
	char TestKey[3][3][3];
	string TestKeyString;
	TestKeyString = "myneighbouracdfjklpqstvwxz#";
	
	int i;
	//int j;
	for (i = 0; i < 27; i++){
		//TestKey[int(floor(i/9))][i%3][int(floor(i/3))] = TestKeyString[i];
		TestKey[int(floor(i/9))][i%3][int(floor((i%9)/3))] = TestKeyString[i];
	}
	
	/////////////DisplayKey(TestKey);
	
	/*string TestDecode;
	
	TestDecode = DecodeTrifid(MESSAGE, TestKey, PERIOD);
	
	cin.get();
	
	double TestScore;
	TestScore = Score(MESSAGE, TestKey, PERIOD);
	
	cout << "Score: " << TestScore << "\nDecipherment: " << TestDecode << "\n\n";/**/
	
	cout << "Starting Solve!\n\n";
	
	SolveTrifid(MESSAGE, PERIOD, AlphabetForKey);
	
	cin.get();
    
    return 1;
}
