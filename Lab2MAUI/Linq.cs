using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Lab2MAUI
{
    public class Linq : IStrategy
    {
        public List<Student> Search(Student student, string path)
        {
            List<Student> results = new List<Student>();
            XDocument doc;

            Debug.WriteLine(1);

            doc = XDocument.Load(path);

            var result = from obj in doc.Descendants("student")
                         where
                         (
                         (obj.Attribute("NAME").Value.Equals(student.Name) || student.Name == null) &&
                         (obj.Attribute("FACULTY").Value.Equals(student.Faculty) || student.Faculty == null) &&
                         (obj.Attribute("BUILDNUM").Value.Equals(student.BuildNum) || student.BuildNum == null) &&
                         (obj.Attribute("ROOMNUM").Value.Equals(student.RoomNum) || student.RoomNum == null) &&
                         (obj.Attribute("RESTIME").Value.Equals(student.ResTime) || student.ResTime == null) &&
                         (obj.Attribute("DOCNUM").Value.Equals(student.DocNum) || student.DocNum == null)
                         )
                         select new
                         {
                             name = (string)obj.Attribute("NAME"),
                             faculty = (string)obj.Attribute("FACULTY"),
                             buildnum = (string)obj.Attribute("BUILDNUM"),
                             roomnum = (string)obj.Attribute("ROOMNUM"),
                             restime = (string)obj.Attribute("RESTIME"),
                             docnum = (string)obj.Attribute("DOCNUM")
                         };

            Debug.WriteLine(2);

            foreach (var st in result)
            {
                Debug.WriteLine(3);
                Student newStudent = new Student 
                { 
                    Name = st.name, 
                    Faculty = st.faculty, 
                    BuildNum = st.buildnum, 
                    RoomNum = st.roomnum, 
                    ResTime = st.restime, 
                    DocNum = st.docnum 
                };

                results.Add(newStudent);
            }
            return results;
        }
    }
}
