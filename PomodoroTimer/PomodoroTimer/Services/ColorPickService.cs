using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{

    public class ColorPickService
    {
        static List<string> colors = new List<string>
            {
                "#F44336",
                "#D50000",
                "#C51162",
                "#4A148C",
                "#7986CB",
                "#0D47A1",
                "#2962FF",
                "#01579B",
                "#0091EA",
                "#0097A7",
                "#006064",
                "#004D40",
                "#1B5E20",
                "#33691E",
                "#827717",
                "#AEEA00",
                "#E65100",
                "#F4511E",
                "#DD2C00",
                "#78909C",
                "#212121",
                "#3E2723",
                "#795548"
            };
        private static int ReverseIndex = 0;
        private static int Index = 0;
        private static bool isHead = true;
        public static string NextReverse()
        {
            if (ReverseIndex >= colors.Count)
            {
                ReverseIndex = 0;
            }
            int index = 0;
            if (isHead)
            {
                index = ReverseIndex;
            }
            else
            {
                index =colors.Count - ReverseIndex-1;
                ReverseIndex++;
            }
            isHead = !isHead;

            return colors[index];
        }
        public static string Next()
        {
            Index++;
            Index = Index % colors.Count;
            return colors[Index];
        }
        public static string NextRandom()
        {
            var colorIndex = new Random().Next() % colors.Count;
            return colors[colorIndex];
        }

    }
}
