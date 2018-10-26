/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 8
Due: 3/10/17 by 11:59 pm
Sources: various MSDN articles (DataGridViewCellStyle.BackColor, FromArgb, )
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetEngine
{
    public interface IUndoRedoCommand
    {
        IUndoRedoCommand Execute(Spreadsheet spreadsheet);
    }

    public class UndoRedo
    {
        //First off, declare two stacks, one for undos and one for redos they will be ofthe multicommand class
        private Stack<UndoRedoCollection> undoStack = new Stack<UndoRedoCollection>();
        private Stack<UndoRedoCollection> redoStack = new Stack<UndoRedoCollection>();

        //We need to know if we can even undo. If the stack is empty, then we cannot undo
        //check the count of the undoStack, if it's not 0, return true, else return false.
        public bool CanUndo
        {
            get { return undoStack.Count != 0; }
        }

        //Do the same thing with redo. Check the count of the stack. If 0, return false. else return true
        public bool CanRedo
        {
            get { return redoStack.Count != 0; }
        }

        //Add an undo command to the undo stack
        public void AddUndo(UndoRedoCollection undos)
        {
            //add the undo(s) to the undo stack
            undoStack.Push(undos);
            //clear from the redo stack
            redoStack.Clear();
        }

        //Perform the undo!
        public void Undo(Spreadsheet sheet)
        {
            UndoRedoCollection commands = undoStack.Pop();

            //add the undo to the redo stack.
            redoStack.Push(commands.Restore(sheet));
        }

        //Perform the redo!
        public void Redo(Spreadsheet sheet)
        {
            UndoRedoCollection commands = redoStack.Pop();

            //add the redo back onto the undo stack
            undoStack.Push(commands.Restore(sheet));
        }

        //also need something that well check and see what the description of the edit is. It's attached to the command, so we can peek at it and see what it is and return it.
        public string CheckUndo
        {
            get
            {
                //only check if there is something in the stack
                if (CanUndo)
                {
                    return undoStack.Peek().title;
                }

                //if the stack is empty, return empty string
                return string.Empty;
            }
        }

        public string CheckRedo
        {
            get
            {
                //check only if redo stack is not empty
                if (CanRedo)
                {
                    return redoStack.Peek().title;
                }

                //return empty string
                return string.Empty;
            }
        }

        
    }

    public class UndoRedoCollection
    {
        //private array of command objects (like undos and redos
        private IUndoRedoCommand[] commandObjects;
        public string title;

        //Constructors!
        public UndoRedoCollection()
        { }

        //Constructor with an array of commands
        public UndoRedoCollection(IUndoRedoCommand[] commands, string title)
        {
            commandObjects = commands;
            this.title = title;
        }

        //Constructor with a LIST of commands that will be converted into an array
        public UndoRedoCollection(List<IUndoRedoCommand> commands, string title)
        {
            commandObjects = commands.ToArray();
            this.title = title;
        }

        public UndoRedoCollection Restore(Spreadsheet spreadsheet)
        {
            //create a list the commands can be added to
            List<IUndoRedoCommand> commandList = new List<IUndoRedoCommand>();

            //for each command in the commandObjects array, Call each command
            foreach (IUndoRedoCommand command in commandObjects)
            {
                commandList.Add(command.Execute(spreadsheet));
            }

            //return the commands
            return new UndoRedoCollection(commandList.ToArray(), this.title);
        }

    }
}
