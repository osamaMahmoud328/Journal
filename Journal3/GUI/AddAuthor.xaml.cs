using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for AddAuthor.xaml
    /// </summary>
    public partial class AddAuthor : Window
    {
        public AddAuthor()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            if (textBox.Text == "" && textBox2.Text == "" && textBox2.Text == "" && textBox3.Text == "")
            {
                MessageBox.Show("There is a field is empty");
            }
            else
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Author = new NewAuthor();
                try
                {
                    Author.AName = textBox.Text;
                    Author.Email = textBox1.Text;
                    Author.Company = textBox2.Text;
                    Author.Phone = textBox3.Text;
                }
                catch (Exception)
                {
                    MessageBox.Show("feh 8lt");
                }

                var response = client.PostAsJsonAsync("api/Author", Author).Result;
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("New Author has been added Succesfully");
                    clearTexts();
                }
                else
                {
                    MessageBox.Show(response.StatusCode + "With Message " + response.ReasonPhrase);
                }
            }


        }



        private void clearTexts()
        {
            textBox.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
        }
        private void button_Click(object sender, RoutedEventArgs e)
        {
            BusinessUser ma = new BusinessUser();
            this.Visibility = Visibility.Hidden;
            ma.Visibility = Visibility.Visible;

        }
    }
}
