using BabySitter.DataModel;
using MapControl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI.Notifications;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
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
    public sealed partial class AddReminderPage : Page
    {
        private Pushpin _pushpin = new Pushpin();

        Geolocator geolocator;
        BasicGeoposition geo = new BasicGeoposition();

        public AddReminderPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
            e.Handled = true;
        }

        /// <summary>
        /// Вызывается перед отображением этой страницы во фрейме.
        /// </summary>
        /// <param name="e">Данные события, описывающие, каким образом была достигнута эта страница.
        /// Этот параметр обычно используется для настройки страницы.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            string str;
            description.Document.GetText(Windows.UI.Text.TextGetOptions.None, out str);
            DateTime dt = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy} {1:t}", date.Date.Date, time.Time));
            WorkDB db = new WorkDB();

            if (stype.SelectedIndex == 0)
            {
                Vaccine vac = new Vaccine();
                vac.cabinet = cabinet_v.Text;
                vac.phone = telephone_v.Text;
                vac.vaccine = vaccin.Text;
                vac.fio = fio_v.Text;

                db.InsertReminderVaccine(vac, dt, str);
            }
            if (stype.SelectedIndex == 1)
            {
                Doctor doct = new Doctor();
                doct.cabinet = cabinet_d.Text;
                doct.doctor = doc.Text;
                doct.fio = fio_d.Text;
                doct.phone = telephone_d.Text;

                db.InsertReminderDoctor(doct, dt, str);
            }
            if (stype.SelectedIndex == 2)
            {
                DataModel.Action act = new DataModel.Action();
                act.place = place.Text;


                act.Latitude = geo.Latitude;
                act.Longitude = geo.Longitude;

                db.InsertReminderAction(act, dt, str);
            }

            // Set up the notification text.
            var toastXml = Windows.UI.Notifications.ToastNotificationManager.GetTemplateContent(Windows.UI.Notifications.ToastTemplateType.ToastText02);
            var strings = toastXml.GetElementsByTagName("text");
            strings[0].AppendChild(toastXml.CreateTextNode((stype.SelectedIndex == 0 ? "Прививка" : stype.SelectedIndex == 1 ? "Прием врача" : "Мероприятие")));
            strings[1].AppendChild(toastXml.CreateTextNode("Время: " + dt.ToString()));

            // Create the toast notification object.
            try
            {
                var toast = new Windows.UI.Notifications.ScheduledToastNotification(toastXml, dt);
                ToastNotifier not = ToastNotificationManager.CreateToastNotifier();
                var v = not.GetScheduledToastNotifications();
                not.AddToSchedule(toast);
            }
            catch (Exception ex)
            {

            }
            var dlg = new MessageDialog("Данные успешно сохранены !");
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(MainPage));
        }


        private void stype_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if ((sender as ComboBox).SelectedIndex == 1)
            {
                doctor.Visibility = Windows.UI.Xaml.Visibility.Visible;
                vaccine.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                action.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            if ((sender as ComboBox).SelectedIndex == 2)
            {
                doctor.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                vaccine.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                action.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
            if ((sender as ComboBox).SelectedIndex == 0)
            {
                doctor.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                vaccine.Visibility = Windows.UI.Xaml.Visibility.Visible;
                action.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }

        }

        private async void map_Loaded(object sender, RoutedEventArgs e)
        {
            action.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
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
            else {
                var dlg = new MessageDialog("Службы геолокации отключены !");
                dlg.ShowAsync();
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

        private void map_MapTapped(Windows.UI.Xaml.Controls.Maps.MapControl sender, MapInputEventArgs args)
        {
            coord.Text = "Изменено на " + args.Location.Position.Latitude + " " + args.Location.Position.Longitude;
            geo.Latitude = args.Location.Position.Latitude;
            geo.Longitude = args.Location.Position.Longitude;
        }
    }
}
