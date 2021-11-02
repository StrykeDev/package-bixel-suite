
#include "pch.h"

#include "log.h"

#include "API.h"


// API
API::API()
{
	LOG(3, "An API has been created!");
}

API::~API()
{
	LOG(3, "An API has been destroyed!");
}


// Devices
Device::Device(byte devIndex, std::string devName, eDevType devType, std::shared_ptr<API> devApi, byte devZones)
{
	std::string temp = devName;
	std::replace(temp.begin(), temp.end(), ' ', '_');
	mIndex = devIndex;
	mId = temp;
	mName = devName;
	mType = devType;
	mApi = devApi;
	mZones = devZones;

	OutputInfo();
}

Device::~Device()
{
}


void Device::OutputInfo()
{
	std::string apiName;

	switch (mApi->GetApiName())
	{
	case API_Aura:
		apiName = "Aura";
		break;

	case API_AlienFx:
		apiName = "AlienFx";
		break;
	}

	LOG(0, "Name: " << mName << std::endl\
		<< "API: " << apiName << std::endl\
		<< "Index: " << (int)mIndex << std::endl\
		<< "Zones: " << (int)mZones);
}

void Device::LoadPreset()
{
	auto fileName = "C:\\Users\\Barak\\AppData\\Roaming\\illumi\\preset.ini";

	rOffset = (float)GetPrivateProfileInt(mId.c_str(), "RedOffset", 1, fileName);
	gOffset = (float)GetPrivateProfileInt(mId.c_str(), "GreenOffset", 1, fileName);
	bOffset = (float)GetPrivateProfileInt(mId.c_str(), "BlueOffset", 1, fileName);
	aOffset = (float)GetPrivateProfileInt(mId.c_str(), "AlphaOffset", 1, fileName);
}

void Device::SetOffset(float r, float g, float b, float a)
{
	rOffset = r;
	gOffset = g;
	bOffset = b;
	aOffset = a;
}


int Device::GetZones()
{
	return mZones;
}


void Device::SetZone(byte zoneIndex, byte r, byte g, byte b, byte a)
{
	mApi->SetZone(mIndex, mType, zoneIndex, (byte)(r*rOffset), (byte)(g*gOffset), (byte)(b*bOffset), (byte)(a*aOffset));
}

void Device::SetAllZones(byte r, byte g, byte b, byte a)
{
	mApi->SetAllZones(mIndex, mType, (byte)(r*rOffset), (byte)(g*gOffset), (byte)(b*bOffset), (byte)(a*aOffset));
}

