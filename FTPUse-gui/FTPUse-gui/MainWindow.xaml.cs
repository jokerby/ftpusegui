using System;
using System.Threading;
using System.Windows;

namespace FTPUse_gui
{
    public sealed partial class MainWindow : Window
    {
        private readonly MainViewModel _vm = new MainViewModel();
        public MainWindow()
        {
            SetLanguage();
            InitializeComponent();
            DataContext = _vm;
        }

        private void SetLanguage()
        {
            var dict = new ResourceDictionary();
            switch (Thread.CurrentThread.CurrentUICulture.ToString())
            {
                case "ru-RU":
                    dict.Source = new Uri("..\\Resources\\StringResources.ru-RU.xaml", UriKind.Relative);
                    break;
                case "pl-PL":
                    dict.Source = new Uri("..\\Resources\\StringResources.pl-PL.xaml", UriKind.Relative);
                    break;
                case "en-US":
                default:
                    dict.Source = new Uri("..\\Resources\\StringResources.default.xaml", UriKind.Relative);
                    break;
            }
            Resources.MergedDictionaries.Add(dict);
        }
    }
}