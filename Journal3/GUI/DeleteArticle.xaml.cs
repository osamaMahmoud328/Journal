using System;
using System.Collections.Generic;
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
    /// Interaction logic for DeleteArticle.xaml
    /// </summary>
    public partial class DeleteArticle : Window
    {
        public DeleteArticle()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == "")
            {
                MessageBox.Show("Please Enter The ID");
            }
            else
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var ID = textBox.Text;
                var url = "api/Article/" + ID;

                HttpResponseMessage response = client.DeleteAsync(url).Result;
                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Article with ID : " + ID + " is Deleted");
                    }
                    else
                    {
                        MessageBox.Show(response.StatusCode + "With Message" + response.ReasonPhrase);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }

        }
        private void textBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}
