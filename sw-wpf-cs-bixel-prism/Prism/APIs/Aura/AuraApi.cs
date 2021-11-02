using System;
using AuraSDKWrapper;

namespace Prism.APIs
{
    class AuraApi : Api
    {
        private AuraSDK _aura = new AuraSDK();

        private readonly int _numOfDevs;
        public int NumOfDevs
        {
            get { return _numOfDevs; }
        }

        private int _numOfMb;
        public int NumOfMb
        {
            get { return _numOfMb; }
        }

        private int _numOfGPU;
        public int NumOfGPU
        {
            get { return _numOfGPU; }
        }


        public AuraApi()
        {
            _aura.DetectAuraDevices();
            _numOfDevs = _aura.MBControllersCount + _aura.GPUControllersCount;
            _numOfMb = _aura.MBControllersCount;
            _numOfGPU = _aura.GPUControllersCount;

            for (int i = 0; i < _aura.MBControllersCount; i++)
            {
                _aura.SetMBLedMode(i, 1);
            }
            for (int i = 0; i < _aura.GPUControllersCount; i++)
            {
                _aura.SetGPUCtrlLedMode(i, 1);
            }
        }


        public void Debug()
        {
            Console.WriteLine("Aura Motherboards: " + _aura.MBControllersCount);
            for (int i = 0; i < _aura.MBControllersCount; i++)
            {
                Console.WriteLine("Motherboard " + i + ": Zones: " + _aura.GetMBLedCount(i).ToString());
            }

            Console.WriteLine("Aura GPUs: " + _aura.GPUControllersCount);
            for (int i = 0; i < _aura.GPUControllersCount; i++)
            {
                Console.WriteLine("GPU " + i + ": Zones: " + _aura.GetGPUCtrlLedCount(i).ToString());
            }

            Console.WriteLine();
        }


        public string GetMBName(uint devIndex)
        {
            return "Aura Motherboard " + (devIndex + 1);
        }

        public string GetGPUName(uint devIndex)
        {
            return "Aura GPU " + (devIndex + 1);
        }


        public uint GetMBZones(uint devIndex)
        {
            return (uint)_aura.GetMBLedCount((int)devIndex);
        }

        public uint GetGPUZones(uint devIndex)
        {
            return (uint)_aura.GetGPUCtrlLedCount((int)devIndex);
        }


        public void SetMBColor(uint devIndex, byte[] color)
        {
            _aura.SetMBLedColor((int)devIndex, color);
        }

        public void SetGPUColor(uint devIndex, byte[] color)
        {
            _aura.SetGPUCtrlLedColor((int)devIndex, color);
        }
    }
}
