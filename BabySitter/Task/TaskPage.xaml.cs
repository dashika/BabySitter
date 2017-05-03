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
    public sealed partial class TaskPage : Page
    {
        public TaskPage()
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


        private void delToast()
        {
            // Set up the notification text.
            var toastXml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText02);
            var strings = toastXml.GetElementsByTagName("text");
            strings[0].AppendChild(toastXml.CreateTextNode(param.name));
            strings[1].AppendChild(toastXml.CreateTextNode("Время: " + param.dateTime.ToString()));

            try
            {
                var toast = new Windows.UI.Notifications.ScheduledToastNotification(toastXml, param.dateTime);
                ToastNotifier not = ToastNotificationManager.CreateToastNotifier();
                not.RemoveFromSchedule(toast);
            }
            catch (Exception ex)
            {

            }
        }

        private void createToast()
        {
            var toastXml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText02);
            var strings = toastXml.GetElementsByTagName("text");
            strings[0].AppendChild(toastXml.CreateTextNode(param.name));
            strings[1].AppendChild(toastXml.CreateTextNode("Время: " + param.dateTime.ToString()));

            try
            {
                var toast = new Windows.UI.Notifications.ScheduledToastNotification(toastXml, param.dateTime);
                ToastNotifier not = ToastNotificationManager.CreateToastNotifier();
                not.AddToSchedule(toast);
            }
            catch (Exception ex)
            {

            }
        }


        private void Save_Click(object sender, RoutedEventArgs e)
        {
            delToast();
            WorkDB db = new WorkDB();
            DateTime dt = new DateTime();
            string s = "";

            foreach (var cc in elemEdit)
            {
                if (cc.GetType() == typeof(DatePicker))
                {
                    dt = (cc as DatePicker).Date.Date;
                }
                if (cc.GetType() == typeof(TimePicker))
                {
                    dt = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy} {1:t}", dt.Date.Date, (cc as TimePicker).Time));
                }
                if (cc.GetType() == typeof(RichEditBox))
                {
                    (cc as RichEditBox).Document.GetText(Windows.UI.Text.TextGetOptions.None, out s);
                    param.other = s;
                }
                if (cc.GetType() == typeof(TextBox))
                {
                    param.name = (cc as TextBox).Text;
                }
            }
            param.dateTime = dt;
            param.id_child = CurrentChild.id;
            db.UpdateTask(param);
            createToast();
            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(MainPage));

        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            db.DeleteEveryTask(param);
            delToast();
            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно удалены !");
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(MainPage));
        }

        UIElementCollection elemEdit;

        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            elemEdit = (sender as Grid).Children;
            foreach (var c in elemEdit)
            {
                if (c.GetType() == typeof(RichEditBox))
                {
                    (c as RichEditBox).Document.SetText(Windows.UI.Text.TextSetOptions.None, param.other.ToString());
                }
                if (c.GetType() == typeof(DatePicker))
                {
                    (c as DatePicker).Date = param.dateTime;
                }
                if (c.GetType() == typeof(TimePicker))
                {
                    (c as TimePicker).Time = param.dateTime.TimeOfDay;
                }
               if (c.GetType() == typeof(TextBox))
               {
                   (c as TextBox).Text = param.name;
               }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
