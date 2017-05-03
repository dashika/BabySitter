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
    public sealed partial class AddContactPage : Page
    {
        public AddContactPage()
        {
            this.InitializeComponent();
            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            this.Frame.Navigate(typeof(ContactPage));
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
            WorkDB db = new WorkDB();
            Contact c = new Contact();
            c.address = address.Text;
            c.name = name.Text;
            c.office = whom.Text;
            c.phone = phone.Text;
            c.id_child = CurrentChild.id;

            db.InsertContact(c);
            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно сохранены !");
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(ContactPage));
        }

        private void Button_Click_prev(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ContactPage));
        }
    }
}
