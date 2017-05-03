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

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace BabySitter.Child
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditChildPage : Page
    {
        public EditChildPage()
        {
            this.InitializeComponent();
            try
            {
                WorkDB db = new WorkDB();
                Children child = new Children();
                child = db.SelectChildName(CurrentChild.id);
                name.Text = child.name;
                birthday.Date = child.birthday;
            }
            catch { }
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Change_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Children child = new Children();
                child.name = name.Text.ToString();
                child.birthday = birthday.Date.Date;
                child.id = CurrentChild.id;
                WorkDB db = new WorkDB();
                db.UpdateChildren(child);
                this.Frame.Navigate(typeof(MainPage));
            }
            catch { }
        }

        private void Button_Click_prev(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }
    }
}
