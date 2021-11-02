#pragma once

// 0 - Important
// 1 - Error
// 2 - Warning
// 3 - Info\trace

#define MIN_WARN_LVL	3

#ifdef _DEBUG
	#define LOG(level, message) if (level <= MIN_WARN_LVL) {std::cout << message << std::endl;}
#else
	#define LOG(level, message)
#endif