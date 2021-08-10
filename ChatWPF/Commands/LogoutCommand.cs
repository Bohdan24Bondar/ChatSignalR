using ChatDomain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatWPF
{
    public class LogoutCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly ChatViewModel _viewModel;
        private readonly SignalRChatService _chatService;

        public LogoutCommand(ChatViewModel viewModel, SignalRChatService chatService)
        {
            _viewModel = viewModel;
            _chatService = chatService;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public async void Execute(object parameter)
        {
            try
            {
                string name = parameter as string;

                await _chatService.LogoutAsync(name);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
