using Microsoft.Win32;
using Prism.Models;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Input;
using System.Collections.Generic;
using Prism.APIs;

namespace Prism.ViewModels
{
    class MainViewModel : ObservableObject
    {
        // Metadata
        private static string GetExecutingAssemblyAttribute<T>(Func<T, string> value) where T : Attribute
        {
            T attribute = (T)Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(T));
            return value.Invoke(attribute);
        }

        public static string DonateUrl { get; } = @"https://www.bixel.io/donate?amount=1";
        public static string WebsiteUrl { get; } = @"https://www.bixel.io";
        public static string BuildName { get; } = GetExecutingAssemblyAttribute<AssemblyConfigurationAttribute>(a => a.Configuration);
        public static string Copyright { get; } = GetExecutingAssemblyAttribute<AssemblyCopyrightAttribute>(a => a.Copyright);


        // Settings
        public static RegistryKey regKeyStartup = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private static bool _startup = regKeyStartup.GetValue("Prism") != null;
        public static bool Startup
        {
            get { return _startup; }
            set
            {
                if (_startup != value)
                {
                    _startup = value;
                    if (value)
                    {
                        regKeyStartup.SetValue("Prism", '"' + System.Reflection.Assembly.GetExecutingAssembly().Location + '"');
                    }
                    else
                    {
                        regKeyStartup.DeleteValue("Prism");
                    }
                }
            }
        }

        private static bool _startMinimized = Properties.Settings.Default.Start_Min;
        public static bool StartMinimized
        {
            get { return _startMinimized; }
            set
            {
                if (_startMinimized != value)
                {
                    _startMinimized = value;
                    Properties.Settings.Default.Start_Min = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private static int _updateInterval = Properties.Settings.Default.Update_Interval;
        public static int UpdateInterval
        {
            get { return _updateInterval; }
            set
            {
                if (_updateInterval != value)
                {
                    _updateInterval = value;
                    Properties.Settings.Default.Update_Interval = value;
                    Properties.Settings.Default.Save();
                }
            }
        }

        public static DeviceManager devManager = new DeviceManager();

        private Color _color = new Color();
        public Color Color
        {
            get => _color;
            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }


        // Entry point
        public MainViewModel()
        {
            devManager.Devs[0].RedOffset = 0.5f;
            devManager.Devs[0].GreenOffset = 0.5f;
            devManager.Devs[0].BlueOffset = 0.5f;

            devManager.Devs[1].RedOffset = 0.5f;

            devManager.Devs[2].RedOffset = 0.8f;
            devManager.Devs[2].GreenOffset = 1f;
            devManager.Devs[2].BlueOffset = 1f;

            devManager.Devs[3].RedOffset = 0.5f;
            devManager.Devs[3].GreenOffset = 0.8f;
            devManager.Devs[3].BlueOffset = 1f;

            devManager.AddStrip(2, 4);
            devManager.Strips[0].RedOffset = 1f;
            devManager.Strips[0].GreenOffset = 0.6f;
            devManager.Strips[0].BlueOffset = 0.9f;
            devManager.Strips[0].PinsMode = Pins.RBG;



            //float hue = 0;
            //while (true)
            //{
            //    Color = FromHSV(hue, 1f, 0.1f);
            //    devManager.SetEverything(Color.R, Color.G, Color.B);

            //    hue += 0.1f;
            //    if (hue > 360)
            //    {
            //        hue = 0;
            //    }

            //    Thread.Sleep(1000 / 10);
            //}
            
        }


        static public Color FromHSV(float h, float s, float v)
        {
            if (s == 0)
            {
                int L = (int)v; return Color.FromArgb(255, L, L, L);
            }

            float min, max;

            max = v < 0.5f ? v * (1 + s) : (v + s) - (v * s);
            min = (v * 2f) - max;

            return Color.FromArgb(255, (int)(255 * RGBChannelFromHue(min, max, (h/360) + 1 / 3d)),
                                       (int)(255 * RGBChannelFromHue(min, max, (h/360))),
                                       (int)(255 * RGBChannelFromHue(min, max, (h/360) - 1 / 3d)));
        }

        static double RGBChannelFromHue(double m1, double m2, double h)
        {
            h = (h + 1d) % 1d;
            if (h < 0) h += 1;
            if (h * 6 < 1) return m1 + (m2 - m1) * 6 * h;
            else if (h * 2 < 1) return m2;
            else if (h * 3 < 2) return m1 + (m2 - m1) * 6 * (2d / 3d - h);
            else return m1;
        }
    }
}