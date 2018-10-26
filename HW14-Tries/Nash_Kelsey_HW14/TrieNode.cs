/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 14
Due: 4/28/2017 at 11:59 pm
Sources: Lecture Notes from 4/21/17 and
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nash_Kelsey_HW14
{
    public class TrieNode
    {
        #region Attributes/Fields

        private char _letter;

        public List<TrieNode> children;

        private string _word;

        #endregion

        #region Properties

        public char Letter
        {
            get { return _letter; }
            set
            {
                if (_letter != value)
                {
                    _letter = value;
                }
            }
        }

        public string Word
        {
            get { return _word; }
            set
            {
                //require the node to be a null node to allow it to store a word
                if (_letter == '\0')
                {
                    _word = value;
                }
            }
        }


        #endregion

        #region Constructor

        public TrieNode(char c)
        {
            Letter = c;
            children = new List<TrieNode>();
            _word = string.Empty;
        }

        #endregion

        #region Methods

        public TrieNode AddOrGetChild(char c)
        {
            foreach (TrieNode child in children)
            {
                if (child.Letter == c)
                {
                    return child;
                }
            }

            //if we are here, we have not found the child, so add it
            var newChild = new TrieNode(c);
            children.Add(newChild);
            return newChild;
        }

        #endregion

    }

}
