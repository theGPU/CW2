using DatabaseProvider;
using DatabaseProvider.Controllers;
using DatabaseProvider.DTO;
using DatabaseProvider.DTO.Database;
using DatabaseProvider.DTO.Interface;
using ServiceStack.Logging;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseProviderTests
{
    class Program
    {
        static async Task Main(string[] args)
        {
            LogManager.LogFactory = new ConsoleLogFactory(debugEnabled: true);

            var employees = new List<EmployeePOCO>
            {
                new EmployeePOCO {Name = "Фатина", Surname = "Обломова", MiddleName = "Филипповна", Sex = "Famale", Birthday = DateTime.Today.AddYears(30), Post = "Сотрудник"},
                new EmployeePOCO {Name = "Раиса", Surname = "Тарасова", MiddleName = "Константиновна", Sex = "Famale", Birthday = DateTime.Today, Post = "Сотрудник"},
                new EmployeePOCO {Name = "Севастьян", Surname = "Рзаев", MiddleName = "Матвеевич", Sex = "Male", Birthday = DateTime.Today, Post = "Сотрудник"},
                new EmployeePOCO {Name = "Мирослава", Surname = "Сорокина", MiddleName = "Виталиевна", Sex = "Famale", Birthday = DateTime.Today, Post = "Сотрудник"},
                new EmployeePOCO {Name = "Радим", Surname = "Малышев", MiddleName = "Олегович", Sex = "Male", Birthday = DateTime.Today, Post = "Сотрудник"},
                new EmployeePOCO {Name = "Егор", Surname = "Шин", MiddleName = "Станиславович", Sex = "Male", Birthday = DateTime.Today, Post = "Сотрудник"},
                new EmployeePOCO {Name = "Афанасий", Surname = "Фролов", MiddleName = "Игоревич", Sex = "Male", Birthday = DateTime.Today, Post = "Сотрудник"},
                new EmployeePOCO {Name = "Любомир", Surname = "Астахов", MiddleName = "Валентинович", Sex = "Male", Birthday = DateTime.Today, Post = "Сотрудник"},
                new EmployeePOCO {Name = "Матвей", Surname = "Братиславский", MiddleName = "Ермакович", Sex = "Male", Birthday = DateTime.Today, Post = "Сотрудник"},
                new EmployeePOCO {Name = "Сильвия", Surname = "Кароль", MiddleName = "Алексеевна", Sex = "Famale", Birthday = DateTime.Today, Post = "Управляющий"},
            };

            var components = new List<ComponentPOCO>
            {
                new ComponentPOCO { ComponentType = "Тип компонента 1", ComponentName = "Название компонента 1"},
                new ComponentPOCO { ComponentType = "Тип компонента 2", ComponentName = "Название компонента 2"},
                new ComponentPOCO { ComponentType = "Тип компонента 3", ComponentName = "Название компонента 3"},
                new ComponentPOCO { ComponentType = "Тип компонента 4", ComponentName = "Название компонента 4"},
                new ComponentPOCO { ComponentType = "Тип компонента 5", ComponentName = "Название компонента 5"},
                new ComponentPOCO { ComponentType = "Тип компонента 6", ComponentName = "Название компонента 6"},
                new ComponentPOCO { ComponentType = "Тип компонента 7", ComponentName = "Название компонента 7"},
                new ComponentPOCO { ComponentType = "Тип компонента 8", ComponentName = "Название компонента 8"},
                new ComponentPOCO { ComponentType = "Тип компонента 9", ComponentName = "Название компонента 9"},
                new ComponentPOCO { ComponentType = "Тип компонента 10", ComponentName = "Название компонента 10"}
            };

            var rng = new Random();

            List<KeyValuePair<long, int>> GetComponentsForOrder(int count)
            {
                var usedIds = new List<int>();
                var result = new List<KeyValuePair<long, int>>();
                for (int i = 0; i < count; i++)
                {
                    var id = rng.Next(1, 10);
                    while (usedIds.Contains(id))
                        id = rng.Next(1, 10);

                    usedIds.Add(id);
                    result.Add(new KeyValuePair<long, int>(id, rng.Next(1, 10)));
                }

                return result;
            }

            var componenstOrders = new List<ComponentsOrderPOCO>
            {
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
                new ComponentsOrderPOCO {
                    Components = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))),
                    Price = rng.Next(1, 10000),
                    OrderDate = DateTime.Today,
                    Completed = false,
                    DateOfArrival = DateTime.Today.AddDays(rng.Next(1, 20))
                },
            };

            IDatabaseController databaseController = await GetMySQLController();
            //IDatabaseController databaseController = await GetLiteDBController();

            foreach (var employee in employees)
                await databaseController.AddEmployee(employee);

            foreach (var component in components)
            {
                await databaseController.AddComponent(component);
                await databaseController.AddComponentToStorage(new StoragePOCO { ComponentId = component.Id, Count = 1000 });
                for (var i = 10; i >= 0; i--)
                    await databaseController.AddStorageHistory(new StorageHistoryPOCO { ComponentId = component.Id, Date = DateTime.Now.AddDays(-i), Delta = rng.Next(1, 100) });
            }
            foreach (var order in componenstOrders)
                await databaseController.AddComponentOrder(order);

            var customers = new List<CustomerPOCO>
            {
                new CustomerPOCO { FullName = "Третьяков Ростислав Валерьевич", ContactInfo = "+7 (980) 979-74-49" },
                new CustomerPOCO { FullName = "Филиппов Олег Геннадиевич", ContactInfo = "+7 (940) 893-35-30" },
                new CustomerPOCO { FullName = "Фомов Светозар Ермакович", ContactInfo = "+7 (989) 321-14-41" },
                new CustomerPOCO { FullName = "Сафарова Тамара Львовна", ContactInfo = "+7 (945) 435-68-56" },
                new CustomerPOCO { FullName = "Кудрявцева Элеонора Степановна", ContactInfo = "+7 (996) 339-52-22" },
                new CustomerPOCO { FullName = "Маслов Дмитрий Романович", ContactInfo = "+7 (962) 837-78-85" },
                new CustomerPOCO { FullName = "Куликовская Зинаида Иосифовна", ContactInfo = "+7 (949) 701-35-87" },
                new CustomerPOCO { FullName = "Миллер Матвей Дмитриевич", ContactInfo = "+7 (943) 723-11-26" },
                new CustomerPOCO { FullName = "Кольцов Самсон Святославович", ContactInfo = "+7 (950) 131-12-26" },
                new CustomerPOCO { FullName = "Бровина Галина Закировна", ContactInfo = "+7 (987) 667-54-44" }
            };

            var jobs = new List<JobPOCO>
            {
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
                new JobPOCO { ComponentsUsed = new Dictionary<long, int>(GetComponentsForOrder(rng.Next(1, 4))), Complete = false, Description = "Работа на стадии..."},
            };

            var customerOrders = new List<CustomerOrderPOCO>
            {
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 1 },
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 2 },
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 3 },
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 4 },
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 5 },
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 6 },
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 7 },
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 8 },
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 9 },
                new CustomerOrderPOCO { OrderType = "Ремонт", OrderDetals = "Отремонтировать...", OrderDate = DateTime.Today, Completed = false, LinkedJobId = 10 }
            };

            foreach (var job in jobs)
                await databaseController.AddJob(job);

            for (int i = 0; i < customers.Count; i++)
            {
                CustomerPOCO customer = customers[i];
                await databaseController.AddCustomer(customer);
                var order = customerOrders[i];
                order.Id = 0; //prevent update
                order.CustomerId = customer.Id;
                await databaseController.AddCustomerOrder(order);
            }

            $"CustomerOrderFull for id 3: {new CustomerOrderFullPOCO(await databaseController.GetCustomerOrder(3)).Dump()}".Print();
        }

        private static async Task<IDatabaseController> GetMySQLController()
        {
            IDatabaseController databaseController = new MySQLController();
            await databaseController.ConnectToDatabase("web.ds-host.ru", "thecpu_workshop", "thecpu_workshop", "R8i0L6d8", "DebugEnabled");
            return databaseController;
        }

        /*
        private static async Task<IDatabaseController> GetLiteDBController()
        {
            IDatabaseController databaseController = new LiteDBController();
            await databaseController.ConnectToDatabase("LiteDB.db", "DebugEnabled");
            return databaseController;
        }
        */
    }
}
