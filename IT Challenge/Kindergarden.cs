using System;

namespace IT_Challenge
{
    public class Kindergarden
    {
        private double percentComplete;

        public int KindId { get; set; }
        public string SchoolName { get; set; }
        public int TypeId { get; set; }
        public string TypeLabel { get; set; }
        public int LanId { get; set; }
        public string LanLabel { get; set; }
        public int ChildsCount { get; set; }
        public int FreeSpace { get; set; }

        private float TotalSpace()
        {
            return ChildsCount + FreeSpace;
        }

        public double Percentage()
        {
            return percentComplete = Math.Round(FreeSpace * 100 / TotalSpace(), 2);
        }
    }
    
}
