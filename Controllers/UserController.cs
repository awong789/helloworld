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
    public class UserController : ApiControllerWithHub<ChatHub>
    {
        public static List<User> userTable = new List<User>()
            {
                new User() { userId = 0, username = "admin", password="", online =true}
            };
        private static int lastUserId;

        public IEnumerable<User> GetOnlineUsers()
        {
            lock (userTable)
            {
                return userTable.Where(x => x.online).ToArray();
            }
        }

        public HttpResponseMessage Login(User currentUser)
        {
            lock (userTable)
            {
                if (userTable.Exists(x => x.username == currentUser.username))
                {
                    throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.Accepted));
                }
                try
                {
                    lastUserId = userTable.Max(tdi => tdi.userId);
                }
                catch
                {
                    lastUserId = 0;
                }
                User newUser = new User()
                {
                    userId = Interlocked.Increment(ref lastUserId),
                    username = currentUser.username,
                    online = true
                };

                userTable.Add(newUser);

                Hub.Clients.All.newUserLogin(newUser);

                // Return the new item, inside a 201 response
                var response = Request.CreateResponse(HttpStatusCode.Created, newUser);
                string link = Url.Link("apiRoute", new { controller = "user", id = newUser.userId });
                response.Headers.Location = new Uri(link);
                return response;
            }
        }

        //public User Logout(User currentUser)
        //{
        //    lock (userTable)
        //    {
        //        if (!userTable.Exists(x => x.username == currentUser.username))
        //            throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));

        //        User user = userTable.Where(x => x.username == currentUser.username).FirstOrDefault();
        //        user.online = false;

        //        //Hub.Clients.
        //        return user;
        //    }
        //}
    }
}