using BabySitter.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента пустой страницы см. по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace BabySitter
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class ReminderPage : Page
    {
        BasicGeoposition geo = new BasicGeoposition();

        public ReminderPage()
        {
            this.InitializeComponent();

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
}

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
            e.Handled = true;
        }

        Geolocator geolocator;  

        UIElementCollection elemAdd;
        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            elemAdd = (sender as Grid).Children;
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
                 if (c.GetType() == typeof(ComboBox))
                 {
                     (c as ComboBox).SelectedIndex = param.stype == "Прививка" ? 0 : param.stype == "Прием врача" ? 1 : 2;
                
                     if ((c as ComboBox).SelectedIndex == 0)
                     {
                       InitVaccine(); 
                     }
                     if ((c as ComboBox).SelectedIndex == 1)
                     {
                         InitDoctor();
                     }
                     if ((c as ComboBox).SelectedIndex == 2)
                     {
                         InitAction();
                     }
                 }
             }
        }


        private async void InitVaccine()
        {

            WorkDB db = new WorkDB();
            var res = await db.SelectVaccine(param.type);

            foreach (var cc in elemEdit)
            {
                if (cc.GetType() == typeof(Grid))
                {
                    if ((cc as Grid).Name == "vaccine")
                    foreach (var c in (cc as Grid).Children)
                    {
                        if (c.GetType() == typeof(TextBox))
                        {
                            if ((c as TextBox).Name == "cabinet_v")
                            {
                                (c as TextBox).Text = res.cabinet;
                            }
                            if ((c as TextBox).Name == "telephone_v")
                            {
                                (c as TextBox).Text = res.phone;
                            }
                            if ((c as TextBox).Name == "vaccin")
                            {
                                (c as TextBox).Text = res.vaccine;
                            }
                            if ((c as TextBox).Name == "fio_v")
                            {
                                (c as TextBox).Text = res.fio;
                            }
                        }
                    }
                }
            }
        }

        private async void InitDoctor()
        {
            WorkDB db = new WorkDB();
            var res = await db.SelectDoctor(param.type);

            foreach (var cc in elemEdit)
            {
                if (cc.GetType() == typeof(Grid))
                {
                    if ((cc as Grid).Name == "doctor")
                        foreach (var c in (cc as Grid).Children)
                        {
                            if (c.GetType() == typeof(TextBox))
                            {
                                if ((c as TextBox).Name == "cabinet_d")
                                {
                                    (c as TextBox).Text = res.cabinet;
                                }
                                if ((c as TextBox).Name == "telephone_d")
                                {
                                    (c as TextBox).Text = res.phone;
                                }
                                if ((c as TextBox).Name == "doc")
                                {
                                    (c as TextBox).Text = res.doctor;
                                }
                                if ((c as TextBox).Name == "fio_d")
                                {
                                    (c as TextBox).Text = res.fio;
                                }
                            }
                        }
                }
            }
        }

        private async void InitAction()
        {
            WorkDB db = new WorkDB();
            var res = await db.SelectAction(param.type);

            foreach (var cc in elemEdit)
            {
                if (cc.GetType() == typeof(Grid))
                {
                    if ((cc as Grid).Name == "action")
                        foreach (var c in (cc as Grid).Children)
                        {
                            if (c.GetType() == typeof(TextBox))
                            {
                               
                                if ((c as TextBox).Name == "place")
                                {
                                    (c as TextBox).Text = res.place;
                                }

                            }
                            if (c.GetType() == typeof(Windows.UI.Xaml.Controls.Maps.MapControl))
                            {
                                if (res.Latitude != null && res.Longitude != null)
                                {
                                    var map = c as Windows.UI.Xaml.Controls.Maps.MapControl;
                                    map.MapServiceToken = "qT7BdubG1zzkjb0hgDl58Q";
                                    geolocator = new Geolocator();
                                    geolocator.DesiredAccuracyInMeters = 50;

                                    try
                                    {
                                        BasicGeoposition geo = new BasicGeoposition();
                                        geo.Latitude = res.Latitude;
                                        geo.Longitude = res.Longitude;
                                        Geopoint p = new Geopoint(geo);
                                        await map.TrySetViewAsync(p, 14D);
                                    }
                                    catch (UnauthorizedAccessException)
                                    {
                                        var dlg = new MessageDialog("Службы геолокации отключены !");
                                        dlg.ShowAsync();
                                    }
                                }
                            }
                        }
                }
            }
        }

        private void stype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 1)
            {
                foreach (var c in elemEdit)
                {
                    if (c.GetType() == typeof(Grid))
                    {
                        if ((c as Grid).Name == "doctor")
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Visible;
                        }
                        else
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }
       
                }
            }
            if ((sender as ComboBox).SelectedIndex == 0)
            {
                foreach (var c in elemEdit)
                {
                    if (c.GetType() == typeof(Grid))
                    {
                        if ((c as Grid).Name == "vaccine")
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Visible;
                        }
                        else
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }

                }
            }
            if ((sender as ComboBox).SelectedIndex == 2)
            {
                foreach (var c in elemEdit)
                {
                    if (c.GetType() == typeof(Grid))
                    {
                        if ((c as Grid).Name == "action")
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Visible;
                        }
                        else
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }

                }
            }
           
        }

        Reminder param;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            param = e.Parameter as Reminder;
            base.OnNavigatedTo(e);
        }

        private void stype_SelectionChanged1(object sender, SelectionChangedEventArgs e)
        {
            if ((sender as ComboBox).SelectedIndex == 1)
            {
                foreach (var c in elemAdd)
                {
                    if (c.GetType() == typeof(Grid))
                    {
                        if ((c as Grid).Name == "doctor")
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Visible;
                        }
                        else
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }

                }
            }
            if ((sender as ComboBox).SelectedIndex == 0)
            {
                foreach (var c in elemAdd)
                {
                    if (c.GetType() == typeof(Grid))
                    {
                        if ((c as Grid).Name == "vaccine")
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Visible;
                        }
                        else
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }

                }
            }
            if ((sender as ComboBox).SelectedIndex == 2)
            {
                foreach (var c in elemAdd)
                {
                    if (c.GetType() == typeof(Grid))
                    {
                        if ((c as Grid).Name == "action")
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Visible;
                        }
                        else
                        {
                            (c as Grid).Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                        }
                    }

                }
            }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
             WorkDB db = new WorkDB();
             Vaccine v = new Vaccine();
             Doctor d = new Doctor();
             BabySitter.DataModel.Action a = new DataModel.Action();
             int str = -1;
            DateTime dt = new DateTime();
            string s = "";

            foreach (var cc in elemAdd)
            {
                if (cc.GetType() == typeof(ComboBox))
                {
                    str = (cc as ComboBox).SelectedIndex;
                }
                if (cc.GetType() == typeof(DatePicker))
                {
                    dt = (cc as DatePicker).Date.Date;
                }
                if (cc.GetType() == typeof(RichEditBox))
                {
                    (cc as RichEditBox).Document.GetText(Windows.UI.Text.TextGetOptions.None, out s);
                }
                if (cc.GetType() == typeof(Grid))
                {
                    if ((cc as Grid).Name == "vaccine")
                    {
                        foreach (var c in (cc as Grid).Children)
                        {
                            if (c.GetType() == typeof(TextBox))
                            {
                                if ((c as TextBox).Name == "cabinet_v")
                                {
                                   v.cabinet = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "telephone_v")
                                {
                                    v.phone = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "vaccin")
                                {
                                    v.vaccine = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "fio_v")
                                {
                                    v.fio = (c as TextBox).Text;
                                }
                            }
                        }
                    }

                    if ((cc as Grid).Name == "doctor")
                    {
                        foreach (var c in (cc as Grid).Children)
                        {
                            if (c.GetType() == typeof(TextBox))
                            {
                                if ((c as TextBox).Name == "cabinet_d")
                                {
                                    d.cabinet = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "telephone_d")
                                {
                                    d.phone = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "doc")
                                {
                                    d.doctor = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "fio_d")
                                {
                                    v.fio = (c as TextBox).Text;
                                }
                            }
                        }
                    }

                    if ((cc as Grid).Name == "action")
                    {
                        foreach (var c in (cc as Grid).Children)
                        {
                            if (c.GetType() == typeof(TextBox))
                            {
                                if ((c as TextBox).Name == "place")
                                {
                                   a.place = (c as TextBox).Text;
                                }
                            }
                        }
                    }
                }
            }

            if (str == 0)
            {
                db.InsertReminderVaccine(v, dt, s);
                var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
                dlg.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
            if (str == 1)
            {
                db.InsertReminderDoctor(d, dt, s);
                var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
                dlg.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
            if (str == 2)
            {
                db.InsertReminderAction(a, dt, s);
                var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
                dlg.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void delToast()
        {
            // Set up the notification text.
            var toastXml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText02);
            var strings = toastXml.GetElementsByTagName("text");
            strings[0].AppendChild(toastXml.CreateTextNode(param.stype));
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
            strings[0].AppendChild(toastXml.CreateTextNode(param.stype));
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
            Vaccine v = new Vaccine();
            Doctor d = new Doctor();
            BabySitter.DataModel.Action a = new DataModel.Action();
            int str = -1;
            DateTime dt = new DateTime();
            DateTime t = new DateTime();
            string s = "";

            foreach (var cc in elemEdit)
            {
                if (cc.GetType() == typeof(ComboBox))
                {
                    str = (cc as ComboBox).SelectedIndex;
                }
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
                }
                if (cc.GetType() == typeof(Grid))
                {
                    if ((cc as Grid).Name == "vaccine")
                    {
                        foreach (var c in (cc as Grid).Children)
                        {
                            if (c.GetType() == typeof(TextBox))
                            {
                                if ((c as TextBox).Name == "cabinet_v")
                                {
                                    v.cabinet = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "telephone_v")
                                {
                                    v.phone = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "vaccin")
                                {
                                    v.vaccine = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "fio_v")
                                {
                                    v.fio = (c as TextBox).Text;
                                }
                            }
                        }
                    }

                    if ((cc as Grid).Name == "doctor")
                    {
                        foreach (var c in (cc as Grid).Children)
                        {
                            if (c.GetType() == typeof(TextBox))
                            {
                                if ((c as TextBox).Name == "cabinet_d")
                                {
                                    d.cabinet = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "telephone_d")
                                {
                                    d.phone = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "doc")
                                {
                                    d.doctor = (c as TextBox).Text;
                                }
                                if ((c as TextBox).Name == "fio_d")
                                {
                                    d.fio = (c as TextBox).Text;
                                }
                            }
                        }
                    }

                    if ((cc as Grid).Name == "action")
                    {
                        foreach (var c in (cc as Grid).Children)
                        {
                            if (c.GetType() == typeof(TextBox))
                            {
                                if ((c as TextBox).Name == "place")
                                {
                                    a.place = (c as TextBox).Text;
                                }
                            }
                        }
                    }
                }
            }
           if( param.stype == "Прививка") 
            db.DeleteReminderV(param.type) ;
           if (param.stype == "Мероприятие")
               db.DeleteReminderA(param.type);
           if (param.stype == "Прием врача")
               db.DeleteReminderD(param.type);

            if (str == 0)
            {
             //   db.DeleteReminderV(param.type);
                db.UpdateReminderV(v, dt, s, param);
                param.stype = "Прививка";
                createToast();
                var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
                dlg.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
            if (str == 1)
            {
              //  db.DeleteReminderD(param.type);
                db.UpdateReminderD(d, dt, s, param);
                param.stype = "Прием врача";
                createToast();
                var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
                dlg.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
            if (str == 2)
            {
             //   db.DeleteReminderA(param.type);
                if (geo.Latitude != 0.0 && geo.Longitude != 0.0)
                {
                    a.Latitude = geo.Latitude;
                    a.Longitude = geo.Longitude;
                }
                db.UpdateReminderA(a, dt, s, param);
                param.stype = "Мероприятие";
                createToast();
                var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
                dlg.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            db.DeleteReminder(param);
            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно удалены !");
            delToast();
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private async void map_Loaded(object sender, RoutedEventArgs e)
        {
          //  action.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            var map = sender as Windows.UI.Xaml.Controls.Maps.MapControl;
            map.MapServiceToken = "qT7BdubG1zzkjb0hgDl58Q";
            geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            if (geolocator.LocationStatus != PositionStatus.Disabled && geolocator.LocationStatus != PositionStatus.NotAvailable)
            {
                try
                {
                    Geoposition geoposition = await geolocator.GetGeopositionAsync(
                        maximumAge: TimeSpan.FromMinutes(5),
                        timeout: TimeSpan.FromSeconds(10));
                    await map.TrySetViewAsync(geoposition.Coordinate.Point, 14D);
                }
                catch (UnauthorizedAccessException)
                {
                    var dlg = new MessageDialog("Службы геолокации отключены !");
                    dlg.ShowAsync();
                }
            }
            else
            {
                var dlg = new MessageDialog("Службы геолокации отключены !");
                dlg.ShowAsync();
            }
        }

        private void map_MapTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, Windows.UI.Xaml.Controls.Maps.MapInputEventArgs args)
        {
            foreach (var cc in elemEdit)
            {
                if (cc.GetType() == typeof(Grid))
                foreach (var c in (cc as Grid).Children)
                {
                    if (c.GetType() == typeof(TextBlock))
                    {
                        if ((c as TextBlock).Name == "coord")
                        {
                            (c as TextBlock).Text = "Изменено на " + args.Location.Position.Latitude + " " + args.Location.Position.Longitude;
                        }
                    }
                }
            }
            geo.Latitude = args.Location.Position.Latitude;
            geo.Longitude = args.Location.Position.Longitude;
        }
    }
}
