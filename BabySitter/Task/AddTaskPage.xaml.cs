using BabySitter.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkID=390556

namespace BabySitter
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddTaskPage : Page
    {
        public AddTaskPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
            e.Handled = true;
        }

        Task param;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            param = e.Parameter as Task;
            base.OnNavigatedTo(e);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void createToast(String name, DateTime dt)
        {
            var toastXml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText02);
            var strings = toastXml.GetElementsByTagName("text");
            strings[0].AppendChild(toastXml.CreateTextNode(name));
            strings[1].AppendChild(toastXml.CreateTextNode("Время: " + dt.ToString()));

            try
            {
                var toast = new Windows.UI.Notifications.ScheduledToastNotification(toastXml, dt);
                ToastNotifier not = ToastNotificationManager.CreateToastNotifier();
                not.AddToSchedule(toast);
            }
            catch (Exception ex)
            {

            }
        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();

            Task task = new Task();
            task.name = name.Text;
             DateTime dt = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy} {1:t}", date.Date.Date, time.Time));
             task.dateTime = dt;
             string str;
             description.Document.GetText(Windows.UI.Text.TextGetOptions.None, out str);
             task.other = str;
             task.id_child = CurrentChild.id;

             db.InsertTask(task);
             createToast(task.name, task.dateTime);
             var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
             dlg.ShowAsync();
             this.Frame.Navigate(typeof(MainPage));
        }
    }
}
