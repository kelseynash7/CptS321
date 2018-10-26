/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 9
Due: 3/24/17 by 11:59 pm
Sources: various MSDN articles (XmlWriter Class), http://www.codeguru.com/csharp/csharp/cs_data/xml/article.php/c4227/A-Simple-Way-to-Write-XML-in-NET-XmlTextWriter.htm,
https://www.codeproject.com/Articles/169598/Parse-XML-Documents-by-XMLDocument-and-XDocument

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
using System.IO;

namespace Spreadsheet_KNash
{
    public partial class Form1 : Form
    {
        //Spreadsheet
        private Spreadsheet spreadsheet = new Spreadsheet(50,26);

        //UndoRedo class so we can use it
        public UndoRedo undoRedo = new UndoRedo();

        public Form1()
        {
            InitializeComponent();
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //clear any columns and rows previously added to the form
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();

            //update edit menu - will start them off disabled so user can't try to undo/redo nothing.
            UpdateEditMenu();

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
            //subscribe to the cellbeginedit and cellEndEdit
            dataGridView1.CellEndEdit += dataGridView1_CellEndEdit;
            dataGridView1.CellBeginEdit += dataGridView1_CellBeginEdit;

        }

        // OnCellPropertyChanged
        private void OnCellPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Cell updatedCell = sender as Cell;
            //convert the column to an acceptable integer (subtract 64 to be back in the range)
            int cellCol = (int)(updatedCell.ColumnIndex - 65);

            //if the property changed is Value and we aren't pulling from a null cell - set the dataGridView cell value to the value in the spreadsheet
            if (e.PropertyName == "Value" && updatedCell != null)
            {
                dataGridView1.Rows[updatedCell.RowIndex-1].Cells[cellCol].Value = updatedCell.Value;
            }
            //if the property changed is BGColor adn we aren't pulling from a null cell - set the dataGridiew cell BGColor to the 
            if (e.PropertyName == "BGColor" && updatedCell != null)
            {
                dataGridView1.Rows[updatedCell.RowIndex - 1].Cells[cellCol+1].Style.BackColor = Color.FromArgb((int)updatedCell.BGColor);
            }
        }

