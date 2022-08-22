using MahApps.Metro.Controls;
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
using System.Windows.Shapes;

namespace Messenger_Bot_Manager
{
    /// <summary>
    /// deviceSelect.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class deviceSelect : MetroWindow
    {
        public deviceSelect(string[] deviceIds)
        {
            InitializeComponent();
            foreach (string i in deviceIds) deviceList.Items.Add(i);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.deviceId = deviceList.SelectedItem.ToString();
            Properties.Settings.Default.Save();
            this.Close();
        }
    }
}
