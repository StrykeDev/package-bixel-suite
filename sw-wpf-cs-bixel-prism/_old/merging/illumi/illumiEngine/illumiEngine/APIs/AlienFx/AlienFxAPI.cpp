
#include "pch.h"

#include "Include/LFX2.h"

#include "AppSettings.h"
#include "Log.h"

#include "API.h"

#include "AlienFxAPI.h"

#define AlienFx_DLL "LightFX.dll"


using namespace AlienFx;


LFX2INITIALIZE initFunction;
LFX2RELEASE releaseFunction;
LFX2RESET resetFunction;
LFX2UPDATE updateFunction;
LFX2GETNUMDEVICES getNumDevicesFunction;
LFX2GETDEVDESC getDeviceDescriptionFunction;
LFX2GETNUMLIGHTS getNumLightsFunction;
LFX2SETLIGHTCOL setLightColorFunction;
LFX2GETLIGHTCOL getLightColorFunction;
LFX2GETLIGHTDESC getLightDescriptionFunction;


AlienFxAPI::AlienFxAPI()
{
	HMODULE hLib = LoadLibrary(AlienFx_DLL);

	if (hLib)
	{
		initFunction = (LFX2INITIALIZE)GetProcAddress(hLib, LFX_DLL_INITIALIZE);
		releaseFunction = (LFX2RELEASE)GetProcAddress(hLib, LFX_DLL_RELEASE);
		resetFunction = (LFX2RESET)GetProcAddress(hLib, LFX_DLL_RESET);
		updateFunction = (LFX2UPDATE)GetProcAddress(hLib, LFX_DLL_UPDATE);
		getNumDevicesFunction = (LFX2GETNUMDEVICES)GetProcAddress(hLib, LFX_DLL_GETNUMDEVICES);
		getDeviceDescriptionFunction = (LFX2GETDEVDESC)GetProcAddress(hLib, LFX_DLL_GETDEVDESC);
		getNumLightsFunction = (LFX2GETNUMLIGHTS)GetProcAddress(hLib, LFX_DLL_GETNUMLIGHTS);
		setLightColorFunction = (LFX2SETLIGHTCOL)GetProcAddress(hLib, LFX_DLL_SETLIGHTCOL);
		getLightColorFunction = (LFX2GETLIGHTCOL)GetProcAddress(hLib, LFX_DLL_GETLIGHTCOL);
		getLightDescriptionFunction = (LFX2GETLIGHTDESC)GetProcAddress(hLib, LFX_DLL_GETLIGHTDESC);

		LFX_RESULT result = initFunction();

		if (result == LFX_SUCCESS)
		{
			LOG(0, "AlienFx is ready.");
			mLoaded = true;

			getNumDevicesFunction(&numDevs);
			LOG(3, "AlienFx: " << (int)numDevs << " devices found.");
		}
		else
		{
			switch (result)
			{
			case LFX_ERROR_NODEVS:
				LOG(0, "AlienFx isn't ready!");
				LOG(1, "No devices are available\n");
				break;

			default:
				LOG(0, "AlienFx isn't ready!");
				LOG(1, "Something failed and we not sure what.\n");
				break;
			}
		}
	}
	else
	{
		LOG(0, "AlienFx isn't ready!");
		LOG(1, "\t" << AlienFx_DLL << " not found.\n");
	}
}

AlienFxAPI::~AlienFxAPI()
{
	releaseFunction();
	resetFunction();
}


eAPI AlienFxAPI::GetApiName()
{
	return API_AlienFx;
}

int AlienFxAPI::GetNumDev()
{
	return numDevs;
}


