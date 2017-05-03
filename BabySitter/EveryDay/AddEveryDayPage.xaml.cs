using BabySitter.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class AddEveryDayPage : Page
    {
        public AddEveryDayPage()
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

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            string mess = "";
            EveryDay ed = new EveryDay();
            ed.abdominalDistention = (Int16) abdominalDistention.Value;
            ed.crying = (Int16)crying.Value;        
            ed.date = date.Date.Date;
            string str;
            description.Document.GetText(Windows.UI.Text.TextGetOptions.None, out str);
            ed.description = str;
            ed.diareya = (Int16)diareya.Value;
            ed.gas = (Int16)gaz.Value;
            if (height.Text != "")
                ed.heiht = Convert.ToDouble(height.Text);
            else
            {
                new Windows.UI.Popups.MessageDialog("Поле рост должно быть заполнено.").ShowAsync();
                return;
            }
            ed.locking = (Int16)locking.Value;
            ed.playing = (Int16)playing.Value;
            ed.rashes = (Int16)rashes.Value;
            if (temperature.Text != "")
            ed.temperature = Convert.ToDouble(temperature.Text);
            else
            {
                new Windows.UI.Popups.MessageDialog("Поле температура должно быть заполнено.").ShowAsync();
                return;
            }
            if (weight.Text != "")
            ed.weight = Convert.ToDouble(weight.Text);
            else
            {
                new Windows.UI.Popups.MessageDialog("Поле вес должно быть заполнено.").ShowAsync();
                return;
            }
            ed.id_child = CurrentChild.id;

            WorkDB db = new WorkDB();
            try
            {
                db.InsertEveryDay(ed);
            }
            catch(Exception ex)
            {
               mess = ("Возникла ошибка при сохранении. Повторите попытку!");
                
            }
            if (mess == "")
            {
                var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
                dlg.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
            else
            {
                var dlg = new Windows.UI.Popups.MessageDialog(str);
                dlg.ShowAsync();
                this.Frame.Navigate(typeof(MainPage));
            }
        }

        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
