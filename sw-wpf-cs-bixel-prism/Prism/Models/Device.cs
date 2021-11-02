using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prism.Models
{
    enum DevTypes
    {
        Unknown,
        Motherboard,
        GPU,
        Peripherals,
        LED_Strip
    }

    enum ApiName
    {
        Unknown,
        AlienFX,
        Aura
    }

    enum Pins
    {
        RGB,
        RBG,
        GRB,
        GBR,
        BRG,
        BGR
    }


    struct LEDs
    {
        public bool[] _led;
        public byte[] _color;


        public LEDs(uint numOfLeds)
        {
            _led = new bool[numOfLeds];
            for (int i = 0; i < _led.Length; i++)
            {
                _led[i] = true;
            }

            _color = new byte[numOfLeds * 3];
            for (int i = 0; i < _color.Length; i++)
            {
                _color[i] = 0;
            }
        }


        public void SetLedColor(uint index, byte r, byte g, byte b)
        {
            if (_led[index])
            {
                index *= 3;
                _color[index] = r;
                _color[index + 1] = g;
                _color[index + 2] = b;
            }
        }

        public void SetLed(uint index, bool enable)
        {
            _led[index] = enable;
        }
    }


    class Device
    {
        private string _name = "Unknown Device";
        public string Name { get => _name; }

        private string _nick = "Unknown Device";
        public string Nick { get => _nick; }

        private uint _index = 0;
        public uint Index { get => _index; }

        private uint _zones = 0;
        public uint Zones { get => _zones; }

        private DevTypes _devType = DevTypes.Unknown;
        public DevTypes DevType { get => _devType; }

        private ApiName _apiName = ApiName.Unknown;
        public ApiName ApiName { get => _apiName; }

        private Pins _pinsMode = Pins.RGB;
        public Pins PinsMode { get => _pinsMode; }

        private bool _clampMode = true;
        public bool ClampMode { get => _clampMode; }

        private float _redOffset = 1;
        public float RedOffset { get => _redOffset; set => _redOffset = value; }

        private float _greenOffset = 1;
        public float GreenOffset { get => _greenOffset; set => _greenOffset = value; }

        private float _blueOffset = 1;
        public float BlueOffset { get => _blueOffset; set => _blueOffset = value; }

        public LEDs _leds;


        public Device(string name, uint index, uint zones, DevTypes devType, ApiName apiName, Pins pinsMode)
        {
            _name = name;
            _index = index;
            _zones = zones;
            _devType = devType;
            _apiName = apiName;
            _pinsMode = pinsMode; // temp
            _leds = new LEDs(_zones);

            LoadPreset();
        }


        public void LoadPreset()
        {
            // Load from storage
            // nick
            // pin mode
            // clamp mode
            // rgb offset
        }


        private void FixPinsMode(byte red, byte green, byte blue, out byte outRed, out byte outGreen, out byte outBlue)
        {
            switch (_pinsMode)
            {
                case Pins.RBG:
                    outRed = red;
                    outGreen = blue;
                    outBlue = green;
                    break;

                case Pins.GRB:
                    outRed = green;
                    outGreen = red;
                    outBlue = blue;
                    break;

                case Pins.GBR:
                    outRed = green;
                    outGreen = blue;
                    outBlue = red;
                    break;

                case Pins.BRG:
                    outRed = blue;
                    outGreen = red;
                    outBlue = green;
                    break;

                case Pins.BGR:
                    outRed = blue;
                    outGreen = green;
                    outBlue = red;
                    break;

                default:
                    outRed = red;
                    outGreen = green;
                    outBlue = blue;
                    break;
            }
        }

        private void Clamp(byte red, byte green, byte blue, out byte outRed, out byte outGreen, out byte outBlue)
        {
            outRed = red;
            outGreen = green;
            outBlue = blue;
        }


        public void SetColorZone(uint zone, byte red, byte green, byte blue)
        {

            red = (byte)(red * _redOffset);
            green = (byte)(green * _greenOffset);
            blue = (byte)(blue * _blueOffset);

            if (_pinsMode != Pins.RGB)
            {
                FixPinsMode(red, green, blue, out red, out green, out blue);
            }

            if (_clampMode)
            {
                Clamp(red, green, blue, out red, out green, out blue);
            }

            _leds.SetLedColor(zone, red, green, blue);
        }

        public void SetColor(byte red, byte green, byte blue)
        {
            red = (byte)(red * _redOffset);
            green = (byte)(green * _greenOffset);
            blue = (byte)(blue * _blueOffset);

            if (_pinsMode != Pins.RGB)
            {
                FixPinsMode(red, green, blue, out red, out green, out blue);
            }

            if (_clampMode)
            {
                Clamp(red, green, blue, out red, out green, out blue);
            }

            for (uint i = 0; i < _zones; i++)
            {
                _leds.SetLedColor(i, red, green, blue);
            }
        }


        public byte[] GetColor()
        {
            return _leds._color;
        }
    }


    class LEDStrip
    {
        public Device _dev;
        private uint _zoneIndex;

        public string Name { get => _dev.Name + " LED Strip"; }

        public DevTypes DevType { get => DevTypes.LED_Strip; }

        public ApiName ApiName { get => _dev.ApiName; }

        private Pins _pinsMode = Pins.RGB;
        public Pins PinsMode { get => _pinsMode; set => _pinsMode = value; }

        private bool _clampMode = true;
        public bool ClampMode { get => _clampMode; }

        private float _redOffset = 1;
        public float RedOffset { get => _redOffset; set => _redOffset = value; }

        private float _greenOffset = 1;
        public float GreenOffset { get => _greenOffset; set => _greenOffset = value; }

        private float _blueOffset = 1;
        public float BlueOffset { get => _blueOffset; set => _blueOffset = value; }


        public LEDStrip(Device device, uint zoneIndex)
        {
            _dev = device;
            _zoneIndex = zoneIndex;
            _dev._leds.SetLed(_zoneIndex, false);

            LoadPreset();
        }


        public void LoadPreset()
        {
            // Load from storage
            // pin mode
            // clamp mode
            // rgb offset
        }


        private void FixPinsMode(byte red, byte green, byte blue, out byte outRed, out byte outGreen, out byte outBlue)
        {
            switch (_pinsMode)
            {
                case Pins.RBG:
                    outRed = red;
                    outGreen = blue;
                    outBlue = green;
                    break;

                case Pins.GRB:
                    outRed = green;
                    outGreen = red;
                    outBlue = blue;
                    break;

                case Pins.GBR:
                    outRed = green;
                    outGreen = blue;
                    outBlue = red;
                    break;

                case Pins.BRG:
                    outRed = blue;
                    outGreen = red;
                    outBlue = green;
                    break;

                case Pins.BGR:
                    outRed = blue;
                    outGreen = green;
                    outBlue = red;
                    break;

                default:
                    outRed = red;
                    outGreen = green;
                    outBlue = blue;
                    break;
            }
        }

        private void Clamp(byte red, byte green, byte blue, out byte outRed, out byte outGreen, out byte outBlue)
        {
            outRed = red;
            outGreen = green;
            outBlue = blue;
        }


        public void SetColor(byte red, byte green, byte blue)
        {
            red = (byte)(red * _redOffset);
            green = (byte)(green * _greenOffset);
            blue = (byte)(blue * _blueOffset);

            if (_pinsMode != Pins.RGB)
            {
                FixPinsMode(red, green, blue, out red, out green, out blue);
            }

            if (_clampMode)
            {
                Clamp(red, green, blue, out red, out green, out blue);
            }

            uint ledIndex = _zoneIndex * 3;
            _dev._leds._color[ledIndex] = red;
            _dev._leds._color[ledIndex + 1] = green;
            _dev._leds._color[ledIndex + 2] = blue;
        }
    }
}
