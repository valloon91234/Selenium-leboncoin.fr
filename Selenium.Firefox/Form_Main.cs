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
using System.Web;
using System.Windows.Forms;

namespace Valloon.Selenium
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            String rootDirectoryPath = Config.Get(Config.KEY_DIRECTORY);
            String dataFilename = Config.Get(Config.KEY_DATA);
            String email = Config.Get(Config.KEY_EMAIL);
            String password = Config.Get(Config.KEY_PASSWORD);
            textBox_RootDirectory.Text = rootDirectoryPath;
            textBox_DataFilename.Text = dataFilename;
            textBox_Email.Text = email;
            textBox_Password.Text = password;
        }

        private Robot R = null;

        private void Init()
        {
            if (R != null) return;
            R = new Robot();
        }

        private void button_StartPublshing_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            String rootDirectoryPath = textBox_RootDirectory.Text;
            String dataFilename = textBox_DataFilename.Text;
            String email = textBox_Email.Text;
            String password = textBox_Password.Text;
            if (!new DirectoryInfo(rootDirectoryPath).Exists)
            {
                Robot.Print($"Root Folder \"{rootDirectoryPath}\" does not exist.", ConsoleColor.Red);
            }
            else if (!new FileInfo(dataFilename).Exists)
            {
                Robot.Print($"Data File \"{dataFilename}\" does not exist.", ConsoleColor.Red);
            }
            else
            {
                Config.Write(Config.KEY_DIRECTORY, rootDirectoryPath);
                Config.Write(Config.KEY_DATA, dataFilename);
                Config.Write(Config.KEY_EMAIL, email);
                Config.Write(Config.KEY_PASSWORD, password);
                Init();
                R.StartPublishingThread(rootDirectoryPath, dataFilename, email, password);
            }
        }

        private void Form_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (R != null)
                R.Dispose();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

    }
}
