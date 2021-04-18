using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO
{
    [Alias("Customers")]
    public class CustomerPOCO
    {
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string ContactInfo { get; set; }
    }
}
