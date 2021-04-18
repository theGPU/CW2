using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO.Database
{
    [Alias("Storage")]
    public class StoragePOCO
    {
        [Required]
        public long ComponentId { get; set; }
        [Required]
        public int Count { get; set; }
    }
}
