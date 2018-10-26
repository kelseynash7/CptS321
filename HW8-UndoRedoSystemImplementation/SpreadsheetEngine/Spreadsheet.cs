/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 8
Due: 3/10/17 by 11:59 pm
Sources: various MSDN articles (DataGridViewCellStyle.BackColor, FromArgb, )
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
        //2D Array of Cells!
        public Cell[,] cells;

        //Dictionary that will keep track of dependencies
        private Dictionary<string, HashSet<string>> dependencyDict;

        //Need the undo/redo class so we can define make changes to the spreadsheet using undo/redo. Needs to be public so it can be used in the program
        public UndoRedo undoRedo = new UndoRedo();

        //Constructor
        public Spreadsheet(int numRows, int numColumns) 
        {
            //create cell array
            cells = new Cell[numRows, numColumns];

            dependencyDict = new Dictionary<string, HashSet<string>>();

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
        
        //Called when a cell's propertyChanged event is called. TExt or BGColor
        public void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Text")
            {
                BasicCell temporary = sender as BasicCell;
                string cellname = temporary.ColumnIndex.ToString() + temporary.RowIndex.ToString();
                //Delete the dependencies because the text has changed, need to edit
                DeleteDependency(cellname);

                //if the cell text is a formula, create an new expression tree and set the new dependencies!
                if (temporary.Text != "" && temporary.Text[0] == '=' && temporary.Text.Length > 1)
                {
                    ExpTree tree = new ExpTree(temporary.Text.Substring(1));
                    SetDependency(cellname, tree.GetVariables());
                }

                EvaluateCell(sender as Cell);

            }
            else if (e.PropertyName == "BGColor")
            {
                CellPropertyChanged(sender, new PropertyChangedEventArgs("BGColor"));
            }

        }

        //Get cell function, returns a valid cell OR null 
        public Cell GetCell(int rowIndex, int colIndex)
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

       //Get cell by the location coordinates - will call our original get cell function, this one will 
       //determine what needs to be accessed.
       public Cell GetCell(string location)
        {
            //the first part of location will be the letter (column index)
            char column = location[0];
            //the rest of the location is a number (row index)
            int row;
            //Variable to store the result (Cell)
            Cell cell;

            //Check to make sure the variable is created correctly
            //if first letter is not a character, than the variable is invalid. Just return a null cell
            if (!Char.IsLetter(column))
            {
                return null;
            }

            //If the rest of the string cannot be parsed as an integer, return null
            // If it can, it will be stored in the row variable
           if (!int.TryParse(location.Substring(1), out row))
            {
                return null;
            }

            // set the result to a get cell by sending in the row and the colum
            cell = GetCell(row - 1, column - 64);

            return cell;
        }

        //helper function to evaluate the text value of a cell to determine the value value
        private void EvaluateCell(Cell cell)
        {
            //make BasicCell to evaluate the cell
            BasicCell evalCell = cell as BasicCell;

            //variable for errors, if true, we have an error and should just return null
            bool error = false;

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

                //create an expression tree!
                ExpTree evalTree = new ExpTree(text);
                // get the variables from the tree
                string[] variables = evalTree.GetVariables();

                //go through each variable. They are the locations of each cell needed for the formula. 
                foreach (string variableName in variables)
                {
                    //First check to make sure that there is even a value to reference (call our new GetCell)
                    if (GetCell(variableName) == null)
                    {
                        //there was nothing to reference. Tell the user through the cell and cell prop changed
                        evalCell.setValue("ERROR: BAD VAR REFERENCE.");
                        CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));

                        //break out of the loop & set error to true
                        error = true;
                        break;
                    }

                    // We have determine that the cell reference is valid. Set the variable to the expTree variable 
                    //get the cell we need to edit
                    Cell variableCell = GetCell(variableName);
                    double variableValue;

                    //We will need to chck to make sure it work
                    //if the cell's value is empty, set the variable to 0.
                    if (string.IsNullOrEmpty(variableCell.Value))
                    {
                        evalTree.SetVar(variableName, 0);
                    }
                    //if the value of the cell is not a number, set to 0
                    else if (!double.TryParse(variableCell.Value, out variableValue))
                    {
                        evalTree.SetVar(variableName, 0);
                    }
                    //ELSE: should be valid! Set to the value!
                    else
                    {
                        evalTree.SetVar(variableName, variableValue);
                    }

                    //Don't have to worry about circular references, but self references could be bad here
                    string cellToEval = evalCell.ColumnIndex.ToString() + evalCell.RowIndex.ToString();
                    if (variableName == cellToEval)
                    {
                        evalCell.setValue("ERROR: VAR REF SELF.");
                        CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));

                        error = true;
                        break;
                    }

                }

                //if there is an error, stop here and return
                if (error)
                {
                    return;
                }

                //Now, all variables should be set and we can evaluate the formula using the expression tree
                evalCell.setValue(evalTree.Eval().ToString());
                CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
                
            }

            //last if it's neither of the above, it's not an formula, just test the value to the text of the original cell
            else
            {
                evalCell.setValue(evalCell.Text);
                CellPropertyChanged(cell, new PropertyChangedEventArgs("Value"));
            }

            //VERY LAST THING WE NEED IS TO UPDATE DEPENDENCIES! And evaluate all cells that were dependent on the one we just changed.
            string cellName = evalCell.ColumnIndex.ToString() + evalCell.RowIndex.ToString();
            if (dependencyDict.ContainsKey(cellName))
            {
                foreach (string dependentCell in dependencyDict[cellName])
                {
                    EvaluateCell(GetCell(dependentCell));
                }
            }

        }


        //TODO: Functions to add and remove dependencies from the Dictionary (Call when property changes)
        private void SetDependency(string cellName, string[] variables)
        {
            //iterate through each variable used in the cell
            foreach (string variableName in variables)
            {
                //If it doesn't already have the variable name, add it to the dictionary
                if (!dependencyDict.ContainsKey(variableName))
                {
                    dependencyDict[variableName] = new HashSet<string>();
                }

                //add the dependency for the variable
                dependencyDict[variableName].Add(cellName);
                
            }
        }

        //TODO: Function to remove a dependency, will call only when a cell changes (PropertyChanged)
        private void DeleteDependency(string cellName)
        {
            List<string> dependencyList = new List<string>();

            foreach (string key in dependencyDict.Keys)
            {
                if (dependencyDict[key].Contains(cellName))
                {
                    //add the key to the List
                    dependencyList.Add(key);
                }
            }
            //go through the list and remove from the dependency dict
            foreach (string item in dependencyList)
            {
                HashSet<string> removeSet = dependencyDict[item];
                //if the cell is in the set, remove it.
                if (removeSet.Contains(cellName))
                {
                    removeSet.Remove(cellName);
                }
            }
        }

    }

    
}
