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
    class Program
    {
        static void Main(string[] args)
        {
            // Creating a simple program to run traversals - will let user choose to create a new random tree after all three traversals have run.

            //Default to yes so it will run at least once.
            string userInput = "y";

            while (userInput == "y")
            {
                BST tree = new BST();
                Random random = new Random();

                //use random numbers (0-100) to create a BST (20 - 30 nodes)
                int nodes = random.Next(20, 30);
                
                for (int i = 0; i < nodes; i++)
                {
                    tree.Insert(random.Next(0, 101));
                }

                // Do the traversals
                // #3 No stack, no recursion
                Console.WriteLine("Traversal of the tree with NO stack and NO recursion:");
                tree.InOrderTraversal3(tree.Root);
                Console.WriteLine();

                // #2 Stack, No recursion
                Console.WriteLine("Traversal of the tree using a stack but NO recursion:");
                tree.InOrderTraversal2(tree.Root);
                Console.WriteLine(); // For spacing

                // #1 Recursive
                Console.WriteLine("Traversal of the tree using recursion:");
                tree.InOrderTraversal1(tree.Root);
                Console.WriteLine(); // For spacing


                // Check with user to see if they'd like to go again!
                Console.WriteLine("Again? (y/n)");
                userInput = Console.ReadLine().ToLower();

            }

        }
    }
}
