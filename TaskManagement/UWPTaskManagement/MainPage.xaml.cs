using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using UWPTaskManagement.ViewModels;
using UWPTaskManagement.Dialogs;
using System.Collections;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UWPTaskManagement
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string persistencePath = $@"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)}\eventman.json";

        public MainPage()
        {
            this.InitializeComponent();
            DataContext = new MainViewModel();

            bool toggle = (DataContext as MainViewModel).UseAPI;
            if (toggle)
            {
                LoadButton.Content = "ONLINE";
            }
            else
            {
                LoadButton.Content = "OFFLINE";
            }
        }
/*
        private void Save_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Save(persistencePath);
        }
*/
        private void Load_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).UseAPI = !(DataContext as MainViewModel).UseAPI;
            (DataContext as MainViewModel).Refresh();
            bool toggle = (DataContext as MainViewModel).UseAPI;
            if (toggle)
            {
                LoadButton.Content = "ONLINE";
            }
            else
            {
                LoadButton.Content = "OFFLINE";
            }
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as MainViewModel).Refresh();
            (DataContext as MainViewModel).Query = null;
            (DataContext as MainViewModel).NotifyPropertyChanged("Query");
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            if ((DataContext as MainViewModel).SelectedItem != null)
            {
                Edit_Click(sender, e);
            }
            else
            {
                var q = new ItemDialog(DataContext as MainViewModel);
                _ = await q.ShowAsync();
                string prompt = (DataContext as MainViewModel).AddPrompt;
                if (prompt != null)
                {
                    if (prompt.ToUpper().Equals("TODO"))
                    {
                        var dialog = new ToDoDialog(DataContext as MainViewModel);
                        _ = await dialog.ShowAsync();
                    }
                    else if (prompt.ToUpper().Equals("APPOINTMENT"))
                    {
                        var dialog = new AppointmentDialog(DataContext as MainViewModel);
                        _ = await dialog.ShowAsync();
                    }
                }
                (DataContext as MainViewModel).AddPrompt = null;
            }
        }

        private async void Edit_Click(object sender, RoutedEventArgs e)
        {
            //if ((DataContext as MainViewModel).SelectedItem != null)
            //{
            bool isToDo = (DataContext as MainViewModel).SelectedItem.IsToDo;
            if (isToDo)
            {
                var dialog = new ToDoDialog(DataContext as MainViewModel, (DataContext as MainViewModel).SelectedItem);
                _ = await dialog.ShowAsync();
            }
            else
            {
                var dialog = new AppointmentDialog(DataContext as MainViewModel, (DataContext as MainViewModel).SelectedItem);
                _ = await dialog.ShowAsync();
            }

            // unselect selecteditem
            (DataContext as MainViewModel).SelectedItem = null;
            (DataContext as MainViewModel).NotifyPropertyChanged("SelectedItem");
            //}
        }

        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if ((DataContext as MainViewModel).SelectedItem != null)
            {
                var dialog = new DeleteDialog(DataContext as MainViewModel, (DataContext as MainViewModel).SelectedItem);
                _ = await dialog.ShowAsync();
            }
        }

        private async void View_Click(object sender, RoutedEventArgs e)
        {
            if ((DataContext as MainViewModel).SelectedItem != null)
            {
                bool isToDo = (DataContext as MainViewModel).SelectedItem.IsToDo;
                if (isToDo)
                {
                    var dialog = new ViewToDoDialog((DataContext as MainViewModel).SelectedItem);
                    _ = await dialog.ShowAsync();
                }
                else
                {
                    var dialog = new ViewAppointmentDialog((DataContext as MainViewModel).SelectedItem);
                    _ = await dialog.ShowAsync();
                }
            }
        }

        private async void Sort_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new ListSortDialog(DataContext as MainViewModel);
            _ = await dialog.ShowAsync();
        }

        private void Items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   // to allow for item deselection, selection mode set to multiple
            // this is to prevent more than one item from being in SelectedItems
            ListBox listBox = sender as ListBox;
            if( e.AddedItems.Count > 0)
            {
                object last = e.AddedItems[0];
                foreach (object item in listBox.SelectedItems)
                {
                    if (item != last) { _ = listBox.SelectedItems.Remove(item); }
                }
                ItemManip.Content = "Edit Selected";
            } 
            else if ( e.AddedItems.Count == 0)
            {
                ItemManip.Content = "New Item";
            }
        }

        private void IsCompleted_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox lCheck = sender as CheckBox;
            int id = int.Parse(lCheck.Tag.ToString());
            var item = (DataContext as MainViewModel).Items.FirstOrDefault(i => i.ID == id);
            item.IsCompleted = (bool)lCheck.IsChecked;
            (DataContext as MainViewModel).Edit(item);
        }
    }
}