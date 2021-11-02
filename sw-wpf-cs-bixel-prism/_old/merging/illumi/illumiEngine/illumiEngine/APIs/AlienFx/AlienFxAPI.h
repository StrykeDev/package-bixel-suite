#pragma once


namespace AlienFx
{
	class AlienFxAPI : public API
	{
		unsigned int numDevs;

	public:
		AlienFxAPI();
		~AlienFxAPI();

		eAPI GetApiName() override;
		int GetNumDev() override;

		void SetZone(byte devIndex, eDevType devType, byte zoneIndex, byte r, byte g, byte b, byte a) override;
		void SetAllZones(byte devIndex, eDevType devType , byte r, byte g, byte b, byte a) override;
		void SetAllDevZones(byte r, byte g, byte b, byte a) override;

		int GetDevZones(byte devIndex);
		std::string GetDevName(byte devIndex);
	};
}
