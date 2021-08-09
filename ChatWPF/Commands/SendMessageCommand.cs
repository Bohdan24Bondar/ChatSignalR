using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ChatDomain;

namespace ChatWPF
{
    class SendMessageCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;

        private readonly ChatViewModel _viewModel;
        private readonly SignalRChatService _chatService;

        public SendMessageCommand(ChatViewModel viewModel, SignalRChatService chatService)
        {
            _viewModel = viewModel;
            _chatService = chatService;
        }

        //public SendMessageCommand(SignalRChatService chatService)
        //{
        //    _chatService = chatService;
        //}

        public bool CanExecute(object parameter)
        {
            return true;
        }

        //public async void Execute(object parameter)
        //{
        //    try
        //    {
        //        await _chatService.SendMessage(new ChatMessage
        //        {
        //            //Message = _viewModel.Message,
        //            //Author = _viewModel.Author,
        //            //Time = _viewModel.Time
        //            Message = (string)parameter,
        //            //Author = _viewModel.Author,
        //            Time = DateTime.Now
        //        });
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public async void Execute(object parameter)
        {
            try
            {
                string recepient = _viewModel.SelectedParticipant.Name;
                string msg = parameter as string;

                await _chatService.SendMessage(recepient, _viewModel.UserName, new ChatMessage
                {
                    Message = msg,
                    Author = _viewModel.Author,
                    Time = DateTime.Now,
                    IsSenderedMessage = false
                }); ;

                _viewModel.SelectedParticipant.Chatter.Add(new ChatMessage { Message = msg, Time = DateTime.Now, IsSenderedMessage = true});
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
