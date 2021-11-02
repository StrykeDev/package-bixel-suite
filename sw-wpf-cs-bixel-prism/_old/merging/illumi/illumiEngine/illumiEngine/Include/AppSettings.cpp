
#include "pch.h"

#include "AppSettings.h"


AppSettings::AppSettings()
{
	//LoadSettings();
}

AppSettings::~AppSettings()
{
}


void AppSettings::LoadSettings()
{
	auto fileName = "C:\\Users\\Barak\\AppData\\Roaming\\illumi\\config.ini";

	hz = GetPrivateProfileInt("Application", "Hz", 20, fileName);
	brightness = (float)GetPrivateProfileInt("Application", "Brightness", 1, fileName);
	speed = (float)GetPrivateProfileInt("Application", "Speed", 1, fileName);
	clampMode = GetPrivateProfileInt("Application", "ClampMode", 1, fileName);
	updateStatic = GetPrivateProfileInt("Application", "UpdateStatic", 1, fileName);
	gameMode = GetPrivateProfileInt("Application", "GameMode", 0, fileName);
	timeOut = GetPrivateProfileInt("Application", "TimeOut", 0, fileName);

	r = (byte)GetPrivateProfileInt("Color", "Red", 255, fileName);
	g = (byte)GetPrivateProfileInt("Color", "Green", 255, fileName);
	b = (byte)GetPrivateProfileInt("Color", "Blue", 255, fileName);
	a = (byte)GetPrivateProfileInt("Color", "Alpha", 255, fileName);

	timeOutBrightness = (float)GetPrivateProfileInt("TimeOut", "TimeOutBrightness", 1, fileName);
	timeOutMinutes = GetPrivateProfileInt("TimeOut", "TimeOutMinutes", 20, fileName);
}

void AppSettings::SaveSettings()
{
	auto fileName = "C:\\Users\\Barak\\AppData\\Roaming\\illumi\\config.ini";

	hz = GetPrivateProfileInt("Application", "Hz", 20, fileName);
	brightness = (float)GetPrivateProfileInt("Application", "Brightness", 1, fileName);
	speed = (float)GetPrivateProfileInt("Application", "Speed", 1, fileName);
	clampMode = GetPrivateProfileInt("Application", "ClampMode", 1, fileName);
	updateStatic = GetPrivateProfileInt("Application", "UpdateStatic", 1, fileName);
	gameMode = GetPrivateProfileInt("Application", "GameMode", 0, fileName);
	timeOut = GetPrivateProfileInt("Application", "TimeOut", 0, fileName);

	r = (byte)GetPrivateProfileInt("Color", "Red", 255, fileName);
	g = (byte)GetPrivateProfileInt("Color", "Green", 255, fileName);
	b = (byte)GetPrivateProfileInt("Color", "Blue", 255, fileName);
	a = (byte)GetPrivateProfileInt("Color", "Alpha", 255, fileName);

	timeOutBrightness = (float)GetPrivateProfileInt("TimeOut", "TimeOutBrightness", 1, fileName);
	timeOutMinutes = GetPrivateProfileInt("TimeOut", "TimeOutMinutes", 20, fileName);
}
