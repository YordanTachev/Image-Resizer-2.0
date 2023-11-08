using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Image_Resizer
{
    public partial class Form1 : Form
    {
        Bitmap orgImage;

        int newWidth;
        int newHeight;

        Bitmap newImage;
        int sF;

        void ThreadWOrker1(int from, int to)
        {

            for (int x = from; x < to; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    int red = 0, green = 0, blue = 0, pixelCount = 0;

                    for (int newX = 0; newX < orgImage.Width / 2; newX++)
                    {

                        for (int newY = 0; newY < orgImage.Height; newY++)
                        {
                            int finalX = x * orgImage.Width / newWidth;
                            int finalY = y * orgImage.Height / newHeight;

                            Color color = orgImage.GetPixel(finalX, finalY);
                            red += color.R;
                            green += color.G;
                            blue += color.B;
                            pixelCount++;
                        }
                    }

                    Color averageColor = Color.FromArgb(red / pixelCount, green / pixelCount, blue / pixelCount);
                    newImage.SetPixel(x, y, averageColor);
                }
            }
        }
        void ThreadWOrker2(int from, int to)
        {

            for (int x = from; x < to; x++)
            {
                for (int y = 0; y < newHeight; y++)
                {
                    int red = 0, green = 0, blue = 0, pixelCount = 0;

                    for (int newX = orgImage.Width / 2; newX < orgImage.Width; newX++)
                    {

                        for (int newY = 0; newY < orgImage.Height; newY++)
                        {
                            int finalX = x * orgImage.Width / newWidth;
                            int finalY = y * orgImage.Height / newHeight;

                            Color color = orgImage.GetPixel(finalX, finalY);
                            red += color.R;
                            green += color.G;
                            blue += color.B;
                            pixelCount++;
                        }
                    }

                    Color averageColor = Color.FromArgb(red / pixelCount, green / pixelCount, blue / pixelCount);
                    newImage.SetPixel(x, y, averageColor);
                }
            }
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "images | *.png;*.jpg;*.jpeg;*.gif";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtSelect.Text = ofd.FileName;
                orgImage = new Bitmap(ofd.FileName);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
                txtSave.Text = fbd.SelectedPath;
        }

        private void btnResize_Click(object sender, EventArgs e)
        {


            if (orgImage != null && !string.IsNullOrEmpty(textBox1.Text))
            {
                sF = Int32.Parse(textBox1.Text);
                Thread t1 = new Thread(p => ThreadWOrker1(0, newWidth / 2));
                Thread t2 = new Thread(p => ThreadWOrker2(newWidth / 2, newWidth));
                t1.Start();
                t1.Join();
                t2.Start();
                t2.Join();
                MessageBox.Show("Image Resized");
            }
            else
            {
                MessageBox.Show("Please select the image and provide the resize value first.");
            }



            newWidth = orgImage.Width * sF;
            newHeight = orgImage.Height * sF;
            newImage = new Bitmap(newWidth, newHeight);

         
        }

        private void btnFSave_Click(object sender, EventArgs e)
        {
            if (newImage != null && !string.IsNullOrEmpty(txtSave.Text))
            {
                newImage.Save(txtSave.Text + "\\rImage.png");
                MessageBox.Show("Image Saved");
            }
            else
            {
                MessageBox.Show("Please resize the image and provide the save path first.");
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