        // Demo Button
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
                spreadsheet.cells[r, 2].Text = "This is cell B" + (r+1); //col 1 = col B
            }

            //Set text in all cells of column A to "=B#", where # is the row number for the cell
            for (int r = 0; r < 50; r++)
            {
                spreadsheet.cells[r, 1].Text = "=B" + (r + 1); //col 0 = col A
            }
        }

        // CELLBEGINEDIT
        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            // What cell are we editing??
            int row = e.RowIndex;
            int column = e.ColumnIndex;

            // Get that cell from our spreadsheet
            Cell selectedCell = spreadsheet.GetCell(row, column + 1);


            // set the Value of the grid's cell to the spreadsheet Cell's Text field
            dataGridView1.Rows[row].Cells[column].Value = selectedCell.Text;
        }

        // CELLENDEDIT 
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // What cell did we edit?
            int row = e.RowIndex;
            int column = e.ColumnIndex;

            //Need add to the undo stack in this class. First declare an array of our command interface
            IUndoRedoCommand[] undos = new IUndoRedoCommand[1];

            // variable for the cell's text
            string text;

            // Get that cell from our spreadsheet
            Cell editedCell = spreadsheet.GetCell(row, column + 1);

            // Get the value from the cell we are editing and apply it to text field
            if (dataGridView1.Rows[row].Cells[column].Value == null)
            {
                text = " ";
            }
            else
            {
                text = dataGridView1.Rows[row].Cells[column].Value.ToString();
            }
            
            //add the text that will be replaced to the undo stack
            undos[0] = new RestoreText(editedCell, editedCell.Text);

            // set the text of the cell to the newly inputed text
            editedCell.Text = text; // This should also update the value of the cell

            //add the undo array to the undoRedo varaiable of the form along with a descriptive title of what it would be undoing if we called it.
            undoRedo.AddUndo(new UndoRedoCollection(undos, "Cell Text Change"));

            //set the grid cell to the value (evaluated) of the spreadsheet cell
            dataGridView1.Rows[row].Cells[column].Value = editedCell.Value;

            //Update the edit menu
            UpdateEditMenu();

        }

        // Method that changes the cell color of all selected cells using a color dialog box
        private void chooseCellBackgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uint chosenColor = 0;
            ColorDialog colorDialog = new ColorDialog();
            //To implement undo/redo for bg color, need a list of commands
            List<IUndoRedoCommand> undo = new List<IUndoRedoCommand>();

            //if user selects OK in the dialog box
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                //set chosenColor variable to the color chosen in the dialog (but as a number)
                chosenColor = (uint)colorDialog.Color.ToArgb();

                //now we need to update all of the cells that were selected.
                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                {
                    //get the cell
                    Cell spreadsheetCell = spreadsheet.GetCell(cell.RowIndex, cell.ColumnIndex);

                    //add the old color to the undo command list
                    undo.Add(new RestoreBGColor(spreadsheetCell, spreadsheetCell.BGColor));

                    //set the bgcolor to the chosen color
                    spreadsheetCell.BGColor = chosenColor;
                }
                //add the undo list & description to the undo stack.
                undoRedo.AddUndo(new UndoRedoCollection(undo, "Cell Background Color Change."));

                //update the menu
                UpdateEditMenu();
            }
        }

        //Event for what happens when the undo button is clicked.
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //call the undo function and pass in the spreadsheet.
            undoRedo.Undo(spreadsheet);
            //update menu
            UpdateEditMenu();
        }

        //Event for what happens when the redo button is clicked.
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //call the redo function and pass in the spreadsheet.
            undoRedo.Redo(spreadsheet);
            //update menu
            UpdateEditMenu();
        }

        //Function that updates the edit menu. This disables the undo and/or redo buttons when there is nothing in the stacks to undo and/or redo. Also will update the text label that goes along with the button that describes what it's undoing or redoing. Was going to do this without a function but would be repeating a lot of code...
        private void UpdateEditMenu()
        {
            //get the menu items from the menu strip
            ToolStripMenuItem menuitems = (menuStrip1.Items[1] as ToolStripMenuItem);

            //iterate through the items and make changes based on what was just done & functions like CanUndo and CanRedo from the UndoRedo class
            foreach (ToolStripItem item in menuitems.DropDownItems)
            {
                if (item.Text.Contains("Undo"))
                {
                    //set the enabled property based on whether or not we can undo
                    item.Enabled = undoRedo.CanUndo;
                    //set the text property to undo concatenated with the description of the undo
                    item.Text = string.Format("Undo {0}", undoRedo.CheckUndo);
                }
                else if (item.Text.Contains("Redo"))
                {
                    //set the enabled property based on whether or not we can redo
                    item.Enabled = undoRedo.CanRedo;
                    //set text property to "redo" concat with description of redo
                    item.Text = string.Format("Redo {0}", undoRedo.CheckRedo);
                }


            }

        }

        //Save menu item event
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create save file dialog
            var saveFileDialog = new SaveFileDialog();
            
            //if the user names file and hits OK
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                //create the file we are saving
                Stream outfile = new FileStream(saveFileDialog.FileName, FileMode.Create, FileAccess.Write);

                //Save the cells to the newly created file
                spreadsheet.Save(outfile);

                //Close the file so we can actually use it
                outfile.Dispose();
            }
        }

        //Load event
        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //create an open file dialog
            var loadFileDialog = new OpenFileDialog();

            //if the user selects a file and hits okay
            if (loadFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Clear the spreadsheet
                ClearSpreadsheet();

                //create stream to load the file into
                Stream infile = new FileStream(loadFileDialog.FileName, FileMode.Open, FileAccess.Read);

                //load the file into the spreadsheet
                spreadsheet.Load(infile);

                //Close the stream so we can use it
                infile.Dispose();

                //Clear the undo and redo stacks
                undoRedo.ClearStacks();
            }

            //update the edit menu
            UpdateEditMenu();
        }


        //Clear Spreadsheet function
        public void ClearSpreadsheet()
        {
            //each row
            for (int rowIndex = 0; rowIndex < 50; rowIndex++)
            {
                //each column
                for (int colIndex = 0; colIndex < 26; colIndex++)
                {
                    //We really only need to clear cells that have been edited.
                    if (spreadsheet.cells[rowIndex, colIndex].Text != string.Empty || spreadsheet.cells[rowIndex, colIndex].Value != string.Empty || spreadsheet.cells[rowIndex, colIndex].BGColor != 4294967295)
                    {
                        //Clear the cell
                       spreadsheet.cells[rowIndex, colIndex].Clear();
                    }
                }
            }
        }

        }
    }

