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
using CptS321;
using System.IO;
using System.Xml;
using System.Xml.Linq;

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
                string cellname = temporary.GetCellName();
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
            string cellName = evalCell.GetCellName();
            if (dependencyDict.ContainsKey(cellName))
            {
                foreach (string dependentCell in dependencyDict[cellName])
                {
                    EvaluateCell(GetCell(dependentCell));
                }
            }

        }


        //Functions to add and remove dependencies from the Dictionary (Call when property changes)
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

        //Function to remove a dependency, will call only when a cell changes (PropertyChanged)
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

        //Method that saves the spreadsheet to a stream to an XML file. Will only save the cells that have non-default values
        public void Save(Stream saveFile)
        {
            XmlWriter saveXML = XmlWriter.Create(saveFile);

            //start writing the document
            saveXML.WriteStartDocument();
            //start by creating a "spreadsheet" element that is our main element. Everything else will be a child
            saveXML.WriteStartElement("Spreadsheet");

            //save any cells that have non-default values
            foreach  (Cell indCell in cells)
            {
                //Check for non default values text and background color and  | indCell.Value != string.Empty??
                if (indCell.Text != string.Empty | indCell.BGColor != 4294967295)
                {
                    //start a cell element
                    saveXML.WriteStartElement("cell");

                    //determine the cell name
                    string cellname = indCell.GetCellName();

                    //add the name of the cell
                    saveXML.WriteAttributeString("name", cellname);

                    //nest the text and background values into the cell element
                    saveXML.WriteElementString("text", indCell.Text);
                    saveXML.WriteElementString("bgcolor", indCell.BGColor.ToString());

                    saveXML.WriteWhitespace("\n");

                    //end the cell element
                    saveXML.WriteEndElement();
                }
            }

            //end the spreadsheet element
            saveXML.WriteEndElement();

            //Close the writer to save
            saveXML.Close();
        }

        //Loads a spreadsheet from a stream, looks for bgcolor and text values but they should be able to be in any order. Should also ignore any weird tags.
        public void Load(Stream loadFile)
        {
            //create an xmlDocument to read the xml file and load the loadFile
            XmlDocument loadedFile = new XmlDocument();
            loadedFile.Load(loadFile);

            //find each cell node in the spreadsheet and set the text and bg color of whatever cell it represents 
            XmlNode sheet = loadedFile.SelectSingleNode("Spreadsheet");
            XmlNodeList cellList = sheet.SelectNodes("cell");

            foreach (XmlNode cell in cellList)
            {
                string cellname = cell.Attributes.GetNamedItem("name").Value;

                Cell editCell = GetCell(cellname);

                //Get the text value. Check if it's empty, if it is, we can just leave it and don't need to reset it to empty.
                if (cell.SelectSingleNode("text").InnerText.ToString() != string.Empty)
                {
                    editCell.Text = cell.SelectSingleNode("text").InnerText.ToString();
                }

                //Same with the bg color check if value is default, don't change if default
                if (cell.SelectSingleNode("bgcolor").InnerText.ToString() != "4294967295")
                {
                    uint Color;

                    if (uint.TryParse(cell.SelectSingleNode("bgcolor").InnerText, out Color))
                    {
                        editCell.BGColor = Color;
                    }

                }
            }
        }
    }

    
}
