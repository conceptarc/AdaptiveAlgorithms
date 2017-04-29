#pragma once

#include "TabuSearch.h"

TabuSearch::TabuSearch() {
	tabuList = nullptr;
	currentPermutation = nullptr;
	listSize = 0;
}

TabuSearch::~TabuSearch() {
	if (tabuList != nullptr) {
		for (int i = 0; i < listSize; i++) {
			delete tabuList[i];
		}
		delete tabuList;
		tabuList = nullptr;
	}

	if (currentPermutation != nullptr) {
		delete currentPermutation;
		currentPermutation = nullptr;
	}
}

// wait, I should switch to .NET for visualization (UI)