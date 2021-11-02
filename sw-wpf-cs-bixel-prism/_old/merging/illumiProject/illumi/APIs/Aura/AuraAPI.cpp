
#include "pch.h"

#include "Include\AURALightingSDK.h"

#include "AppSettings.h"
#include "Log.h"

#include "API.h"

#include "AuraAPI.h"

#define AURA_DLL "AURA_SDK.dll"


using namespace Aura;


EnumerateMbControllerFunc EnumerateMbController;
SetMbModeFunc SetMbMode;
SetMbColorFunc SetMbColor;
GetMbColorFunc GetMbColor;
GetMbLedCountFunc GetMbLedCount;

EnumerateGPUFunc EnumerateGPU;
SetGPUModeFunc SetGPUMode;
SetGPUColorFunc SetGPUColor;
GetMbColorFunc GetGPUColor;
GetGPULedCountFunc GetGPULedCount;

MbLightControl* mbController;
GPULightControl* gpuController;


AuraAPI::AuraAPI()
{
	HMODULE hLib = LoadLibraryA(AURA_DLL);

	if (hLib)
	{
		(FARPROC&)EnumerateMbController = GetProcAddress(hLib, "EnumerateMbController");
		(FARPROC&)SetMbMode = GetProcAddress(hLib, "SetMbMode");
		(FARPROC&)SetMbColor = GetProcAddress(hLib, "SetMbColor");
		(FARPROC&)GetMbColor = GetProcAddress(hLib, "GetMbColor");
		(FARPROC&)GetMbLedCount = GetProcAddress(hLib, "GetMbLedCount");

		(FARPROC&)EnumerateGPU = GetProcAddress(hLib, "EnumerateGPU");
		(FARPROC&)SetGPUMode = GetProcAddress(hLib, "SetGPUMode");
		(FARPROC&)SetGPUColor = GetProcAddress(hLib, "SetGPUColor");
		(FARPROC&)GetMbColor = GetProcAddress(hLib, "GetGPUColor");
		(FARPROC&)GetGPULedCount = GetProcAddress(hLib, "GetGPULedCount");


		LOG(0, "Aura SDK is ready.");
		mLoaded = true;

		mbCount = (byte)EnumerateMbController(NULL, 0);
		LOG(3, "Aura: " << (int)mbCount << " motherboard controllers found.");

		for (byte i = 0; i < mbCount; i++)
		{
			mbController = new MbLightControl[i];
			EnumerateMbController(mbController, mbCount);
			SetMbMode(mbController[i], 1);

			mbZones = (byte)GetMbLedCount(mbController[i]);
			LOG(3, "Aura: Motherboard ID: " << i + 1 << " has " << (int)mbZones << " light zones.");

			mbColorArray = new byte[mbZones * 3];
			ZeroMemory(mbColorArray, mbZones * 3);
		}

		gpuCount = (byte)EnumerateGPU(NULL, 0);
		LOG(3, "Aura: " << (int)gpuCount << " GPU controllers found.");

		for (byte i = 0; i < gpuCount; i++)
		{
			gpuController = new GPULightControl[gpuCount];
			EnumerateGPU(gpuController, gpuCount);
			SetGPUMode(gpuController[i], 1);

			gpuZones = (byte)GetGPULedCount(gpuController[i]);
			LOG(3, "Aura: GPU ID: " << i + 1 << " has " << (int)gpuZones << " light zones.");

			gpuColorArray = new byte[gpuZones * 3];
			ZeroMemory(gpuColorArray, gpuZones * 3);
		}
	}
	else
	{
		LOG(0, "Aura SDK isn't ready!");
		LOG(1, AURA_DLL << " not found.\n");
	}
}

AuraAPI::~AuraAPI()
{
	delete[] mbController;
	delete[] mbColorArray;
	delete[] gpuController;
	delete[] gpuColorArray;
}


eAPI Aura::AuraAPI::GetApiName()
{
	return API_Aura;
}

int Aura::AuraAPI::GetNumDev()
{
	return mbCount + gpuCount;
}


void Aura::AuraAPI::SetZone(byte devIndex, eDevType devType, byte zoneIndex, byte r, byte g, byte b, byte a)
{
	byte alpha;
	float brightness = mApp->brightness;

	if (mApp->clampMode)
	{
		float clampRatio = ((r * ((a * brightness) / 255)) + (g * ((a * brightness) / 255)) + (b * ((a * brightness) / 255))) / 255;

		if (clampRatio > 1)
		{
			alpha = (byte)((a * brightness) / clampRatio);
		}
		else
		{
			alpha = (byte)(a * brightness);
		}
	}
	else
	{
		alpha = (byte)(a * brightness);
	}

	switch (devType)
	{
	case Motherboard:
		SetMbZone(devIndex, zoneIndex, r, g, b, alpha);
		break;
	case GPU:
		SetGpuZone(devIndex, zoneIndex, r, g, b, alpha);
		break;
	case Keyboard:
		break;
	case Mouse:
		break;
	case Peripheral:
		break;
	case Accessories:
		break;
	default:
		break;
	}
}

