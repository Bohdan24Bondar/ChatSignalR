using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ChatWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            HubConnection connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/chat")
                .Build();
            ChatViewModel viewModel = ChatViewModel.CreatedConnectedViewModel(new SignalRChatService(connection));

            //base.OnStartup(e);

            MainWindow window = new MainWindow
            {
                //DataContext = new MainViewModel(viewModel)
                DataContext = viewModel
            };

            window.Show();
        }
    }
}
