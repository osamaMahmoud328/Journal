using JournalAccess;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace GUI
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        Articlet ar = new Articlet();
        public Login()
        {
            InitializeComponent();

            fun();
            register_canvas.Visibility = Visibility.Visible;
            rotate_canvas.Visibility = Visibility.Hidden;
            resize_canvas.Visibility = Visibility.Hidden;
            crop_canvas.Visibility = Visibility.Hidden;
            image_properties.Visibility = Visibility.Hidden;
        }
        private BitmapImage getarray(byte[] imageData)
        {
            if (imageData == null)
                return null;

            else
            {
                var image = new BitmapImage();
                using (var mem = new MemoryStream(imageData))
                {
                    mem.Position = 0;
                    image.BeginInit();
                    image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = null;
                    image.StreamSource = mem;
                    image.EndInit();
                }
                image.Freeze();
                return image;
            }
        }


        public void fun()
        {
            Articlet art = new Articlet();
            Visitors_grid.ItemsSource = art.retrieve();

        }





        private byte[] getImageArraybyte(BitmapImage image) {
            MemoryStream ms = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(ms);
            return ms.ToArray();

        }

        //ENCRYPT THE PASSWORD


        public string encrypt(string password)
        {
            string result = "";
            for (int i = 0; i < password.Length; i++)
            {
                result += (char)(password[i] + 1);
            }

            return result;
        }

        public string Decrypt(byte[] Pass)
        {
            string res = Encoding.UTF8.GetString(Pass);
            return res;

        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BusinessUser bs = new BusinessUser();
            MainWindow mw = new MainWindow();

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:62135/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var user = username_txt.Text.Trim();
            var url = "api/Login/" + user;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var Users = response.Content.ReadAsAsync<ABusers>().Result;


                    byte[] pss = Convert.FromBase64String(Users.Pass);
                    string ne = Decrypt(pss);

                    if (ne == passwordbox.Password.ToString())
                    {
                        MessageBox.Show("Welcome To Our Service, Enjoy Your Day");
                        if (Users.Type_user == "Admin")
                        {
                            this.Visibility = Visibility.Hidden;
                            mw.Visibility = Visibility.Visible;

                            HttpClient client3 = new HttpClient();
                            client3.BaseAddress = new Uri("http://localhost:62135/");

                            client3.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                            var AName = username_txt.Text;
                            var url3 = "api/Login/" + AName;

                            var response3 = client3.GetAsync(url3).Result;

                            try
                            {
                                if (response3.IsSuccessStatusCode)
                                {
                                    var user2 = response3.Content.ReadAsAsync<ABusers>().Result;
                                    List<ABusers> list = new List<ABusers>();
                                    list.Add(user2);
                                    mw.username_label.Content = user2.Username;
                                    mw.image1.Source = getarray(user2.Picture);
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
                        else
                        {
                            this.Visibility = Visibility.Hidden;
                            bs.Visibility = Visibility.Visible;
                        }
                    }
                    else
                    { MessageBox.Show("Wrong Password"); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("User Name Or Password Are Wrong");
                }
            }
            else
            {
                MessageBox.Show(response.StatusCode + "not found" + response.ReasonPhrase);
            }

            HttpClient client2 = new HttpClient();
            client2.BaseAddress = new Uri("http://localhost:62135/");

            client2.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var UName = username_txt.Text;
            var url2 = "api/Login/" + UName;

            var response2 = client2.GetAsync(url2).Result;

            try
            {
                if (response2.IsSuccessStatusCode)
                {
                    var user2 = response2.Content.ReadAsAsync<ABusers>().Result;
                    List<ABusers> list = new List<ABusers>();
                    list.Add(user2);
                    bs.username.Content = user2.Username;
                    bs.image_control.Source = getarray(user2.Picture);
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
        
 
        private void cancel_btn(object sender, RoutedEventArgs e)
        {
            register_canvas.Visibility = Visibility.Hidden;
            login_canvas.Visibility = Visibility.Visible;

        }

        private void submit_btn(object sender, RoutedEventArgs e)
        {
            var user = new ABusers();

            try
            {
                //check for user inpu........

                if (user.check(UserName_T.Text, FirstName_T.Text, LastName_T.Text, Email_txt.Text, Password_T.Text, company_txt.Text,phone_txt.Text) == 1)
                {
                    // End check for user inpu........

                    //set user input to object..........

                    user.setAll(UserName_T.Text, FirstName_T.Text, LastName_T.Text, Email_txt.Text, Password_T.Text, company_txt.Text, phone_txt.Text);
                    //End setting user input to object..........

                    MessageBox.Show(user.Username + user.FirstName + user.LastName + user.Pass + user.Phone + user.AB_iD + user.Company);

                    // Add New User.....
                    user.Picture = getImageArraybyte(bm);
                    user.adduser(user);

                    //End Adding New User.....
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("some thing happend please try to fill your info again");
            }
        }


        private void register_btn(object sender, RoutedEventArgs e)
        {
            login_canvas.Visibility = Visibility.Hidden;
            register_canvas.Visibility = Visibility.Visible;

        }
        BitmapImage bm;
        private void browse_btn(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG Image |*.png | JPG Image |* .jpg";
            ofd.Title = "Open Image";

            bool result = (bool)ofd.ShowDialog();
            string path = ofd.FileName;
            image_text.Text = path;
            if (result)
            {
                bm = new BitmapImage();
                bm.BeginInit();
                bm.UriSource = new Uri(image_text.Text);
                bm.EndInit();
                image_control.Source = bm;
                image_properties.Visibility = Visibility.Visible;
            }
        }

        private void clear_btn(object sender, RoutedEventArgs e)
        {
            clearTexts();
        }

        private void clearTexts()
        {
            UserName_T.Text = "";
            FirstName_T.Text = "";
            LastName_T.Text = "";
            Password_T.Text = "";
            Email_txt.Text = "";
            phone_txt.Text = "";
            company_txt.Text = "";
            image_text.Text = "";

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

        private void resize_button(object sender, RoutedEventArgs e)
        {
            image_properties.Visibility = Visibility.Hidden;
            resize_canvas.Visibility = Visibility.Visible;
        }

        private void resize_back_btn(object sender, RoutedEventArgs e)
        {
            resize_canvas.Visibility = Visibility.Hidden;
            image_properties.Visibility = Visibility.Visible;
        }

        private void rotate_button(object sender, RoutedEventArgs e)
        {
            image_properties.Visibility = Visibility.Hidden;
            rotate_canvas.Visibility = Visibility.Visible;

        }

        private void rotate_back_btn(object sender, RoutedEventArgs e)
        {
            image_properties.Visibility = Visibility.Visible;
            rotate_canvas.Visibility = Visibility.Hidden;
        }

        private void Apply_rotate(object sender, RoutedEventArgs e)
        {
            string text = rotate_txt.Text;

            if (text == "")
                MessageBox.Show("The textbox is empty ,please enter a suitable degree");
            else
            {
                double number = Convert.ToDouble(text);
                TransformedBitmap transformation = new TransformedBitmap();
                transformation.BeginInit();
                transformation.Source = bm;
                RotateTransform rotate = new RotateTransform(number);
                transformation.Transform = rotate;
                transformation.EndInit();
                image_control.Source = transformation;

            }
        }

        private void crop_btn(object sender, RoutedEventArgs e)
        {
            image_properties.Visibility = Visibility.Hidden;
            crop_canvas.Visibility = Visibility.Visible;
        }

        private void crop_cancel_btn(object sender, RoutedEventArgs e)
        {
            image_properties.Visibility = Visibility.Visible;
            crop_canvas.Visibility = Visibility.Hidden;
        }

        private void apply_btn(object sender, RoutedEventArgs e)
        {
            try
            {
                int X = Convert.ToInt32(X_txt.Text);
                int Y = Convert.ToInt32(Y_txt.Text);
                int height = Convert.ToInt32(height_txt.Text);
                int width = Convert.ToInt32(width_txt.Text);

                string path = image_text.Text;
                Bitmap x = CropImage(X, Y, height, width, path);

                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(@"E:\crop.jpg");
                myBitmapImage.EndInit();
                bm = myBitmapImage;
                image_control.Source = bm;

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


        //compress button
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
           

          //  Compress(getImageArraybyte(bm));

        }
        public static byte[] Compress(byte[] data)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, CompressionLevel.Optimal))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        private void phone_txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void textBox_GotFocus(object sender, RoutedEventArgs e)
        {
            textBox.Text = "";
        }

        private void X_txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void Label_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void rotate_txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void Y_txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void width_txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void height_txt_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void height_text_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void width_text_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void Visitors_grid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Articlet ar = new Articlet();
            List<Articlet> er = new List<Articlet>();
            er.Add(ar.search(textBox.Text.Trim()));
            Visitors_grid.ItemsSource=  er;
        }


        private void image_text_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}