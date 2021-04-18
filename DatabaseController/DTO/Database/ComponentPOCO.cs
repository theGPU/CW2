using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO
{
    [Alias("Components")]
    public class ComponentPOCO
    {
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        public string ComponentType { get; set; }
        [Required]
        public string ComponentName { get; set; }
    }
}
