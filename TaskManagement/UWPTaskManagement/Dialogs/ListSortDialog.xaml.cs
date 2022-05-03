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
    public sealed partial class ListSortDialog : ContentDialog
    {
        private string _sort;
        private bool _importCheck;

        public ListSortDialog(MainViewModel mvm)
        {
            this.InitializeComponent();
            DataContext = mvm;

            var sort = (DataContext as MainViewModel).SortBy;
            if(sort == "PRIORITY")
            {
                PrioritySort.IsChecked = true;
            } 
            else if(sort == "ITEM ID")
            {
                IDSort.IsChecked = true;
            }
            _importCheck = (DataContext as MainViewModel).ShowComplete;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            bool propchange = false;
            if((DataContext as MainViewModel).SortBy != _sort)
            {
                (DataContext as MainViewModel).SortBy = _sort;
                propchange = true;
            }

            if((DataContext as MainViewModel).ShowComplete != _importCheck)
            {
                propchange = true;
            }

            if(propchange)
            {
                (DataContext as MainViewModel).NotifyPropertyChanged("Items");
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            _sort = (radioButton.Content as string).ToUpper();
        }
    }
}
