using System.Reflection;
using System.Xml.Xsl;
using System.Xml;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using Microsoft.Maui;

namespace Lab2MAUI
{
    public partial class MainPage : ContentPage
    {
        private string xmlFilePath = "";
        public MainPage()
        {
            InitializeComponent();
            SaxBtn.IsChecked = true;
        }
        private async void GetAllAuthors()
        {
            XmlDocument doc = new XmlDocument();
            var appLocation = Assembly.GetEntryAssembly().Location;
            var appPath = Path.GetDirectoryName(appLocation);
            Directory.SetCurrentDirectory(appPath);

            try
            {
                doc.Load(xmlFilePath);
            }
            catch 
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Incorrect file!", "OK");
                return;
            }

            XmlElement xRoot = doc.DocumentElement;
            XmlNodeList childNodes = xRoot.SelectNodes("student");

            Debug.WriteLine(childNodes.Count);
            for (int i = 0; i < childNodes.Count; i++)
            {
                XmlNode n = childNodes.Item(i);
                addItems(n);
            }

        }
        private void addItems(XmlNode n)
        {
            if (!NamePicker.Items.Contains(n.SelectSingleNode("@NAME").Value))
                NamePicker.Items.Add(n.SelectSingleNode("@NAME").Value);
            if (!FacultyPicker.Items.Contains(n.SelectSingleNode("@FACULTY").Value))
                FacultyPicker.Items.Add(n.SelectSingleNode("@FACULTY").Value);
            if (!BuildNumPicker.Items.Contains(n.SelectSingleNode("@BUILDNUM").Value))
                BuildNumPicker.Items.Add(n.SelectSingleNode("@BUILDNUM").Value);
            if (!RoomNumPicker.Items.Contains(n.SelectSingleNode("@ROOMNUM").Value))
                RoomNumPicker.Items.Add(n.SelectSingleNode("@ROOMNUM").Value);
            if (!ResTimePicker.Items.Contains(n.SelectSingleNode("@RESTIME").Value))
                ResTimePicker.Items.Add(n.SelectSingleNode("@RESTIME").Value);
            if (!DocNumPicker.Items.Contains(n.SelectSingleNode("@DOCNUM").Value))
                DocNumPicker.Items.Add(n.SelectSingleNode("@DOCNUM").Value);
        }
        private void SearchBtnHandler(object sender, EventArgs e)
        {
            editor.Text = "";
            Student student = GetSelectedParameters();
            IStrategy analyzer = GetSelectedAnalyzer();
            PerformSearch(student, analyzer);
        }
        private Student GetSelectedParameters()
        {
            Student student = new Student();

            if (NameCheckBox.IsChecked)
            {
                if (NamePicker.SelectedIndex != -1)
                    student.Name = NamePicker.SelectedItem.ToString();
                else
                    student.Name = "";
            }
            if (FacultyCheckBox.IsChecked)
            {
                if (FacultyPicker.SelectedIndex != -1)
                    student.Faculty = FacultyPicker.SelectedItem.ToString();
                else
                    student.Faculty = "";
            }
            if (BuildNumCheckBox.IsChecked)
            {
                if (BuildNumPicker.SelectedIndex != -1)
                    student.BuildNum = BuildNumPicker.SelectedItem.ToString();
                else
                    student.BuildNum = "";
            }
            if (RoomNumCheckBox.IsChecked)
            {
                if (RoomNumPicker.SelectedIndex != -1)
                    student.RoomNum = RoomNumPicker.SelectedItem.ToString();
                else
                    student.RoomNum = "";
            }
            if (ResTimeCheckBox.IsChecked)
            {
                if (ResTimePicker.SelectedIndex != -1)
                    student.ResTime = ResTimePicker.SelectedItem.ToString();
                else
                    student.ResTime = "";
            }
            if (DocNumCheckBox.IsChecked)
            {
                if (DocNumPicker.SelectedIndex != -1)
                    student.DocNum = DocNumPicker.SelectedItem.ToString();
                else
                    student.DocNum = "";
            }
            return student;
        }
        private IStrategy GetSelectedAnalyzer()
        {
            IStrategy analyzer = null;

            try
            {
                if (SaxBtn.IsChecked)
                {
                    analyzer = new Sax();
                }
                if (DomBtn.IsChecked)
                {
                    analyzer = new Dom();
                }
                if (LinqBtn.IsChecked)
                {
                    analyzer = new Linq();
                }
            }
            catch
            {
                return null;
            }
            return analyzer;
        }
        private void PerformSearch(Student student, IStrategy analyzer)
        {
            Searcher search = new Searcher(student, analyzer, xmlFilePath);
            List<Student> results = search.SearchAlgorithm();

            if (results == null) return;

            foreach (Student st in results)
            {
                
                editor.Text += "ПІП студента/ки: " + st.Name + "\n";
                editor.Text += "Факультет: " + st.Faculty + "\n";
                editor.Text += "Гуртожиток: " + st.BuildNum + "\n";
                editor.Text += "Кімната: " + st.RoomNum + "\n";
                editor.Text += "Час проживання: " + st.ResTime + "\n";
                editor.Text += "Номер перепустки: " + st.DocNum + "\n";
                editor.Text += "\n";
            }
        }
        private void ClearFields(object sender, EventArgs e)
        {
            editor.Text = "";

            NameCheckBox.IsChecked = false;
            FacultyCheckBox.IsChecked = false;
            BuildNumCheckBox.IsChecked = false;
            RoomNumCheckBox.IsChecked = false;
            ResTimeCheckBox.IsChecked = false;
            DocNumCheckBox.IsChecked = false;

            NamePicker.SelectedItem = null;
            FacultyPicker.SelectedItem = null;
            BuildNumPicker.SelectedItem = null;
            RoomNumPicker.SelectedItem = null;
            ResTimePicker.SelectedItem = null;
            DocNumPicker.SelectedItem = null;
        }
        private async void OnExitBtnClicked(object sender, EventArgs e)
        {
            var result = await Application.Current.MainPage.DisplayAlert("Exit", "You want to exit the program?", "Yes", "No");
            if (result)
            {
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
            }
        }
        private async void OnTransformToHTMLBtnClicked(object sender, EventArgs e)
        {
            Debug.WriteLine("Transform to HTML button clicked");

            XslCompiledTransform xct = LoadXSLT();
            if (xct == null)
            {
                Debug.WriteLine("XSLT loading failed");
                return;
            }

            string xmlPath = xmlFilePath;
            if (xmlFilePath.Length == 0)
            {
                Debug.WriteLine("XML file path is empty");
                return;
            }
            string htmlPath = xmlFilePath.Substring(0, xmlFilePath.Length - 4) + ".html";

            XsltArgumentList xslArgs = await CreateXSLTArguments();
            Debug.WriteLine("XSLT arguments created");

            TransformXMLToHTML(xct, xslArgs, xmlPath, htmlPath);
            Debug.WriteLine("Transformation completed");

            await Application.Current.MainPage.DisplayAlert("Message", "File saved.", "Ok");
        }
        private async void OnOpenFileButton(object sender, EventArgs e)
        {
            var fileResult = await FilePicker.PickAsync();

            if (fileResult != null)
            {
                xmlFilePath = fileResult.FullPath;
            }
            editor.Text = "";
            GetAllAuthors();
        }

