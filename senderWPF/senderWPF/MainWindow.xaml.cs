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
using static SenderClient;

namespace chatWPF
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            client.connect();

            ShowMessage showMessage = this.showMessage;
            client.setShowMessage(showMessage);
        }

        SenderClient client = new SenderClient();
        void showMessage(string message)
        {
            messages.Text += "・" + input.Text + "\n";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            client.send(input.Text);
            input.Clear();
        }

        private void textBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
