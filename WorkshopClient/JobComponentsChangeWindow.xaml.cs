using DatabaseProvider.DTO;
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
    /// Логика взаимодействия для JobComponentsChangeWindow.xaml
    /// </summary>
    public partial class JobComponentsChangeWindow : Window
    {
        FullJobPOCO Job;
        List<KeyValuePair<TextBox, TextBox>> AllComponents = new List<KeyValuePair<TextBox, TextBox>>();
        Dictionary<long, int> PreEditComponentsUsage = new Dictionary<long, int>();
        public JobComponentsChangeWindow(FullJobPOCO job)
        {
            Job = job;
            InitializeComponent();
            MainLabel.Content = $"Изменение использованных компонентов в работе #{Job.Id}";
            foreach (var usedComponent in Job.ComponentsUsed)
                PreEditComponentsUsage.Add(usedComponent.Key.Id, usedComponent.Value);

            FillComponents();
        }

        private void FillComponents()
        {
            foreach (var componentUsed in Job.ComponentsUsed)
                AddComponentField(componentUsed);

            AddComponentField(new KeyValuePair<ComponentPOCO, int>(new ComponentPOCO { Id = -1, ComponentName = "Укажите компонент", ComponentType = "" }, 0));
        }

        private void AddComponentField(KeyValuePair<ComponentPOCO, int> componentUsed)
        {
            var newRow = new RowDefinition();
            newRow.Height = new GridLength(30);
            ComponentsGrid.RowDefinitions.Add(newRow);

            var componentIdTextBox = new TextBox();
            componentIdTextBox.Text = componentUsed.Key.Id.ToString();
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

            /*
            var componentLabel = new Label();
            componentLabel.Content = $"{component.Id}: [{component.ComponentType}] {component.ComponentName}: ";
            componentLabel.Foreground = Brushes.White;
            componentLabel.VerticalContentAlignment = VerticalAlignment.Center;
            componentLabel.FontSize = 12;
            ComponentsGrid.Children.Add(componentLabel);
            Grid.SetRow(componentLabel, AllComponents.Count);
            Grid.SetColumn(componentLabel, 0);
            */

            var componentCountTextBox = new TextBox();
            componentCountTextBox.Text = componentUsed.Value.ToString();
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
                {
                    AddComponentField(new KeyValuePair<ComponentPOCO, int>(new ComponentPOCO { Id = -1, ComponentName = "Укажите компонент", ComponentType = "" }, -1));
                }

                if (!long.TryParse(sendettextBox.Text, out var id))
                {
                    sendettextBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    sendettextBox.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFABADB3");
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveComponents();
            this.Close();
        }

        private async void SaveComponents()
        {
            var componentUsagePerId = new Dictionary<long, int>();

            var halted = false;
            foreach (var componentBoxes in AllComponents)
            {
                var idBox = componentBoxes.Key;
                var countBox = componentBoxes.Value;

                if (long.TryParse(idBox.Text, out var componentId))
                {
                    idBox.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFABADB3");
                    if (int.TryParse(countBox.Text, out var componentCount))
                    {
                        countBox.BorderBrush = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFABADB3");

                        if (componentUsagePerId.ContainsKey(componentId))
                            componentUsagePerId[componentId] += componentCount;
                        else
                            componentUsagePerId.Add(componentId, componentCount);
                    } else
                    {
                        halted = true;
                        countBox.BorderBrush = Brushes.Red;
                    }
                } else
                {
                    halted = true;
                    idBox.BorderBrush = Brushes.Red;
                }
            }

            if (halted)
                return;

            var job = await DatabaseProvider.Database.GetJob(Job.Id);
            foreach (var component in componentUsagePerId)
            {
                if (component.Key != -1 && await DatabaseProvider.Database.GetComponentInStorage(component.Key) != null)
                {
                    if (component.Value > 0)
                    {
                        if (job.ComponentsUsed.ContainsKey(component.Key))
                            job.ComponentsUsed[component.Key] = component.Value;
                        else
                            job.ComponentsUsed.Add(component.Key, component.Value);
                    } else if (job.ComponentsUsed.ContainsKey(component.Key))
                    {
                        job.ComponentsUsed.Remove(component.Key);
                    }
                }
            }
            await DatabaseProvider.Database.UpdateJob(job);

            foreach (var preEditComponent in PreEditComponentsUsage)
            {
                var component = await DatabaseProvider.Database.GetComponentInStorage(preEditComponent.Key);
                var componentInView = MainWindow.AllStorage.Find(x => x.Component.Id == preEditComponent.Key);
                componentInView.Count += preEditComponent.Value;
                component.Count += preEditComponent.Value;
                if (componentUsagePerId.ContainsKey(component.ComponentId))
                {
                    componentInView.Count -= componentUsagePerId[component.ComponentId];
                    component.Count -= componentUsagePerId[component.ComponentId];
                    componentUsagePerId.Remove(component.ComponentId);
                }
                await DatabaseProvider.Database.UpdateComponentInStorage(component);
            }

            foreach (var componentPostEdit in componentUsagePerId)
            {
                if (componentPostEdit.Key != -1)
                {
                    var component = await DatabaseProvider.Database.GetComponentInStorage(componentPostEdit.Key);
                    if (component != null)
                    {
                        MainWindow.AllStorage.Find(x => x.Component.Id == componentPostEdit.Key).Count -= componentPostEdit.Value;
                        component.Count -= componentPostEdit.Value;
                        await DatabaseProvider.Database.UpdateComponentInStorage(component);
                    }
                }
            }

            MainWindow.AllJobs[MainWindow.AllJobs.IndexOf(MainWindow.AllJobs.First(x => x.Id == Job.Id))] = await FullJobPOCO.BuildFullJobDTO(await DatabaseProvider.Database.GetJob(Job.Id));
            ((MainWindow)this.Owner).RefreshJobsDataGrid();
            ((MainWindow)this.Owner).JobsDataGrid_SelectionChanged(null, null);

            ((MainWindow)this.Owner).RefreshStorageDataGrid();
            ((MainWindow)this.Owner).StorageDatagrid_SelectionChanged(null, null);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
