/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 10
Due: 3/31/17 by 11:59 pm
Sources: Lecture Notes

*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace CptS321
{
    public abstract class Cell : INotifyPropertyChanged
    {
        //Constructor
        public Cell(int row, char col)
        {
            rowIndex = row;
            columnIndex = col;
        }

        //RowIndex private variable and public "readonly" property
        private readonly int rowIndex;
        public int RowIndex
        {
            get { return rowIndex; }
        }

        //ColumnIndex private variable and public "readonly" property
        private readonly char columnIndex;
        public char ColumnIndex
        {
            get { return columnIndex; }
        }

        //Text private variable and public property
        protected string text = string.Empty;
        public string Text
        {
            get { return text; }
            set
            {
                //check to see if text is not already set to value
                if (text == value)
                { return; }
                
                //else text is changing to something new
                text = value;
                OnPropertyChanged("Text");
                
            }
        }

        //Value private variable and public property
        protected string value;
        public string Value
        {
            get { return value; }
        }

        //BackGround Color private variable and public property
        private uint bgcolor = 4294967295; //(also set to white. It will be every cell's default color)
        public uint BGColor
        {
            get { return bgcolor; }
            set
            {
                if (value != bgcolor)
                {
                    bgcolor = value;

                    OnPropertyChanged("BGColor");
                }
            }
        }

        //Method to clear a cell
        public void Clear()
        {
            Text = string.Empty;
            BGColor = 4294967295;
        }

        //Method to get the name of a cell
        public string GetCellName()
        {
            string cellname = ColumnIndex.ToString() + RowIndex.ToString();

            return cellname;
        }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        #endregion
    }

   
}
