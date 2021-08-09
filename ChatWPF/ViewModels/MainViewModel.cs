using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWPF
{
    public class MainViewModel
    {
        public ChatViewModel ViewModel { get; }

        public MainViewModel(ChatViewModel viewModel)
        {
            ViewModel = viewModel;
        }
    }
}
