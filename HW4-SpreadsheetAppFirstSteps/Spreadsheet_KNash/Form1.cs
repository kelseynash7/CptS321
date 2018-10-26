/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 4
Due: 2/10/17 by 11:59 pm
Sources: various MSDN articles (Substring, INotifyPropertyChanged, Abstract classes)
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
using SpreadsheetEngine;
using CptS321;

namespace Spreadsheet_KNash
{
    public partial class Form1 : Form
    {
        private Spreadsheet spreadsheet = new Spreadsheet(50,26);

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //clear any columns and rows previously added to the form
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            //add columns A-Z
            for (char c = 'A'; c <= 'Z'; c++)
            {
                //sets the column name and the header name to the current letter c.
                dataGridView1.Columns.Add(c.ToString(), c.ToString());
            }

            //now add rows 1 -50
            for (int i = 1; i <= 50; i++)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[i - 1].HeaderCell.Value = i.ToString();
                
            }

            //subscribing to the CellPropertyChanged event
            spreadsheet.CellPropertyChanged += OnCellPropertyChanged;

        }

        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell updatedCell = sender as Cell;
            //convert the column to an acceptable integet (subtract 64 to be back in the range)
            int cellCol = (int)(updatedCell.ColumnIndex - 64);

            //if the property changed is Value and we aren't pulling from a null cell - set the dataGridView cell value to the value in the spreadsheet
            if (e.PropertyName == "Value" && updatedCell != null)
            {
                dataGridView1.Rows[updatedCell.RowIndex-1].Cells[cellCol].Value = updatedCell.Value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Set 50 random cells to "Hello World!"
            Random randomNums = new Random();

            for (int i = 0; i < 50; i++)
            {
                int row = randomNums.Next(0, 50);
                int colnum = randomNums.Next(2, 26); //starting from 2 so because of next test

                spreadsheet.cells[row, colnum].Text = "Hello World!";
            }

            //Set every cell in column B to "This is Cell B#" where # is the row number for the cell
            for (int r = 0; r < 50; r++)
            {
                spreadsheet.cells[r, 1].Text = "This is cell B" + (r+1); //col 1 = col B
            }

            //Set text in all cells of column A to "=B#", where # is the row number for the cell
            for (int r = 0; r < 50; r++)
            {
                spreadsheet.cells[r, 0].Text = "=B" + (r + 1); //col 0 = col A
            }
        }
    }
}
