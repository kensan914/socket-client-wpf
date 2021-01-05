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
using static ReceiverClient;

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

        ReceiverClient client = new ReceiverClient();
        void showMessage(string message)
        {
            // ↓messagesが呼び出し元では別スレッドにあたるため対処
            this.Dispatcher.Invoke((Action)(() =>
            {
                messages.Text += "・" + message + "\n";
            }));
        }
    }
}
