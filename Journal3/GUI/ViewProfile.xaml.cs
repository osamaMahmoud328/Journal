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
    /// Interaction logic for ViewProfile.xaml
    /// </summary>
    public partial class ViewProfile : Window
    {
        public ViewProfile()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "")
            {
                MessageBox.Show("Please Enter Author Name");
            }
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Profile = new NewAuthor();
                var AName = textBox.Text;
                var url = "api/Author/" + AName;

                var response = client.GetAsync(url).Result;

                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var details = response.Content.ReadAsAsync<NewAuthor>().Result;
                        List<NewAuthor> list = new List<NewAuthor>();

                        list.Add(details);

                        textBox1.Text = details.AName;
                        textBox2.Text = details.Email;
                        textBox3.Text = details.Company;
                        textBox4.Text = details.Phone;
                    }
                    else
                    {
                        MessageBox.Show(response.StatusCode + "With Message " + response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            BusinessUser mw = new BusinessUser();
            this.Visibility = Visibility.Hidden;
            mw.Visibility = Visibility.Visible;
        }
        private void textBox_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            textBox.Text = " ";
        }
    }
}
