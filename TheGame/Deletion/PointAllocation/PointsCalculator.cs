using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheGame.PointAllocation
{
    public static class PointsCalculator
    {

        public static int CalculatePointsForDirectory(DirectoryInfo directory)
        {
            int points = 0;

            // Flat value for folder?
            // Flat value per file?

            foreach (FileInfo child in directory.GetFiles())
                points += CalculatePointsForFile(child);

            foreach (DirectoryInfo child in directory.GetDirectories())
                points += CalculatePointsForDirectory(child);

            return points;
        }

        public static int CalculatePointsForFile(FileInfo file)
        {
            int points = 0;

            // Creation Date
            int daysCreatedAgo = (int) (file.CreationTimeUtc - DateTime.Now).TotalDays;
            if (daysCreatedAgo > 10)
                points += (daysCreatedAgo * (1 / (daysCreatedAgo % 365))); // currently add more points the older the file is, but decreasing year on year, make it the newer the better

            //Size
            points += (int) file.Length / 100;

            return points;
        }
    }
}