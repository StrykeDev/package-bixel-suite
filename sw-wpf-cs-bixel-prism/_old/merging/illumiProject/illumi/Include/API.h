#pragma once


enum eAPI
{
	API_Unknown, API_Aura, API_AlienFx
};

enum eDevType
{
	UnknownDevice, Motherboard, GPU, Keyboard, Mouse, Peripheral, Accessories
};


class API
{
protected:
	std::shared_ptr<AppSettings> mApp;
	bool mLoaded = false;

public:
	API();
	~API();

	void Setup(std::shared_ptr<AppSettings> appSettings);

	bool IsLoaded();
	virtual eAPI GetApiName() = 0;

	virtual int GetNumDev() = 0;
	virtual void SetZone(byte devIndex, eDevType devType ,byte zoneIndex, byte r, byte g, byte b, byte a) = 0;
	virtual void SetAllZones(byte devIndex, eDevType devType, byte r, byte g, byte b, byte a) = 0;
	virtual void SetAllDevZones(byte r, byte g, byte b, byte a) = 0;
};

class Device
{
	byte mIndex;
	std::string mId;
	std::string mName;
	eDevType mType;
	std::shared_ptr<API> mApi;
	byte mZones;

	float rOffset = 1.0;
	float gOffset = 1.0;
	float bOffset = 1.0;
	float aOffset = 1.0;

public:
	Device(byte devIndex, std::string devId, eDevType devType, std::shared_ptr<API> devApi, byte devZones);
	~Device();

	void OutputInfo();
	void LoadPreset();
	void SetOffset(float r, float g, float b, float a);

	int GetZones();
	void SetZone(byte zoneIndex, byte r, byte g, byte b, byte a);
	void SetAllZones(byte r, byte g, byte b, byte a);
};

