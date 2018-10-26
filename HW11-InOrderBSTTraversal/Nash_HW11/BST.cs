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
    public class BST
    {
        // Fields
        private Node root;

        // Constructor
        public BST()
        {
            root = null;
        }

        // Property (root access)
        public Node Root
        {
            get { return root; }
            set
            {
                if (root != value)
                {
                    root = value;
                }
            }
        }

        // Methods

        // INSERT FUNCTION
        public void Insert(int data)
        {
            // create new node to insert node with value that is passed into the function
            Node newNode = new Node(data);

            // create current and parent (previous) nodes to add to easily add to the tree.
            // We always start at the root node, so current defaults to root
            Node current = root;
            Node parent = null;

            // Search for insert point for new node!
            while (current != null)
            {
                // check for duplicate, we do not allow duplicates in our BST
                if (current.Data == data)
                    return;
                // now compare new data to current data. If new is less than current, we move to the left
                else if (current.Data > data)
                {
                    //Move to left tree
                    parent = current;
                    current = current.LeftChild;
                }
                //if new is larger than current, move to right tree
                else if (current.Data < data)
                {
                    //Move to right tree
                    parent = current;
                    current = current.RightChild;
                }
            }

            // Should now be in the correct position to add to the tree but we need to check if we even moved at all
            if (parent == null)
            {
                //this is our first node, so it should be the root!
                root = newNode;
            }
            else
            {
                // we had to move, not root. Verify data in current against parent to determine which child it should be.
                if (parent.Data > data)
                {
                    // left subtree
                    parent.LeftChild = newNode;
                }
                else //parent data < data
                {
                    parent.RightChild = newNode;
                }
            }
        }

        // InOrder Traversal #1 - The usual using two recursive calls
        public void InOrderTraversal1(Node current)
        {
            if (current != null)
            {
                // First go as far left as possible
                InOrderTraversal1(current.LeftChild);

                // Print the current node on a single line
                Console.Write(current.Data + " ");

                // Now travel right
                InOrderTraversal1(current.RightChild);
            }
        }

        // InOrder Traversal #2 - traversal with a stack and no recursion
        public void InOrderTraversal2(Node root)
        {
            // need a stack to push values on to and 
            Stack<Node> printStack = new Stack<Node>();
            Node current = new Node();
            current = root;
            bool success = false;

            // Keep looping until current is null and stack has no elements in it.
            while (success != true)
            {
                // first we need to add to the stack as we get as far left in the tree as we can
                if  (current != null)
                {
                    printStack.Push(current);
                    current = current.LeftChild;
                }
                // if current is null, we now need to check the stack
                else
                {
                    //if the stack is not empty, we have something to print.
                    if (printStack.Count != 0)
                    {
                        current = printStack.Pop();
                        Console.Write(current.Data + " ");

                        // move to the right of current
                        current = current.RightChild;
                    }
                    else
                    {
                        // if we are here, we are done
                        success = true;
                    }
                }
            }
        }

        // InOrder Traversal #3 - no recursion and no stack.
        public void InOrderTraversal3(Node root)
        {
            Node current = new Node();
            Node previous = new Node();

            current = root;

            //keep looping while current is not null
            while (current != null)
            {
                //check to see if current's left child is null, if so print current and move right
                if (current.LeftChild == null)
                {
                    Console.Write(current.Data + " ");
                    current = current.RightChild;
                }
                else
                {
                    // set previous to current's left child
                    previous = current.LeftChild;

                    //move as far right as you can on the left child
                    while (previous.RightChild != null && previous.RightChild != current)
                    {
                        previous = previous.RightChild;
                    }
                    // if previous' right child is null, then set current to it's right child
                    if (previous.RightChild == null)
                    {
                        previous.RightChild = current;
                        current = current.LeftChild;
                    }
                    // revert changes so we're not editing the tree and move current to it's right child
                    else
                    {
                        previous.RightChild = null;
                        Console.Write(current.Data + " ");
                        current = current.RightChild;
                    }
                }
            }
        }
    }
}
