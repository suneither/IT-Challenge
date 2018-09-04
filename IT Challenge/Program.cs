using System.IO;
using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace IT_Challenge
{
    class Program
    {
        static void Main(string[] args)
        {
            // Adding data to Kindergardens List
            var linesList = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader("Data.txt", Encoding.GetEncoding(1257)))
                {
                    while (!sr.EndOfStream)
                    {
                        linesList.Add(sr.ReadLine());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("File couldn't be read:");
                Console.WriteLine(e.Message);
            }

            var columnNames = new List<string>();
            columnNames.AddRange(linesList[0].Split(';'));
            linesList.Remove(linesList[0]);

            List<Kindergarden> kindergardensList = new List<Kindergarden>();
            foreach (var line in linesList)
            {
                var lineObjects = line.Split(';');
                Kindergarden kindergarden = new Kindergarden
                {
                    KindId = int.Parse(lineObjects[columnNames.IndexOf("DARZ_ID")]),
                    SchoolName = lineObjects[columnNames.IndexOf("SCHOOL_NAME")],
                    TypeId = int.Parse(lineObjects[columnNames.IndexOf("TYPE_ID")]),
                    TypeLabel = lineObjects[columnNames.IndexOf("TYPE_LABEL")],
                    LanId = int.Parse(lineObjects[columnNames.IndexOf(("LAN_ID"))]),
                    LanLabel = lineObjects[columnNames.IndexOf("LAN_LABEL")],
                    ChildsCount = int.Parse(lineObjects[columnNames.IndexOf("CHILDS_COUNT")]),
                    FreeSpace = int.Parse(lineObjects[columnNames.IndexOf("FREE_SPACE")])
                };
                kindergardensList.Add(kindergarden);
            }

            // Finding the lowest and the highest
            var highest = 0;
            var lowest = 9999999;
            foreach (var item in kindergardensList)
            {
                if(item.ChildsCount > highest)
                {
                    highest = item.ChildsCount;
                }
                if(item.ChildsCount < lowest)
                {
                    lowest = item.ChildsCount;
                }
            }

            // Getting all lines with the lowest and highest CHILDS_COUNT value
            var highLowList = new List<Kindergarden>();
            foreach (var item in kindergardensList)
            {
                if(item.ChildsCount == highest || item.ChildsCount == lowest)
                {
                    highLowList.Add(item);
                }
            }

            // Forming lines from SCHOOL_NAME, TYPE_LABEL and LAN_LABEL
            var formedLinesList = new List<string>();
            foreach (var item in highLowList)
            {
                var schoolName = item.SchoolName.Substring(0, 3);
                var typeLabel = item.TypeLabel.Replace("iki", "-");
                typeLabel = typeLabel.Substring(4, typeLabel.Length - 9);
                typeLabel = Regex.Replace(typeLabel, @"\s+", "");
                var lanLabel = item.LanLabel.Substring(0, 4);
                var formedLine = String.Concat(schoolName, "_", typeLabel, "_", lanLabel);
                formedLinesList.Add(formedLine);
            }

            using (StreamWriter sw = new StreamWriter("FirstTask.txt"))
            {
                sw.WriteLine("Highest value: " + highest + " Lowest value: " + lowest);
                foreach (var line in formedLinesList)
                {
                    sw.WriteLine(line);
                }
            }

            // Finding how much kindergardens have free space as a percentage
            List<Kindergarden> freeSpaceList = new List<Kindergarden>();
            foreach (var item in kindergardensList)
            {
                foreach (var free in freeSpaceList)
                {
                    if(free.LanLabel == item.LanLabel)
                    {
                        free.ChildsCount += item.ChildsCount;
                        free.FreeSpace += item.FreeSpace;
                        goto CONTINUE;
                    }
                }

                var kindergarden = new Kindergarden
                {
                    LanLabel = item.LanLabel,
                    ChildsCount = item.ChildsCount,
                    FreeSpace = item.FreeSpace
                };
                freeSpaceList.Add(kindergarden);
                CONTINUE:;
            }

            using (StreamWriter sr = new StreamWriter("SecondTask.txt"))
            {
                sr.WriteLine("Kindergardens with free space:");
                foreach (var kindergarden in freeSpaceList)
                {
                    sr.WriteLine(kindergarden.LanLabel + " " + kindergarden.Percentage() + "%");
                }
            }

            // Select all kindergardens, which has 2 to 4 free spaces. Group them by name and sort them from Z to A.
            var kindergardenTwoToFourList = new List<Kindergarden>();
            foreach (var item in kindergardensList)
            {
                foreach (var kinder in kindergardenTwoToFourList)
                {
                    if(kinder.SchoolName == item.SchoolName)
                    {
                        kinder.FreeSpace += item.FreeSpace;
                        goto CONTINUE;
                    }
                }
                var kindergarden = new Kindergarden
                {
                    SchoolName = item.SchoolName,
                    FreeSpace = item.FreeSpace
                };
                kindergardenTwoToFourList.Add(kindergarden);
                CONTINUE:;
            }

            Predicate<Kindergarden> removeKindergarden = (Kindergarden p) => { return p.FreeSpace < 2 || p.FreeSpace > 4; };
            kindergardenTwoToFourList.RemoveAll(removeKindergarden);
            var sortedKindergardens = kindergardenTwoToFourList.OrderByDescending(person => person.SchoolName);

            using (StreamWriter sr = new StreamWriter("ThirdTask.txt"))
            {
                sr.WriteLine("Kindergardens which has 2 to 4 free spaces:");
                foreach (var item in kindergardenTwoToFourList)
                {
                    sr.WriteLine(item.SchoolName + " " + item.FreeSpace);
                }
            }

            Console.Read();
        }
    }
}
