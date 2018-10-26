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

            //compile expression to the root node
            this.root = Compile(expression);

            //Clear the dictionary when a new tree is made
            variableDict = new Dictionary<string, double>();
        }

        //Sets the specified variable with the value in the ExpTree variables dictionary
        public void SetVar(string varName, double varValue)
        {
            variableDict[varName] = varValue;
        }

        //Implement this member function with no parameters that evaluates the expression to a double value
        public double Eval()
        {
            return Eval(root);
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
                try
                { return variableDict[varnode.Name]; }
                catch
                {
                    Console.WriteLine("Variable " + varnode.Name + " has not been defined. Continuing evaluation with variable equal to 0.");
                }
            }

            OpNode opnode = node as OpNode;
            if (opnode != null) 
            {
                switch(opnode.Op)
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

        //Build node 
        private static Node BuildSimple(string term)
        {
            double num;
            //if term is a number, put in a constNode
            if (double.TryParse(term, out num))
            {
                return new ConstNode(num);
            }
            //if term is a variable, put in varNode
            VarNode node = new VarNode(term);
            

            return new VarNode(term);
        }

        //Compile function
        private static Node Compile(string exp)
        {
            //Remove any spaces from the expression
            exp = exp.Replace(" ", ""); 

            for (int i = exp.Length -1; i >= 0; i--)
            {
                switch (exp[i])
                {
                    case '+':
                    case '-':
                    case '*':
                    case '/':
                        return new OpNode(
                            exp[i],
                            Compile(exp.Substring(0, i)),
                            Compile(exp.Substring(i + 1)));
                        
                }

            }

            //Go through the entire string if there are no operators...
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

           // public abstract double eval();

        }


        private class ConstNode : Node
        {
            public ConstNode(double number)
            {
                opValue = number;
            }

           /* public override double eval()
            {
                return opValue;
            }*/
        }

        private class VarNode : Node
        {
            public VarNode(string varName)
            {
                name = varName;
            }

            /*public override double eval()
            {

                return opValue;
            }*/
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


           /* public override double eval()
            {
                switch (op)
                {
                    case '+':
                        return left.eval() + right.eval();

                    case '-':
                        return left.eval() - right.eval();

                    case '*':
                        return left.eval() * right.eval();

                    case '/':
                        return left.eval() / right.eval();
                    default:
                        return -1.0;
                        
                }

            }*/
        }
    }
}
