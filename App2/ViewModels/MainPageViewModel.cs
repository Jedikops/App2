using App2.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App2.ViewModels
{
    public class MainPageViewModel : Observable
    {

        private Friend _selectedFriend;


        public MainPageViewModel()
        {
            Friends = new ObservableCollection<Friend> {
                new Friend() { FirstName = "Monika", LastName = "Król" },
                new Friend() { FirstName = "Piotr", LastName = "Szelong"}
            };
        }

        public ObservableCollection<Friend> Friends { get; private set; }
        public Friend SelectedFriend
        {
            get
            {
                return _selectedFriend;
            }
            set
            {
                if (_selectedFriend != value)
                {
                    _selectedFriend = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
