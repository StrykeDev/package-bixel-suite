using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blaze.Models
{
    class PreviewDisplay
    {
        private int _height;
        private int _width;
        private int _top;
        private int _left;
        private string _lable;
        private float _lableSize;

        public int Height { get => _height; set => _height = value; }
        public int Width { get => _width; set => _width = value; }
        public int Top { get => _top; set => _top = value; }
        public int Left { get => _left; set => _left = value; }
        public string Lable { get => _lable; set => _lable = value; }
        public float LableSize { get => _lableSize; set => _lableSize = value; }

        public PreviewDisplay(int height, int width, int top, int left)
        {
            Height = height;
            Width = width;
            Top = top;
            Left = left;

            Lable = string.Format("{0} x {1}", Width, Height);
            LableSize = Width / 10;
        }
    }
}
