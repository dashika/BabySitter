using BabySitter.DataModel;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BabySitter
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddChildPage : Page
    {
        TimeSpan tick;

        public AddChildPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
            e.Handled = true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            Children c = new Children();
            c.name = name.Text;
            c.birthday = birthday.Date.Date;

           CurrentChild.id = await db.InsertChildren(c, 0);

           CommandInvokedHandleкOk();

            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
            await dlg.ShowAsync();
          //  this.Frame.Navigate(typeof(MainPage));
        }

        private async void CommandInvokedHandleкOk()
        {       
               addvaccineD( "Прививка от гепатита В (1)", 1.0);
   
                addvaccineD("Прививка от туберкулеза", 7.0);

                addvaccineM("Прививка от гепатита В (2)", 1);

                addvaccineM( "Прививка от дифтерии (1)",3);

                addvaccineM( "Прививка от коклюша (1)",3);

                addvaccineM("Прививка от столбняка(1)",3);

                addvaccineM("Прививка от полиомелита(1)",3);

                addvaccineM( "Прививка от гемофильной инфекции (1)",3);

                addvaccineM( "Прививка от дифтерии (2)",4);

                addvaccineM( "Прививка от коклюша (2)",4);

                addvaccineM( "Прививка от столбняка(2)",4);

                addvaccineM( "Прививка от полиомелита(2)",4);

                addvaccineM( "Прививка от гемофильной инфекции (2)",4);

                addvaccineM( "Прививка от дифтерии (3)", 6);

                addvaccineM( "Прививка от коклюша (3)",6);

                addvaccineM("Прививка от столбняка(3)",6);

                addvaccineM( "Прививка от полиомелита(3)",6);

                addvaccineM("Прививка от гемофильной инфекции (3)",6);

                addvaccineM("Прививка от гепатита В (3)",6);

                addvaccineY("Прививка от кори",1);

                addvaccineY( "Прививка от краснухи", 1);

                addvaccineY( "Прививка от эпидемического паротита",1);

                addvaccineY("Ревакцинация от дифтерии", 1,6);

                addvaccineY("Ревакцинация от коклюша",1,6);

                addvaccineY( "Ревакцинация от столбняка", 1,6);

                addvaccineY("Ревакцинация от полиомелита",1,6);

                addvaccineY( "Ревакцинация от гемофильной инфекции",1,6);

                addvaccineY("Ревакцинация от полиомелита(2)",1,8);

                addvaccineY( "Ревакцинация от кори",6);

               addvaccineY( "Ревакцинация от краснухи", 6);

                addvaccineY( "Ревакцинация от эпидемического паротита", 6);

                addvaccineY( "Ревакцинация от дифтерии(2)", 6);

                addvaccineY("Ревакцинация от столбняка(2)", 6);

                addvaccineY("Ревакцинация от туберкулеза",7);

                addvaccineY("Ревакцинация от дифтерии(3)",14);

                addvaccineY( "Ревакцинация от столбняка(3)",14);

                addvaccineY("Ревакцинация от полимелита(3)",14);

                addvaccineY("Ревакцинация против туберкулеза бациллой Кальметта–Герена",14);


                this.Frame.Navigate(typeof(MainPage));
            
        }

        async void addvaccineM(String str, int dt)
        {
            WorkDB db = new WorkDB();
            Vaccine v = new Vaccine();
            v.cabinet = "";
            v.fio = "";
            v.phone = "";
            v.vaccine = str;
            DateTime date = birthday.Date.AddMonths(dt).DateTime;
            db.InsertReminderVaccine(v, date, "Стандартное");
        }

       async  void addvaccineY(String str, int dt, int m = 0)
        {
            WorkDB db = new WorkDB();
            Vaccine v = new Vaccine();
            v.cabinet = "";
            v.fio = "";
            v.phone = "";
            v.vaccine = str;
            DateTime date = birthday.Date.AddYears(dt).AddMonths(m).DateTime;
            db.InsertReminderVaccine(v, date, "Стандартное");
        }

        async void addvaccineD(String str, double dt)
        {
            WorkDB db = new WorkDB();
            Vaccine v = new Vaccine();
            v.cabinet = "";
            v.fio = "";
            v.phone = "";
            v.vaccine = str;
            DateTime date = birthday.Date.AddDays(dt).DateTime;
            db.InsertReminderVaccine(v, date, "Стандартное");
        }

        private void Button_Click_prev(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
