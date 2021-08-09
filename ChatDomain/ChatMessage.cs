using System;

namespace ChatDomain
{
    public class ChatMessage
    {
        public string Message { get; set; }

        public string Author { get; set; }

        public DateTime Time { get; set; }

        public bool IsSenderedMessage { get; set; }

    }
}
