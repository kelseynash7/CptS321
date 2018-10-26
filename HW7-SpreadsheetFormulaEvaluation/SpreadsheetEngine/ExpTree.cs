/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 7
Due: 3/3/17 by 11:59 pm
Sources: various MSDN articles (BeginCellEdit, EndCellEdit)
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CptS321
{
    public class ExpTree
    {
        //Fields
        private Node root;
        protected Dictionary<string, double> variableDict;

        //public field to hold current expression
        public string exp;


        //Constructor that constructs an expression tree using the expression provided
        public ExpTree(string expression)
        {
            exp = expression;

            //Clear the dictionary when a new tree is made
            variableDict = new Dictionary<string, double>();

            //compile expression to the root node
            this.root = Compile(expression);


        }

        //Sets the specified variable with the value in the ExpTree variables dictionary
        public void SetVar(string varName, double varValue)
        {
            variableDict[varName] = varValue;
        }

        //Implement this member function with no parameters that evaluates the expression to a double value
        public double Eval()
        {
            if (root != null)
            {
                return Eval(root);
            }
            else
            {
                return double.NaN;
            }
        }

        private double Eval(Node node)
        {
            //Evaluate based on the kind of node

            ConstNode constnode = node as ConstNode;
            if (constnode != null)
            {
                return constnode.OpValue;
            }

            VarNode varnode = node as VarNode;
            if (varnode != null)
            {
                // used to be a try/catch, but now we set every new variable to 0 when the tree is made, so there will always be a value to obtain.
                return variableDict[varnode.Name];
            }

            OpNode opnode = node as OpNode;
            if (opnode != null)
            {
                switch (opnode.Op)
                {
                    case '+':
                        return Eval(opnode.Left) + Eval(opnode.Right);

                    case '-':
                        return Eval(opnode.Left) - Eval(opnode.Right);

                    case '*':
                        return Eval(opnode.Left) * Eval(opnode.Right);

                    case '/':
                        return Eval(opnode.Left) / Eval(opnode.Right);
                }

            }

            return 0;
        }

        //Method to find the lowest precedence operator in the expression
        private static int GetLowOpIndex(string exp)
        {
            // need a counter for the parenthesis, if the counter isn't at 0, then we are inside a set of parenthesis
            int parenthCounter = 0;
            // index of the lowest precedence op. Initialize to -1. No op = -1
            int index = -1;

            //Start from the end of the expression and traverse
            for (int i = exp.Length - 1; i >= 0; i--)
            {
                switch (exp[i])
                {
                    case ')':
                        //closing parenthesis, decrease count
                        parenthCounter--;
                        break;
                    case '(':
                        //opening parenthesis, increase count
                        parenthCounter++;
                        break;
                    //Addition and subtraction have the same precedence
                    case '+':
                    case '-':
                        // check to see if we are in parenthesis or not, do nothing if we are
                        if (parenthCounter == 0)
                        {
                            //this is the lowest precedence op, return it's index
                            return i;
                        }
                        break;

                    // multiplication and division have the same precedence
                    case '*':
                    case '/':
                        //need the check the parenthesis counter and the index. If the index isn't -1, there is another op before this one
                        if (parenthCounter == 0 && index == -1)
                        {
                            //keep 
                            index = i;
                        }
                        break;
                }

            }
            //if the parentheses counter is not 0, we have a problem.
            if (parenthCounter != 0)
            {
                // -2 will be our indicator that there is a parentheses problem
                return -2;
            }

            //return the index of a * or / because there were no + or -
            return index;

        }

        //Build node 
        private Node BuildSimple(string term)
        {
            double num;
            //if term is a number, put in a constNode
            if (double.TryParse(term, out num))
            {
                return new ConstNode(num);
            }
            //if term is a variable, put in varNode -- will not start with a number!
            //get the entry for the variable into the dictionary and set to equal 0
            SetVar(term, 0);
            return new VarNode(term);

        }

        //Compile function
        private Node Compile(string exp)
        {
            //Remove any spaces from the expression
            exp = exp.Replace(" ", "");

            //check for the expression to be totally enclosed in ()
            if (exp[0] == '(')
            {
                //counter for parenthesis
                int counter = 1;

                for (int i = 1; i < exp.Length; i++)
                {
                    if (exp[i] == ')')
                    {
                        //deincrement counter
                        counter--;
                        //if we are not in the middle of a set of parenthesis count will be 0
                        if (counter == 0)
                        {
                            //if we are at the end of the expression
                            if (i == exp.Length - 1)
                            {
                                //call compile on the rest of the string
                                return Compile(exp.Substring(1, exp.Length - 2));
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (exp[i] == '(')
                    {
                        //increment counter
                        counter++;
                    }
                }

                //If we get to the end and the parenthesis counter is not 0, we have an odd number, do not build tree.
                if (counter != 0)
                {
                    Console.WriteLine("ERROR: Invalid expression, too many or too few parentheses");
                    exp = "ERROR";
                    return null;
                }
            }



            //Call GetLowOpIndex and build an op node for character at that index
            int index = GetLowOpIndex(exp);
            //check to make sure there is actually an operator
            if (index != -1 && index != -2)
            {
                return new OpNode(exp[index], Compile(exp.Substring(0, index)), Compile(exp.Substring(index + 1)));
            }
            else if (index == -2)
            {
                // There are a bad number of parenthesis
                Console.WriteLine("ERROR: Invalid expression, too many or too few parentheses");
                this.exp = "ERROR";
                return null;
            }

            return BuildSimple(exp);

        }



        //Base Class Node - VarNode, ConstNode and OpNode nodes will inherit from here,
        private abstract class Node
        {
            protected string name;
            protected double opValue;
            //Property to get and set name
            public string Name
            {
                get { return name; }
                set
                {
                    if (name == value)
                        return;

                    name = value;
                }
            }
            //Property to get and set value
            public double OpValue
            {
                get { return opValue; }
                set
                {
                    if (opValue == value)
                        return;

                    opValue = value;
                }
            }

        }


        private class ConstNode : Node
        {
            // constructor
            public ConstNode(double number)
            {
                opValue = number;
            }

        }

        private class VarNode : Node
        {
            public VarNode(string varName)
            {
                name = varName;
            }

        }

        private class OpNode : Node
        {
            private char op;
            private Node left, right;

            public OpNode(char Op, Node Left, Node Right)
            {
                op = Op;
                left = Left;
                right = Right;
            }

            public char Op
            {
                get { return op; }
            }

            public Node Left
            {
                get { return left; }
            }
            public Node Right
            {
                get { return right; }
            }

        }

        public string[] GetVariables()
        {
            return variableDict.Keys.ToArray();
        }
    }

}
