/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 3
Due: 2/3/17 by 11:59 pm
Sources: various MSDN articles
         C# 6.0 In A Nutshell - Course Textbook
*/

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

namespace KelseyNash_HW3
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void SaveFileOnClick(object sender, EventArgs e)
        {
            //Display SaveFileDialog to give user the option to save the file
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Plain Text File|*.txt";
            saveDialog.DefaultExt = "txt";
            saveDialog.Title = "Save a file...";
            
            //We want to make sure that the user presses OK before we attempt to save the file. 
            DialogResult saveResult = saveDialog.ShowDialog();

            //if user said okay, check to make sure the file is not an empty string and then we can save!
            if (saveResult == DialogResult.OK)
            {
                //using closes the file after we leave the curly brackets. Use streamWriter to write to the file
                using (StreamWriter save = new StreamWriter(saveDialog.OpenFile()))
                {
                    save.Write(textBox1.Text);
                }             
            }
        }


        private void LoadText(TextReader sr)
        {
            this.textBox1.Text = sr.ReadToEnd();
        }

        private void LoadFileClick(object sender, EventArgs e)
        {
            //Display OpenFileDialog to give user the option to open a specific file.
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Title = "Open a file...";
            openDialog.Filter = "Plain Text File|*.txt";
            openDialog.DefaultExt = "txt";

            DialogResult openResult = openDialog.ShowDialog();

            //if user says okay/open, try to open the file!
            if (openResult == DialogResult.OK)
            {
                //open file with a Stream Reader
                using (StreamReader open = new StreamReader(openDialog.OpenFile()))
                {
                    LoadText(open);
                }
            }
        }

        private void LoadFib50Click(object sender, EventArgs e)
        {
            FibonacciTextReader fib = new FibonacciTextReader(50);
            LoadText(fib);
        }

        private void LoadFib100Click(object sender, EventArgs e)
        {
            FibonacciTextReader fib = new FibonacciTextReader(100);
            LoadText(fib);
        }
    }
}
