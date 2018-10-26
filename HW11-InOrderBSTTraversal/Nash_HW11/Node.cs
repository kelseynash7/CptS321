/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 11
Due: 4/7/17 by 11:59 pm
Sources: Homework #1, http://www.geeksforgeeks.org/inorder-tree-traversal-without-recursion-and-without-stack/ <- for help with traversal #3
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nash_HW11
{
    public class Node
    {
        // Fields
        private int _data;
        private Node leftChild;
        private Node rightChild;

        // Constructors
        // one with no parameters, one with data
        public Node() { }

        public Node (int data)
        {
            _data = data;
            leftChild = null;
            rightChild = null;
        }

        //public accessors

        public int Data
        {
            get { return _data; }
            set
            {
                if (_data != value)
                {
                    _data = value;
                }
            }
        }

        public Node LeftChild
        {
            get { return leftChild; }
            set
            {
                if ( leftChild != value)
                {
                    leftChild = value;
                }
            }
        }

        public Node RightChild
        {
            get { return rightChild; }
            set
            {
                if (rightChild != value)
                {
                    rightChild = value;
                }
            }
        }




    }
}
