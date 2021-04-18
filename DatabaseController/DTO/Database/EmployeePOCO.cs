using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO
{
    [Alias("Employees")]
    public class EmployeePOCO
    {
        [AutoIncrement]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string MiddleName { get; set; }
        [Required]
        public string Sex { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Ignore]
        public int Age { get; set; }
        [Required]
        public string Post { get; set; }
    }
}
