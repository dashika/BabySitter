using BabySitter.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public sealed partial class ContactPage : Page
    {
  private ObservableCollection<DataModel.Contact> mMyList = new ObservableCollection<DataModel.Contact>();
        public ObservableCollection<DataModel.Contact> MyList
        {
            get { return mMyList; }
        }

        public ContactPage()
        {
            this.InitializeComponent();
            DB();

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

        async void DB()
        {
            WorkDB db = new WorkDB();

            try
            {
                var list = await db.SelectContact(CurrentChild.id);
                foreach (var c in list)
                {
                        mMyList.Add(c);
                }
               
            }
            catch (System.NullReferenceException e)
            { }
            catch (AggregateException e)
            {}
        }


        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            var c = (sender as ListView).SelectedValue;
            this.Frame.Navigate(typeof(EditContactPage), c);
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AddContactPage));
        }

        private void Button_Click_prev(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
