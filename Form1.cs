using SHDocVw;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace RedirectIEToChrome
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            notifyIcon1.Visible = true;
            ShowInTaskbar = false;
            Hide();
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer1.Interval = 500;
            timer1.Tick += new EventHandler(Timer1_Tick);
            timer1.Enabled = true;
        }

        private void Timer1_Tick(object Sender, EventArgs e)
        {
            try
            {
                ProcessIExplore();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Minimized;
                notifyIcon1.Visible = true;
                ShowInTaskbar = false;
                Hide();
            }
            else
            {
                WindowState = FormWindowState.Normal;
                notifyIcon1.Visible = false;
                ShowInTaskbar = true;
                Show();
            }
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            ShowInTaskbar = true;
            Show();
        }

        private void ProcessIExplore()
        {
            ShellWindows ieShellWindows = new ShellWindows();
            string sProcessType;
            InternetExplorer currentiexplore = null;

            foreach (InternetExplorer ieTab in ieShellWindows)
            {
                sProcessType = Path.GetFileNameWithoutExtension(ieTab.FullName).ToLower();
                if (sProcessType.Equals("iexplore") && !ieTab.LocationURL.Contains("about:Tabs"))
                {
                    currentiexplore = ieTab;
                }
            }

            if (currentiexplore != null)
            {
                if (currentiexplore.LocationURL.Equals(string.Empty))
                    ProcessIExplore();
                else
                    Process.Start("chrome", currentiexplore.LocationURL);
                currentiexplore.Quit();
                return;
            }
        }
    }
}
