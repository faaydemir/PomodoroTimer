using System;
using System.Collections.Generic;
using System.Text;

namespace Helper.Services
{
    public class ColorPicker
    {
        
        public Random Random { get; }

        private readonly List<string> ColorScheme;
        private readonly Dictionary<object, string> ColorDic;
        private List<string> AvailableColors;


        public ColorPicker(List<string> colorScheme)
        {
            ColorScheme  = colorScheme;
            ColorDic = new Dictionary<object, string>();
            Random = new Random();
            AvailableColors =ColorScheme.GetRange(0, ColorScheme.Count); ;
        }

        public string GetByKey(object key)
        {
            if (!ColorDic.ContainsKey(key))
            {
                ColorDic[key] = GetRandom();
            }
            return ColorDic[key];
        }

        public string GetRandom()
        {
            int index = Random.Next(AvailableColors.Count);
            var selectedColor =  AvailableColors[index];
            AvailableColors.Remove(selectedColor);

            if (AvailableColors.Count == 0)
            {
                AvailableColors = ColorScheme.GetRange(0, ColorScheme.Count);
            }
            return selectedColor;
        }

        public string this[object key]
        {
            get
            {
                return GetByKey(key);
            }
        }
    }

    public class ColorPickService
    {
        public static readonly List<string> Colors = new List<string>
        {
            "#0D47A1",
            "#1F77B4",
            "#AEC7E8",
            "#DD2C00",
            "#FF7F0E",
            "#FFBB78",
            "#1B5E20",
            "#2CA02C",
            "#98DF8A",
            "#C51162",
            "#D50000",
            "#D62728",
            "#FF9896",
            "#4A148C",
            "#9467BD",
            "#C5B0D5",
            "#8C564B",
            "#C49C94",
            "#3E2723",
            "#795548",
            "#E377C2",
            "#F7B6D2",
            "#7F7F7F",
            "#AEEA00",
            "#BCBD22",
            "#DBDB8D",
            "#17BECF",
            "#9EDAE5",
            "#78909C",
        };

        private static ColorPicker _colorPicker = new ColorPicker(Colors);

        public static string GetRandom()
        {
            return _colorPicker.GetRandom();
        }

        public static string GetByKey(object key)
        {
            return _colorPicker.GetByKey(key);
        }
    }
}
