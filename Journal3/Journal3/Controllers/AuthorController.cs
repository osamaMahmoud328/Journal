using JournalAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Journal3.Controllers
{
    public class AuthorController : ApiController
    {
        public IEnumerable<Authort> Get()
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.Authorts.ToList();
            }
        }

        public HttpResponseMessage Get(string id)
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                var aut = entities.Authorts.FirstOrDefault(c => c.AName == id);
                if (User == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no Article with such ID");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, aut);
                }
            }
        }

        public HttpResponseMessage PostAuthor(Authort Naut)
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                try
                {
                    entities.Authorts.Add(Naut);
                    entities.SaveChanges();

                    var msg = Request.CreateResponse(HttpStatusCode.Created, Naut);
                    msg.Headers.Location = new Uri(Request.RequestUri + "/" + Naut.AName);

                    return msg;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }






    }
}
