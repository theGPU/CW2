using DatabaseProvider.DTO;
using DatabaseProvider.DTO.Database;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider.Controllers
{
    public class MySQLController : IDatabaseController
    {
        public string DatabaseType => "MySQL";

        private OrmLiteConnectionFactory DbFactory;
        private IDbConnection Connection;
        private string ConnectionString;
        private bool isDebug = false;

        public async Task<bool> ConnectToDatabase(params string[] args) //server, database, login, password
        {
            if (args.Last() == "DebugEnabled")
                isDebug = true;

            ConnectionString = $"Server={args[0]};Database={args[1]};Uid={args[2]};Pwd={args[3]};";
            DbFactory = new OrmLiteConnectionFactory(ConnectionString, MySqlDialect.Provider);
            Connection = await DbFactory.OpenAsync();
            TryCreateDatabase();
            IDatabaseController.DatabaseController = this;
            return true;
        }

        private void TryCreateDatabase()
        {
            if (isDebug)
                Connection.DropAndCreateTables(typeof(ComponentPOCO), typeof(ComponentsOrderPOCO), typeof(EmployeePOCO), typeof(CustomerPOCO), typeof(JobPOCO), typeof(CustomerOrderPOCO), typeof(StoragePOCO), typeof(StorageHistoryPOCO));
            else
                Connection.CreateTableIfNotExists(typeof(ComponentPOCO), typeof(ComponentsOrderPOCO), typeof(EmployeePOCO), typeof(CustomerPOCO), typeof(JobPOCO), typeof(CustomerOrderPOCO), typeof(StoragePOCO), typeof(StorageHistoryPOCO));
        }



        public async Task<bool> AddEmployee(EmployeePOCO employee)
        {
            return await Connection.SaveAsync(employee);
        }
        public async Task<bool> DeleteEmployee(long id)
        {
            return await Connection.DeleteByIdAsync<EmployeePOCO>(id) == 1;
        }
        public async Task<bool> UpdateEmployee(EmployeePOCO employee)
        {
            return await Connection.UpdateAsync(employee) == 1;
        }
        public async Task<EmployeePOCO> GetEmployee(long id)
        {
            return await Connection.SingleByIdAsync<EmployeePOCO>(id);
        }
        public async Task<List<EmployeePOCO>> GetAllEmployees()
        {
            return await Connection.SelectAsync<EmployeePOCO>();
        }



        public async Task<bool> AddComponent(ComponentPOCO component)
        {
            return await Connection.SaveAsync(component);
        }
        public async Task<bool> DeleteComponent(long id)
        {
            return await Connection.DeleteByIdAsync<ComponentPOCO>(id) == 1;
        }
        public async Task<bool> UpdateComponent(ComponentPOCO component)
        {
            return await Connection.UpdateAsync(component) == 1;
        }
        public async Task<ComponentPOCO> GetComponent(long id)
        {
            return await Connection.SingleByIdAsync<ComponentPOCO>(id);
        }
        public async Task<List<ComponentPOCO>> GetAllComponents()
        {
            return await Connection.SelectAsync<ComponentPOCO>();
        }



        public async Task<bool> AddComponentOrder(ComponentsOrderPOCO order)
        {
            return await Connection.SaveAsync(order);
        }
        public async Task<bool> DeleteComponentOrder(long id)
        {
            return await Connection.DeleteByIdAsync<ComponentsOrderPOCO>(id) == 1;
        }
        public async Task<bool> UpdateComponentOrder(ComponentsOrderPOCO order)
        {
            return await Connection.UpdateAsync(order) == 1;
        }
        public async Task<ComponentsOrderPOCO> GetComponentOrder(long id)
        {
            return await Connection.SingleByIdAsync<ComponentsOrderPOCO>(id);
        }
        public async Task<List<ComponentsOrderPOCO>> GetAllComponentOrders()
        {
            return await Connection.SelectAsync<ComponentsOrderPOCO>();
        }



        public async Task<bool> AddCustomer(CustomerPOCO customer)
        {
            return await Connection.SaveAsync(customer);
        }
        public async Task<bool> DeleteCustomer(long id)
        {
            return await Connection.DeleteByIdAsync<CustomerPOCO>(id) == 1;
        }
        public async Task<bool> UpdateCustomer(CustomerPOCO customer)
        {
            return await Connection.UpdateAsync(customer) == 1;
        }
        public async Task<CustomerPOCO> GetCustomer(long id)
        {
            return await Connection.SingleByIdAsync<CustomerPOCO>(id);
        }
        public async Task<List<CustomerPOCO>> GetAllCustomers()
        {
            return await Connection.SelectAsync<CustomerPOCO>();
        }



        public async Task<bool> AddJob(JobPOCO job) 
        {
            return await Connection.SaveAsync(job);
        }
        public async Task<bool> DeleteJob(long id)
        {
            return await Connection.DeleteByIdAsync<JobPOCO>(id) == 1;
        }
        public async Task<bool> UpdateJob(JobPOCO job)
        {
            return await Connection.UpdateAsync(job) == 1;
        }
        public async Task<JobPOCO> GetJob(long id)
        {
            return await Connection.SingleByIdAsync<JobPOCO>(id);
        }
        public async Task<List<JobPOCO>> GetAllJobs()
        {
            return await Connection.SelectAsync<JobPOCO>();
        }



        public async Task<bool> AddCustomerOrder(CustomerOrderPOCO order)
        {
            return await Connection.SaveAsync(order);
        }
        public async Task<bool> DeleteCustomerOrder(long id)
        {
            return await Connection.DeleteByIdAsync<CustomerOrderPOCO>(id) == 1;
        }
        public async Task<bool> UpdateCustomerOrder(CustomerOrderPOCO order)
        {
            return await Connection.UpdateAsync(order) == 1;
        }
        public async Task<CustomerOrderPOCO> GetCustomerOrder(long id)
        {
            return await Connection.SingleByIdAsync<CustomerOrderPOCO>(id);
        }
        public async Task<List<CustomerOrderPOCO>> GetAllCustomerOrders()
        {
            return await Connection.SelectAsync<CustomerOrderPOCO>();
        }
        public async Task<List<CustomerOrderPOCO>> GetCustomerOrders(long id)
        {
            return await Connection.SelectAsync<CustomerOrderPOCO>(x => x.CustomerId == id);
        }



        public async Task<bool> AddComponentToStorage(StoragePOCO storage)
        {
            return await Connection.SaveAsync(storage);
        }
        public async Task<bool> DeleteComponentFromStorage(long id)
        {
            return await Connection.DeleteByIdAsync<StoragePOCO>(id) == 1;
        }
        public async Task<bool> UpdateComponentInStorage(StoragePOCO storage)
        {
            return await Connection.UpdateAsync(storage) == 1;
        }
        public async Task<StoragePOCO> GetComponentInStorage(long id)
        {
            return await Connection.SingleByIdAsync<StoragePOCO>(id);
        }
        public async Task<List<StoragePOCO>> GetAllComponentsInStorage()
        {
            return await Connection.SelectAsync<StoragePOCO>();
        }



        public async Task<bool> AddStorageHistory(StorageHistoryPOCO storageHistory)
        {
            return await Connection.SaveAsync(storageHistory);
        }
        public async Task<bool> DeleteStorageHistory(long id)
        {
            return await Connection.DeleteByIdAsync<StoragePOCO>(id) == 1;
        }
        public async Task<bool> UpdateStorageHistory(StorageHistoryPOCO storageHistory)
        {
            return await Connection.UpdateAsync(storageHistory) == 1;
        }
        public async Task<StorageHistoryPOCO> GetStorageHistory(long id)
        {
            return await Connection.SingleByIdAsync<StorageHistoryPOCO>(id);
        }
        public async Task<List<StorageHistoryPOCO>> GetAllStorageHistory()
        {
            return await Connection.SelectAsync<StorageHistoryPOCO>();
        }
    }
}
