using BabySitter.DataModel;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace BabySitter
{
    static class CurrentChild
    {
        public static int id { get; set; }
    }

    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        DateTime dt = DateTime.Now.Date.Date;

        private ObservableCollection<DataModel.Childs> mMyChild = new ObservableCollection<DataModel.Childs>();
        public ObservableCollection<DataModel.Childs> MyListChild
        {
            get { return mMyChild; }
        }

        private ObservableCollection<DataModel.EveryDay> mMyListEveryDay = new ObservableCollection<DataModel.EveryDay>();
        public ObservableCollection<DataModel.EveryDay> MyListEveryDay
        {
            get { return mMyListEveryDay; }
        }

        private ObservableCollection<DataModel.Task> mMyListTask = new ObservableCollection<DataModel.Task>();
        public ObservableCollection<DataModel.Task> MyListTask
        {
            get { return mMyListTask; }
        }

        private ObservableCollection<DataModel.Reminder> mMyListReminder = new ObservableCollection<DataModel.Reminder>();
        public ObservableCollection<DataModel.Reminder> MyListReminder
        {
            get { return mMyListReminder; }
        }



        public MainPage()
        {
            this.InitializeComponent();
            WorkDB db = new WorkDB();
            Child();
            Windows.UI.ViewManagement.ApplicationView.GetForCurrentView().SetDesiredBoundsMode(Windows.UI.ViewManagement.ApplicationViewBoundsMode.UseCoreWindow);
        }


        async void Child()
        {           
            try
            {
                WorkDB db = new WorkDB();

                var list_ = await db.SelectAllChildren();
                foreach (var c in list_)
                {
                    Childs ch = new Childs();
                    ch.id = c.id;
                    ch.name = c.name;
                    ch.birthday = String.Format("{0:dd.MM.yyyy}", c.birthday.Date);
                    mMyChild.Add(ch);
                }

                if (CurrentChild.id != 0)
                {
                    DB(dt);
                }
                if (CurrentChild.id != 0)
                {
                    childName.Text = db.SelectChildName(CurrentChild.id).name;
                }
            }
            catch
            { }
        }

        async void DB(DateTime dt)
        {
            try
            {
                WorkDB db = new WorkDB();
                var list = await db.SelectAllTask(CurrentChild.id);
                foreach (var c in list)
                {
                    Task task = new Task()
                    {
                        id = c.id,
                        dateTime = c.dateTime,
                        other = c.other,
                        name = c.name
                    };

                    if ((c.dateTime.Year == dt.Year) & (c.dateTime.Month == dt.Month) & (c.dateTime.Day == dt.Day))
                        mMyListTask.Add(task);
                }
                var list1 = await db.SelectAllReminder(CurrentChild.id);
                foreach (var c in list1)
                {
                    if ((c.dateTime.Year == dt.Year) & (c.dateTime.Month == dt.Month) & (c.dateTime.Day == dt.Day))
                        mMyListReminder.Add(c);
                }
                var list2 = await db.SelectAllEveryDay(dt, CurrentChild.id);
                foreach (var c in list2)
                {
                    mMyListEveryDay.Add(c);
                }
            }
            catch (System.NullReferenceException e)
            { }
            catch (AggregateException e)
            {

            }
        }


        /// <summary>
        /// everyday item click
        /// </summary>
        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var c = (sender as ListView).SelectedValue;
            this.Frame.Navigate(typeof(EveryDayPage), c);
        }

        /// <summary>
        /// reminder selected in list
        /// </summary>
        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var c = (sender as ListView).SelectedValue;
            this.Frame.Navigate(typeof(ReminderPage), c);
        }

        private void date_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            dt = (sender as DatePicker).Date.Date;
            mMyListEveryDay.Clear();
            MyListEveryDay.Clear();
            mMyListReminder.Clear();
            MyListReminder.Clear();
            mMyListTask.Clear();
            MyListTask.Clear();
            if (CurrentChild.id != 0)
                DB(dt);
        }

        private void AddRem_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentChild.id != 0)
            {
                this.Frame.Navigate(typeof(AddReminderPage));
            }
            else
            {
                new Windows.UI.Popups.MessageDialog("Выберите или добавьте нового ребенка в списке(-ок) !").ShowAsync();
            }
        }

        private void AddRem_Click_1(object sender, RoutedEventArgs e)
        {
            if (CurrentChild.id != 0)
                this.Frame.Navigate(typeof(AddTaskPage));
            else
            {
                new Windows.UI.Popups.MessageDialog("Выберите или добавьте нового ребенка в списке(-ок) !").ShowAsync();
            }
        }

        private void AddRem_Click_2(object sender, RoutedEventArgs e)
        {
            if (CurrentChild.id != 0)
                this.Frame.Navigate(typeof(AddEveryDayPage));
            else
            {
                new Windows.UI.Popups.MessageDialog("Выберите или добавьте нового ребенка в списке(-ок) !").ShowAsync();
            }
        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {
            mMyListEveryDay.Clear();
            MyListEveryDay.Clear();
            mMyListReminder.Clear();
            MyListReminder.Clear();
            mMyListTask.Clear();
            MyListTask.Clear();
            mMyChild.Clear();
            MyListChild.Clear();
            Child();
        }

        private void ListView_SelectionChanged_2(object sender, SelectionChangedEventArgs e)
        {
            var c = (sender as ListView).SelectedValue;
            this.Frame.Navigate(typeof(TaskPage), c);
        }

        private void Favorite_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentChild.id != 0)
                this.Frame.Navigate(typeof(FavoritePage));
            else
            {
                new Windows.UI.Popups.MessageDialog("Выберите или добавьте нового ребенка в списке(-ок) !").ShowAsync();
            }
        }

        private void Contact_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentChild.id != 0)
                this.Frame.Navigate(typeof(ContactPage));
            else
            {
                new Windows.UI.Popups.MessageDialog("Выберите или добавьте нового ребенка в списке(-ок) !").ShowAsync();
            }
        }

        private void ListView_SelectionChanged_3(object sender, SelectionChangedEventArgs e)
        {
            mMyListEveryDay.Clear();
            MyListEveryDay.Clear();
            mMyListReminder.Clear();
            MyListReminder.Clear();
            mMyListTask.Clear();
            MyListTask.Clear();

            var current = (sender as ListBox).SelectedValue as Childs;
            if (current != null)
            {
                CurrentChild.id = current.id;
                WorkDB db = new WorkDB();
                childName.Text = db.SelectChildName(CurrentChild.id).name;
            }
            DB(dt);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(LicensePage));
        }

        private void Del_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Windows.UI.Popups.MessageDialog("Удалить " + childName.Text.ToString() + "?");
            dlg.Commands.Add(new UICommand("Да",new UICommandInvokedHandler(this.CommandInvokedHandler)));
            dlg.Commands.Add(new UICommand("Нет", new UICommandInvokedHandler(this.CommandInvokedHandler)));
            dlg.ShowAsync();
        }

        private void CommandInvokedHandler(IUICommand command)
        {
            if(command.Label.Equals("Да"))
            {
                try
                {
                    WorkDB db = new WorkDB();
                    Children child = new Children();
                    child = db.SelectChildName(CurrentChild.id);
                    db.DeleteChildren(child);
                    mMyListEveryDay.Clear();
                    MyListEveryDay.Clear();
                    mMyListReminder.Clear();
                    MyListReminder.Clear();
                    mMyListTask.Clear();
                    MyListTask.Clear();
                    mMyChild.Clear();
                    MyListChild.Clear();
                    Child();
                }
                catch { }
            }
        }

        private void Upd_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Child.EditChildPage));
        }
    }
}
