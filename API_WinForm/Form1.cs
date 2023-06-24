using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace API_WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadImageFromAPIAsync();
        }

        private async Task LoadImageFromAPIAsync()
        {

            string imageUrl = "https://pixabay.com/api/?key=37619946-e0bf9bf12e55406f12d15ff99&q=yellow+flowers&image_type=photo";

            try
            {
                WebClient client = new WebClient();
                byte[] imageData = await client.DownloadDataTaskAsync(imageUrl);
                using (MemoryStream ms = new MemoryStream(imageData))
                {
                    Image image = Image.FromStream(ms);
                    pictureBox_Pictute.Image = image;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при загрузке изображения: " + ex.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void button_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();        
        }  
    }
}
