using DatabaseProvider.DTO;
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
    /// Логика взаимодействия для CreateComponentsOrderWindow.xaml
    /// </summary>
    public partial class CreateComponentsOrderWindow : Window
    {
        List<KeyValuePair<TextBox, TextBox>> AllComponents = new List<KeyValuePair<TextBox, TextBox>>();
        public CreateComponentsOrderWindow()
        {
            InitializeComponent();
            AddComponentField();
            OrderDatePicker.SelectedDate = DateTime.Today;
            ArrivalDatePicker.SelectedDate = DateTime.Today;
        }

        private void AddComponentField()
        {
            var newRow = new RowDefinition();
            newRow.Height = new GridLength(30);
            ComponentsGrid.RowDefinitions.Add(newRow);

            var componentIdTextBox = new TextBox();
            componentIdTextBox.Text = "-1";
            componentIdTextBox.Foreground = Brushes.White;
            componentIdTextBox.Height = 20;
            componentIdTextBox.Width = 50;
            componentIdTextBox.HorizontalAlignment = HorizontalAlignment.Left;
            componentIdTextBox.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF444446");
            componentIdTextBox.KeyDown += ComponentIdTextBox_KeyDown;
            componentIdTextBox.Tag = AllComponents.Count;
            ComponentsGrid.Children.Add(componentIdTextBox);
            Grid.SetRow(componentIdTextBox, AllComponents.Count);
            Grid.SetColumn(componentIdTextBox, 0);

            var componentCountTextBox = new TextBox();
            componentCountTextBox.Text = "0";
            componentCountTextBox.Foreground = Brushes.White;
            componentCountTextBox.Height = 20;
            componentCountTextBox.Width = 80;
            componentCountTextBox.HorizontalAlignment = HorizontalAlignment.Left;
            componentCountTextBox.Margin = new Thickness(10, 0, 0, 0);
            componentCountTextBox.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF444446");
            componentCountTextBox.Tag = AllComponents.Count;
            ComponentsGrid.Children.Add(componentCountTextBox);
            Grid.SetRow(componentCountTextBox, AllComponents.Count);
            Grid.SetColumn(componentCountTextBox, 1);

            AllComponents.Add(new KeyValuePair<TextBox, TextBox>(componentIdTextBox, componentCountTextBox));
        }

        private void ComponentIdTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                var sendettextBox = (TextBox)sender;
                if ((int)sendettextBox.Tag == AllComponents.Count - 1 && sendettextBox.Text != "-1")
                    AddComponentField();

                if (!long.TryParse(sendettextBox.Text, out var id))
                    sendettextBox.BorderBrush = Brushes.Red;
                else
                    sendettextBox.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFABADB3");
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (double.TryParse(PriceTextBox.Text, out var price))
            {
                PriceTextBox.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFABADB3");

                var halted = false;
                var components = new Dictionary<long, int>();
                foreach (var componentBoxes in AllComponents)
                {
                    long id;
                    int count;
                    if (long.TryParse(componentBoxes.Key.Text, out id))
                    {
                        componentBoxes.Key.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFABADB3");
                    } else
                    {
                        halted = true;
                        componentBoxes.Key.Background = Brushes.Red;
                    }

                    if (int.TryParse(componentBoxes.Value.Text, out count))
                    {
                        componentBoxes.Value.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFABADB3");
                    } else
                    {
                        halted = true;
                        componentBoxes.Value.Background = Brushes.Red;
                    }

                    if (!halted && id != -1 && MainWindow.AllComponents.Find(x => x.Id == id) != null)
                    {
                        if (components.ContainsKey(id))
                            components[id] += count;
                        else
                            components.Add(id, count);
                    }
                }

                if (halted)
                    return;

                var result = new ComponentsOrderPOCO
                {
                    Components = components,
                    Price = price,
                    OrderDate = OrderDatePicker.SelectedDate.Value,
                    DateOfArrival = ArrivalDatePicker.SelectedDate.Value,
                    Completed = false
                };
                await DatabaseProvider.Database.AddComponentOrder(result);
                MainWindow.AllComponentsOrders.Add(result);
                ((MainWindow)this.Owner).RefreshComponentsOrdersDataGrid();
                ((MainWindow)this.Owner).ComponentsOrdersDatagrid_SelectionChanged(null, null);
                this.Close();
            } else
            {
                PriceTextBox.BorderBrush = Brushes.Red;
            }
        }
    }
}
