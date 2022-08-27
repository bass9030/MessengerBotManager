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
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Messenger_Bot_Manager
{
    /// <summary>
    /// CreateBotWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CreateBotWindow : MetroWindow
    {
        static string regexSearch = new string(Path.GetInvalidFileNameChars()) + new string(Path.GetInvalidPathChars());
        Regex r = new Regex(string.Format("[{0}]", Regex.Escape(regexSearch)));
        public Bot bot { get; private set; }

        public CreateBotWindow()
        {
            InitializeComponent();
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            bot = new();
            bot.Name = botName.Text;
            bot.Path = Path.Combine(Properties.Settings.Default.programPath, botName.Text);
            bot.Type = (BotType)Enum.Parse(typeof(BotType), ((ComboBoxItem)botType.SelectedItem).Tag.ToString());
            bot.isOn = false;

            Close();
        }

        private void botName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(r.IsMatch(botName.Text))
            {
                int targetIdx = (botName.SelectionStart - botName.GetCharacterIndexFromLineIndex(0)) - 1;
                if (targetIdx < 0) targetIdx = 0;
                botName.Text = r.Replace(botName.Text, "");
                botName.Select(targetIdx, 0);
            }
        }
    }
}
