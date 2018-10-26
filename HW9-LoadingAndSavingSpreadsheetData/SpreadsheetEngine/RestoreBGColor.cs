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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace SpreadsheetEngine
{
    public class RestoreBGColor :IUndoRedoCommand
    {
        //need the cell and the color so declare private variables for each
        private Cell cell;
        private uint color;

        public RestoreBGColor(Cell updateCell, uint updateColor)
        {
            cell = updateCell;
            color = updateColor;
        }

        //restore previous color of cell
        public IUndoRedoCommand Execute(Spreadsheet sheet)
        {
            //get the cell name from the cell
            string cellName = this.cell.ColumnIndex.ToString() + this.cell.RowIndex.ToString();

            //store the previous color
            uint currentColor = cell.BGColor;

            //set the background color
            cell.BGColor = color;

            //return the cell and color
            return new RestoreBGColor(cell, currentColor);

        }

    }
}
