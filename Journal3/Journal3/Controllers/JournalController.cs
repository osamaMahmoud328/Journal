
using JournalAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Journal3.Controllers
{
    public class JournalController : ApiController
    {

       

        
        public IEnumerable<ABuser> Get()
            {
            using (JournalEntities _entites =new JournalEntities())
                {
                _entites.Configuration.ProxyCreationEnabled = false;

                return _entites.ABusers.ToList();


                }

            }

        public HttpResponseMessage Get(int id)
        {

            using (JournalEntities _entites = new JournalEntities())
            {
                _entites.Configuration.ProxyCreationEnabled = false;

                var user = _entites.ABusers.FirstOrDefault(c => c.AB_iD == id);
                if (user == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound,"This user is not found" );
                else
                    return Request.CreateResponse(HttpStatusCode.OK,user);


            }



        }

        public HttpResponseMessage Delete(int id)
        {
            using (JournalEntities _entites = new JournalEntities())
            {
                _entites.Configuration.ProxyCreationEnabled = false;
                var user = _entites.ABusers.FirstOrDefault(aa => aa.AB_iD == id);
                if (user == null)
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "This user is not found");
                else
                {
                    _entites.ABusers.Remove(user);
                    _entites.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK);

                }

            }




        }


        public HttpResponseMessage Put(int ID, ABuser info)
        {
            using (JournalEntities _ent = new JournalEntities())
            {
                try
                {
                    var User = _ent.ABusers.FirstOrDefault(c => c.AB_iD == ID);
                    if (info == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Invalid ID , There is no such a user with this ID = " + ID);
                    }
                    else
                    {

                        User.Username = info.Username;
                        User.FirstName = info.FirstName;
                        User.LastName = info.LastName;
                        User.Pass = info.Pass;
                        User.Phone = info.Phone;
                        User.Company = info.Company;
                        User.Email = info.Email;
                        User.Picture = info.Picture;
                        User.Articlets = info.Articlets;
                        _ent.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, info);
                    }
                }
                catch (Exception ex)
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
                }
            }
        }

       
        public HttpResponseMessage Post(ABuser user)
        {
            using (JournalEntities _entites = new JournalEntities())
            {
               // _entites.Configuration.ProxyCreationEnabled = false;

                try
                {

                    _entites.ABusers.Add(user);
                    _entites.SaveChanges();

                    var msg = Request.CreateResponse(HttpStatusCode.Created, user);
                    msg.Headers.Location = new Uri(Request.RequestUri + "/" + user.AB_iD);
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