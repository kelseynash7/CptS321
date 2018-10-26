/*
Name: Kelsey Nash
Student ID: 11093115
Homework # 12
Due: 4/14/2017 at 11:59 pm
Sources: (Issues with setting the results box) http://stackoverflow.com/questions/10775367/cross-thread-operation-not-valid-control-textbox1-accessed-from-a-thread-othe
MSDN WebClient and Invoke Method Articles
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nash_HW12
{
    public class Elements
    {

        //Constructor
        public Elements()
        {
            _downloadString = string.Empty;
            L1 = createRandomList();
            L2 = createRandomList();
            L3 = createRandomList();
            L4 = createRandomList();
            L5 = createRandomList();
            L6 = createRandomList();
            L7 = createRandomList();
            L8 = createRandomList();
        }

        private string _downloadString;

        public string DownloadString
        {
            get { return _downloadString; }
            set
            {
                if (_downloadString != value)
                {
                    _downloadString = value;
                }
            }
        }

        private string _resultString;

        public string ResultString
        {
            get { return _resultString; }
            set
            {
                if (_resultString != value)
                {
                    _resultString = value;
                }
            }
        }

        /*For Part 2*/

        public List<int> L1;
        public List<int> L2;
        public List<int> L3;
        public List<int> L4;
        public List<int> L5;
        public List<int> L6;
        public List<int> L7;
        public List<int> L8;

        private List<int> createRandomList()
        {
            var random = new Random();
            var list = new List<int>();

            for (int i = 0; i < 1000000; i++)
            {
                list.Add(random.Next());
            }

            return list;
        }
        
        public void RandomizeLists()
        {
            L1 = createRandomList();
            L2 = createRandomList();
            L3 = createRandomList();
            L4 = createRandomList();
            L5 = createRandomList();
            L6 = createRandomList();
            L7 = createRandomList();
            L8 = createRandomList();
        }

    }
}
