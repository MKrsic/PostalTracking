using System;
using System.Collections.Generic;

namespace PostalTracking.DAL.Entities
{
    public partial class PackageTracking
    {
        public int Id { get; set; }
        public int? PackageId { get; set; }
        public int? StatusId { get; set; }
        public DateTime? StatusTime { get; set; }

        public Package Package { get; set; }
        public Status Status { get; set; }
    }
}
