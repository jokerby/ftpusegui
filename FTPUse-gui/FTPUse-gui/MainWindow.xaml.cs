using System;
using System.Windows;

namespace FTPUse_gui
{
    public sealed partial class MainWindow
    {
        private readonly MainViewModel _vm = new MainViewModel();
        public MainWindow()
        {
            //TODO replace by LocExtension
            Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("..\\Resources\\StringResources.default.xaml", UriKind.Relative) });
            InitializeComponent();
            DataContext = _vm;
        }
    }
}