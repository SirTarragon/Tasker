using Library.TaskManagement.models;
using Library.TaskManagement.services;
using Library.TaskManagement.Standard.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWPTaskManagement.ViewModels;

namespace UWPTaskManagement.services
{
    class ItemServiceProxy
    {
        private ItemService itemService;

        public ItemServiceProxy()
        {
            itemService = ItemService.Current;
        }

        public ObservableCollection<ItemViewModel> Items
        {
            get
            {
                if (!string.IsNullOrEmpty(Query))
                {
                    return new ObservableCollection<ItemViewModel>
                        (itemService.Search(Query).Select(i => new ItemViewModel(i)));
                } 
                else
                {
                    return new ObservableCollection<ItemViewModel>
                    (itemService.Items.Select(i => new ItemViewModel(i)));
                }
            }
        }

        public int IDCount
        {
            get
            {
                return itemService.IDCount;
            }
        }

        public string Query { get; set; }

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

        public async Task<ItemDTO> Add(ItemViewModel item)
        {
            return await itemService.Add(item.BoundItem);
        }

        public async Task<ItemDTO> Remove(ItemViewModel item)
        {
            return await Remove(item.ID);
        }

        public async Task<ItemDTO> Remove(int id)
        {
            return await itemService.Remove(id);
        }

        public IEnumerable<ItemDTO> Search(string key)
        {
            return itemService.Search(key);
        }
    }
}
