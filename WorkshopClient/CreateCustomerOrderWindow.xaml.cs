using DatabaseProvider.DTO;
using DatabaseProvider.DTO.Database;
using DatabaseProvider.DTO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WorkshopClient
{
    /// <summary>
    /// Логика взаимодействия для CreateCustomerOrderWindow.xaml
    /// </summary>
    public partial class CreateCustomerOrderWindow : Window
    {
        public CreateCustomerOrderWindow()
        {
            InitializeComponent();
            OrderDatePicker.SelectedDate = DateTime.Today;

            foreach (var customer in MainWindow.AllCustomers)
                CustomerComboBox.Items.Add($"{customer.Id}: {customer.FullName}");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerComboBox.SelectedItem != null)
            {
                var newJob = new JobPOCO
                {
                    Complete = false,
                    ComponentsUsed = new Dictionary<long, int>(),
                    Description = ""
                };
                await DatabaseProvider.Database.AddJob(newJob);

                var newCustomerOrder = new CustomerOrderPOCO
                {
                    LinkedJobId = newJob.Id,
                    CustomerId = MainWindow.AllCustomers.Find(x => $"{x.Id}: {x.FullName}" == (string)CustomerComboBox.SelectedItem).Id,
                    OrderDate = OrderDatePicker.SelectedDate.Value,
                    OrderType = OrderTypeTextBox.Text,
                    OrderDetals = OrderDescriptionTextBox.Text,
                    Completed = false
                };
                await DatabaseProvider.Database.AddCustomerOrder(newCustomerOrder);
                newJob.Description = $"Относится к заказу #{newCustomerOrder.Id}";
                await DatabaseProvider.Database.UpdateJob(newJob);

                MainWindow.AllJobs.Add(await FullJobPOCO.BuildFullJobDTO(newJob));
                MainWindow.AllOrders.Add(await CustomerOrderFullPOCO.BuildCustomerOrderFullDTO(newCustomerOrder));

                ((MainWindow)this.Owner).RefreshJobsDataGrid();
                ((MainWindow)this.Owner).JobsDataGrid_SelectionChanged(null, null);
                ((MainWindow)this.Owner).RefreshOrdersDataGrid();
                ((MainWindow)this.Owner).CustomerOrdersDatagrid_SelectionChanged(null, null);

                this.Close();
            }
        }
    }
}
