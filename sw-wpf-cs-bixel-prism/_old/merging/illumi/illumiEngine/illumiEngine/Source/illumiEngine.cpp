
// Pre-compiled header
#include "pch.h"

// Includes
#include "AppSettings.h"
#include "API.h"
#include "log.h"
#include "Colors.h"


// APIs
#include "../APIs/AlienFx/AlienFxAPI.h"
#include "../APIs/Aura/AuraAPI.h"

// Source
#include "illumiEngine.h"

#ifdef _DEBUG
#define HIDE_CONSOLE()
#else
#define HIDE_CONSOLE()	ShowWindow(GetConsoleWindow(), SW_HIDE);
#endif

#define TITLE			"TITLE illumi engine"
#define CONSOLE_COLOR	"COLOR F0"



HKEY OpenRegistryKey(HKEY hRootKey, LPCSTR strSubKey)
{
	HKEY hKey;
	RegOpenKeyEx(hRootKey, strSubKey, NULL, KEY_ALL_ACCESS, &hKey);

	return hKey;
}

DWORD GetValueFromRegistry(HKEY hRootKey, LPCTSTR lpValue)
{
	DWORD data = 0;
	DWORD dtype = REG_DWORD;
	DWORD dSize = sizeof(data);
	LONG lErr = RegQueryValueEx(hRootKey, lpValue, NULL, &dtype, (LPBYTE)&data, &dSize);

	if (ERROR_SUCCESS != lErr)
	{
		std::cout << "\nCould Not find any Value.";
	}

	return data;
}



