using System;
using System.Text;
using LightFX;

namespace Prism.APIs
{
    class AlienFxApi : Api
    {
        private readonly uint _numOfDevs;
        public uint NumOfDevs
        {
            get { return _numOfDevs; }
        }

        private LightFXController _lightFX = new LightFXController();


        public AlienFxApi()
        {
            var result = _lightFX.LFX_Initialize();
            if (result == LFX_Result.LFX_Success)
            {
                _lightFX.LFX_GetNumDevices(out _numOfDevs);
            }
            else
            {
                switch (result)
                {
                    case LFX_Result.LFX_Error_NoDevs:
                        throw new Exception("There is not AlienFX device available.");

                    default:
                        throw new Exception("There was an error initializing the AlienFX device.");
                }
            }
        }


        public void Debug()
        {
            Console.WriteLine("AlienFX devices: " + _numOfDevs);
            for (uint i = 0; i < _numOfDevs; i++)
            {
                _lightFX.LFX_GetDeviceDescription(i, out StringBuilder devDescription, 255, out LFX_DeviceType type);
                _lightFX.LFX_GetNumLights(i, out uint lights);
                Console.WriteLine("Device " + i + ": Name: " + devDescription.ToString() + " LEDs: " + lights);
            }
        }


        public string GetDeviceName(uint devIndex)
        {
            _lightFX.LFX_GetDeviceDescription(devIndex, out StringBuilder devDescription, 255, out LFX_DeviceType type);
            return devDescription.ToString();
        }

        public uint GetDeviceZones(uint devIndex)
        {
            _lightFX.LFX_GetNumLights(devIndex, out uint outZones);
            return outZones;
        }


        public void SetDeviceLights(uint devIndex, byte[] color)
        {
            var LFXcolor = new LFX_ColorStruct(255, color[0], color[1], color[2]);

            _lightFX.LFX_GetNumLights(devIndex, out uint lights);
            for (uint i = 0; i < lights; i++)
            {
                _lightFX.LFX_SetLightColor(devIndex, i, LFXcolor);
            }

            _lightFX.LFX_Update();
        }
    }
}
