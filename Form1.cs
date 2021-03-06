using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Security.Principal;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (e == null)
            {
                vwifi(null, null, false);
                this.Close();
            }
            else
            {
                base.OnClosing(e);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!IsAdmin())
            {
                RestartElevated();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string ssid = this.textBox1.Text, key = textBox2.Text;
            bool connect = false; 
            if (!connect)
            {
                if (this.textBox1.Text == null || this.textBox1.Text == "")
                {
                    MessageBox.Show("SSID cannot be left blank!",
                    "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {

                    if (textBox2.Text == null || textBox2.Text == "")
                    {
                        MessageBox.Show("Key value cannot be left blank!",
                        "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        if (key.Length >= 6)
                        {
                            vwifi(ssid, key, true);
                            connect = true;
                        }
                        else
                        {
                            MessageBox.Show("Key should be more then or Equal to 6 Characters !",
                            "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            else
            {
                vwifi(null, null, false);
                this.textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Text = "Start";
                connect = false;
            }
        }

       

        private void vwifi(string ssid, string key, bool status)
        {
            button1.Text = "Loading...";
            ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe");
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.CreateNoWindow = false;
            processStartInfo.UseShellExecute = false;
            Process process = Process.Start(processStartInfo);
             if (process != null)
           {
               if (status)
               {
                   process.StandardInput.WriteLine("netsh wlan set hostednetwork mode=allow ssid=" + ssid + " key=" + key);
                   process.StandardInput.WriteLine("netsh wlan start hosted network");
                   process.StandardInput.Close();
               }
               else
               {
                   process.StandardInput.WriteLine("netsh wlan stop hostednetwork");
                   process.StandardInput.Close();
               }
           }

            this.textBox1.Enabled = false;
            textBox2.Enabled = false;
            button1.Text = "Stop";
        }

        public static bool IsAdmin()
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            WindowsPrincipal p = new WindowsPrincipal(id);
            return p.IsInRole(WindowsBuiltInRole.Administrator);
        }

        public void RestartElevated()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.UseShellExecute = true;
            startInfo.CreateNoWindow = false;
            startInfo.WorkingDirectory = Environment.CurrentDirectory;
            startInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
            startInfo.Verb = "runas";
            try
            {
                Process p = Process.Start(startInfo);
            }
            catch
            {


            }
            Application.Exit();

        }
    }
    }
