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
    class Trie
    {
        private TrieNode _root;
        public TrieNode Root
        {
            get { return _root; }
        }



        #region Constructor
        public Trie()
        {
            //the root won't actually store anything important. So set it to empty
            _root = new TrieNode(' ');
        }

        #endregion

        #region Methods

        //Need to be able to add words to the trie (AddString)
        //Credit to lecture 
        public void AddString(string word)
        {
            TrieNode node = _root;

            foreach (char letter in word)
            {
                node = node.AddOrGetChild(letter);
            }

            //manually add the null terminator at the end
            node = node.AddOrGetChild('\0');
            //Verify that the node is set to null, then add the word to the node.
            if (node.Letter == '\0')
            {
                node.Word = word;
            }
            
        }
    
        public List<string> FindAllWords(string prefix)
        {
            List<string> results = new List<string>();

            //current node - start at the root
            TrieNode current = _root;

            //We want to iterate to the end of the prefix and then get any words from there.
            foreach (char letter in prefix)
            {
                //check through the child nodes for each letter of the prefix
                foreach (TrieNode child in current.children)
                {
                    //if the letter is in the child node
                    if (child.Letter == letter)
                    {
                        current = child;
                        //break out of loop looking for children
                        break;
                    }
               
                }

                //make sure that wewe found a letter if we didn't we neet to return an empty list
                if (current.Letter != letter)
                {
                    return results;
                }
            }
            //If we're here, we have iterated to the end of the prefix.
            //Get get all the words that have the prefix
            FindChildWords(current, results);

            return results;


        }

        //Helper function that finds each word that uses the prefix and adds it to a list.
        private void FindChildWords(TrieNode node, List<string> results)
        {
            TrieNode SearchFromNode = node;

            foreach (TrieNode child in node.children)
            {
                //first check if the node is the null node
                if (child.Letter == '\0')
                {
                    //if it is, add it's word to the results
                    results.Add(child.Word);
                }
                else
                {
                    //recursively call FindChildWords on the next child node.
                    FindChildWords(child, results);
                }
            }
        }

        #endregion
    }
}