int main()
{
	LOG(0, "\nStarting illumi engine...");
	HIDE_CONSOLE();
	system(TITLE);
	system(CONSOLE_COLOR);

	std::shared_ptr<AppSettings> appSettings = std::make_shared<AppSettings>();

	appSettings->hz = 5;
	appSettings->speed = 0.1;
	appSettings->brightness = 1;
	appSettings->r = 255;
	appSettings->g = 255;
	appSettings->b = 255;
	appSettings->a = 48;

	LOG(0, "Hz: " << appSettings->hz);
	LOG(0, "Brightness: " << appSettings->brightness);
	LOG(0, "Speed: " << appSettings->speed);
	LOG(0, "Clamp Mode: " << appSettings->clampMode);
	LOG(0, "Update Static: " << appSettings->updateStatic);
	LOG(0, "Game Mode: " << appSettings->gameMode);
	LOG(0, "TimeOut: " << appSettings->timeOut);
	LOG(0, "\nDefualt Color:");
	LOG(0, "  Red: " << (int)appSettings->r);
	LOG(0, "  Green: " << (int)appSettings->g);
	LOG(0, "  Blue: " << (int)appSettings->b);
	LOG(0, "  Alpha: " << (int)appSettings->a);


	// Default color
	colorRGB defaultColor(appSettings->r, appSettings->g, appSettings->b, appSettings->a);


	LOG(0, "\nLoading APIs...");
	std::shared_ptr<Aura::AuraAPI> auraApi = std::make_shared<Aura::AuraAPI>();
	std::shared_ptr<AlienFx::AlienFxAPI> alienFxApi = std::make_shared<AlienFx::AlienFxAPI>();

	// Check if the API load successfully and add it to the APIs list
	std::vector<std::shared_ptr<API>> apis;
	apis.reserve(2);

	auraApi->IsLoaded() ? apis.emplace_back(auraApi) : (void)NULL;
	alienFxApi->IsLoaded() ? apis.emplace_back(alienFxApi) : (void)NULL;

	// Get number of devices and update the APIs
	int devicesNum = 0;
	for (int i = 0; i < (int)apis.size(); i++)
	{
		devicesNum += apis[i]->GetNumDev();
		apis[i]->Setup(appSettings);
	}


	LOG(0, "\nScanning for devices...");
	std::vector<Device> devs;
	devs.reserve(devicesNum);

	// Get devices info from each API
	for (int i = 0; i < (int)apis.size(); i++)
	{
		switch (apis[i]->GetApiName())
		{
		case API_Aura:
			for (int j = 0; j < auraApi->GetMbNum(); j++)
			{
				devs.emplace_back(Device(j, "Asus Motherboard", Motherboard, auraApi, auraApi->GetMbZones(j)));
			}

			for (int j = 0; j < auraApi->GetGpuNum(); j++)
			{
				devs.emplace_back(Device(j, "Asus GPU", GPU, auraApi, auraApi->GetGpuZones(j)));
			}
			break;

		case API_AlienFx:
			for (int j = 0; j < alienFxApi->GetNumDev(); j++)
			{
				devs.emplace_back(Device(j, alienFxApi->GetDevName(j), Peripheral, alienFxApi, alienFxApi->GetDevZones(j)));
			}
			break;

		default:
			for (int j = 0; j < apis[i]->GetNumDev(); j++)
			{
				devs.emplace_back(Device(j, "Unknown device", UnknownDevice, apis[i], 0));
			}
			break;
		}
	}


	LOG(0, "\nReady...");

	HKEY rootkey = HKEY_CURRENT_USER;
	LPCTSTR dir = "Software\\Microsoft\\Windows\\DWM";
	LPCTSTR name = "AccentColor";
	HKEY acolor;
	DWORD value;
	DWORD type = REG_DWORD;
	DWORD size = sizeof(value);
	RegQueryValueEx(rootkey, name, NULL, &type, (LPBYTE)&value, &size);

	HKEY key = OpenRegistryKey(rootkey, dir);
	DWORD c = GetValueFromRegistry(key, name);


	auraApi->SetAllDevZones((byte)GetRValue(c), (byte)GetGValue(c), (byte)GetBValue(c), 255);
	alienFxApi->SetAllDevZones((byte)GetRValue(c), (byte)GetGValue(c), (byte)GetBValue(c), 255);

	int r = 32;
	int g = 255;
	int b = 128;
	bool set;

	while (true)
	{
		r = 999;
		g = 999;
		b = 999;
		set = 1;

		while (set)
		{
			LOG(0, "\nColor: ");
			std::cin >> r >> g >> b;

			if (r >= 0 && r <= 255 && g >= 0 && g <= 255 && b >= 0 && b <= 255)
			{
				set = 0;
			}
			else
			{
				LOG(0, "Wrong color.");
				r = 999;
				g = 999;
				b = 999;
			}
		}

		auraApi->SetAllDevZones((byte)r, (byte)g, (byte)b, 255);
		alienFxApi->SetAllDevZones((byte)r, (byte)g, (byte)b, 255);

	}

	/*
		auto length = (30s / 3) / appSettings->speed;

		while (true)
		{
			colorRGB fromColor(255, 0, 0, 255);
			colorRGB targetColor(0, 255, 0, 255);
			auto start = std::chrono::high_resolution_clock::now();

			while (std::chrono::high_resolution_clock::now() < start + length)
			{
				system("cls");
				std::chrono::duration<float> remaining = std::chrono::high_resolution_clock::now() - start;

				defaultColor = lerp(fromColor, targetColor, remaining.count() / length.count());

				auraApi->SetAllDevZones(defaultColor.red, defaultColor.green, defaultColor.blue, defaultColor.alpha);
				alienFxApi->SetAllDevZones(defaultColor.red, defaultColor.green, defaultColor.blue, defaultColor.alpha);

				std::this_thread::sleep_for(1000ms / appSettings->hz);
			}

			fromColor = { 0, 255, 0, 255 };
			targetColor = { 0, 0, 255, 255 };
			start = std::chrono::high_resolution_clock::now();

			while (std::chrono::high_resolution_clock::now() < start + length)
			{
				system("cls");
				std::chrono::duration<float> remaining = std::chrono::high_resolution_clock::now() - start;

				defaultColor = lerp(fromColor, targetColor, remaining.count() / length.count());

				auraApi->SetAllDevZones(defaultColor.red, defaultColor.green, defaultColor.blue, defaultColor.alpha);
				alienFxApi->SetAllDevZones(defaultColor.red, defaultColor.green, defaultColor.blue, defaultColor.alpha);

				std::this_thread::sleep_for(1000ms / appSettings->hz);
			}


			fromColor = { 0, 0, 255, 255 };
			targetColor = { 255, 0, 0, 255 };
			start = std::chrono::high_resolution_clock::now();

			while (std::chrono::high_resolution_clock::now() < start + length)
			{
				system("cls");
				std::chrono::duration<float> remaining = std::chrono::high_resolution_clock::now() - start;

				defaultColor = lerp(fromColor, targetColor, remaining.count() / length.count());

				auraApi->SetAllDevZones(defaultColor.red, defaultColor.green, defaultColor.blue, defaultColor.alpha);
				alienFxApi->SetAllDevZones(defaultColor.red, defaultColor.green, defaultColor.blue, defaultColor.alpha);

				std::this_thread::sleep_for(1000ms / appSettings->hz);
			}

		}


		LOG(0, "\nDone.");
		std::cin.get();

		return 1;*/
}

