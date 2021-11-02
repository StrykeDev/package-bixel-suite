using Prism.APIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Models
{
    class DeviceManager
    {
        private AlienFxApi _alienFxApi = new AlienFxApi();
        private AuraApi _auraApi = new AuraApi();

        private List<Device> _devs = new List<Device>();
        public List<Device> Devs { get => _devs; }

        private List<LEDStrip> _strips = new List<LEDStrip>();
        public List<LEDStrip> Strips { get => _strips; }

        public DeviceManager()
        {
            Detect();
            LoadStrips();
        }


        public void Detect()
        {
            _devs.Clear();

            // Alien FX
            for (uint i = 0; i < _alienFxApi.NumOfDevs; i++)
            {
                _devs.Add(new Device(_alienFxApi.GetDeviceName(i), i, _alienFxApi.GetDeviceZones(i), DevTypes.Peripherals, ApiName.AlienFX, Pins.RGB));
            }

            // Aura
            for (uint i = 0; i < _auraApi.NumOfMb; i++)
            {
                _devs.Add(new Device(_auraApi.GetMBName(i), i, _auraApi.GetMBZones(i), DevTypes.Motherboard, ApiName.Aura, Pins.RBG));
            }

            for (uint i = 0; i < _auraApi.NumOfGPU; i++)
            {
                _devs.Add(new Device(_auraApi.GetGPUName(i), i, _auraApi.GetGPUZones(i), DevTypes.GPU, ApiName.Aura, Pins.RGB));
            }
        }

        public void LoadStrips()
        {
            // Load LED stips
        }


        public void AddStrip(int devIndex, uint zoneIndex)
        {
            Strips.Add(new LEDStrip(_devs[devIndex], zoneIndex));
        }


        public void SetEverything(byte red, byte green, byte blue)
        {
            for (int i = 0; i < _devs.Count; i++)
            {
                SetAllZoneColor(i, red, green, blue);
            }

            for (int i = 0; i < _strips.Count; i++)
            {
                SetStripColor(i, red, green, blue);
            }
        }

        public void SetAllZoneColor(int index, byte red, byte green, byte blue)
        {
            _devs[index].SetColor(red, green, blue);
            UpdateColor(index);
        }

        public void SetZoneColor(int index, uint zone, byte red, byte green, byte blue)
        {
            _devs[index].SetColorZone(zone, red, green, blue);
            UpdateColor(index);
        }

        public void SetStripColor(int index, byte red, byte green, byte blue)
        {
            _strips[index].SetColor(red, green, blue);
            UpdateStripColor(index);
        }

        public void UpdateColor(int index)
        {
            switch (_devs[index].ApiName)
            {
                case ApiName.AlienFX:
                    _alienFxApi.SetDeviceLights(_devs[index].Index, _devs[index].GetColor());
                    break;

                case ApiName.Aura:
                    switch (_devs[index].DevType)
                    {
                        case DevTypes.Motherboard:
                            _auraApi.SetMBColor(_devs[index].Index, _devs[index].GetColor());
                            break;

                        case DevTypes.GPU:
                            _auraApi.SetGPUColor(_devs[index].Index, _devs[index].GetColor());
                            break;
                    }
                    break;
            }
        }

        public void UpdateStripColor(int index)
        {
            switch (_strips[index].ApiName)
            {
                case ApiName.AlienFX:
                    _alienFxApi.SetDeviceLights(_strips[index]._dev.Index, _strips[index]._dev.GetColor());
                    break;

                case ApiName.Aura:
                    switch (_strips[index]._dev.DevType)
                    {
                        case DevTypes.Motherboard:
                            _auraApi.SetMBColor(_strips[index]._dev.Index, _strips[index]._dev.GetColor());
                            break;

                        case DevTypes.GPU:
                            _auraApi.SetGPUColor(_strips[index]._dev.Index, _strips[index]._dev.GetColor());
                            break;
                    }
                    break;
            }
        }

        public void UpdateAll()
        {
            for (int i = 0; i < _devs.Count; i++)
            {
                UpdateColor(i);
            }
        }
    }
}
