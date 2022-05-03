using Library.TaskManagement.models;
using Library.TaskManagement.Standard.DTO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Windows.UI.Xaml;

namespace UWPTaskManagement.ViewModels
{
    public class ItemViewModel
    {
        public Visibility IsCompleteVisibility
        {
            get
            {
                return IsToDo ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public string Name
        {
            get
            {
                return BoundItem?.Name ?? string.Empty;
            }
        }
        public string Description
        {
            get
            {
                return BoundItem?.Description ?? string.Empty;
            }
        }

        public int Priority
        {
            get
            {
                return BoundItem?.Priority ?? 1;
            }
        }

        public String Date
        {
            get
            {
                return IsToDo
                    ? BoundToDo?.Deadline?.Date.ToShortDateString() ?? DateTime.Today.ToShortDateString()
                    : (BoundAppointment?.Start?.Date.ToShortDateString() +
                    " - " + BoundAppointment?.End?.Date.ToShortDateString()) ?? DateTime.Today.ToShortDateString();
            }
        }

        public ObservableCollection<string> Attendees
        {
            get
            {
                return !IsToDo
                    ? new ObservableCollection<string>(BoundAppointment.Attendees)
                    : null;
            }
        }

        public ItemDTO BoundItem
        {
            get
            {
                if (IsToDo)
                {
                    return BoundToDo;
                }
                else { return BoundAppointment; }
            }
        }

        public int ID
        {
            get
            {
                return BoundItem.ID;
            }
        }

        public bool IsCompleted
        {
            get
            {
                if (IsToDo)
                {
                    return BoundToDo.IsCompleted;
                }
                else { return false; }
            }
            set
            {
                if (IsToDo)
                {
                    BoundToDo.IsCompleted = value;
                }
            }
        }

        public bool IsToDo
        {
            get
            {
                return BoundToDo != null;
            }
        }

        public ToDoDTO BoundToDo { get; set; }
        public AppointmentDTO BoundAppointment { get; set; }

        public ItemViewModel(ItemDTO item)
        {
            if (item is AppointmentDTO)
            {
                BoundAppointment = item as AppointmentDTO;
                IsCompleted = false;
                BoundToDo = null;
            }
            else if (item is ToDoDTO)
            {
                BoundToDo = item as ToDoDTO;
                BoundAppointment = null;
                IsCompleted = (item as ToDoDTO).IsCompleted;
            }
            else
            {
                BoundToDo = item as ToDoDTO;
                BoundAppointment = null;
            }
        }
    }
}