using BabySitter.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// Шаблон элемента пустой страницы задокументирован по адресу http://go.microsoft.com/fwlink/?LinkId=234238

namespace BabySitter
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class EveryDayPage : Page
    {
        private ObservableCollection<NameValueItem> mMyListPlay = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListPlay
        {
            get { return mMyListPlay; }
        }
        private ObservableCollection<NameValueItem> mMyListCry = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListCry
        {
            get { return mMyListCry; }
        }

        private ObservableCollection<NameValueItem> mMyListWeight = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListWeight
        {
            get { return mMyListWeight; }
        }

        private ObservableCollection<NameValueItem> mMyListHeight = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListHeight
        {
            get { return mMyListHeight; }
        }

        private ObservableCollection<NameValueItem> mMyListTemperature = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListTemperature
        {
            get { return mMyListTemperature; }
        }

        private ObservableCollection<NameValueItem> mMyListRashes = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListRashes
        {
            get { return mMyListRashes; }
        }
        private ObservableCollection<NameValueItem> mMyListabdominalDistention = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListabdominalDistention
        {
            get { return mMyListabdominalDistention; }
        }
        private ObservableCollection<NameValueItem> mMyListGaz = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListGaz
        {
            get { return mMyListGaz; }
        }
        private ObservableCollection<NameValueItem> mMyListLock = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListLock
        {
            get { return mMyListLock; }
        }
        private ObservableCollection<NameValueItem> mMyListDiareya = new ObservableCollection<NameValueItem>();
        public ObservableCollection<NameValueItem> MyListDiareya
        {
            get { return mMyListDiareya; }
        }
        List<EveryDay> list = new List<EveryDay>();

        public EveryDayPage()
        {
            this.InitializeComponent();
            UpdateCharts();

            Windows.Phone.UI.Input.HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
            e.Handled = true;
        }

        private Random _random = new Random();

        private async void UpdateCharts()
        {
            WorkDB db = new WorkDB();
            list = await db.SelectEveryDay30(CurrentChild.id);
            mMyListWeight.Add(new NameValueItem { Name = "", Value = 0 });
            mMyListHeight.Add(new NameValueItem { Name = "", Value = 0 });
            mMyListTemperature.Add(new NameValueItem { Name = "", Value = 34 });

        }

        public class NameValueItem
        {
            public string Name { get; set; }
            public double Value { get; set; }
        }

        EveryDay param;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            param = e.Parameter as EveryDay;
            base.OnNavigatedTo(e);
        }

        UIElementCollection elemEdit ;
        private void grid_Loaded(object sender, RoutedEventArgs e)
        {
            elemEdit = (sender as Grid).Children;
            foreach(var c in elemEdit)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    if((c as TextBox).Name == "weight")
                    {
                        (c as TextBox).Text = param.weight.ToString();
                    }
                    if ((c as TextBox).Name == "height")
                    {
                        (c as TextBox).Text = param.heiht.ToString();
                    }
                    if ((c as TextBox).Name == "temperature")
                    {
                        (c as TextBox).Text = param.temperature.ToString();
                    }
                }
                if (c.GetType() == typeof(Slider))
                {
                    if ((c as Slider).Name == "rashes") (c as Slider).Value = param.rashes;
                    if ((c as Slider).Name == "abdominalDistention") (c as Slider).Value = param.abdominalDistention;
                    if ((c as Slider).Name == "play") (c as Slider).Value = param.playing;
                    if ((c as Slider).Name == "cry") (c as Slider).Value = param.crying;
                    if ((c as Slider).Name == "locking") (c as Slider).Value = param.locking;
                    if ((c as Slider).Name == "gas") (c as Slider).Value = param.gas;
                    if ((c as Slider).Name == "diareya") (c as Slider).Value = param.diareya;
                }
                if (c.GetType() == typeof(DatePicker))
                {
                    (c as DatePicker).Date = param.date;
                }
                if (c.GetType() == typeof(RichEditBox))
                {
                    (c as RichEditBox).Document.SetText(Windows.UI.Text.TextSetOptions.None, param.description);
                }
            }
        }

        UIElementCollection elemAdd;
        private void Grid_Loaded_1(object sender, RoutedEventArgs e)
        {
            elemAdd = (sender as Grid).Children;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var c in list)
            {
                mMyListWeight.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.weight });
                mMyListHeight.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.heiht });
                mMyListTemperature.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.temperature });
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            foreach (var c in list)
            {
                mMyListRashes.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.rashes });
                mMyListabdominalDistention.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.abdominalDistention });
                mMyListGaz.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.gas });
                mMyListLock.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.locking });
                mMyListDiareya.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.diareya });
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            foreach (var c in list)
            {
                mMyListPlay.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.playing });
                mMyListCry.Add(new NameValueItem { Name = c.date.Date.ToString("dd/MM/yyyy"), Value = c.crying });
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            EveryDay ed = param;
            ed.id = param.id;
            foreach (var c in elemEdit)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    if ((c as TextBox).Name == "weight")
                    {
                        if ((c as TextBox).Text != "")
                     ed.weight = Convert.ToDouble( (c as TextBox).Text );
                        else
                        {
                            new MessageDialog("Поле вес должно быть заполнено.").ShowAsync();
                            return;
                        }
                    }
                    if ((c as TextBox).Name == "height")
                    {
                        if ((c as TextBox).Text != "")
                        ed.heiht = Convert.ToDouble((c as TextBox).Text);
                        else
                        {
                            new MessageDialog("Поле рост должно быть заполнено.").ShowAsync();
                            return;
                        }
                    }
                    if ((c as TextBox).Name == "temperature")
                    {
                        if ((c as TextBox).Text != "")
                        ed.temperature = Convert.ToDouble((c as TextBox).Text);
                        else
                        {
                            new MessageDialog("Поле температура должно быть заполнено.").ShowAsync();
                            return;
                        }
                    }
                }
                if (c.GetType() == typeof(Slider))
                {
                    if ((c as Slider).Name == "rashes") ed.rashes = Convert.ToInt16( (c as Slider).Value );
                    if ((c as Slider).Name == "abdominalDistention") ed.abdominalDistention = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "play") ed.playing = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "cry") ed.crying = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "locking") ed.locking = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "gas") ed.gas = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "diareya") ed.diareya = Convert.ToInt16((c as Slider).Value);
                }
                if (c.GetType() == typeof(DatePicker))
                {
                    ed.date =  Convert.ToDateTime((c as DatePicker).Date.Date);
                }
                if (c.GetType() == typeof(RichEditBox))
                {
                    string str;
                    (c as RichEditBox).Document.GetText(Windows.UI.Text.TextGetOptions.None, out str);
                    ed.description = str;
                }
            }
             db.UpdateEveryDay(ed);
             var dlg = new MessageDialog("Сохранение записи завершенно.");
             dlg.ShowAsync();
             this.Frame.Navigate(typeof(MainPage));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            db.DeleteEveryDay(param);
            var dlg = new Windows.UI.Popups.MessageDialog("Данные успешно удалены !");
            dlg.ShowAsync();
            this.Frame.Navigate(typeof(MainPage));
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            WorkDB db = new WorkDB();
            EveryDay ed = new EveryDay();
            foreach (var c in elemAdd)
            {
                if (c.GetType() == typeof(TextBox))
                {
                    if ((c as TextBox).Name == "weight")
                    {
                        ed.weight = Convert.ToDouble((c as TextBox).Text);
                    }
                    if ((c as TextBox).Name == "height")
                    {
                        ed.heiht = Convert.ToDouble((c as TextBox).Text);
                    }
                    if ((c as TextBox).Name == "temperature")
                    {
                        ed.temperature = Convert.ToDouble((c as TextBox).Text);
                    }
                }
                if (c.GetType() == typeof(Slider))
                {
                    if ((c as Slider).Name == "rashes") ed.rashes = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "abdominalDistention") ed.abdominalDistention = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "play") ed.playing = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "cry") ed.crying = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "locking") ed.locking = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "gas") ed.gas = Convert.ToInt16((c as Slider).Value);
                    if ((c as Slider).Name == "diareya") ed.diareya = Convert.ToInt16((c as Slider).Value);
                }
                if (c.GetType() == typeof(DatePicker))
                {
                  ed.date = Convert.ToDateTime((c as DatePicker).Date.Date);
                }
                if (c.GetType() == typeof(RichEditBox))
                {
                    string str;
                    (c as RichEditBox).Document.GetText(Windows.UI.Text.TextGetOptions.None, out str);
                    ed.description = str;
                }
            }
            db.InsertEveryDay(ed);
            var dlg = new MessageDialog("Добавление записи завершенно.");
            dlg.ShowAsync();
        }

        private void Button_Click_prev(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));
        }

    }
}
