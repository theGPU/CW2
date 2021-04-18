using DatabaseProvider.DTO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO.Interface
{
    public class FullStoragePOCO
    {
        public static async Task<FullStoragePOCO> BuildFullStorageDTO(StoragePOCO storage)
        {
            var result = new FullStoragePOCO
            {
                Component = await IDatabaseController.DatabaseController.GetComponent(storage.ComponentId),
                Count = storage.Count
            };
            return result;
        }
        public FullStoragePOCO(StoragePOCO storage)
        {
            Component = IDatabaseController.DatabaseController.GetComponent(storage.ComponentId).Result;
            Count = storage.Count;
        }
        public FullStoragePOCO() { }
        public ComponentPOCO Component { get; set; }
        public int Count { get; set; }
    }
}
