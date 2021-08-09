using ChatDomain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatWPF
{
    public class LoginCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly ChatViewModel _viewModel;
        private readonly SignalRChatService _chatService;

        public LoginCommand(ChatViewModel viewModel, SignalRChatService chatService)
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
            //string s = _viewModel.UserName;
            try
            {
                string name = parameter as string;
                List<User> users = new List<User>();//todo
                users = await _chatService.LoginAsync(name);//возарвщает всех пользователей
                _viewModel.UserMode = UserModes.Chat;

                if (users != null)
                {
                    _viewModel.Participants = new ObservableCollection<Participant>();
                    users.ForEach(u => _viewModel.Participants.Add(new Participant { Name = u.Name }));
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
