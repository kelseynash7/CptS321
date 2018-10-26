/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 3
Due: 2/3/17 by 11:59 pm
Sources: various MSDN articles
         C# 6.0 In A Nutshell - Course Textbook
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Numerics;

namespace KelseyNash_HW3
{
    public class FibonacciTextReader : TextReader
    {
        private int numLine;
        private int currentLine;

        public FibonacciTextReader(int numLines)
        {
            numLine = numLines;
            currentLine = 1; //We always start on line 1
        }


        public override string ReadLine()
        {
            if (currentLine <= numLine)
            {
                //call the findFibonacci helper function to get the fibonacci number, but subtract 1 because line number starts at 1 but the sequence starts at 0
                return currentLine.ToString() + ": " + FindFibonacci(currentLine - 1).ToString();
            }
            else
            {
                return null;
            }
            
        }

        public override string ReadToEnd()
        {
            StringBuilder result = new StringBuilder();

            for (int count = 1; count <= numLine; count++ )
            {
                result.AppendLine(ReadLine());
                //increment currentLine
                currentLine += 1;
            }

            return result.ToString();
        }

        public BigInteger FindFibonacci(BigInteger number)
        {

            // iterative approach faster for large numbers (rather than recursive)
            BigInteger a = 0, b = 1;

            //start from 0 and compute by counting up
            for (BigInteger i = 0; i < number; i++)
            {
                //temp = n -2
                BigInteger temp = a;
                //a = n -1 : will be equal to n-2 at the start of the next loop!
                a = b;
                //fib = n-2 + n-1
                b = temp + b;
            }
            return a;

        }
        
    }
}
