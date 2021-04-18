using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO.Database
{
    [Alias("StorageHistory")]
    public class StorageHistoryPOCO
    {
        [AutoIncrement]
        public long Id { get; set; }
        public long ComponentId { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public int Delta { get; set; }
    }
}
