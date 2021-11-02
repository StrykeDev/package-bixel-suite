#pragma once


namespace Aura
{
	class AuraAPI : public API
	{
		byte mbCount;
		byte mbZones;
		byte* mbColorArray;

		byte gpuCount;
		byte gpuZones;
		byte* gpuColorArray;

	public:
		AuraAPI();
		~AuraAPI();

		eAPI GetApiName() override;
		int GetNumDev() override;

		void SetZone(byte devIndex, eDevType devType, byte zoneIndex, byte r, byte g, byte b, byte a) override;
		void SetAllZones(byte devIndex, eDevType devType, byte r, byte g, byte b, byte a) override;
		void SetAllDevZones(byte r, byte g, byte b, byte a) override;

		int GetMbNum();
		int GetMbZones(byte mbIndex);
		void SetMbZone(byte mbIndex, byte lightIndex, byte r, byte g, byte b, byte a);
		void SetAllMbZones(byte mbIndex, byte r, byte g, byte b, byte a);

		int GetGpuNum();
		int GetGpuZones(byte gpuIndex);
		void SetGpuZone(byte gpuIndex, byte lightIndex, byte r, byte g, byte b, byte a);
		void SetAllGpuZones(byte gpuIndex, byte r, byte g, byte b, byte a);
	};
}
