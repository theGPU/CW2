using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO.Interface
{
    class CustomerFullPOCO
    {
        public CustomerFullPOCO(CustomerPOCO customer)
        {
            Id = customer.Id;
            FullName = customer.FullName;
            PhoneNumber = customer.ContactInfo;
            CustomerOrders = IDatabaseController.DatabaseController.GetCustomerOrders(Id).Result;
        }
        public CustomerFullPOCO() { }

        public long Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public List<CustomerOrderPOCO> CustomerOrders { get; set; }
    }
}
