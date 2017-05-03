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
    public sealed partial class EditContactPage : Page
    {
        public EditContactPage()
        {
            this.InitializeComponent();

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            this.Frame.Navigate(typeof(ContactPage));
            e.Handled = true;
        }

        Contact param;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            param = e.Parameter as Contact;
            base.OnNavigatedTo(e);
            name.Text = param.name;
            phone.Text = param.phone;
            address.Text = param.address;
            whom.Text = param.office;
        }

        private void Button_Click_prev(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(ContactPage));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            param.name = name.Text;
            param.office = whom.Text;
            param.phone = phone.Text;
            param.address = address.Text;
            db.UpdateContact(param);
            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно Сохранениы !");
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(ContactPage));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            db.DeleteContact(param);
            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно удалены !");
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(ContactPage));
        }
    }
}
