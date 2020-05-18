using MiniTC.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MiniTC
{
    /// <summary>
    /// Logika interakcji dla klasy PanelTC.xaml
    /// </summary>
    public partial class PanelTC : UserControl
    {
        #region Dependency properties'y
        public string Sciezka
        {
            get { return (string)GetValue(SciezkaDP); }
            set { SetValue(SciezkaDP, value); }
        }
        public static readonly DependencyProperty SciezkaDP = DependencyProperty.Register(
            nameof(Sciezka), typeof(string), typeof(PanelTC), new FrameworkPropertyMetadata(null));

        public string[] Dyski
        {
            get { return (string[])GetValue(DyskiDP); }
            set { SetValue(DyskiDP, value); }
        }
        public static readonly DependencyProperty DyskiDP = DependencyProperty.Register(
            nameof(Dyski), typeof(string[]), typeof(PanelTC), new FrameworkPropertyMetadata(null));

        public string WybranyDysk
        {
            get { return (string)GetValue(WybranyDyskDP); }
            set {  SetValue(WybranyDyskDP, value); }
        }
        public static readonly DependencyProperty WybranyDyskDP = DependencyProperty.Register(
            nameof(WybranyDysk), typeof(string), typeof(PanelTC), new FrameworkPropertyMetadata(null));

        public string[] Foldery
        {
            get { return (string[])GetValue(FolderyDP); }
            set { SetValue(FolderyDP, value); }
        }
        public static readonly DependencyProperty FolderyDP = DependencyProperty.Register(
            nameof(Foldery), typeof(string[]), typeof(PanelTC), new FrameworkPropertyMetadata(null));

        public string WybranyFolder
        {
            get { return (string)GetValue(WybranyFolderDP); }
            set { SetValue(WybranyFolderDP, value); }
        }
        public static readonly DependencyProperty WybranyFolderDP = DependencyProperty.Register(
            nameof(WybranyFolder), typeof(string), typeof(PanelTC), new FrameworkPropertyMetadata(null));       
        #endregion

        #region Zdarzenie
        public static readonly RoutedEvent MojeZdarzenieZarejestrowane =
        EventManager.RegisterRoutedEvent(nameof(MojeZdarzenie),
                    RoutingStrategy.Bubble, typeof(RoutedEventHandler),
                    typeof(PanelTC));

        public event RoutedEventHandler MojeZdarzenie
        {
            add { AddHandler(MojeZdarzenieZarejestrowane, value); }
            remove { RemoveHandler(MojeZdarzenieZarejestrowane, value); }
        }
        private void odswiezDyski(object sender, EventArgs e)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(PanelTC.MojeZdarzenieZarejestrowane);
            RaiseEvent(newEventArgs);
        }
        #endregion

        public PanelTC()
        {
            InitializeComponent();
        }        
    }
}
