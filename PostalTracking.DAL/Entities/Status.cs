using System;
using System.Collections.Generic;

namespace PostalTracking.DAL.Entities
{
    public partial class Status
    {
        public Status()
        {
            PackageTracking = new HashSet<PackageTracking>();
        }

        public int Id { get; set; }
        public string StatusDescription { get; set; }
        public bool Active { get; set; }

        public ICollection<PackageTracking> PackageTracking { get; set; }
    }
}
