using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using JournalAccess;
namespace Journal3.Controllers
{
    public class ArticleController : ApiController
    {
        public IEnumerable<Articlet> GetArticle()
        {

            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                return entities.Articlets.ToList();
            }
        }

        public HttpResponseMessage GetArticle(int ID)
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                var art = entities.Articlets.FirstOrDefault(c => c.ID == ID);
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

        public HttpResponseMessage PostArticle(Articlet Nart)
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                try
                {
                    entities.Articlets.Add(Nart);
                    entities.SaveChanges();

                    var msg = Request.CreateResponse(HttpStatusCode.Created, Nart);
                    msg.Headers.Location = new Uri(Request.RequestUri + "/" + Nart.Article_ID);

                    return msg;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        public HttpResponseMessage PutArticle(int ID, Articlet Nart)
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                try
                {
                    var Oart = entities.Articlets.FirstOrDefault(c => c.Article_ID == ID);
                    if (Oart == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no record with such ID");
                    }
                    else
                    {
                        Oart.AName = Nart.AName;
                        Oart.Article_Date = Nart.Article_Date;
                        Oart.ARtName = Nart.ARtName;
                        Oart.Subj_Article = Nart.Subj_Article;
                        Oart.ID = Nart.ID;
                        Oart.Type_user = Nart.Type_user;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, Nart);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

        public HttpResponseMessage Delete(int ID)
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;
                var article = entities.Articlets.FirstOrDefault(c => c.Article_ID == ID);
                if (entities == null)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "There is no record with such ID");
                }
                else
                {
                    entities.Articlets.Remove(article);
                    entities.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
            }
        }

    }
}
