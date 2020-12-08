using JournalAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Journal3.Controllers
{
    public class Article2Controller : ApiController
    {
        public IEnumerable<Articlet> GetArticle()
        {

            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.Articlets.ToList();
            }
        }

        public HttpResponseMessage GetArticle(string id)
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                var art = entities.Articlets.FirstOrDefault(c => c.ARtName == id);
                if (User == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no Article with such ID");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, art);
                }
            }
        }
    }
}
