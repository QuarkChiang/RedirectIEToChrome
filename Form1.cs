using SHDocVw;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace RedirectIEToChrome
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Process.Start("chrome", @"https://www.google.com.tw/");
            Process.Start("iexplore", @"https://www.google.com.tw/");
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
            ProcessIExplore();
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

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
            ShowInTaskbar = true;
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Process[] processesarrary = Process.GetProcessesByName("iexplore");

            foreach (Process precess in processesarrary)
            {
                Console.WriteLine(precess.ProcessName);
                precess.WaitForExit(100);
                precess.CloseMainWindow();
            }
        }

        private void ProcessIExplore()
        {
            ShellWindows ieShellWindows = new ShellWindows();
            string sProcessType;
            string result = string.Empty;
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
                result = currentiexplore.LocationURL;
                if (result == string.Empty)
                    ProcessIExplore();
                else
                    Process.Start("chrome", result);
                currentiexplore.Quit();
                return;
            }

            Console.WriteLine("Not Found");
        }

        private string GetInternetExplorerUrl()
        {
            ShellWindows ieShellWindows = new ShellWindows();
            string sProcessType;
            string result = string.Empty;
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
                result = currentiexplore.LocationURL;
                currentiexplore.Quit();
            }

            return result;
        }
    }
}
