/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 12
Due: 4/14/2017 at 11:59 pm
Sources: (Issues with setting the results box) http://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe
MSDN WebClient and Invoke Method Articles
*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nash_HW12
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Elements formElements = new Elements();

        //Disables the part 1 UI elements and  creates a new thread to run the download 
        private void DownloadButton_Click(object sender, EventArgs e)
        {
            //Disable URL Textbox, result-data textbox and start-download-button (Set enabled to False)
            URLTextBox.Enabled = false;
            resultsBox.Enabled = false;
            DownloadButton.Enabled = false;

            Thread downloadThread = new Thread(DownloadString);
            downloadThread.Start();
        }

        //Method to actually do the downloading
        private void DownloadString()
        {
            var downloadedString = string.Empty;

            if (!string.IsNullOrWhiteSpace(formElements.DownloadString))
            {
                using (var webClient = new WebClient())
                {
                    downloadedString = webClient.DownloadString(formElements.DownloadString);

                    this.BeginInvoke(new Action(ThreadCompleted1));

                    SetText1(downloadedString);
                }

            }
            else
            {
                this.BeginInvoke(new Action(ThreadCompleted1));
            }

        }

        //Sets the variable for downloading the string to the value in the URL textbox whenever the text box changes.
        private void URLTextBox_TextChanged(object sender, EventArgs e)
        {
            formElements.DownloadString = URLTextBox.Text;
        }

        //Re-enables the UI elements
        private void ThreadCompleted1()
        {
            //Restore UI Element States
            DownloadButton.Enabled = true;
            resultsBox.Enabled = true;
            URLTextBox.Enabled = true;            
        }

        //These allow for threadsafe setting of the results text box since it will be set in the downloading thread, not the thread that created it (UI thread)
        delegate void SetTextCallback(string text);

        private void SetText1(string text)
        {
            if (this.resultsBox.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText1);
                this.Invoke(d, new object[] { text });

            }
            else
            {
                this.resultsBox.Text = text;
            }
        }


        /* PART TWO*/

      //First sort all 8 lists on a single thread, one after the other, then sort each list on it's own thread
        private void sortingButton_Click(object sender, EventArgs e)
        {
            //disable the UI elements for this section.
            sortingButton.Enabled = false;
            timeResults.Enabled = false;

            //randomize the lists
            formElements.RandomizeLists();

            //create a thread that will actually do the two parts
            Thread SortingThread = new Thread(SortingThreads);
            SortingThread.Start();
        }

        //Re-enables the UI elements for part 2
        private void ThreadCompleted2()
        {

            //Re-enable UI elements
            sortingButton.Enabled = true;
            timeResults.Enabled = true;
        }
        
        //Allows me to set the results in the UI in a thread-safe way
        private void SetText2(string text)
        {
            if (this.timeResults.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText2);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.timeResults.Text = text;
            }
        }

        //Function for the single thread that will sort all of the lists
        private void SortAllLists()
        {
            formElements.L1.Sort();
            formElements.L2.Sort();
            formElements.L3.Sort();
            formElements.L4.Sort();
            formElements.L5.Sort();
            formElements.L6.Sort();
            formElements.L7.Sort();
            formElements.L8.Sort();
        }

        //utility function for a single thread sorting one list
        private void SortOneList(object data)
        {
            List<int> list = data as List<int>;
            list.Sort();
        }

        //Function that the original new thread calls to the create do 1. sort 8 lists with 1 thread and then 2. sort 8 lists with 8 threads
        private void SortingThreads()
        {
            Stopwatch timer1 = new Stopwatch();

            //create a thread and sort all the lists
            Thread sort1 = new Thread(SortAllLists);

            timer1.Start();

            sort1.Start();
            sort1.Join();

            timer1.Stop();

            var oneThreadTime = timer1.ElapsedMilliseconds;

            Stopwatch timer2 = new Stopwatch();

            //resort Lists
            formElements.RandomizeLists();

            //create 8 threads one for each list
            Thread t1 = new Thread(new ParameterizedThreadStart(SortOneList));
            Thread t2 = new Thread(new ParameterizedThreadStart(SortOneList));
            Thread t3 = new Thread(new ParameterizedThreadStart(SortOneList));
            Thread t4 = new Thread(new ParameterizedThreadStart(SortOneList));
            Thread t5 = new Thread(new ParameterizedThreadStart(SortOneList));
            Thread t6 = new Thread(new ParameterizedThreadStart(SortOneList));
            Thread t7 = new Thread(new ParameterizedThreadStart(SortOneList));
            Thread t8 = new Thread(new ParameterizedThreadStart(SortOneList));

            //start timer
            timer2.Start();

            //start all the threads
            t1.Start(formElements.L1);
            t2.Start(formElements.L2);
            t3.Start(formElements.L3);
            t4.Start(formElements.L4);
            t5.Start(formElements.L5);
            t6.Start(formElements.L6);
            t7.Start(formElements.L7);
            t8.Start(formElements.L8);

            //stop timer
            timer2.Stop();

            var timeElapsed = timer2.ElapsedMilliseconds;
            StringBuilder formatting = new StringBuilder();

            formatting.AppendLine($"Single-Threaded Time: {oneThreadTime} milliseconds.");
            formatting.AppendLine($"Multi-Threaded Time: {timeElapsed} milliseconds.");

            SetText2(formatting.ToString());

            this.BeginInvoke(new Action(ThreadCompleted2));
        }
    }
}
