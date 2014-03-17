using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Wazzap.Hubs;
using Wazzap.Models;

namespace Wazzap.Controllers
{    
    public class ChatController : ApiControllerWithHub<ChatHub>
    {
        
        public static List<ChatMessage> messageTable = new List<ChatMessage>()
        {
            new ChatMessage() {ID = 1, user = new User(){ username="admin", online= true},timestamp = DateTime.Now, message = "Welcome to chat server"}            
        };

        private static int lastMessageTableId;

        public IEnumerable<ChatMessage> GetChatMessages()
        {
            lock (messageTable)
            {
                return messageTable.ToArray();
            }
        }

        public HttpResponseMessage SendMessage(ChatMessage message)
        {
            lock (messageTable)
            {
                lastMessageTableId = messageTable.Max(tdi => tdi.ID);
                ChatMessage newMessage = new ChatMessage()
                {
                    ID = Interlocked.Increment(ref lastMessageTableId),
                    user = message.user,
                    message = message.message,
                    timestamp = DateTime.Now                   
                };

                Hub.Clients.All.newChat(newMessage);

                var response = Request.CreateResponse(HttpStatusCode.Created, newMessage);
                string link = Url.Link("apiRoute", new { controller = "chat", id = newMessage.ID });
                response.Headers.Location = new Uri(link);
                return response;
            }
        }
    }
}