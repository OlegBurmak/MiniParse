using System;

namespace UkrNetParse.Items
{
    public class TopsItem
    {
        public int NewsId { get; set; }
        public double DateCreated { get; set; }

        public TopsItem[] News { get; set; } = null;
        public string Title { get; set; }
        public string Url { get; set; }

        public DateTime Time
        {
            get
            {
                return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(DateCreated).ToLocalTime();
            }
        }
        
    }
}