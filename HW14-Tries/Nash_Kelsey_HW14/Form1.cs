/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 14
Due: 4/28/2017 at 11:59 pm
Sources: Lecture Notes from 4/21/17 and
*/

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

namespace Nash_Kelsey_HW14
{
    public partial class Form1 : Form
    {
        //Trie dictionary
        Trie dictionaryTree = new Trie();

        public Form1()
        {
           
            InitializeComponent();

            //Path to the list of words
            string path = Path.Combine(Environment.CurrentDirectory, "wordsEn.txt");

            //string array that stores the initial dictionary.
            string[] dictionary = File.ReadAllLines(path);

            //add each word to our Trie
            foreach (string word in dictionary)
            {
                //Add to the trie.
                dictionaryTree.AddString(word);

            }
        }

        //When the user enters a prefix, we find the related words and display them.
        private void userInput_TextChanged(object sender, EventArgs e)
        {
            //get the prefix from the text box
            string prefix = userInput.Text;

            //find all the words in the dictonary that start with the prefix and return a list of results
            List<string> results = dictionaryTree.FindAllWords(prefix);

            StringBuilder putInResultsBox = new StringBuilder();

            foreach (string word in results)
            {
                putInResultsBox.AppendLine(word);
            }

            //set the results to the list of words!
            autoCompleteResults.Text = putInResultsBox.ToString();
        }
    }
}
