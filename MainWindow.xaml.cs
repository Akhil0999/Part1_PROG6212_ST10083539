using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TimeManagementClassLibrary;

namespace Part1_PROG6212_ST10083539
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Module> moduleLst = new List<Module>();
        List<double> SelfStudyHoursLst = new List<double>();
        List<Display1> display1Lst = new List<Display1>();
        List<StudySesh> studySeshLst = new List<StudySesh>();
        List<Display2> display2Lst = new List<Display2>();
        public MainWindow()
        {
            InitializeComponent();
           
        } //LOGIC STARTS BELOW
       

        //Add Module button
        private void btnAddModule_Click(object sender, RoutedEventArgs e)
        {
            //Takes module information and adds it to a list for displaying
            string moduleCode = Convert.ToString(edtMCode.Text);
            string moduleName = Convert.ToString(edtMName.Text);
            int moduleCredits = Convert.ToInt32(edtMCredits.Text);
            double moduleHoursRequired = Convert.ToDouble(edtMHoursPW.Text);

            MessageBoxResult confirmation = System.Windows.MessageBox.Show("Do you want to save this module?", "", MessageBoxButton.YesNo);
            if (confirmation == MessageBoxResult.Yes)
            {
               Module newModule = new Module(moduleCode, moduleName, moduleCredits, moduleHoursRequired);
               moduleLst.Add(newModule);

               MessageBox.Show("You added a module to the list!");
            } else { MessageBox.Show("Cancelled!"); }          

        }

        //Display Self Study Hours for each module Button
        private void btnDisplaySelfStudyHours_Click(object sender, RoutedEventArgs e)
        {
            Module.SSHPW(moduleLst, display1Lst, Convert.ToInt32(edtTotalWeeksInSemes.Text));

            foreach (Module module in moduleLst)
            {
                listBox.ItemsSource = display1Lst;
            }
        }

        //Add Study Session Button
        private void btnRecordStudySesh_Click(object sender, RoutedEventArgs e)
        {
            string moduleNameToFind = Convert.ToString(edtSearchModule.Text);
            DateTime studyDate = Convert.ToDateTime(StudyDate.SelectedDate);
            double hoursStudied = Convert.ToDouble(edtHoursStudied.Text);

            MessageBoxResult confirmation = System.Windows.MessageBox.Show("Do you want to save study session?", "", MessageBoxButton.YesNo);
            if (confirmation == MessageBoxResult.Yes)
            {
                StudySesh newStudySesh = new StudySesh(moduleNameToFind, studyDate, hoursStudied);
                studySeshLst.Add(newStudySesh);
                MessageBox.Show("You added a study session !");
            }
            else { MessageBox.Show("Cancelled!"); }

            // MessageBox.Show(Convert.ToString(studyDate));
        }

        //Display remaining Self Study Hours for each module Button
        private void btnDisplayRemainingSelfStudyHours_Click(object sender, RoutedEventArgs e)
        {
            StudySesh.RemainingSSHPW(display1Lst, display2Lst, studySeshLst);
            foreach(Display2 display2 in display2Lst)
            {
                listBox.ItemsSource = display2Lst;
            }
        }
    }
    
    //classes: MODULE / STUDY SESH / DISPLAY 1 / DISPLAY 2
    class StudySesh
    {
        string module;
        DateTime date;
        double hoursSS;

        public StudySesh(string module, DateTime date, double hoursSS)
        {
            this.module = module;
            this.date = date;
            this.hoursSS = hoursSS;
        }

        public string Module { get => module; set => module = value; }
        public DateTime Date { get => date; set => date = value; }
        public double HoursSS { get => hoursSS; set => hoursSS = value; }

        //Method will calculate remaining self study hours: moduleLst & display1Lst has the names of each module (for matching)/ display1Lst and studySeshLst has the hours for calculation (display1.SSHPW-studySesh.hoursSS)/ display2Lst will be populated with the calculated values and module names
        public static void RemainingSSHPW(List<Display1> display1Lst, List<Display2> display2Lst, List<StudySesh> studySeshLst)
        {
            double remSSHPW;
            foreach (StudySesh studySesh in studySeshLst)
            {
                remSSHPW = 6;

                Display2 obj = new Display2(remSSHPW, studySesh.module);
                display2Lst.Add(obj);
            }

        }
    }

    class Module
    {
        string code;
        string name;
        int credits;
        double hoursPerWeek;

        public Module(string code, string name, int credits, double hoursPerWeek)
        {
            this.Code = code;
            this.Name = name;
            this.Credits = credits;
            this.HoursPerWeek = hoursPerWeek;
        }

        public string Code { get => code; set => code = value; }
        public string Name { get => name; set => name = value; }
        public int Credits { get => credits; set => credits = value; }
        public double HoursPerWeek { get => hoursPerWeek; set => hoursPerWeek = value; }

        public override string ToString()
        {
            return this.Name + " - " + this.code + " - " + this.credits + " credits - " + this.hoursPerWeek + " hours(weekly)";
        }

        //Self Study Hours per week(SSHPW) CALCULATION : METHOD will populate a list of objects for display 1
        public static void SSHPW(List<Module> moduleLst, List<Display1> display1Lst, int totalWeeks)
        {
            double SSHPW;
            foreach (Module module in moduleLst)
            {
                SSHPW = ((module.credits * 10) / totalWeeks) - module.hoursPerWeek;

                Display1 obj = new Display1(SSHPW, module.name);
                display1Lst.Add(obj);
            }

        }
    }

    public class Display1
    {
        double SSHPW;
        string ModuleName;

        public Display1(double sSHPW, string moduleName)
        {
            SSHPW = sSHPW;
            ModuleName = moduleName;
        }

        public double SSHPW1 { get => SSHPW; set => SSHPW = value; }
        public string ModuleName1 { get => ModuleName; set => ModuleName = value; }

        public override string ToString()
        {
            return this.ModuleName + " : " + this.SSHPW + " Self Study Hrs(per week)";
        }
    }

    public class Display2
    {
        double remSSHPW;
        string ModuleName;

        public Display2(double remSSHPW, string moduleName)
        {
            this.remSSHPW = remSSHPW;
            ModuleName = moduleName;
        }

        public double RemSSHPW { get => remSSHPW; set => remSSHPW = value; }
        public string ModuleName1 { get => ModuleName; set => ModuleName = value; }



        public override string ToString()
        {
            return this.ModuleName + " : " + this.remSSHPW + " Hrs of self study remaining";
        }

    }  

}
