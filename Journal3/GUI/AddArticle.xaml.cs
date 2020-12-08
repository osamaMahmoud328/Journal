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
    /// Interaction logic for AddArticle.xaml
    /// </summary>
    public partial class AddArticle : Window
    {
        public AddArticle()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {


            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == ""  || textBox.Text == "" )
            {
                MessageBox.Show("There are some missing content");
            }
            else
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var Article = new NewArticle();

                try
                {
                    Article.AName = textBox.Text;
                    Article.ARtName = textBox1.Text;
                    Article.Article_ID= Convert.ToInt16(textBox2.Text);
                    Article.Subj_Article = textBox3.Text;
                    Article.Article_Date = Convert.ToDateTime(textBox4.Text);
                    Article.ID = Convert.ToInt16(textBox6.Text);
                    Article.Type_user = combobox.Text;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
                try
                {
                    var response = client.PostAsJsonAsync("api/Article", Article).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("New Article has been added Succesfully");
                        clearTexts();
                    }
                    else
                    {
                        MessageBox.Show(response.StatusCode + "With Message " + response.ReasonPhrase);
                    }
                }
                catch {
                    MessageBox.Show("No connection to the server","Connection Error");
                }
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {

            BusinessUser mw = new BusinessUser();
            this.Visibility = Visibility.Hidden;
            mw.Visibility = Visibility.Visible;
        }
        private void clearTexts()
        {
            textBox.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
           combobox.Text = "";
            textBox6.Text = "";
        }

        private void textBox2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void textBox6_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void textBox_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox4_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox4.Text = "";
        }
    }
}
