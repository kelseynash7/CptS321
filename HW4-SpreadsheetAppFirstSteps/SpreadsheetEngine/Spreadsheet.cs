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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;


namespace SpreadsheetEngine
{
    public class Spreadsheet
    {

        //Constructor
        public Spreadsheet(int numRows, int numColumns) 
        {
            cells = new Cell[numRows, numColumns];
            
            for (int i = 0; i < numRows; i++)
            {
                for (int c = 0; c < numColumns; c++)
                {
                    //convert to a Char
                    char letter =(char)(c + 64);
                    //create cell
                    cells[i, c] = new BasicCell(i+1, letter);
                    //subscribe to the cell's propertyChanged event
                    cells[i, c].PropertyChanged += OnPropertyChanged;
                }
            }

            rowCount = numRows;
            columnCount = numColumns;

        }

        //2D Array of Cells!
        public Cell[,] cells;

        //Inherited Cell Class - will set values of cells
        private class BasicCell : Cell
        {
            //Constructor
            public BasicCell(int row, char col) : base(row, col)
            {

            }
            //setValue function
           public void setValue(string newValue)
            {
                value = newValue;
                OnPropertyChanged("Value");
            }
        }

        //Column Count member variable and property
        private int columnCount;
        public int ColumnCount
        {
            get { return columnCount; }
            set { columnCount = value; }
        }

        //row count member variable and property
        private int rowCount;
        public int RowCount
        {
            get { return rowCount; }
            set { rowCount = value; }
        }

        //CellPropertyChanged EventHandler
        public event PropertyChangedEventHandler CellPropertyChanged;
        
        //Called when a cell's propertyChanged event is called.
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                EvaluateCell(sender as Cell);
            }
        }

        //Get cell function, returns a valid cell OR null 
        Cell GetCell(int rowIndex, int colIndex)
        {

            if (cells[rowIndex, colIndex] != null)
            {
                return cells[rowIndex, colIndex];
            }
            else
            {
                return null;
            }
        }

        //helper function to evaluate the text value of a cell to determine the value value
        private void EvaluateCell(Cell cell)
        {
            //make BasicCell to evaluate the cell
            BasicCell evalCell = cell as BasicCell;

            //First check to see if it's empty
            if (string.IsNullOrWhiteSpace(evalCell.Text))
            {
                //if text is empty, the value should be empty
                evalCell.setValue("");
                CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
            }

            //next check to see if there is an '=' to make it a formula (and that it's more than just the =...
            else if (evalCell.Text.Length > 1 && evalCell.Text[0] == '=')
            {
                //first get rid of the = at (0)
                string text = evalCell.Text.Substring(1);

                //At this point, there should be two or threecharacters left in the string text and it should be a cell
                int tempCol;
                int tempRow;

                if (text.Length == 2)
                {
                    tempCol = text[0] - 65;
                    tempRow = text[1] - 49;
                }
                else //3 characters in text
                {
                    tempCol = text[0] - 65;
                    text = text.Substring(1);
                    int.TryParse(text, out tempRow);
                    tempRow--;
                }


                //get the cell that is referred to
                Cell temp = GetCell(tempRow, tempCol);
                 
                //if we got null back, that cell does not exist
                if (cell == null)
                {
                    evalCell.setValue("null");
                    CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                }
                //otherwise it was a valid cell, set the value to the text of the referenced cell
                else
                {
                    evalCell.setValue(temp.Text);
                    CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                }
            }

            //last if it's neither of the above, it's not an formula, just test the value to the text of the original cell
            else
            {
                evalCell.setValue(evalCell.Text);
                CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
            }
        }

    }

    
}
