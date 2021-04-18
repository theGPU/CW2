using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO
{
    [Alias("ComponentsOrders")]
    public class ComponentsOrderPOCO
    {
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        public Dictionary<long, int> Components { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public bool Completed { get; set; } = false;
        public DateTime DateOfArrival { get; set; }
    }
}
