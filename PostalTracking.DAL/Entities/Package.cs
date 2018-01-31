using System;
using System.Collections.Generic;

namespace PostalTracking.DAL.Entities
{
    public partial class Package
    {
        public Package()
        {
            PackageTracking = new HashSet<PackageTracking>();
        }

        public int Id { get; set; }
        public int? SenderId { get; set; }
        public int? ReceiverId { get; set; }

        public Customer Receiver { get; set; }
        public Customer Sender { get; set; }
        public ICollection<PackageTracking> PackageTracking { get; set; }
    }
}