void Aura::AuraAPI::SetAllZones(byte devIndex, eDevType devType, byte r, byte g, byte b, byte a)
{
	byte alpha;
	float brightness = mApp->brightness;

	if (mApp->clampMode)
	{
		float clampRatio = ((r * ((a * brightness) / 255)) + (g * ((a * brightness) / 255)) + (b * ((a * brightness) / 255))) / 255;

		if (clampRatio > 1)
		{
			alpha = (byte)((a * brightness) / clampRatio);
		}
		else
		{
			alpha = (byte)(a * brightness);
		}
	}
	else
	{
		alpha = (byte)(a * brightness);
	}

	switch (devType)
	{
	case Motherboard:
		SetAllMbZones(devIndex, r, g, b, alpha);
		break;
	case GPU:
		SetAllGpuZones(devIndex, r, g, b, alpha);
		break;
	case Keyboard:
		break;
	case Mouse:
		break;
	case Peripheral:
		break;
	case Accessories:
		break;
	default:
		break;
	}
}

void Aura::AuraAPI::SetAllDevZones(byte r, byte g, byte b, byte a)
{
	byte alpha;
	float brightness = mApp->brightness;

	if (mApp->clampMode)
	{
		float clampRatio = ((r * ((a * brightness) / 255)) + (g * ((a * brightness) / 255)) + (b * ((a * brightness) / 255))) / 255;

		if (clampRatio > 1)
		{
			alpha = (byte)((a * brightness) / clampRatio);
		}
		else
		{
			alpha = (byte)(a * brightness);
		}
	}
	else
	{
		alpha = (byte)(a * brightness);
	}

	for (byte i = 0; i < mbCount; i++)
	{
		SetAllMbZones(0, r, g, b, alpha);
	}
	for (byte i = 0; i < gpuCount; i++)
	{
		SetAllGpuZones(0, r, g, b, alpha);
	}
}


int AuraAPI::GetMbNum()
{
	return (int)mbCount;
}

int AuraAPI::GetMbZones(byte mbIndex)
{
	return (int)GetMbLedCount(mbController[mbIndex])-1;
}

void AuraAPI::SetMbZone(byte mbIndex, byte lightIndex, byte r, byte g, byte b, byte a)
{
	mbColorArray[lightIndex * 3] = (byte)(r * (a / 255.0f));
	mbColorArray[lightIndex * 3 + 1] = (byte)(b * (a / 255.0f));
	mbColorArray[lightIndex * 3 + 2] = (byte)(g * (a / 255.0f));

	SetMbColor(mbController[mbIndex], mbColorArray, mbZones * 3);

	LOG(3, "Aura - Motherboard ID: " << (int)mbIndex << " Zone: " << (int)lightIndex << " set to " << (int)(r * (a / 255.0f)) << ", " << (int)(g * (a / 255.0f)) << ", " << (int)(b * (a / 255.0f)));
}

void AuraAPI::SetAllMbZones(byte mbIndex, byte r, byte g, byte b, byte a)
{
	for (unsigned int lightIndex = 0; lightIndex < mbZones; lightIndex++)
	{
		mbColorArray[lightIndex * 3] = (byte)(r * (a / 255.0f));
		mbColorArray[lightIndex * 3 + 1] = (byte)(b * (a / 255.0f));
		mbColorArray[lightIndex * 3 + 2] = (byte)(g * (a / 255.0f));
	}

	SetMbColor(mbController[mbIndex], mbColorArray, mbZones * 3);

	LOG(3, "Aura - Motherboard ID: " << (int)mbIndex << " Zone: All set to " << (int)(r * (a / 255.0f)) << ", " << (int)(g * (a / 255.0f)) << ", " << (int)(b * (a / 255.0f)));
}


int AuraAPI::GetGpuNum()
{
	return (int)gpuCount;
}

int AuraAPI::GetGpuZones(byte gpuIndex)
{
	return (int)GetGPULedCount(gpuController[gpuIndex]);
}

void AuraAPI::SetGpuZone(byte gpuIndex, byte lightIndex, byte r, byte g, byte b, byte a)
{
	gpuColorArray[lightIndex * 3] = (byte)(r * (a / 255.0f));
	gpuColorArray[lightIndex * 3 + 1] = (byte)(g * (a / 255.0f));
	gpuColorArray[lightIndex * 3 + 2] = (byte)(b * (a / 255.0f));

	SetGPUColor(gpuController[gpuIndex], gpuColorArray, gpuZones * 3);

	LOG(3, "Aura - GPU ID: " << (int)gpuIndex << " Zone: " << (int)lightIndex << " set to " << (int)(r * (a / 255.0f)) << ", " << (int)(g * (a / 255.0f)) << ", " << (int)(b * (a / 255.0f)));
}

void AuraAPI::SetAllGpuZones(byte gpuIndex, byte r, byte g, byte b, byte a)
{
	for (unsigned int lightIndex = 0; lightIndex < gpuZones; lightIndex++)
	{
		gpuColorArray[lightIndex * 3] = (byte)(r * (a / 255.0f));
		gpuColorArray[lightIndex * 3 + 1] = (byte)(g * (a / 255.0f));
		gpuColorArray[lightIndex * 3 + 2] = (byte)(b * (a / 255.0f));
	}

	SetGPUColor(gpuController[gpuIndex], gpuColorArray, gpuZones * 3);

	LOG(3, "Aura - GPU ID: " << (int)gpuIndex << " Zone: All set to " << (int)(r * (a / 255.0f)) << ", " << (int)(g * (a / 255.0f)) << ", " << (int)(b * (a / 255.0f)));
}
