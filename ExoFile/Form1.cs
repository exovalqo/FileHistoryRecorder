using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Timer = System.Timers.Timer;
using System.Threading;

namespace HistoryDirectoryChecker
{
    public partial class Form1 : Form
    {
        int counter = 0;
        int amount = 0;
        FileSystemWatcher watcher = new FileSystemWatcher();
        String currentPath = System.IO.Path.GetDirectoryName(
         System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\SavedFiles";
        
        public Form1()
        {
            InitializeComponent();
            currentPath = currentPath.Remove(0, 6);

        }

        private void ListView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void checker()
        {

               
                watcher.Path = textBox1.Text;
            int decider = 0;
            if (cbAccess.Checked)
                decider = 32 | decider;
            if(cbW.Checked)
                decider = 16 | decider;
            if (cbAttribute.Checked)
                decider = 4 | decider;
            if(cbDirectories.Checked)
                watcher.IncludeSubdirectories = true;
            if (cbRename.Checked)
                decider = 1 | 2 | decider;
            if (cbSize.Checked)
                decider = decider | 8;
           
                watcher.NotifyFilter = (System.IO.NotifyFilters)decider;

                watcher.Changed += OnChanged;
                watcher.Created += OnCreated;
                watcher.Deleted += OnChanged;
                watcher.Renamed += OnRenamed;

                




        }
        private void Button1_Click(object sender, EventArgs e)
        {

            ListView.CheckForIllegalCrossThreadCalls = false;
            if (cbSaveFiles.Checked)
                Directory.CreateDirectory(currentPath);

           

            if (button1.Text.Equals("Record"))
            {
                
                
                try
                {
                    checker();
                    watcher.EnableRaisingEvents = true;
                    label5.Visible = false;
                    button1.Text = "Stop";
                    button1.ForeColor = Color.IndianRed;
                    timer1.Enabled = true;

                }
                catch(System.ArgumentException)
                {
                    label5.Visible = true;
                    return;
                }
                
            }
            else
            {
                label5.Visible = false;
                button1.Text = "Record";
                button1.ForeColor = Color.ForestGreen;
                timer1.Enabled = false;
                counter = 0;
                watcher.EnableRaisingEvents = false;
                watcher = new FileSystemWatcher();
            }


        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            amount++;
            listView1.Items.Add(new ListViewItem(new[] { amount.ToString(), e.FullPath, e.ChangeType.ToString() }));
            
        }
        private void OnCreated(object source, FileSystemEventArgs e)
        {
            amount++;
            if (cbSaveFiles.Checked)
            {
                if (!Path.GetDirectoryName(e.FullPath).Equals(currentPath))
                {
                    int count = 1;

                    string fileNameOnly = Path.GetFileNameWithoutExtension(e.FullPath);
                    string extension = Path.GetExtension(e.FullPath);
                    string path = Path.GetDirectoryName(currentPath);
                    string newFullPath = currentPath + "\\" + fileNameOnly + extension;

                    while (File.Exists(newFullPath))
                    {
                        string tempFileName = string.Format("{0}({1})", fileNameOnly, count++);
                        newFullPath = Path.Combine(currentPath, tempFileName + extension);
                    }
                    Thread.Sleep(100);
                    try
                    {
                        File.Copy(e.FullPath, newFullPath, false);
                    }catch(Exception ex)
                    {
                        listView1.Items.Add(new ListViewItem(new[] { amount.ToString(),"Error File Not Found", "Error"}));
                    }
                }
                
            }
            listView1.Items.Add(new ListViewItem(new[] { amount.ToString(), e.FullPath, e.ChangeType.ToString() }));
           
        }
        private void OnRenamed(object source, RenamedEventArgs e)
        {
            // Specify what is done when a file is renamed.
            amount++;
            listView1.Items.Add(new ListViewItem(new[] { amount.ToString(), e.OldFullPath + " renamed to " + e.FullPath, "Renamed" }));
            
        }
    
    private void Timer1_Tick(object sender, EventArgs e)
        {
            
            label4.Text = "Time Running: " + ++counter / 2;
            
            
            
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
        }
    }

}
