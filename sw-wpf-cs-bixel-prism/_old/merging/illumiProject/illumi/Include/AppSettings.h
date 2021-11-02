#pragma once


class AppSettings
{
public:
	// Default color
	byte r = 255;
	byte g = 255;
	byte b = 255;
	byte a = 255;

	// Settings
	int hz = 1;
	float brightness = 0.25;
	float speed = 0.25;
	bool clampMode = true;
	bool updateStatic = true;
	bool gameMode = false;
	bool timeOut = false;

	// TimeOut settings
	float timeOutBrightness = 0.25;
	int timeOutMinutes = 20;

public:
	AppSettings();
	~AppSettings();

	void LoadSettings();
	void SaveSettings();
};

