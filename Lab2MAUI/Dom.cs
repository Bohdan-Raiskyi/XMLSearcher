using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Lab2MAUI
{
    public class Dom : IStrategy
    {
        public List<Student> Search(Student student, string path)
        {
            List<Student> results = new List<Student>();
            XmlDocument doc = new XmlDocument();

            try
            {
                doc.Load(path);
            }
            catch
            {
                return null;
            }

            XmlNode node = doc.DocumentElement;
            foreach (XmlNode n in node.ChildNodes)
            {
                string name = "";
                string faculty = "";
                string buildnum = "";
                string roomnum = "";
                string restime = "";
                string docnum = "";

                foreach (XmlAttribute attribute in n.Attributes) 
                { 
                    switch (attribute.Name) 
                    { 
                        case "NAME": 
                            if (attribute.Value.Equals(student.Name) || student.Name == null) 
                                name = attribute.Value; 
                            break; 
                        case "FACULTY": 
                            if (attribute.Value.Equals(student.Faculty) || student.Faculty == null) 
                                faculty = attribute.Value; 
                            break; 
                        case "BUILDNUM": 
                            if (attribute.Value.Equals(student.BuildNum) || student.BuildNum == null) 
                                buildnum = attribute.Value; 
                            break;
                        case "ROOMNUM": 
                            if (attribute.Value.Equals(student.RoomNum) || student.RoomNum == null) 
                                roomnum = attribute.Value; 
                            break; 
                        case "RESTIME": 
                            if (attribute.Value.Equals(student.ResTime) || student.ResTime == null) 
                                restime = attribute.Value; 
                            break;
                        case "DOCNUM": 
                            if (attribute.Value.Equals(student.DocNum) || student.DocNum == null) 
                                docnum = attribute.Value; 
                            break; 
                        default: 
                            break;
                    } 
                }

                if (name != "" && faculty != "" && buildnum != "" && roomnum != "" && restime != "" && docnum != "")
                {
                    Student newStudent = new Student 
                    { 
                        Name = name, 
                        Faculty = faculty, 
                        BuildNum = buildnum, 
                        RoomNum = roomnum, 
                        DocNum = docnum, 
                        ResTime = restime 
                    };

                    results.Add(newStudent);
                }
            }
            return results;
        }
    }
}
