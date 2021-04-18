using DatabaseProvider.DTO;
using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS1998 // В асинхронном методе отсутствуют операторы await, будет выполнен синхронный метод
namespace DatabaseProvider.Controllers
{
    /*
    public class LiteDBController : IDatabaseController
    {
        public string DatabaseType => "LiteDB";

        private LiteDatabase Database;
        private ILiteCollection<ComponentsOrderDTO> ComponentsOrderCol;
        private ILiteCollection<EmployeeDTO> EmployeesCol;
        private ILiteCollection<CustomerDTO> CustomersCol;
        private ILiteCollection<CustomerOrderDTO> CustomerOrderCol;
        private bool isDebug = false;

        public async Task<bool> ConnectToDatabase(params string[] args) //filename
        {
            if (args.Last() == "DebugEnabled")
                isDebug = true;

            Database = new LiteDatabase(args[0]);
            TryCreateDatabase();
            IDatabaseController.DatabaseController = this;
            return true;
        }

        private void TryCreateDatabase()
        {
            if (isDebug)
            {
                Database.DropCollection("ComponentsOrders");
                Database.DropCollection("Employees");
                Database.DropCollection("Customers");
                Database.DropCollection("Orders");
            }

            ComponentsOrderCol = Database.GetCollection<ComponentsOrderDTO>("ComponentsOrders");
            EmployeesCol = Database.GetCollection<EmployeeDTO>("Employees");
            CustomersCol = Database.GetCollection<CustomerDTO>("Customers");
            CustomerOrderCol = Database.GetCollection<CustomerOrderDTO>("Orders");
        }



        public async Task<bool> AddEmployee(EmployeeDTO employee)
        {
            EmployeesCol.Insert(employee);
            return true;
        }
        public async Task<bool> DeleteEmployee(long id)
        {
            return EmployeesCol.Delete(id);
        }
        public async Task<bool> UpdateEmployee(EmployeeDTO employee)
        {
            return EmployeesCol.Update(employee);
        }
        public async Task<EmployeeDTO> GetEmployee(long id)
        {
            return EmployeesCol.FindById(id);
        }
        public async Task<List<EmployeeDTO>> GetAllEmployees()
        {
            return EmployeesCol.FindAll().ToList();
        }



        public async Task<bool> AddComponentOrder(ComponentsOrderDTO order)
        {
            ComponentsOrderCol.Insert(order);
            return true;
        }
        public async Task<bool> DeleteComponentOrder(long id)
        {
            return ComponentsOrderCol.Delete(id);
        }
        public async Task<bool> UpdateComponentOrder(ComponentsOrderDTO order)
        {
            return ComponentsOrderCol.Update(order);
        }
        public async Task<ComponentsOrderDTO> GetComponentOrder(long id)
        {
            return ComponentsOrderCol.FindById(id);
        }
        public async Task<List<ComponentsOrderDTO>> GetAllComponentOrders()
        {
            return ComponentsOrderCol.FindAll().ToList();
        }



        public async Task<bool> AddCustomer(CustomerDTO customer)
        {
            CustomersCol.Insert(customer);
            return true;
        }
        public async Task<bool> DeleteCustomer(long id)
        {
            return CustomersCol.Delete(id);
        }
        public async Task<bool> UpdateCustomer(CustomerDTO customer)
        {
            return CustomersCol.Update(customer);
        }
        public async Task<CustomerDTO> GetCustomer(long id)
        {
            return CustomersCol.FindById(id);
        }
        public async Task<List<CustomerDTO>> GetAllCustomers()
        {
            return CustomersCol.FindAll().ToList();
        }



        public async Task<bool> AddCustomerOrder(CustomerOrderDTO order)
        {
            CustomerOrderCol.Insert(order);
            return true;
        }
        public async Task<bool> DeleteCustomerOrder(long id)
        {
            return CustomerOrderCol.Delete(id);
        }
        public async Task<bool> UpdateCustomerOrder(CustomerOrderDTO order)
        {
            return CustomerOrderCol.Update(order);
        }
        public async Task<CustomerOrderDTO> GetCustomerOrder(long id)
        {
            return CustomerOrderCol.FindById(id);
        }
        public async Task<List<CustomerOrderDTO>> GetAllCustomerOrders()
        {
            return CustomerOrderCol.FindAll().ToList();
        }
        public async Task<List<CustomerOrderDTO>> GetCustomerOrders(long id)
        {
            return CustomerOrderCol.Find(x => x.CustomerId == id).ToList();
        }
    }
    */
}