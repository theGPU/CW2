using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO.Database
{
    [Alias("Jobs")]
    public class JobPOCO
    {
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        public Dictionary<long, int> ComponentsUsed { get; set; }
        public string Description { get; set; }
        [Required]
        public bool Complete { get; set; }
    }
}
