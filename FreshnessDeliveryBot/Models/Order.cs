using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshnessDeliveryBot.Models
{
    public class Order
    {
        public int OrderID { get; set; }

        public string Status { get; set; }

        public DateTime OrderDate { get; set; }

        public Person person { get; set; }
        public long PersonId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
