#pragma once

using namespace std;

class TabuSearch {
private:
	TabuSearch(); // prevent others from initializing this abstract class
	~TabuSearch();
	int** tabuList;
	int* currentPermutation;
	int listSize;

	void InitializeTabuList(int size);
	virtual void SearchNeighbors();
	virtual double CalculateCost(int* permutation);
};