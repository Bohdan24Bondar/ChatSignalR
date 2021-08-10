using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChatDomain;

namespace ChatWPF
{
    public class ChatViewModel : BaseViewModel
    {
        
        public ObservableCollection<ChatMessage> Messages { get; }

        public ICommand SendMessage { get; }

        public ICommand LoginCom { get; }

        public ICommand LogoutCom { get; }

        private ObservableCollection<Participant> _participants;
        public ObservableCollection<Participant> Participants
        {
            get { return _participants; }
            set
            {
                _participants = value;
                OnPropertyChanged();
            }
        }

        private Participant _selectedParticipant;
        public Participant SelectedParticipant
        {
            get { return _selectedParticipant; }
            set
            {
                _selectedParticipant = value;
                OnPropertyChanged();
            }
        }

        //private async Task<bool> Login()
        //{
        //    try
        //    {
        //        List<User> users = new List<User>();//todo
        //        users = await _chatService.LoginAsync(_author);//возарвщает всех пользователей

        //        if (users != null)
        //        {
        //            users.ForEach(u => Participants.Add(new Participant { Name = u.Name}));
        //            //UserMode = UserModes.Chat;
        //            //IsLoggedIn = true;
        //            return true;
        //        }
        //        else
        //        {
        //            //dialogService.ShowNotification("Username is already in use");
        //            return false;
        //        }

        //    }
        //    catch (Exception) 
        //    { 
        //        return false; 
        //    }
        //}

        private readonly SignalRChatService _chatService;

        public ChatViewModel(SignalRChatService chatService)
        {
            _participants = new ObservableCollection<Participant>();
            Messages = new ObservableCollection<ChatMessage>();
            SendMessage = new SendMessageCommand(this, chatService);
            LoginCom = new LoginCommand(this, chatService);
            LogoutCom = new LogoutCommand(this, chatService);
            _chatService = chatService;
            _chatService.MessageRecived += ChatService_MessageRecived;
            _chatService.LoggedIn += _chatService_LoggedIn;
            _chatService.LoggedOut += _chatService_LoggedOut;
            _author = string.Empty;
        }

        private void _chatService_LoggedOut(string name)
        {
            Participant loggedOutUser = Participants.FirstOrDefault(p => string.Equals(p.Name, name));

            if (loggedOutUser != null)
            {
                ObservableCollection<Participant> participants = new ObservableCollection<Participant>();

                foreach (var u in Participants)
                {
                    if (u.Name == name)
                    {
                        continue;
                    }

                    participants.Add(u);
                }

                Participants = participants;
            }
        }

        //private void _chatService_LoggedIn(User currentUser)
        //{
        //    Participant loggedUser = Participants.FirstOrDefault(p => string.Equals(p.Name, currentUser.Name));

        //    if (loggedUser == null)
        //    {
        //        ctxTaskFactory.StartNew(() => Participants.Add(new Participant
        //        {
        //            Name = currentUser.Name,
        //        })).Wait();
        //    }
        //}

        private void _chatService_LoggedIn(User currentUser)
        {
            Participant loggedUser = Participants.FirstOrDefault(p => string.Equals(p.Name, currentUser.Name));

            if (loggedUser == null)
            {
                Participants.Add(new Participant
                {
                    Name = currentUser.Name,
                });
            }
        }

        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        private string _errorMessage = string.Empty;
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
                OnPropertyChanged(nameof(HasErrorMessage));
            }
        }

        public bool HasErrorMessage => !string.IsNullOrEmpty(ErrorMessage);

        private bool _isConnected;
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
                OnPropertyChanged(nameof(IsConnected));
            }
        }

        private string _author;

        public string Author
        {
            get => _author;

            set
            {
                _author = value;
                OnPropertyChanged(nameof(Author));
            }
        }

        private string _message;

        public string Message
        {
            get => _message;

            set
            {
                _message = value;
                OnPropertyChanged(nameof(Message));
            }
        }

        private DateTime _time;

        public DateTime Time
        {
            get => _time;

            set
            {
                _time = value;
                OnPropertyChanged(nameof(Time));
            }
        }

        private UserModes _userMode;
        public UserModes UserMode
        {
            get { return _userMode; }
            set
            {
                _userMode = value;
                OnPropertyChanged();
            }
        }

        //private void ChatService_MessageRecived(ChatMessage message)
        //{
        //    if (message != null)
        //    {
        //        Messages.Add(message);
        //    }
        //}

        private void ChatService_MessageRecived(string name, string sender, ChatMessage sendedMessage)
        {
            ChatMessage cm = new ChatMessage 
            { 
                Author = sender, 
                Message = sendedMessage.Message, 
                Time = sendedMessage.Time,
                IsSenderedMessage = sendedMessage.IsSenderedMessage 
            };

            var serchedUser = _participants.Where((u) => string.Equals(u.Name, sender)).FirstOrDefault();
            serchedUser.Chatter.Add(cm);
        }

        public static ChatViewModel CreatedConnectedViewModel(SignalRChatService chatService)
        {
            ChatViewModel viewModel = new ChatViewModel(chatService);

            chatService.Connect().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    viewModel.ErrorMessage = "Unable to connect to chat hub";
                }
            });

            return viewModel;
        }
    }
}
