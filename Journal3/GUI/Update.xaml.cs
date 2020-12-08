using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
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
    /// Interaction logic for Update.xaml
    /// </summary>
    public partial class Update : Window
    {
        public Update()
        {
            InitializeComponent();
            image_properties.Visibility = Visibility.Hidden;
            crop_canvas.Visibility = Visibility.Hidden;
            resize_canvas.Visibility = Visibility.Hidden;
            rotate_canvas.Visibility = Visibility.Hidden;
        }
        //back button
        private void back_btn(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            MainWindow mw = new MainWindow();
            mw.Visibility = Visibility.Visible;

        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            clear();
        }



        //clear method


        private void clear()
        {
            username_txt.Text = "";
            password_txt.Text = "";
            first_txt.Text = "";
            last_txt.Text = "";
            company_txt.Text = "";
            //combobox.Text = "";
            email_txt.Text = "";
            image_path_txt.Text = "";
            phone_txt.Text = "";
            image_control.Source = null;

        }

        private void update_btn(object sender, RoutedEventArgs e)
        {
            if (id_txt.Text == "" || password_txt.Text == "" || phone_txt.Text == "" || first_txt.Text == "" || last_txt.Text == "" || email_txt.Text == "" || username_txt.Text == "")
            {
                MessageBox.Show("There are some missing content");
            }
            var email = new EmailAddressAttribute();

            bool check = email.IsValid(email_txt.Text);
            if (!check)
                MessageBox.Show("Not the desired format of email ,please enter your email as the format shown beside email textbox", "Adminstrator Message");
            else
            {
                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://localhost:62135/");

                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var user = new ABusers();

                    user.Username = username_txt.Text;
                    byte[] data = UTF8Encoding.UTF8.GetBytes(password_txt.Text);


                    user.Pass = Convert.ToBase64String(data).ToString();

                    user.Email = email_txt.Text;
                    user.FirstName = first_txt.Text;
                    user.LastName = last_txt.Text;
                    user.Company = company_txt.Text;
                    user.Phone = phone_txt.Text;
                    user.Picture = getImageArraybyte(bm);

                    var id = id_txt.Text.Trim();
                    var url = "api/journal/" + id;

                    var response = client.PutAsJsonAsync(url, user).Result;

                    /*if (string.IsNullOrWhiteSpace(this.textBox.Text) && string.IsNullOrWhiteSpace(this.textBox1.Text)
                        && string.IsNullOrWhiteSpace(this.textBox6.Text)
                        && string.IsNullOrWhiteSpace(this.textBox4.Text))
                    {
                        MessageBox.Show("Those cannot be left blank!");
                    }*/

                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("User has been updated successfully");
                        clear();
                        // RetrieveData();
                    }
                }

                catch (Exception ex)
                {
                    MessageBox.Show("Please run the API project first \n" + "Exception Message : " + ex.Message);
                }
            }
        }

        private void search_btn_Click(object sender, RoutedEventArgs e)
        {

        }
        BitmapImage bm;
        private void browse_btn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG Image |*.png | JPG Image |* .jpg";
            ofd.Title = "Open Image";

            bool result = (bool)ofd.ShowDialog();
            string path = ofd.FileName;
            image_path_txt.Text = path;
            if (result)
            {
                bm = new BitmapImage();
                bm.BeginInit();
                bm.UriSource = new Uri(image_path_txt.Text);
                bm.EndInit();
                image_control.Source = bm;
                image_properties.Visibility = Visibility.Visible;
            }
        }

        private void crop_btn(object sender, RoutedEventArgs e)
        {
            image_properties.Visibility = Visibility.Hidden;
            crop_canvas.Visibility = Visibility.Visible;
        }

        private void compress_btn(object sender, RoutedEventArgs e)
        {

        }

        private void resize_btn(object sender, RoutedEventArgs e)
        {
            image_properties.Visibility = Visibility.Hidden;
            resize_canvas.Visibility = Visibility.Visible;
        }

        private void rotate_btn(object sender, RoutedEventArgs e)
        {
            rotate_canvas.Visibility = Visibility.Visible;
        }

        private void apply_btn(object sender, RoutedEventArgs e)
        {
            try
            {
                int X = Convert.ToInt32(X_txt.Text);
                int Y = Convert.ToInt32(Y_txt.Text);
                int height = Convert.ToInt32(height_txt.Text);
                int width = Convert.ToInt32(width_txt.Text);

                string path = image_path_txt.Text;
                Bitmap x = CropImage(X, Y, height, width, path);

                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(@"E:\crop.jpg");
                myBitmapImage.EndInit();
                bm = myBitmapImage;
                image_control.Source =bm;




            }
            catch (Exception ex)
            {
                MessageBox.Show(//"File Not Found\n" +
                    "Exception message : " + ex.Message);


            }

        }

        public Bitmap CropImage(int x, int y, int width, int height, String browse)
        {
            //string imagePath = @"C:\Users\ahmed ^-^\Desktop\GPA.jpg";
            string imagePath = browse;
            string savepath = @"E:\crop.jpg";
            Bitmap croppedImage;


            // Here we capture the resource - image file.
            using (var originalImage = new Bitmap(imagePath))
            {
                System.Drawing.Rectangle crop = new System.Drawing.Rectangle(x, y, width, height);

                // Here we capture another resource.
                croppedImage = originalImage.Clone(crop, originalImage.PixelFormat);

            } // Here we release the original resource - bitmap in memory and file on disk.

            // At this point the file on disk already free - you can record to the same path.
            croppedImage.Save(savepath, ImageFormat.Jpeg);

            // It is desirable release this resource too.
            croppedImage.Dispose();
            return croppedImage;
        }




        //saving the image in database 

        private byte[] getImageArraybyte(BitmapImage image)
        {

            MemoryStream ms = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(ms);
            return ms.ToArray();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Apply_rotate(object sender, RoutedEventArgs e)
        {
            try
            {
                string text = rotate_txt.Text;
                double number = Convert.ToDouble(text);

                if (text == "")
                    MessageBox.Show("The textbox is empty ,please enter a suitable degree");
                else
                {
                    TransformedBitmap transformation = new TransformedBitmap();
                    transformation.BeginInit();
                    transformation.Source = bm;
                    RotateTransform rotate = new RotateTransform(number);
                    transformation.Transform = rotate;
                    transformation.EndInit();
                    image_control.Source = transformation;
                }
            }

            catch (Exception)
            {

                MessageBox.Show("please enteronethe following degrees 0,90,180,270,360");


            }
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (width_text.Text == "" | height_text.Text == "")
                MessageBox.Show("PLease enter the height and the width of the image", "Content Required Message");
            else
            {

                int w = Convert.ToInt32(width_text.Text);
                int h = Convert.ToInt32(height_text.Text);


                TransformedBitmap tb = new TransformedBitmap();
                tb.BeginInit();
                tb.Source = bm;
                tb.Transform = new ScaleTransform(bm.PixelWidth / w, bm.PixelHeight / h);
                tb.EndInit();
                image_control.Source = tb;
            }
        }

        private void rotate_back_btn(object sender, RoutedEventArgs e)
        {
            rotate_canvas.Visibility = Visibility.Hidden;
            image_properties.Visibility = Visibility.Visible;
        }

        private void resize_back_btn(object sender, RoutedEventArgs e)
        {
            resize_canvas.Visibility = Visibility.Hidden;
            image_properties.Visibility = Visibility.Visible;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            crop_canvas.Visibility = Visibility.Hidden;
            image_properties.Visibility = Visibility.Visible;
        }

        private void X_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void height_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void Y_txt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Y_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void width_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void width_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void height_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void rotate_txt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void rotate_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void id_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void phone_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }
    }
}