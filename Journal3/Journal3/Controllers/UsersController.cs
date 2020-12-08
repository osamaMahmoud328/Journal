using JournalAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Journal3.Controllers
{
    [RoutePrefix("api/Business")]
    public class UsersController : ApiController
    {
        [Route("users")]

        public IEnumerable<ABuser> Get()
        {
            using (JournalEntities _entites = new JournalEntities())
            {
                _entites.Configuration.ProxyCreationEnabled = false;
                List<ABuser> admins = new List<ABuser>();
                var users = _entites.ABusers.ToList();
                foreach (ABuser admin in users)
                {
                    if (admin.Type_user == "User")
                    {
                        admins.Add(admin);

                    }

                }

                return admins;


            }


        }
    }
}