using System;
using System.Linq;
using Windows.UI;

namespace Paint
{
    class FigureInfo
    {
        public double Width { get; set; }

        public double CanvasLeft { get; set; }

        public double CanvasTop { get; set; }

        public int CanvasZ { get; set; }

        public string FigureName { get; set; }

        public Color FigureBrush { get; set; }
    }
}
