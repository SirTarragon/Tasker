using Library.TaskManagement.models;
using Library.TaskManagement.Standard.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UWPTaskManagement.ViewModels;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Content Dialog item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UWPTaskManagement.Dialogs
{
    public sealed partial class AppointmentDialog : ContentDialog
    {
        private MainViewModel _main;
        private int _priority = 1;
        public AppointmentDialog(MainViewModel mvm)
        {
            this.InitializeComponent();
            _main = mvm;
            DataContext = new AppointmentDTO();
        }

        public AppointmentDialog(MainViewModel mvm, ItemViewModel item)
        {
            this.InitializeComponent();
            _main = mvm;
            DataContext = item.BoundAppointment;

            _priority = item.BoundAppointment.Priority;
            switch (_priority)
            {
                case 1:
                    OneR.IsChecked = true;
                    break;
                case 2:
                    TwoR.IsChecked = true;
                    break;
                case 3:
                    ThreeR.IsChecked = true;
                    break;
                case 4:
                    FourR.IsChecked = true;
                    break;
                case 5:
                    FiveR.IsChecked = true;
                    break;
            }
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            (DataContext as AppointmentDTO).Priority = _priority;
            AppointmentDTO item = DataContext as AppointmentDTO;
            if (_main.Items.Any(i => i.ID == item.ID))
            {
                _main.Edit(new ItemViewModel(item));
            }
            else
            {
                _main.Add(new ItemViewModel(item));
            }
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            //_main.Refresh();
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            _priority = Int32.Parse(radioButton.Content as string);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            (DataContext as AppointmentDTO).Attendees.Add(Attendee.Text);
            Attendee.Text = "";
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            object selectedItem = AttendeeListBox.SelectedItem;
            var inputbox = Attendee.Text as string;
            if(selectedItem != null)
            {
                (DataContext as AppointmentDTO).Attendees.Remove(selectedItem as string);
            } 
            else if(inputbox != null && (DataContext as AppointmentDTO).Attendees.Any(i => i.ToUpper().Equals(inputbox.ToUpper())))
            {
                var item = (DataContext as AppointmentDTO).Attendees.FirstOrDefault(i => i.ToUpper().Equals(inputbox.ToUpper()));
                var index = (DataContext as AppointmentDTO).Attendees.IndexOf(item);
                (DataContext as AppointmentDTO).Attendees.RemoveAt(index);
                Attendee.Text = "";
            }
        }
    }
}