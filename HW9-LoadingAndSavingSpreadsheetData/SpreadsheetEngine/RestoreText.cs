/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 9
Due: 3/24/17 by 11:59 pm
Sources: various MSDN articles (XmlWriter Class), http://www.codeguru.com/csharp/csharp/cs_data/xml/article.php/c4227/A-Simple-Way-to-Write-XML-in-NET-XmlTextWriter.htm,
https://www.codeproject.com/Articles/169598/Parse-XML-Documents-by-XMLDocument-and-XDocument

*/

using CptS321;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    public class RestoreText : IUndoRedoCommand
    {
        private Cell cell;
        private string text;

        public RestoreText(Cell c, string t)
        {
            cell = c;
            text = t;
        }


        //Restores a cell with the text it had previously (undo OR redo)
        public IUndoRedoCommand Execute(Spreadsheet spreadsheet)
        {
            //determine the "name of the cell so we can get it from the spreadsheet and edit it!
            string cellName = this.cell.ColumnIndex.ToString() + this.cell.RowIndex.ToString();
            //
            Cell cell = spreadsheet.GetCell(cellName);

            //get the text that is currently in the cell, we will want it in case we undo/redo
            string currentText = cell.Text;

            //set the cell's text to the updated text
            cell.Text = this.text;

            //return the old cell and old text
            return new RestoreText(cell, currentText);
        }
    }
}