        private XslCompiledTransform LoadXSLT()
        {
            XslCompiledTransform xct = new XslCompiledTransform();
            try
            {
                xct.Load(Path.GetDirectoryName(xmlFilePath) + "/Transform.xslt");
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error loading XSLT: " + ex.Message);
                return null;
            }
            return xct;
        }
        private async Task<XsltArgumentList> CreateXSLTArguments()
        {
            XsltArgumentList xslArgs = new XsltArgumentList();

            // Add filter values from Pickers (null means "show all")
            AddParamIfNotNull(xslArgs, "name", NamePicker.SelectedItem?.ToString());
            AddParamIfNotNull(xslArgs, "faculty", FacultyPicker.SelectedItem?.ToString());
            AddParamIfNotNull(xslArgs, "buildnum", BuildNumPicker.SelectedItem?.ToString());
            AddParamIfNotNull(xslArgs, "roomnum", RoomNumPicker.SelectedItem?.ToString());
            AddParamIfNotNull(xslArgs, "restime", ResTimePicker.SelectedItem?.ToString());
            AddParamIfNotNull(xslArgs, "docnum", DocNumPicker.SelectedItem?.ToString());

            // Add visibility flags from CheckBoxes
            xslArgs.AddParam("showName", "", NameCheckBox.IsChecked.ToString().ToLower());
            xslArgs.AddParam("showFaculty", "", FacultyCheckBox.IsChecked.ToString().ToLower());
            xslArgs.AddParam("showBuildNum", "", BuildNumCheckBox.IsChecked.ToString().ToLower());
            xslArgs.AddParam("showRoomNum", "", RoomNumCheckBox.IsChecked.ToString().ToLower());
            xslArgs.AddParam("showResTime", "", ResTimeCheckBox.IsChecked.ToString().ToLower());
            xslArgs.AddParam("showDocNum", "", DocNumCheckBox.IsChecked.ToString().ToLower());

            return xslArgs;
        }
        private void AddParamIfNotNull(XsltArgumentList xslArgs, string name, string? value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                xslArgs.AddParam(name, "", value);
            }
        }
        private void TransformXMLToHTML(XslCompiledTransform xct, XsltArgumentList xslArgs, string xmlPath, string htmlPath)
        {
            using (XmlReader xr = XmlReader.Create(xmlPath))
            {
                using (XmlWriter xw = XmlWriter.Create(htmlPath))
                {
                    try
                    {
                        xct.Transform(xr, xslArgs, xw);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error during transformation: " + ex.Message);
                    }
                }
            }
        }
    }

}
