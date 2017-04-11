using App2.Base;
using System;

namespace App2
{
    public class Friend : Observable
    {
        private string _firstName;
        private string _lastName;
        private bool _isDeveloper;
        private DateTime _birthDay;

        public string FirstName
        {
            get { return _firstName; }
            set
            {
                _firstName = value; OnPropertyChanged();
            }
        }
        public string LastName
        {
            get { return _lastName; }
            set
            {
                _lastName = value; OnPropertyChanged();
            }
        }

        public bool IsDeveloper
        {
            get { return _isDeveloper; }
            set
            {
                _isDeveloper = value; OnPropertyChanged();
            }
        }

        public DateTime BirthDay
        {
            get { return _birthDay; }
            set
            {
                _birthDay = value; OnPropertyChanged();
            }
        }

    }
}