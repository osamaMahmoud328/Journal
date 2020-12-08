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
    /// Interaction logic for BusinessUser.xaml
    /// </summary>
    public partial class BusinessUser : Window
    {
        public BusinessUser()
        {
            InitializeComponent();
            header.Content = "Welcome";
        }
     

        private void RetrieveData()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {

                    HttpResponseMessage respone = client.GetAsync("api/Article").Result;
                    if (respone.IsSuccessStatusCode)
                    {
                        var data = respone.Content.ReadAsAsync<IEnumerable<NewArticle>>().Result;
                        dataGrid.ItemsSource = data;
                    }
                }

                catch (Exception e)
                {
                    string x = Convert.ToString(e);
                    MessageBox.Show(x);
                }
            }

            catch
            {

                MessageBox.Show("No connection to the server", "Connection Error");
            }
        }

        private void update_btn(object sender, RoutedEventArgs e)
        {
            Update_Article up = new Update_Article();
            up.Visibility = Visibility.Visible;
        }

        private void Add_article_btn(object sender, RoutedEventArgs e)
        {
            AddArticle ar = new AddArticle();
            this.Visibility = Visibility.Hidden;
            ar.Visibility = Visibility.Visible;
        }

        private void retrieve_btn(object sender, RoutedEventArgs e)
        {
            RetrieveData();
        }

        private void add_author_btn(object sender, RoutedEventArgs e)
        {
            AddAuthor aa = new AddAuthor();
            this.Visibility = Visibility.Hidden;
            aa.Visibility = Visibility.Visible;

        }

        private void view_profile_btn(object sender, RoutedEventArgs e)
        {

            ViewProfile vp = new ViewProfile();
            this.Visibility = Visibility.Hidden;
            vp.Visibility = Visibility.Visible;

        }
        private void delete_btn(object sender, RoutedEventArgs e)
        {
            DeleteArticle del = new DeleteArticle();
            del.Visibility = Visibility.Visible;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
            Login l = new Login();
            l.Visibility = Visibility.Visible;
        }

        private void button6_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                try
                {

                    HttpResponseMessage respone = client.GetAsync("api/Author").Result;
                    if (respone.IsSuccessStatusCode)
                    {
                        var data = respone.Content.ReadAsAsync<IEnumerable<NewAuthor>>().Result;
                        dataGrid.ItemsSource = data;
                    }
                }

                catch (Exception ex)
                {
                    string x = Convert.ToString(ex);
                    MessageBox.Show(x);
                }
            }
            catch (Exception ex)
            {
                string x = Convert.ToString(ex);
                MessageBox.Show(x);
            }
        }
    }

}