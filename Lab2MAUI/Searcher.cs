using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab2MAUI
{
    public class Searcher
    {
        private Student student;
        private IStrategy strategy;
        static private string xmlFilePath;
        public Searcher(Student st, IStrategy str, string path)
        {
            student = st;
            strategy = str;
            xmlFilePath = path;
        }
        public List<Student> SearchAlgorithm()
        {
            return strategy.Search(student, xmlFilePath);
        }
    }
}
