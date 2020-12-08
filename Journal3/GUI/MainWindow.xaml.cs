using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           
            search_canvas.Visibility = Visibility.Hidden;
            image_properties.Visibility = Visibility.Hidden;
            resize_canvas.Visibility = Visibility.Hidden;
            delete_canvas.Visibility = Visibility.Hidden;
            rotate_canvas.Visibility = Visibility.Hidden;
            crop_canvas.Visibility = Visibility.Hidden;
          //  MessageBox.Show(encrypt("ramy"));
          //  MessageBox.Show(dcrypt(encrypt("ramy")));
             
        }

        //start of Retrieving Data
        private void retrieveData()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync("api/journal").Result;
                if (response.IsSuccessStatusCode)
                {
                    var Users = response.Content.ReadAsAsync<IEnumerable<ABusers>>().Result;
                  
                    DataGrid.ItemsSource = Users;

                }

                else
                    MessageBox.Show(response.StatusCode + " With Message " + response.ReasonPhrase);

            }

            catch {

                MessageBox.Show("No connection to the server,please check your connection", "Connection Error");
            }

            }
        //End of retreiving Data 


        BitmapImage bm;
        
        //Upload image button
        private void Button_Click(object sender, RoutedEventArgs e)
        {


        }//End of upload button


        private byte[] getImageArraybyte(BitmapImage image)
        {

            MemoryStream ms = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(ms);
            return ms.ToArray();

        }


        //browse button 
        private void Browse_Button_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "PNG Image |*.png | JPG Image |* .jpg";
            ofd.Title = "Open Image";

            bool result = (bool)ofd.ShowDialog();
            string path = ofd.FileName;
            imagetext.Text = path;
            if (result)
            {
                bm = new BitmapImage();
                bm.BeginInit();
                bm.UriSource = new Uri(imagetext.Text);
                bm.EndInit();
                username_image.Source = bm;
                image_properties.Visibility = Visibility.Visible;

            }
            
        
        }

      
        private void clearTexts()
        {
            username_txt.Text = "";
            firstname_txt.Text = "";
            lastname_txt.Text = "";
            password_txt.Text = "";
            email_txt.Text = "";
            phone_txt.Text = "";
            company_txt.Text = "";
            combobox.Text = "";
            imagetext.Text = "";


        }

        private void combobox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Login l = new Login();
            l.Visibility = Visibility.Visible;
        }

        private void resize_button(object sender, RoutedEventArgs e)
        {
            image_properties.Visibility = Visibility.Hidden;
            resize_canvas.Visibility = Visibility.Visible;
        }

        //apply inside canvas
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
                username_image.Source = tb;
               
            }
        }

      

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {

        }
        //Button search 
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            search_canvas.Visibility = Visibility.Visible;
            delete_canvas.Visibility = Visibility.Hidden;


            }

        private void go_button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:62135/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var id = search_text.Text.Trim();
            var url = "api/journal/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                var users = response.Content.ReadAsAsync<ABusers>().Result;
                List<ABusers> userlist = new List<ABusers>();
                userlist.Add(users);
                DataGrid.ItemsSource = userlist;
                UserProfile up = new UserProfile();
                up.first_txt.Text = users.FirstName;
                up.last_txt.Text = users.LastName;
                up.username_txt.Text = users.Username;
                up.company_txt.Text = users.Company;
                up.role_txt.Text = users.Type_user;
                up.search_control.Source = getarray(users.Picture);

                up.Visibility = Visibility.Visible;
                search_canvas.Visibility = Visibility.Hidden;

            }
            else
                MessageBox.Show(response.StatusCode + " With Message " + response.ReasonPhrase);
        }

        private BitmapImage getarray(Byte[] imageData)
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        
        }

        //button delete 
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            search_canvas.Visibility = Visibility.Hidden;
            delete_canvas.Visibility = Visibility.Visible;
           


        }

        private void delete_button_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:62135/");
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var id = delete_text.Text.Trim();
            var url = "api/journal/" + id;
            HttpResponseMessage response = client.DeleteAsync(url).Result;
            if (response.IsSuccessStatusCode)
                MessageBox.Show("User with id "+id+" is deleted successfully ", "Delete User");
            else
                MessageBox.Show(response.StatusCode + " With Message " + response.ReasonPhrase);


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
        //End of the encryption method 

        //Dycrpt the password








            //End of dcryption method 


        private void showusers_btn(object sender, RoutedEventArgs e)
        {
           
            retrieveData();

        }

        private void adduser_btn(object sender, RoutedEventArgs e)
        {
            try {

                var user = new ABusers();
                if (firstname_txt.Text == "" || lastname_txt.Text == "" || password_txt.Text == "" || email_txt.Text == "" || phone_txt.Text == "" || company_txt.Text == "" || combobox.Text == "" || imagetext.Text == "")
                {


                    MessageBox.Show("Some information are missing ,please fill the form completely", "Information Message");

                }
                if (firstname_txt.Text.Any(c => char.IsNumber(c)))
                {
                    MessageBox.Show("Please enter your firstname in letters,numbers ae not allowed ", "Adminstrator Message");
                    firstname_txt.Focus();

                }
                if (lastname_txt.Text.Any(c => char.IsNumber(c)))
                {
                    MessageBox.Show("Please enter your lastname in letters,numbers ae not allowed ", "Adminstrator Message");
                    firstname_txt.Focus();

                }
                if (password_txt.Text.Length < 8)
                {
                    MessageBox.Show("Password is at least 8 characters ", "Adminstrator Message");
                    password_txt.Focus();

                }


                if (phone_txt.Text.Any(c => char.IsLetter(c)) && phone_txt.Text.Length < 11)
                {
                    MessageBox.Show("Phone is only numeric digits and must be at least 11 number ", "Adminstrator Message");
                    password_txt.Focus();

                }

                var email = new EmailAddressAttribute();

                bool check = email.IsValid(email_txt.Text);
                if (!check)
                    MessageBox.Show("Not the desired format of email ,please enter your email as the format shown beside email textbox", "Adminstrator Message");

                else
                {



                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri("http://localhost:62135/");
                    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                    user.FirstName = firstname_txt.Text;
                    user.LastName = lastname_txt.Text;
                    user.Username = username_txt.Text;


                    byte[] data = UTF8Encoding.UTF8.GetBytes(password_txt.Text);
                    user.Pass = Convert.ToBase64String(data).ToString();
                    user.Email = email_txt.Text;
                    user.Phone = phone_txt.Text;
                    user.Company = company_txt.Text;
                    user.Type_user = combobox.Text;
                    user.Picture = getImageArraybyte(bm);
                    var response = client.PostAsJsonAsync("api/journal", user).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show("New User has been added Sucessfully");
                        clearTexts();
                        retrieveData();

                    }

                    else
                    {
                        MessageBox.Show(response.StatusCode + " With Message " + response.ReasonPhrase);


                    }
                } }
            catch(Exception ex)
            {
                MessageBox.Show("Username is already exist");
            }
        }

        

        private void Exit_search(object sender, RoutedEventArgs e)
        {
            search_canvas.Visibility = Visibility.Hidden;

       
        }

        private void Exit_delete(object sender, RoutedEventArgs e)
        {
            delete_canvas.Visibility = Visibility.Hidden;
        }

      
        private void rotate_button(object sender, RoutedEventArgs e)
        {
            rotate_canvas.Visibility = Visibility.Visible;
        }

        private void Apply_rotate(object sender, RoutedEventArgs e)
        {
            string text = rotate_txt.Text;
     
            if (text == "")
                MessageBox.Show("The textbox is empty ,please enter a suitable degree");
            else {
                double number = Convert.ToDouble(text);
                TransformedBitmap transformation = new TransformedBitmap();
                transformation.BeginInit();
                transformation.Source = bm;
                RotateTransform rotate = new RotateTransform(number);
                transformation.Transform = rotate;
                transformation.EndInit();
                username_image.Source = transformation;

                //Image i = new Image();
                //i.image_deal.Source = transformation;
                //i.image_deal.Visibility = Visibility.Visible;
                //i.Visibility = Visibility.Visible;



            }

        }

      

        private void admins_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync("api/admins/check").Result;
                if (response.IsSuccessStatusCode)
                {
                    var users = response.Content.ReadAsAsync<IEnumerable<ABusers>>().Result;
                    DataGrid.ItemsSource = users;



                }

                else
                {

                    MessageBox.Show(response.StatusCode + "With Message" + response.ReasonPhrase);


                }

            }

            catch {
                MessageBox.Show("No connection to the server ,please check yuor connection", "Connection Error");
            }
        }

        private void users_btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:62135/");
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync("api/Business/users").Result;
                if (response.IsSuccessStatusCode)
                {
                    var users = response.Content.ReadAsAsync<IEnumerable<ABusers>>().Result;
                    DataGrid.ItemsSource = users;



                }

                else
                {

                    MessageBox.Show(response.StatusCode + "With Message" + response.ReasonPhrase);


                }
            }
            catch {
                MessageBox.Show("No connection to the server ,please check your connection to the server", "Connection Error");

            }

        }

       
        private void update_btn(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            Update u = new Update();
            u.Visibility = Visibility.Visible;
        }

        private void apply_btn(object sender, RoutedEventArgs e)
        {
            try
            {
                int X = Convert.ToInt32(X_txt.Text);
                int Y = Convert.ToInt32(Y_txt.Text);
                int height = Convert.ToInt32(height_txt.Text);
                int width = Convert.ToInt32(width_txt.Text);

                string path = imagetext.Text;
                Bitmap x = CropImage(X, Y, height, width, path);

                BitmapImage myBitmapImage = new BitmapImage();
                myBitmapImage.BeginInit();
                myBitmapImage.UriSource = new Uri(@"E:\crop.jpg");
                myBitmapImage.EndInit();
                bm = myBitmapImage;
                username_image.Source = bm;




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

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            clearTexts();
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

        private void crop_btn(object sender, RoutedEventArgs e)
        {
            image_properties.Visibility = Visibility.Hidden;
            crop_canvas.Visibility = Visibility.Visible;
        }

        private void crop_cancel_btn(object sender, RoutedEventArgs e)
        {
            crop_canvas.Visibility = Visibility.Hidden;
            image_properties.Visibility = Visibility.Visible;
          
        }

        private void phone_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
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

        private void rotate_txt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void delete_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
                e.Handled = true;
        }

        private void search_text_PreviewTextInput(object sender, TextCompositionEventArgs e)
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
    }
    }


