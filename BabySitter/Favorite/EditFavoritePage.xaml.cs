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
    public sealed partial class EditFavoritePage : Page
    {
        public EditFavoritePage()
        {
            this.InitializeComponent();

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            this.Frame.Navigate(typeof(FavoritePage));
            e.Handled = true;
        }

        Favorite param;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            param = e.Parameter as Favorite;
            base.OnNavigatedTo(e);
            date.Date = param.dateTime.Date.Date;
            description.Document.SetText(Windows.UI.Text.TextSetOptions.None, param.name);
        }

        private void Button_Click_prev(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FavoritePage));
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            param.dateTime = date.Date.Date;
            String s;
            description.Document.GetText(Windows.UI.Text.TextGetOptions.None, out s);
            param.name = s;
            db.UpdateFavorite(param);
            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно Сохранениы !");
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(FavoritePage));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            db.DeleteFavorite(param);
            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно удалены !");
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(FavoritePage));
        }
    }
}
