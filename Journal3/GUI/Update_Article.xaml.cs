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
    /// Interaction logic for Update_Article.xaml
    /// </summary>
    public partial class Update_Article : Window
    {
        public Update_Article()
        {
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            if (textBox.Text == "" || textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
            {
                MessageBox.Show("There are some missing content");
            }
            else
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");

                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var article = new NewArticle();

                article.AName = textBox.Text;
                article.ARtName = textBox1.Text;
                article.Subj_Article = textBox3.Text;
                article.Article_Date = Convert.ToDateTime(textBox4.Text);
                article.ID = Convert.ToInt16(textBox5.Text);
                article.Type_user = textBox6.Text;

                var id = textBox2.Text;
                var url = "api/Article/" + id;

                var response = client.PutAsJsonAsync(url, article).Result;

                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("Article with ID" + textBox4.Text + "has been updated successfully");
                        clearTexts();
                    }
                    else
                    {
                        MessageBox.Show("There is an Error");

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Convert.ToString(ex));
                }
            }
        }

        private void clearTexts()
        {
            textBox.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        private void textBox2_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void textBox5_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void textBox5_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void textBox4_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox4.Text = "";
        }
    }
}
