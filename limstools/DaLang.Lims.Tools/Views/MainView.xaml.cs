using DaLang.Lims.Tools.Views;
using System.Windows;
using System.Windows.Controls;

namespace DaLang.Lims.Tools;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
        this.Hide();
        e.Cancel = true;
    }

    private void Window_Loaded(object sender, RoutedEventArgs e)
    {
        //this.Hide();
    }
    private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        //日志文本增加，滚动到最后
        (sender as TextBox)!.ScrollToEnd();
    }

    private void OpenCamera_Click(object sender, RoutedEventArgs e)
    {
        CameraView view = new CameraView();
        view.ShowDialog();
    }
}