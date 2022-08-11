using SixLabors.Fonts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Extensions
{
    public static class FontExtensions
    {
        public static Font ScaleToWidth(this Font font, string text, float width, float max = 1)
        {
            var size = TextMeasurer.Measure(text, new TextOptions(font));
            var scaling = width / size.Width;

            return new Font(font, font.Size * Math.Min(max, scaling));
        }
    }
}
