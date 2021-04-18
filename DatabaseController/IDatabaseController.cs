using DatabaseProvider.DTO;
using DatabaseProvider.DTO.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseProvider
{
    public interface IDatabaseController
    {
        public static IDatabaseController DatabaseController;
        public string DatabaseType { get; }

        public Task<bool> ConnectToDatabase(params string[] args);

        public Task<bool> AddEmployee(EmployeePOCO employee);
        public Task<bool> DeleteEmployee(long id);
        public Task<bool> UpdateEmployee(EmployeePOCO employee);
        public Task<EmployeePOCO> GetEmployee(long id);
        public Task<List<EmployeePOCO>> GetAllEmployees();

        public Task<bool> AddComponent(ComponentPOCO component);
        public Task<bool> DeleteComponent(long id);
        public Task<bool> UpdateComponent(ComponentPOCO component);
        public Task<ComponentPOCO> GetComponent(long id);
        public Task<List<ComponentPOCO>> GetAllComponents();

        public Task<bool> AddComponentOrder(ComponentsOrderPOCO order);
        public Task<bool> DeleteComponentOrder(long id);
        public Task<bool> UpdateComponentOrder(ComponentsOrderPOCO order);
        public Task<ComponentsOrderPOCO> GetComponentOrder(long id);
        public Task<List<ComponentsOrderPOCO>> GetAllComponentOrders();

        public Task<bool> AddCustomer(CustomerPOCO customer);
        public Task<bool> DeleteCustomer(long id);
        public Task<bool> UpdateCustomer(CustomerPOCO customer);
        public Task<CustomerPOCO> GetCustomer(long id);
        public Task<List<CustomerPOCO>> GetAllCustomers();

        public Task<bool> AddJob(JobPOCO job);
        public Task<bool> DeleteJob(long id);
        public Task<bool> UpdateJob(JobPOCO job);
        public Task<JobPOCO> GetJob(long id);
        public Task<List<JobPOCO>> GetAllJobs();

        public Task<bool> AddCustomerOrder(CustomerOrderPOCO order);
        public Task<bool> DeleteCustomerOrder(long id);
        public Task<bool> UpdateCustomerOrder(CustomerOrderPOCO order);
        public Task<CustomerOrderPOCO> GetCustomerOrder(long id);
        public Task<List<CustomerOrderPOCO>> GetAllCustomerOrders();
        public Task<List<CustomerOrderPOCO>> GetCustomerOrders(long id);

        public Task<bool> AddComponentToStorage(StoragePOCO storage);
        public Task<bool> DeleteComponentFromStorage(long id);
        public Task<bool> UpdateComponentInStorage(StoragePOCO storage);
        public Task<StoragePOCO> GetComponentInStorage(long id);
        public Task<List<StoragePOCO>> GetAllComponentsInStorage();

        public Task<bool> AddStorageHistory(StorageHistoryPOCO storageHistory);
        public Task<bool> DeleteStorageHistory(long id);
        public Task<bool> UpdateStorageHistory(StorageHistoryPOCO storageHistory);
        public Task<StorageHistoryPOCO> GetStorageHistory(long id);
        public Task<List<StorageHistoryPOCO>> GetAllStorageHistory();
    }
}
