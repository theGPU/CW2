using DatabaseProvider.DTO;
using DatabaseProvider.DTO.Database;
using DatabaseProvider.DTO.Interface;
using ServiceStack.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WorkshopClient
{
    public partial class MainWindow : Window
    {
        public static List<CustomerOrderFullPOCO> AllOrders;
        public static List<FullJobPOCO> AllJobs;
        public static List<CustomerPOCO> AllCustomers;
        public static List<ComponentPOCO> AllComponents;
        public static List<ComponentsOrderPOCO> AllComponentsOrders;
        public static List<FullStoragePOCO> AllStorage;

        private bool OrdersFilterCustomerIdValid = false;
        private long OrdersFilterCustomerIdNumber = -1;

        [DllImport("Kernel32")]
        public static extern void AllocConsole();

        [DllImport("Kernel32")]
        public static extern void FreeConsole();

        public MainWindow()
        {
            AllocConsole();
            LogManager.LogFactory = new ConsoleLogFactory(debugEnabled: true);
            DatabaseProvider.Init("MySQL", new string[] { "localhost", "workshop", "root", "TestPassword" });
            InitializeComponent();
            OrderDetalsGrid.Visibility = Visibility.Hidden;
            JobDetalsGrid.Visibility = Visibility.Hidden;
            CustomerDetalsGrid.Visibility = Visibility.Hidden;
            ComponentDetalsGrid.Visibility = Visibility.Hidden;
            StorageDetalsGrid.Visibility = Visibility.Hidden;
            ComponentsOrdersDetalsGrid.Visibility = Visibility.Hidden;
            FillCustomerOrders();
            FillJobs();
            FillCustomers();
            FillComponents();
            FillStorage();
            FillComponentsOrders();
        }

        #region customerOrders
        private async void FillCustomerOrders()
        {
            AllOrders = (await Task.WhenAll((await DatabaseProvider.Database.GetAllCustomerOrders()).Select(async x => await CustomerOrderFullPOCO.BuildCustomerOrderFullDTO(x)))).ToList();

            var _itemSourceList = new CollectionViewSource() { Source = AllOrders };
            ICollectionView orderDatagridItemList = _itemSourceList.View;
            orderDatagridItemList.Filter = CustomerOrdersDatagridFilter;
            CustomerOrdersDatagrid.ItemsSource = orderDatagridItemList;
        }

        private void OrdersFilterWaitingComplete_Checked(object sender, RoutedEventArgs e)
        {
            OrdersFilterCompleted.IsChecked = false;
            RefreshOrdersDataGrid();
        }

        private void OrdersFilterWaitingComplete_Unchecked(object sender, RoutedEventArgs e) => RefreshOrdersDataGrid();

        private void OrdersFilterCompleted_Checked(object sender, RoutedEventArgs e)
        {
            OrdersFilterWaitingComplete.IsChecked = false;
            RefreshOrdersDataGrid();
        }

        private void OrdersFilterCompleted_Unchecked(object sender, RoutedEventArgs e) => RefreshOrdersDataGrid();

        public void RefreshOrdersDataGrid() => ((ICollectionView)CustomerOrdersDatagrid.ItemsSource).Refresh();

        private void OrdersFilterCustomerId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var OrdersFilterCustomerIdText = OrdersFilterCustomerId.Text;
                OrdersFilterCustomerIdValid = OrdersFilterCustomerIdText.Length > 0 && long.TryParse(OrdersFilterCustomerIdText, out OrdersFilterCustomerIdNumber);
                RefreshOrdersDataGrid();
            }
        }

        private bool CustomerOrdersDatagridFilter(object entry)
        {
            var item = (CustomerOrderFullPOCO)entry;
            return ((!OrdersFilterWaitingComplete.IsChecked.Value && !OrdersFilterCompleted.IsChecked.Value) || 
                (OrdersFilterWaitingComplete.IsChecked.Value && !item.Completed || OrdersFilterCompleted.IsChecked.Value && item.Completed)) && 
                (!OrdersFilterCustomerIdValid || OrdersFilterCustomerIdValid && item.Customer.Id == OrdersFilterCustomerIdNumber);
        }

        public void CustomerOrdersDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomerOrdersDatagrid.SelectedItem != null)
            {
                OrderDetalsGrid.Visibility = Visibility.Visible;
                PrepareMenuForCustomerOrdersItem((CustomerOrderFullPOCO)CustomerOrdersDatagrid.SelectedItem);
            } else
            {
                OrderDetalsGrid.Visibility = Visibility.Hidden;
            }
        }

        private void PrepareMenuForCustomerOrdersItem(CustomerOrderFullPOCO item)
        {
            Order_OrderNumberLabel.Content = $"Заказ #{item.Id}";
            Order_ClientIdLabel.Content = $"Id клиента: {item.Customer.Id}";
            Order_JobIdLabel.Content = $"Id работы: {item.Job.Id}";
            Order_OrderComplete.IsChecked = item.Completed;
            Order_OrderComplete.IsEnabled = !item.Completed;
            Order_OrderDateLabel.Content = item.OrderDate.ToString("g");
            Order_OrderDescriptionTextBox.Text = item.OrderDetals;
            Order_OrderTypeTextBox.Text = item.OrderType;
        }

        private async void Order_OrderComplete_Click(object sender, RoutedEventArgs e)
        {
            var checbox = (CheckBox)sender;
            var itemEntry = (CustomerOrderFullPOCO)CustomerOrdersDatagrid.SelectedItem;
            if (checbox.IsChecked.Value)
            {
                checbox.IsEnabled = false;
                var orderEntry = await DatabaseProvider.Database.GetCustomerOrder(itemEntry.Id);
                orderEntry.Completed = true;
                await DatabaseProvider.Database.UpdateCustomerOrder(orderEntry);
                if (!itemEntry.Job.Complete)
                {
                    var jobEntry = await DatabaseProvider.Database.GetJob(itemEntry.Job.Id);
                    jobEntry.Complete = true;
                    await DatabaseProvider.Database.UpdateJob(jobEntry);
                }
                var entry = AllOrders.Find(x => x.Id == itemEntry.Id);
                entry.Job.Complete = true;
                entry.Completed = true;
                RefreshOrdersDataGrid();
            }
        }

        private async void Order_OrderTypeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var itemEntry = (CustomerOrderFullPOCO)CustomerOrdersDatagrid.SelectedItem;
                var orderEntry = await DatabaseProvider.Database.GetCustomerOrder(itemEntry.Id);
                orderEntry.OrderType = Order_OrderTypeTextBox.Text;
                await DatabaseProvider.Database.UpdateCustomerOrder(orderEntry);
                var entry = AllOrders.Find(x => x.Id == itemEntry.Id);
                entry.OrderType = Order_OrderTypeTextBox.Text;
                RefreshOrdersDataGrid();
            }
        }

        private async void Order_OrderDescriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var itemEntry = (CustomerOrderFullPOCO)CustomerOrdersDatagrid.SelectedItem;
                var orderEntry = await DatabaseProvider.Database.GetCustomerOrder(itemEntry.Id);
                orderEntry.OrderDetals = Order_OrderDescriptionTextBox.Text;
                await DatabaseProvider.Database.UpdateCustomerOrder(orderEntry);
                var entry = AllOrders.Find(x => x.Id == itemEntry.Id);
                entry.OrderDetals = Order_OrderDescriptionTextBox.Text;
                RefreshOrdersDataGrid();
            }
        }

        private void Order_ToJobButton_Click(object sender, RoutedEventArgs e)
        {
            var itemEntry = (CustomerOrderFullPOCO)CustomerOrdersDatagrid.SelectedItem;
            foreach (var item in JobsDataGrid.Items)
            {
                if (((FullJobPOCO)item).Id == itemEntry.Job.Id)
                {
                    JobsDataGrid.SelectedItem = item;
                    JobsDataGrid.ScrollIntoView(item);
                    JobsDataGrid.Focus();
                    break;
                }
            }
            JobsFilterCompleted.IsChecked = false;
            JobsFilterWaitingComplete.IsChecked = false;
            JobsTab.IsSelected = true;
            JobsDataGrid_SelectionChanged(null, null);
        }

        private void Order_ToCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            var itemEntry = (CustomerOrderFullPOCO)CustomerOrdersDatagrid.SelectedItem;
            foreach (var item in CustomersDatagrid.Items)
            {
                if (((CustomerPOCO)item).Id == itemEntry.Customer.Id)
                {
                    CustomersDatagrid.SelectedItem = item;
                    CustomersDatagrid.ScrollIntoView(item);
                    CustomersDatagrid.Focus();
                    break;
                }
            }
            ClientsTab.IsSelected = true;
            CustomersDatagrid_SelectionChanged(null, null);
        }

        private void Orders_CreateNewOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var createWindow = new CreateCustomerOrderWindow();
            createWindow.Owner = this;
            createWindow.ShowDialog();
        }
        #endregion customerOrders

        #region jobs
        private async void FillJobs()
        {
            AllJobs = (await Task.WhenAll((await DatabaseProvider.Database.GetAllJobs()).Select(async x => await FullJobPOCO.BuildFullJobDTO(x)))).ToList();
            var _itemSourceList = new CollectionViewSource() { Source = AllJobs };
            ICollectionView jobsDatagridItemList = _itemSourceList.View;
            jobsDatagridItemList.Filter = JobsDataGridFilter;
            JobsDataGrid.ItemsSource = jobsDatagridItemList;
        }

        private bool JobsDataGridFilter(object entry)
        {
            var item = (FullJobPOCO)entry;
            return ((!JobsFilterCompleted.IsChecked.Value && !JobsFilterWaitingComplete.IsChecked.Value) ||
                (JobsFilterCompleted.IsChecked.Value && item.Complete || JobsFilterWaitingComplete.IsChecked.Value && !item.Complete));
        }

        private void JobsFilterWaitingComplete_Checked(object sender, RoutedEventArgs e)
        {
            JobsFilterCompleted.IsChecked = false;
            RefreshJobsDataGrid();
        }
        private void JobsFilterWaitingComplete_Unchecked(object sender, RoutedEventArgs e) => RefreshJobsDataGrid();
        private void JobsFilterCompleted_Checked(object sender, RoutedEventArgs e)
        {
            JobsFilterWaitingComplete.IsChecked = false;
            RefreshJobsDataGrid();
        }
        private void JobsFilterCompleted_Unchecked(object sender, RoutedEventArgs e) => RefreshJobsDataGrid();
        public void RefreshJobsDataGrid() => ((ICollectionView)JobsDataGrid.ItemsSource).Refresh();

        public void JobsDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (JobsDataGrid.SelectedItem != null)
            {
                JobDetalsGrid.Visibility = Visibility.Visible;
                var selectedItem = (FullJobPOCO)JobsDataGrid.SelectedItem;
                Job_JobNumberLabel.Content = $"Работа #{selectedItem.Id}";
                Job_JobCompleteCheckBox.IsChecked = selectedItem.Complete;
                Job_JobCompleteCheckBox.IsEnabled = !selectedItem.Complete;

                var usedComponentsSB = new StringBuilder();
                foreach (var component in selectedItem.ComponentsUsed)
                    usedComponentsSB.AppendLine($"{component.Key.ComponentName}: {component.Value}");
                Job_JobComponentsUsedTextBlock.Text = usedComponentsSB.ToString();
                Job_JobDescriptionTextBox.Text = selectedItem.Description;
            } else
            {
                JobDetalsGrid.Visibility = Visibility.Collapsed;
            }
        }

        private async void Job_JobCompleteCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (Job_JobCompleteCheckBox.IsChecked == true)
            {
                Job_JobCompleteCheckBox.IsEnabled = false;
                var itemEntry = (FullJobPOCO)JobsDataGrid.SelectedItem;
                var jobEntry = await DatabaseProvider.Database.GetJob(itemEntry.Id);
                jobEntry.Complete = true;
                await DatabaseProvider.Database.UpdateJob(jobEntry);
                var entry = AllJobs.Find(x => x.Id == itemEntry.Id);
                entry.Complete = true;
                RefreshJobsDataGrid();

                AllOrders.ForEach(x => { if (x.Job.Id == itemEntry.Id) x.Job.Complete = true; });
                RefreshOrdersDataGrid();
            }
        }

        private async void Job_JobDescriptionTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var itemEntry = (FullJobPOCO)JobsDataGrid.SelectedItem;
                var jobEntry = await DatabaseProvider.Database.GetJob(itemEntry.Id);
                jobEntry.Description = Job_JobDescriptionTextBox.Text;
                await DatabaseProvider.Database.UpdateJob(jobEntry);
                var entry = AllJobs.Find(x => x.Id == itemEntry.Id);
                entry.Description = Job_JobDescriptionTextBox.Text;
                RefreshJobsDataGrid();
            }
        }

        private void Job_ChangeComponentsUsage_Click(object sender, RoutedEventArgs e)
        {
            var componentsChangeWindow = new JobComponentsChangeWindow((FullJobPOCO)JobsDataGrid.SelectedItem);
            componentsChangeWindow.Owner = this;
            componentsChangeWindow.ShowDialog();
        }
        #endregion jobs

        #region customers
        private async void FillCustomers()
        {
            AllCustomers = await DatabaseProvider.Database.GetAllCustomers();
            var _itemSourceList = new CollectionViewSource() { Source = AllCustomers };
            ICollectionView customersDatagridItemList = _itemSourceList.View;
            customersDatagridItemList.Filter = CustomersDataGridFilter;
            CustomersDatagrid.ItemsSource = customersDatagridItemList;
        }

        private bool CustomersDataGridFilter(object entry)
        {
            //var item = (FullJobDTO)entry;
            return true;
        }

        public void CustomersDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (CustomersDatagrid.SelectedItem != null)
            {
                var selectedItem = (CustomerPOCO)CustomersDatagrid.SelectedItem;
                CustomerDetalsGrid.Visibility = Visibility.Visible;
                Customers_CustomerFullNameTextBox.Text = selectedItem.FullName;
                Customers_CustomerContactInfoTextBox.Text = selectedItem.ContactInfo;
            } else
            {
                CustomerDetalsGrid.Visibility = Visibility.Hidden;
            }
        }

        private async void Customers_CustomerFullNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var itemEntry = (CustomerPOCO)CustomersDatagrid.SelectedItem;
                var customerEntry = await DatabaseProvider.Database.GetCustomer(itemEntry.Id);
                customerEntry.FullName = Customers_CustomerFullNameTextBox.Text;
                await DatabaseProvider.Database.UpdateCustomer(customerEntry);
                var entry = AllCustomers.Find(x => x.Id == itemEntry.Id);
                entry.FullName = Customers_CustomerFullNameTextBox.Text;

                var customerId = entry.Id;
                var shortcutName = CustomerOrderFullPOCO.ShortcutCustomerName(entry.FullName);
                AllOrders.Where(x => x.Customer.Id == customerId).ToList().ForEach(x => x.Customer.FullName = shortcutName);

                RefreshCustomersDataGrid();
                RefreshOrdersDataGrid();
            }
        }

        private async void Customers_CustomerContactInfoTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var itemEntry = (CustomerPOCO)CustomersDatagrid.SelectedItem;
                var customerEntry = await DatabaseProvider.Database.GetCustomer(itemEntry.Id);
                customerEntry.ContactInfo = Customers_CustomerContactInfoTextBox.Text;
                await DatabaseProvider.Database.UpdateCustomer(customerEntry);
                var entry = AllCustomers.Find(x => x.Id == itemEntry.Id);
                entry.ContactInfo = Customers_CustomerContactInfoTextBox.Text;
                RefreshCustomersDataGrid();
            }
        }

        private void Customers_CustomerOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            var itemEntry = (CustomerPOCO)CustomersDatagrid.SelectedItem;
            OrdersFilterCustomerId.Text = itemEntry.Id.ToString();
            OrdersFilterCustomerIdValid = true;
            OrdersFilterCustomerIdNumber = itemEntry.Id;
            OrdersTab.IsSelected = true;
            RefreshOrdersDataGrid();
        }

        private async void Customer_AddCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            var newCustomer = new CustomerPOCO
            {
                FullName = "Полное имя",
                ContactInfo = "Контакты"
            };
            await DatabaseProvider.Database.AddCustomer(newCustomer);
            AllCustomers.Add(newCustomer);

            RefreshCustomersDataGrid();
        }

        private void RefreshCustomersDataGrid() => ((ICollectionView)CustomersDatagrid.ItemsSource).Refresh();
        #endregion customers

        #region components
        private async void FillComponents()
        {
            AllComponents = await DatabaseProvider.Database.GetAllComponents();
            var _itemSourceList = new CollectionViewSource() { Source = AllComponents };
            ICollectionView componentsDatagridItemList = _itemSourceList.View;
            componentsDatagridItemList.Filter = ComponentsDataGridFilter;
            ComponentsDatagrid.ItemsSource = componentsDatagridItemList;
        }

        private bool ComponentsDataGridFilter(object entry)
        {
            //var item = (FullJobDTO)entry;
            return true;
        }

        private async void Components_ComponentAddButton_Click(object sender, RoutedEventArgs e)
        {
            var newComponent = new ComponentPOCO()
            {
                ComponentName = "ComponentName",
                ComponentType = "ComponentType"
            };
            await DatabaseProvider.Database.AddComponent(newComponent);
            AllComponents.Add(newComponent);
            var newStorage = new StoragePOCO()
            {
                ComponentId = newComponent.Id,
                Count = 0
            };
            await DatabaseProvider.Database.AddComponentToStorage(newStorage);

            var storage = await FullStoragePOCO.BuildFullStorageDTO(newStorage);
            AllStorage.Add(storage);

            RefreshStorageDataGrid();
            RefreshComponentsDataGrid();
        }

        private void RefreshComponentsDataGrid() => ((ICollectionView)ComponentsDatagrid.ItemsSource).Refresh();

        private void ComponentsDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComponentsDatagrid.SelectedItem != null)
            {
                ComponentDetalsGrid.Visibility = Visibility.Visible;
                var itemEntry = (ComponentPOCO)ComponentsDatagrid.SelectedItem;
                Components_ComponentTypeTextBox.Text = itemEntry.ComponentType;
                Components_ComponentNameTextBox.Text = itemEntry.ComponentName;
            } else
            {
                ComponentDetalsGrid.Visibility = Visibility.Hidden;
            }
        }

        private async void Components_ComponentTypeTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var itemEntry = (ComponentPOCO)ComponentsDatagrid.SelectedItem;
                var componentEntry = await DatabaseProvider.Database.GetComponent(itemEntry.Id);
                componentEntry.ComponentType = Components_ComponentTypeTextBox.Text;
                await DatabaseProvider.Database.UpdateComponent(componentEntry);
                var entry = AllComponents.Find(x => x.Id == itemEntry.Id);
                entry.ComponentType = Components_ComponentTypeTextBox.Text;
                RefreshComponentsDataGrid();

                var entryStorage = AllStorage.Find(x => x.Component.Id == itemEntry.Id);
                entryStorage.Component.ComponentType = Components_ComponentTypeTextBox.Text;
                RefreshStorageDataGrid();
                StorageDatagrid_SelectionChanged(null, null);
            }
        }

        private async void Components_ComponentNameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var itemEntry = (ComponentPOCO)ComponentsDatagrid.SelectedItem;
                var orderEntry = await DatabaseProvider.Database.GetComponent(itemEntry.Id);
                orderEntry.ComponentName = Components_ComponentNameTextBox.Text;
                await DatabaseProvider.Database.UpdateComponent(orderEntry);
                var entry = AllComponents.Find(x => x.Id == itemEntry.Id);
                entry.ComponentName = Components_ComponentNameTextBox.Text;
                RefreshComponentsDataGrid();

                var entryStorage = AllStorage.Find(x => x.Component.Id == itemEntry.Id);
                entryStorage.Component.ComponentName = Components_ComponentNameTextBox.Text;
                RefreshStorageDataGrid();
                StorageDatagrid_SelectionChanged(null, null);
            }
        }
        #endregion components

        #region componentsOrders
        private async void FillComponentsOrders()
        {
            AllComponentsOrders = await DatabaseProvider.Database.GetAllComponentOrders();
            var _itemSourceList = new CollectionViewSource() { Source = AllComponentsOrders };
            ICollectionView componentsOrdersDatagridItemList = _itemSourceList.View;
            componentsOrdersDatagridItemList.Filter = StorageDataGridFilter;
            ComponentsOrdersDatagrid.ItemsSource = componentsOrdersDatagridItemList;
        }

        public void ComponentsOrdersDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComponentsOrdersDatagrid.SelectedItem != null)
            {
                var selectedItem = (ComponentsOrderPOCO)ComponentsOrdersDatagrid.SelectedItem;
                ComponentsOrdersDetalsGrid.Visibility = Visibility.Visible;
                ComponentsOrders_OrderLabel.Content = $"Заказ #{selectedItem.Id}";
                ComponentsOrders_CompletedCheckBox.IsChecked = selectedItem.Completed;
                ComponentsOrders_CompletedCheckBox.IsEnabled = !selectedItem.Completed;

                var idsStringBuilder = new StringBuilder();
                foreach (var component in selectedItem.Components)
                    idsStringBuilder.AppendLine($"{AllComponents.First(x => x.Id == component.Key).ComponentName}: {component.Value}");
                ComponentsOrders_ComponentsIdsTextBlock.Text = idsStringBuilder.ToString();
            }
            else
            {
                CustomerDetalsGrid.Visibility = Visibility.Hidden;
            }
        }

        private async void ComponentsOrders_CompletedCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (ComponentsOrders_CompletedCheckBox.IsChecked.Value)
            {
                ComponentsOrders_CompletedCheckBox.IsEnabled = false;
                var selectedItem = (ComponentsOrderPOCO)ComponentsOrdersDatagrid.SelectedItem;
                selectedItem.Completed = true;
                await DatabaseProvider.Database.UpdateComponentOrder(selectedItem);
                RefreshComponentsOrdersDataGrid();

                foreach (var component in selectedItem.Components)
                {
                    var storage = await DatabaseProvider.Database.GetComponentInStorage(component.Key);
                    storage.Count += component.Value;
                    await DatabaseProvider.Database.UpdateComponentInStorage(storage);
                    var storageInView = AllStorage.First(x => x.Component.Id == component.Key);
                    storageInView.Count += component.Value;
                    RefreshStorageDataGrid();
                    StorageDatagrid_SelectionChanged(null, null);
                }
            }
        }

        public void RefreshComponentsOrdersDataGrid() => ((ICollectionView)ComponentsOrdersDatagrid.ItemsSource).Refresh();

        private void ComponentsOrders_CreateNewButton_Click(object sender, RoutedEventArgs e)
        {
            var creatorWindow = new CreateComponentsOrderWindow();
            creatorWindow.Owner = this;
            creatorWindow.ShowDialog();
        }
        #endregion componentsOrders

        #region storage
        private async void FillStorage()
        {
            AllStorage = (await Task.WhenAll((await DatabaseProvider.Database.GetAllComponentsInStorage()).Select(async x => await FullStoragePOCO.BuildFullStorageDTO(x)))).ToList();
            var _itemSourceList = new CollectionViewSource() { Source = AllStorage };
            ICollectionView storageDatagridItemList = _itemSourceList.View;
            storageDatagridItemList.Filter = StorageDataGridFilter;
            StorageDatagrid.ItemsSource = storageDatagridItemList;
        }

        private bool StorageDataGridFilter(object entry)
        {
            return true;
        }

        public void StorageDatagrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (StorageDatagrid.SelectedItem != null)
            {
                StorageDetalsGrid.Visibility = Visibility.Visible;
                var itemEntry = (FullStoragePOCO)StorageDatagrid.SelectedItem;
                Storage_StorageComponentLabel.Content = $"Компонент #{itemEntry.Component.Id}";
                Storage_StorageCountTextBox.Text = itemEntry.Count.ToString();
            }
            else
            {
                StorageDatagrid.Visibility = Visibility.Hidden;
            }
        }

        private async void Storage_StorageCountTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (int.TryParse(Storage_StorageCountTextBox.Text, out var count))
                {
                    var itemEntry = (FullStoragePOCO)StorageDatagrid.SelectedItem;
                    var storageEntry = await DatabaseProvider.Database.GetComponentInStorage(itemEntry.Component.Id);
                    storageEntry.Count = count;
                    await DatabaseProvider.Database.UpdateComponentInStorage(storageEntry);
                    var entry = AllStorage.Find(x => x.Component.Id == itemEntry.Component.Id);
                    entry.Count = count;
                    RefreshStorageDataGrid();
                }
            }
        }

        private void Storage_ToComponentButton_Click(object sender, RoutedEventArgs e)
        {
            var itemEntry = (FullStoragePOCO)StorageDatagrid.SelectedItem;
            foreach (var item in ComponentsDatagrid.Items)
            {
                if (((ComponentPOCO)item).Id == itemEntry.Component.Id)
                {
                    ComponentsDatagrid.SelectedItem = item;
                    ComponentsDatagrid.ScrollIntoView(item);
                    CustomersDatagrid.Focus();
                    break;
                }
            }
            ComponentsTab.IsSelected = true;
            ComponentsDatagrid_SelectionChanged(null, null);
        }

        public void RefreshStorageDataGrid() => ((ICollectionView)StorageDatagrid.ItemsSource).Refresh();
        #endregion storage
    }
}
