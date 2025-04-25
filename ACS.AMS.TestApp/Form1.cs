using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ACS.AMS.ServicePlugin;

namespace ACS.AMS.TestApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            UpdateButtonText();
        }

        private DataTransferHandler GetDataHandler()
        {
            var tbl = new DataTransferHandler();
            //tbl.ServiceManager = new DummyServiceManager();
            tbl.InitPlugin(new DummyServiceManager());
            tbl.LoadPlugin();

            return tbl;
        }

        private void buttonMail_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
            UpdateButtonText();
        }

        private void UpdateButtonText()
        {
            if (timer1.Enabled)
                buttonMail.Text = "Stop Mail Service";
            else
                buttonMail.Text = "Start Mail Service";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                DataTransferHandler dh = GetDataHandler();
                dh.SyncAllData();

                SetStatusText("Process Completed");
            }
            catch (Exception ex)
            {
                SetStatusText(ex.InnerException.Message);
            }
        }

        private void SetStatusText(string txt)
        {
            textBox1.Text = txt + Environment.NewLine + textBox1.Text;

            if(textBox1.Text.Length > 5000)
            {
                textBox1.Text = textBox1.Text.Substring(0, 3000);
            }
        }
    }
}
