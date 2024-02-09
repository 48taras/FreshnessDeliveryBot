using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshnessDeliveryBot.Models
{
    public class Person
    {
        public long CustomerID { get; set; }

        public string FirstName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public ICollection<Order> Orders { get; set; }
    }
}
