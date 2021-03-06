﻿using System;
using System.Collections.Generic;

namespace PostalTracking.DAL.Entities
{
    public partial class Customer
    {
        public Customer()
        {
            PackageReceiver = new HashSet<Package>();
            PackageSender = new HashSet<Package>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Ponumber { get; set; }
        public DateTime? CreatedAt { get; set; }

        public ICollection<Package> PackageReceiver { get; set; }
        public ICollection<Package> PackageSender { get; set; }
    }
}
