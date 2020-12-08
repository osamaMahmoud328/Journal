using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI
{
    class Articlet
    {
        public string AName { get; set; }
        public string ARtName { get; set; }
        public string Subj_Article { get; set; }
        public System.DateTime Article_Date { get; set; }
        public int ID { get; set; }
        public string Type_user { get; set; }

        public virtual ABusers ABuser { get; set; }
        // public virtual Authort Authort { get; set; }



        public IEnumerable<Articlet> retrieve()
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:62135/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync("api/Article").Result;
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var Users = response.Content.ReadAsAsync<IEnumerable<Articlet>>().Result;
                    return Users;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("no data");
                }
            }
            else
                MessageBox.Show(response.StatusCode + "not found" + response.ReasonPhrase + "nahs");
            return null;

        }

        public Articlet search(string username)
        {

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:62135/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var user = username.Trim();
            var url = "api/Article2/" + user;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var Users = response.Content.ReadAsAsync<Articlet>().Result;
                    List<Articlet> artc = new List<Articlet>();
                    artc.Add(Users);
                    return Users;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
            else

                MessageBox.Show(response.StatusCode + "not found" + response.ReasonPhrase);
            return null;

        }
    }
}