void AlienFxAPI::SetZone(byte devIndex, eDevType devType, byte zoneIndex, byte r, byte g, byte b, byte a)
{
	LFX_COLOR rgba;
	rgba.red = r;
	rgba.green = g;
	rgba.blue = b;
	float brightness = mApp->brightness;

	if (mApp->clampMode && devIndex < 1)
	{
		float clampRatio = ((r * ((a * brightness) / 255)) + (g * ((a * brightness) / 255)) + (b * ((a * brightness) / 255))) / 255;

		if (clampRatio > 1)
		{
			rgba.brightness = (byte)((a * brightness) / clampRatio);
			LOG(3, "AlienFx - Device ID: " << (int)devIndex << " Zone: " << (int)zoneIndex << " set to " << (int)(r * (((a * brightness) / clampRatio) / 255.0f)) << ", " << (int)(g * (((a * brightness) / clampRatio) / 255.0f)) << ", " << (int)(b * (((a * brightness) / clampRatio) / 255.0f)));
		}
		else
		{
			rgba.brightness = (byte)(a * brightness);
			LOG(3, "AlienFx - Device ID: " << (int)devIndex << " Zone: " << (int)zoneIndex << " set to " << (int)(r * ((a * brightness) / 255.0f)) << ", " << (int)(g * ((a * brightness) / 255.0f)) << ", " << (int)(b * ((a * brightness) / 255.0f)));
		}
	}
	else
	{
		rgba.brightness = (byte)(a * brightness);
		LOG(3, "AlienFx - Device ID: " << (int)devIndex << " Zone: " << (int)zoneIndex << " set to " << (int)(r * ((a * brightness) / 255.0f)) << ", " << (int)(g * ((a * brightness) / 255.0f)) << ", " << (int)(b * ((a * brightness) / 255.0f)));
	}

	setLightColorFunction(devIndex, zoneIndex, &rgba);
	updateFunction();
}

void AlienFxAPI::SetAllZones(byte devIndex, eDevType devType, byte r, byte g, byte b, byte a)
{
	unsigned int numZones;
	getNumLightsFunction(devIndex, &numZones);
	
	LFX_COLOR rgba;
	rgba.red = r;
	rgba.green = g;
	rgba.blue = b;
	float brightness = mApp->brightness;

	switch (devIndex)
	{
	case 1:
		rgba.red = rgba.red * 0.5;
		rgba.green = rgba.green;
		rgba.blue = rgba.blue;

	default:
		break;
	}

	if (mApp->clampMode && devIndex < 1)
	{
		float clampRatio = ((r * ((a * brightness) / 255)) + (g * ((a * brightness) / 255)) + (b * ((a * brightness) / 255))) / 255;

		if (clampRatio > 1)
		{
			rgba.brightness = (byte)((a * brightness) / clampRatio);
			LOG(3, "AlienFx - Device ID: " << (int)devIndex << " Zone: All set to " << (int)(r * (((a * brightness) / clampRatio) / 255.0f)) << ", " << (int)(g * (((a * brightness) / clampRatio) / 255.0f)) << ", " << (int)(b * (((a * brightness) / clampRatio) / 255.0f)));
		}
		else
		{
			rgba.brightness = (byte)(a * brightness);
			LOG(3, "AlienFx - Device ID: " << (int)devIndex << " Zone: All set to " << (int)(r * ((a * brightness) / 255.0f)) << ", " << (int)(g * ((a * brightness) / 255.0f)) << ", " << (int)(b * ((a * brightness) / 255.0f)));
		}
	}
	else
	{
		rgba.brightness = (byte)(a * brightness);
		LOG(3, "AlienFx - Device ID: " << (int)devIndex << " Zone: All set to " << (int)(r * ((a * brightness) / 255.0f)) << ", " << (int)(g * ((a * brightness) / 255.0f)) << ", " << (int)(b * ((a * brightness) / 255.0f)));
	}

	for (unsigned int zoneIndex = 0; zoneIndex < numZones; zoneIndex++)
	{
		setLightColorFunction(devIndex, zoneIndex, &rgba);
	}

	updateFunction();
}

void AlienFxAPI::SetAllDevZones(byte r, byte g, byte b, byte a)
{
	for (byte i = 0; i < numDevs; i++)
	{
		SetAllZones(i, Peripheral, r, g, b, a);
	}
}


int AlienFxAPI::GetDevZones(byte devIndex)
{
	unsigned int numLights;
	getNumLightsFunction(devIndex, &numLights);
	return numLights;
}

std::string AlienFxAPI::GetDevName(byte devIndex)
{
	unsigned char devType = 0;
	unsigned char* devTypePtr = &devType;
	unsigned int descSize = 255;
	char* desc = new char[descSize];

	getDeviceDescriptionFunction(devIndex, desc, descSize, &devType);

	std::string out = desc;
	delete[] desc;

	if (out == "ROCCAT IskuFX") return "ROCCAT Tyon";
	if (out == "ROCCAT ISKU FX") return "ROCCAT IskuFX";
	return out;
}
