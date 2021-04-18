using DatabaseProvider.DTO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO.Interface
{
    public class FullJobPOCO
    {
        public static async Task<FullJobPOCO> BuildFullJobDTO(JobPOCO job)
        {
            var result = new FullJobPOCO();
            result.Id = job.Id;
            result.ComponentsUsed = new Dictionary<ComponentPOCO, int>();
            foreach (var component in job.ComponentsUsed)
                result.ComponentsUsed.Add(await IDatabaseController.DatabaseController.GetComponent(component.Key), component.Value);
            result.Description = job.Description;
            result.Complete = job.Complete;
            return result;
        }
        public FullJobPOCO(JobPOCO job)
        {
            Id = job.Id;
            ComponentsUsed = new Dictionary<ComponentPOCO, int>();
            foreach (var component in job.ComponentsUsed)
                ComponentsUsed.Add(IDatabaseController.DatabaseController.GetComponent(component.Key).Result, component.Value);
            Description = job.Description;
            Complete = job.Complete;
        }
        public FullJobPOCO() { }
        public long Id { get; set; }
        public Dictionary<ComponentPOCO, int> ComponentsUsed { get; set; }
        public string Description { get; set; }
        public bool Complete { get; set; }
    }
}
