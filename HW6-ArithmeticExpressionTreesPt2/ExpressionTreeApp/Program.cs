using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CptS321;

namespace ExpressionTreeApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string userInput;

            ExpTree Tree = new ExpTree("5+A2+7");

           
            //Loop through menu while 
            do
            {
                //MENU
                StringBuilder Menu = new StringBuilder();
                Menu.AppendLine("Menu (current expression = \"" + Tree.exp + "\")");
                Menu.AppendLine("   1 = Enter a new expression");
                Menu.AppendLine("   2 = Set a variable value");
                Menu.AppendLine("   3 = Evaluate Tree");
                Menu.AppendLine("   4 = Quit");

                Console.WriteLine(Menu);

                userInput = Console.ReadLine();


                //Enter a new expression means building a new expression tree.
                if (userInput == "1")
                {
                    Console.WriteLine("Enter new expression: ");
                    string expression = Console.ReadLine();
                    Tree = new ExpTree(expression);
                    
                }
                //Set a new variable value
                else if (userInput == "2")
                {
                    //Check for error
                    if (Tree.exp != "ERROR")
                    {
                        Console.Write("Enter variable name: ");
                        string varName = Console.ReadLine();
                        Console.Write("Enter variable value: ");
                        string varValue = Console.ReadLine();
                        double num;
                        //make sure user enters a number
                        while (!double.TryParse(varValue, out num))
                        {
                            Console.Write("Enter variable value: ");
                            varValue = Console.ReadLine();
                        }
                        Tree.SetVar(varName, num);
                    }
                    else
                    {
                        Console.WriteLine("No Variables in an ERROR.");
                    }
                }
                //Evaluate the expression
                else if (userInput == "3")
                {
                    // check to make sure we don't have an errored expression
                    if (Tree.exp == "ERROR")
                    {
                        Console.WriteLine("ERROR: Cannot compute an error.");
                    }
                    else
                    {
                        Console.WriteLine(Tree.Eval());
                    }
                }

            } while (userInput != "4");

        }

      

    }
}
