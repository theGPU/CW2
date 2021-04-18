using DatabaseProvider.DTO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.DTO.Interface
{
    public class CustomerOrderFullPOCO
    {
        public static string ShortcutCustomerName(string name)
        {
            var nameParts = name.Split(" ");
            if (nameParts.Length == 1)
                return name;
            else if (nameParts.Length == 2)
                return $"{nameParts[0]} {nameParts[1][0]}.";
            else //3+
                return $"{nameParts[0]} {nameParts[1][0]}. {nameParts[2][0]}.";
        }

        public static async Task<CustomerOrderFullPOCO> BuildCustomerOrderFullDTO(CustomerOrderPOCO order)
        {
            var result = new CustomerOrderFullPOCO();
            result.Id = order.Id;
            result.OrderType = order.OrderType;
            result.Job = await FullJobPOCO.BuildFullJobDTO(await IDatabaseController.DatabaseController.GetJob(order.LinkedJobId));
            result.OrderDetals = order.OrderDetals;
            result.OrderDate = order.OrderDate;
            result.Completed = order.Completed;
            result.Customer = await IDatabaseController.DatabaseController.GetCustomer(order.CustomerId);
            result.Customer.FullName = ShortcutCustomerName(result.Customer.FullName);
            return result;
        }

        public CustomerOrderFullPOCO(CustomerOrderPOCO order)
        {
            Id = order.Id;
            OrderType = order.OrderType;
            Job = new FullJobPOCO(IDatabaseController.DatabaseController.GetJob(order.LinkedJobId).Result);
            OrderDetals = order.OrderDetals;
            OrderDate = order.OrderDate;
            Completed = order.Completed;
            Customer = IDatabaseController.DatabaseController.GetCustomer(order.CustomerId).Result;
            Customer.FullName = ShortcutCustomerName(Customer.FullName);
        }
        public CustomerOrderFullPOCO() { }

        public long Id { get; set; }
        public CustomerPOCO Customer { get; set; }
        public string OrderType { get; set; }
        public string OrderDetals { get; set; }
        public FullJobPOCO Job { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Completed { get; set; }
    }
}
