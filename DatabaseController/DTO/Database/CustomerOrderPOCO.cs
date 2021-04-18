using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO
{
    [Alias("Orders")]
    public class CustomerOrderPOCO
    {
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        public long CustomerId { get; set; }
        [Required]
        public string OrderType { get; set; }
        [Required]
        public string OrderDetals { get; set; }
        public long LinkedJobId { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public bool Completed { get; set; }
    }
}
