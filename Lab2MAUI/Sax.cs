using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;
using System.Threading.Tasks;

namespace Lab2MAUI
{
    public class Sax : IStrategy
    {
        public List<Student> Search(Student student, string path)
        {
            List<Student> results = new List<Student>();
            XmlTextReader xmlReader;

            try
            {
                xmlReader = new XmlTextReader(path);
            }
            catch
            {
                return null;
            }

            while (xmlReader.Read())
            {
                if (xmlReader.HasAttributes)
                {
                    while (xmlReader.MoveToNextAttribute())
                    {
                        string name = "";
                        string faculty = "";
                        string buildnum = "";
                        string roomnum = "";
                        string restime = "";
                        string docnum = "";

                        if (xmlReader.Name.Equals("NAME") && (xmlReader.Value.Equals(student.Name) || student.Name == null))
                        {
                            name = xmlReader.Value;
                            xmlReader.MoveToNextAttribute();
                            if (xmlReader.Name.Equals("FACULTY") && (xmlReader.Value.Equals(student.Faculty) || student.Faculty == null))
                            {
                                faculty = xmlReader.Value;
                                xmlReader.MoveToNextAttribute();
                                if (xmlReader.Name.Equals("BUILDNUM") && (xmlReader.Value.Equals(student.BuildNum) || student.BuildNum == null))
                                {
                                    buildnum = xmlReader.Value;
                                    xmlReader.MoveToNextAttribute();
                                    if (xmlReader.Name.Equals("ROOMNUM") && (xmlReader.Value.Equals(student.RoomNum) || student.RoomNum == null))
                                    {
                                        roomnum = xmlReader.Value;
                                        xmlReader.MoveToNextAttribute();
                                        if (xmlReader.Name.Equals("RESTIME") && (xmlReader.Value.Equals(student.ResTime) || student.ResTime == null))
                                        {
                                            restime = xmlReader.Value;
                                            xmlReader.MoveToNextAttribute();
                                            if (xmlReader.Name.Equals("DOCNUM") && (xmlReader.Value.Equals(student.DocNum) || student.DocNum == null))
                                            {
                                                docnum = xmlReader.Value;
                                            }
                                        }
                                    }
                                }
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
                                ResTime = restime,
                                DocNum = docnum 
                            };

                            results.Add(newStudent);
                        }
                    }
                }
            }
            xmlReader.Close();
            return results;
        }
    }
}
