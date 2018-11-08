using System;
using System.Collections.Generic;
using System.Text;

namespace PomodoroTimer.Services
{

    public class ColorPickService
    {
        static List<string> colors = new List<string>
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
        private static int ReverseIndex = 0;
        private static int Index = 0;
        private static bool isHead = true;
        private static Random random;
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
                index = colors.Count - ReverseIndex - 1;
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
            if (random == null)
                random = new Random();
            var colorIndex = random.Next() % colors.Count;
            return colors[colorIndex];
        }

    }
}
