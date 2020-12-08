using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GUI
{
    class ABusers
    {
        
        public int AB_iD { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Type_user { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        public byte[] Picture { get; set; }
        public int wind { get; set; }
        public ABusers()
        {
            this.Type_user = "User";
        }

        public void setID(int id)
        {

            this.AB_iD = id;
        }
        public void setUSername(String username)
        {

            this.Username = username;
        }
        public void setPAssDecrypt(String Pass)
        {
           
            byte[] data = UTF8Encoding.UTF8.GetBytes(Pass);
            

            this.Pass = Convert.ToBase64String(data).ToString();
          
        }
        public void setEmail(String Email)
        {

            this.Email = Email;
        }
        public void setFirstName(String firstname)
        {

            this.FirstName = firstname;
        }
        public void setLastName(String Lastname)
        {

            this.LastName = Lastname;
        }
        public void setCompany(String company)
        {

            this.Company = company;
        }
        public void setPhone(String phone)
        {

            this.Phone = phone;
        }

        public void setAll(string username, string firstname, string lastname, string Email, string pass, string company, string phone)
        {
            setUSername(username);
            setFirstName(firstname);
            setLastName(lastname);
            setEmail(Email);
            setPAssDecrypt(pass);
            setCompany(company);
            setPhone(phone);
        }

        public int getID()
        {
            return this.AB_iD;
        }
        public string getUserName()
        {
            return this.Username;
        }
        public string getFirstName()
        {
            return this.FirstName;
        }
        public string getLastName()
        {
            return this.LastName;
        }
        public string getPass()
        {
            return this.Pass;
        }

        //check for user input..................

        public int check(string username, string firstname, string lastname, string Email, string pass, string company, string phone) {
           
                var user = new ABusers();
                if (firstname == "" || lastname == "" || username == "" || pass == "" || Email == "" || phone == "" || company == "")
                {


                    MessageBox.Show("Some information are missing ,please fill the form completely", "Information Message");
                    return 0;
                }
                if (firstname.Any(c => char.IsNumber(c)))
                {
                    MessageBox.Show("Please enter your firstname in letters,numbers ae not allowed ", "Adminstrator Message");
                    return 0;

                }
                if (lastname.Any(c => char.IsNumber(c)))
                {
                    MessageBox.Show("Please enter your lastname in letters,numbers ae not allowed ", "Adminstrator Message");
                    return 0;

                }
                if (pass.Length < 8)
                {
                    MessageBox.Show("Password is at least 8 characters ", "Adminstrator Message");

                    return 0;
                }


                if (phone.Any(c => char.IsLetter(c)) && phone.Length < 11)
                {
                    MessageBox.Show("Phone is only numeric digits and must be at least 11 number ", "Adminstrator Message");

                    return 0;
                }


                var email = new EmailAddressAttribute();

                bool check = email.IsValid(Email);
                if (!check) {
                    MessageBox.Show("Not the desired format of email ,please enter your email as the format shown beside email textbox", "Adminstrator Message");
                    return 0;
                }


                else
                    return 1;




            
            }
   //End check for user input..................

        public void adduser(ABusers obj)
        {
            try {

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.PostAsJsonAsync("api/journal", obj).Result;

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("New User has been added Sucessfully");
                }
            
                else
                {
                    MessageBox.Show(response.StatusCode + " With Message " + response.ReasonPhrase + "User Name IS Already exist");
                }
            }
            catch (Exception ex) {
                MessageBox.Show("some thing wrong had happend");
            }
        }

        

        //////////////////////////////

    }
}
