using ChatDomain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWPF
{
    public class Participant : BaseViewModel
    {
        public string Name { get; set; }

        private bool _isLoggedIn = true;
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { _isLoggedIn = value; OnPropertyChanged(); }
        }

        public ObservableCollection<ChatMessage> Chatter { get; set; }

        public Participant() { Chatter = new ObservableCollection<ChatMessage>(); }
    }
}
