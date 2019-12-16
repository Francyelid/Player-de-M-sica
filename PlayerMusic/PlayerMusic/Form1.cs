using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PlayerMusic
{
    public partial class Form1 : Form
    {
        public string[] paths, files;
        public static int musica = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folder = new FolderBrowserDialog();
            folder.Description = "Selecione uma pasta";
            
            if (folder.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folder.SelectedPath;
                

                DirectoryInfo dir = new DirectoryInfo(@"" + textBox1.Text);

                ListaArquivos(dir);

                MessageBox.Show("Os arquivos irão ser inicializados");

                axWindowsMediaPlayer1.URL = paths[musica];

            }


        }

        public void ListaArquivos(DirectoryInfo dir)
        {
            OpenFileDialog janelaArquivo = new OpenFileDialog();
            janelaArquivo.Multiselect = true;
            
            if (janelaArquivo.ShowDialog() == DialogResult.OK)
            {
                paths = janelaArquivo.FileNames;
                files = janelaArquivo.SafeFileNames;
                
                for (int i = 0; i < files.Length; i++)
                {
                    listBox1.Items.Add(files[i]);
                }
            }
            

            MessageBox.Show("Para selecionar uma música, clique nela uma vez.");

        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                progressBar1.Maximum = (int)axWindowsMediaPlayer1.Ctlcontrols.currentItem.duration;
                timer1.Start();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPaused)
            {
                timer1.Stop();
            }
            else if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                timer1.Stop();
                progressBar1.Value = 0;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsPlaying)
            {
                progressBar1.Value = (int)axWindowsMediaPlayer1.Ctlcontrols.currentPosition;
            }
            if (progressBar1.Value == progressBar1.Maximum)
            {
                if (musica == files.Length - 1)
                {
                    musica = 0;
                }

                else
                {
                    musica++;
                }

                axWindowsMediaPlayer1.URL = paths[musica];
                timer1.Stop();
                progressBar1.Value = 0;
                timer1.Start();
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            musica = listBox1.SelectedIndex;
            axWindowsMediaPlayer1.URL = paths[musica];
        }
    }
}
