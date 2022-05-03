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
    public sealed partial class DeleteDialog : ContentDialog
    {
        private MainViewModel _main;

        public DeleteDialog(MainViewModel mvm, ItemViewModel item)
        {
            this.InitializeComponent();
            _main = mvm;
            DataContext = item.BoundItem;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            _main.Remove(new ItemViewModel(DataContext as ItemDTO));
        }

        private void ContentDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        { }
    }
}
