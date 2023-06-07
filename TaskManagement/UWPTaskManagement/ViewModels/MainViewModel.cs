using Library.TaskManagement.models;
using Library.TaskManagement.services;
using Library.TaskManagement.Standard.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UWPTaskManagement.services;

namespace UWPTaskManagement.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private ItemServiceProxy itemService;

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public MainViewModel()
        { itemService = new ItemServiceProxy(); }

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                IEnumerable<ItemViewModel> returnal = itemService.Items.Where(
                    i => (!ShowComplete && !((i.BoundItem as ToDoDTO)?.IsCompleted ?? false)) || ShowComplete);


                if (SortBy == "PRIORITY")
                {
                    returnal = new ObservableCollection<ItemViewModel>(returnal.OrderByDescending(i => i.Priority));
                }
                else if (SortBy == "ITEM ID")
                {
                    returnal = new ObservableCollection<ItemViewModel>(returnal.OrderBy(i => i.ID));
                }

                return new ObservableCollection<ItemViewModel>(returnal.Select(i => new ItemViewModel(i.BoundItem)));
            }
        }

        public ItemViewModel SelectedItem { get; set; }
        public bool ShowComplete { set; get; }
        public string Query
        {
            get
            {
                return itemService.Query;
            }

            set
            {
                itemService.Query = value;
            }

        }
        public bool UseAPI
        {
            get
            {
                return itemService.UseAPI;
            }
            set
            {
                itemService.UseAPI = value;
            }
        }

        public int IDService
        {
            get
            {
                return itemService.IDCount;
            }
        }

        public string SortBy { get; set; }

        public async void Add(ItemViewModel item)
        {
            if (item.ID == -1)
            {
                item.BoundItem.ID = IDService;
            }
            await itemService.Add(item);
            Refresh();
        }

        public string AddPrompt { get; set; }

        public async void Remove(ItemViewModel item)
        {
            await itemService.Remove(item);
            Refresh();
        }

        public async void RemoveAt(int id)
        {
            await itemService.Remove(id);
            Refresh();
        }

        public async void Edit(ItemViewModel item)
        {
            if (!UseAPI)
            {
                await itemService.Remove(item);
            }
            await itemService.Add(item);
            Refresh();
        }

        public void Search()
        {
            if (!string.IsNullOrEmpty(Query))
            {
                Refresh();
            }
        }

        public void Refresh()
        {
            NotifyPropertyChanged("Items");
        }
    }
}