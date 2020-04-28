using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Weather_Conditions
{
    public partial class Form1 : Form
    {
        // Global variable with server's address
        readonly string BaseUrl = "Http://weather-csharp.herokuapp.com/";
        public Form1()
        {
            InitializeComponent();
        }

        private bool GetWeatherText(string city, string state, out string weatherText, out string errorMessage)
        {
            // Use format method to make a string in the format
            // http://weather-csharp.herokuapp.com/text?city=dallas&state=tx

            string weatherTextUrl = string.Format("{0}text?city={1}&state={2}", BaseUrl, city, state);
            Debug.WriteLine(weatherTextUrl);

            errorMessage = null;
            weatherText = null;

            try
            {
                using (WebClient client = new WebClient())
                {
                    weatherText = client.DownloadString(weatherTextUrl);
                }

                Debug.WriteLine(weatherText);
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                errorMessage = e.Message;
                return false;
            }
        }

        private bool GetWeatherImage(string city, string state, out Image weatherImage, out string errorMessage)
        {
            weatherImage = null;
            errorMessage = null;

            try
            {
                using (WebClient client = new WebClient())
                {
                    // use the format method to make a string in the format
                    // http://weather-csharp.herokuapp.com/photo?city=dallas&state=tx
                    string weatherPhotoUrl = String.Format("{0}photo?city={1}&state={2}", BaseUrl, city, state);
                    string tempFileDirectory = Path.GetTempPath().ToString(); // directory to save the image
                    String weatherFilePath = Path.Combine(tempFileDirectory, "weather_image.jpeg"); // plus filename to make a complete path
                    Debug.WriteLine(weatherFilePath);
                    client.DownloadFile(weatherPhotoUrl, weatherFilePath); // download image from url
                    weatherImage - Image.FromFile(weatherFilePath); // clarify with clara
                }
                return true;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
                errorMessage = e.Message;
                return false;
            }
        }

        private bool LocationDataValid(string city, string state)
        {
            // Make checks on data, return false if any fail
            if (String.IsNullOrWhiteSpace(city))
            {
                return false;
            }
            if (String.IsNullOrWhiteSpace(state))
            {
                return false;
            }
            return true;
        }



        private void btnGetWeather_Click(object sender, EventArgs e)
        {
            // disable button to prevent the user from making another request until the current one is done
            btnGetWeather.Enabled = false;

            // Read data from the textboxes
            string city = txtCity.Text;
            string state = txtState.Text;

            if (LocationDataValid(city, state))
            {
                // Fetch current weather and display it
                if (GetWeatherText(city, state, out string weather, out string errorMessage))
                {
                    lblWeather.Text = weather;
                }
                else
                {
                    MessageBox.Show("There was an unexpected error", "Error");
                }
                if (picWeather.Image != null)
                {
                    picWeather.Image.Dispose();
                }
                if (GetWeatherImage(city, state, out Image image, out string imageerrorMessage))
                {
                    picWeather.Image = image;
                }
            }
            else
            {
                MessageBox.Show("Enter both a city and state", "Error");
            }

                // Enable button so user can do another search
                btnGetWeather.Enabled = true;
            
        }
    }
}
