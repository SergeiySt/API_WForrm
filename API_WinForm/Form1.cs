using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace API_WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "yellow flowers";
        }

        static readonly HttpClient client = new HttpClient();
        private System.Windows.Forms.ProgressBar progressBar;

        private async void Form1_Load(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            await LoadImageFromAPIAsync(textBox1.Text);
        }

        private async Task LoadImageFromAPIAsync(string query)
        {
            //try
            //{
            //    string apiKey = "37619946-e0bf9bf12e55406f12d15ff99";
            //    int imageCount = 50; 
            //    string apiUrl = $"https://pixabay.com/api/?key={apiKey}&q={Uri.EscapeDataString(query)}&image_type=photo&per_page={imageCount}";

            //    HttpResponseMessage response = await client.GetAsync(apiUrl);
            //    response.EnsureSuccessStatusCode();

            //    string jsonResponse = await response.Content.ReadAsStringAsync();

            //    var result = JsonConvert.DeserializeObject<PixabayResponse>(jsonResponse);

            //    if (result != null && result.hits != null && result.hits.Length > 0)
            //    {
            //        for (int i = 0; i < imageCount; i++)
            //        {
            //            var imageHit = result.hits[i];

            //            using (WebClient webClient = new WebClient())
            //            {
            //                byte[] imageData = webClient.DownloadData(imageHit.largeImageURL);

            //                using (MemoryStream ms = new MemoryStream(imageData))
            //                {
            //                    Image image = Image.FromStream(ms);

            //                    PictureBox pictureBox = new PictureBox();
            //                    pictureBox.Image = image;
            //                    pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            //                    pictureBox.Width = 200;
            //                    pictureBox.Height = 200;

            //                    flowLayoutPanel1.Controls.Add(pictureBox);
            //                }
            //            }
            //        }
            //    }
            //    else
            //    {
            //        Console.WriteLine("Рисунки не найдены.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
            //}

            try
            {
                string apiKey = "37619946-e0bf9bf12e55406f12d15ff99";
                int imageCount = 50;
                string apiUrl = $"https://pixabay.com/api/?key={apiKey}&q={Uri.EscapeDataString(query)}&image_type=photo&per_page={imageCount}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);
                response.EnsureSuccessStatusCode();

                string jsonResponse = await response.Content.ReadAsStringAsync();

                var result = JsonConvert.DeserializeObject<PixabayResponse>(jsonResponse);

                if (result != null && result.hits != null && result.hits.Length > 0)
                {
                    progressBar = new System.Windows.Forms.ProgressBar();
                    progressBar.Minimum = 0;
                    progressBar.Maximum = imageCount;
                    progressBar.Value = 0;

                    // Добавляем ProgressBar на форму
                    flowLayoutPanel1.Controls.Add(progressBar);

                    for (int i = 0; i < imageCount; i++)
                    {
                        var imageHit = result.hits[i];

                        using (WebClient webClient = new WebClient())
                        {
                            byte[] imageData = webClient.DownloadData(imageHit.largeImageURL);

                            using (MemoryStream ms = new MemoryStream(imageData))
                            {
                                Image image = Image.FromStream(ms);

                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Image = image;
                                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox.Width = 200;
                                pictureBox.Height = 200;

                                flowLayoutPanel1.Controls.Add(pictureBox);

                                // Выполняем асинхронную загрузку рисунка с отслеживанием прогресса
                                await LoadImageAsync(imageHit.largeImageURL, pictureBox);

                                // Обновляем значение ProgressBar при завершении загрузки каждого рисунка
                                progressBar.Value = i + 1;
                            }
                        }
                    }

                    // Удаляем ProgressBar после завершения загрузки всех рисунков
                    flowLayoutPanel1.Controls.Remove(progressBar);
                    progressBar.Dispose();
                }
                else
                {
                    Console.WriteLine("Рисунки не найдены.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при выполнении запроса: " + ex.Message);
            }
        }

        private async Task LoadImageAsync(string imageUrl, PictureBox pictureBox)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] imageData = await webClient.DownloadDataTaskAsync(imageUrl);

                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(ms);

                    pictureBox.Image = image;
                }
            }
        }

        public class PixabayResponse
        {
            public ImageHit[] hits { get; set; }
        }

        public class ImageHit
        {
            public string largeImageURL { get; set; }
        }
        private void button_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();        
        }

        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
           
        }

        private async void button_Search_Click_1(object sender, EventArgs e)
        {
            flowLayoutPanel1.Controls.Clear();
            string query = textBox1.Text;
            await LoadImageFromAPIAsync(query);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
