
using JournalAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Journal3.Controllers
{
   
    public class LoginController : ApiController
    { 

        public IEnumerable<ABuser> Get()
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;

                return entities.ABusers.ToList();
            }
        }
        public ABuser Get(string ID)
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;

                return entities.ABusers.FirstOrDefault(e => e.Username == ID);
            }
        }


        public HttpResponseMessage Post(ABuser obj)
        {
            using (JournalEntities entities = new JournalEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;

                try
                {
                    entities.ABusers.Add(obj);
                    entities.SaveChanges();
                    var msg = Request.CreateResponse(HttpStatusCode.Created, obj);
                    msg.Headers.Location = new Uri(Request.RequestUri + "/" + obj.AB_iD);
                    return msg;
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

                }
            }
        }
        public HttpResponseMessage Put(int ID, ABuser user)
        {

            using (JournalEntities _entites = new JournalEntities())
            {
                try
                {
                    _entites.Configuration.ProxyCreationEnabled = false;

                    var User = _entites.ABusers.FirstOrDefault(c => c.AB_iD == ID);
                    if (User == null)
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "no record with such id");
                    else
                    {
                        User.Username = user.Username;
                        User.FirstName = user.FirstName;
                        User.LastName = user.LastName;
                        User.Pass = user.Pass;
                        User.Phone = user.Phone;
                        User.Company = user.Company;
                        User.Email = user.Email;
                        User.Articlets = user.Articlets;
                        _entites.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, user);
                    }


                }

                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);



                }
            }
        }
      }
    }

    
    


