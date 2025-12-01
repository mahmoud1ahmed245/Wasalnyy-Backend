using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wasalnyy.BLL.EventHandlers.Abstraction
{
    public interface IChatHubEventHandler
    {
        public Task OnUserConnected(string userId, string connectionId);
        public Task OnUserDisconnected(string connectionId);
        Task OnSendMessage(string senderId, string receiverId, string messageContent, string senderConnectionId);

    }
}
