using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using MahApps.Metro.Controls;
using System.Xml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ICSharpCode.AvalonEdit;
using System.Timers;
using System.Net.Http;
using System.IO.Compression;
using Dragablz;

namespace Messenger_Bot_Manager
{
    public enum BotType
    {
        MSGBOT,
        CHATBOT,
        OTHER
    }
    public class Bot
    {
        public bool isChanged = false;
        public BotType Type { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public bool isOn { get; set; }
    }


    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public RelayCommand BtnCmd { get; set; }

        List<Bot> bots = new List<Bot>();
        public MainWindow()
        {
            new Loading().ShowDialog();
            BtnCmd = new RelayCommand(buttonExecute, buttonCanExecute);
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void buttonExecute(object param)
        {
            MessageBox.Show(param.ToString());
            //Debug.WriteLine(((TabItem)param).Name);
        }

        private bool buttonCanExecute(object param)
        {
            return true;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            BotList.ItemsSource = bots;
            editorTab.ClosingItemCallback = EditorTab_PreviewTabClosing;
            string botPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MessengerBotManager");
            foreach(string bot in Directory.GetDirectories(botPath))
            {
                JObject info = JObject.Parse(File.ReadAllText(Path.Combine(bot, "bot.json")));
                bots.Add(new Bot()
                {
                    Type = (BotType)Enum.Parse(typeof(BotType), info["type"].ToString()),
                    Name = info["name"].ToString(),
                    Path = Path.Combine(bot, info["scriptName"].ToString()),
                    isOn = info["isOn"].ToObject<bool>()
                });
            }

            BotList.Items.Refresh();
        }

        private void EditorTab_PreviewTabClosing(ItemActionCallbackArgs<TabablzControl> args)
        {
            args.Cancel();
        }

        private void TabItem_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (!(e.Source is TabItem tabItem))
            {
                return;
            }

            if (Mouse.PrimaryDevice.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(tabItem, tabItem, DragDropEffects.All);
            }
        }

        private void TabItem_Drop(object sender, DragEventArgs e)
        {
            if (e.Source is TabItem tabItemTarget &&
                e.Data.GetData(typeof(TabItem)) is TabItem tabItemSource &&
                !tabItemTarget.Equals(tabItemSource) &&
                tabItemTarget.Parent is TabControl tabControl)
            {
                int targetIndex = tabControl.Items.IndexOf(tabItemTarget);

                tabControl.Items.Remove(tabItemSource);
                tabControl.Items.Insert(targetIndex, tabItemSource);
                tabItemSource.IsSelected = true;
            }
        }
        private void BotList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BotList.SelectedItems.Count > 1) BotList.SelectedItems.RemoveAt(0);
        }

        private void BotList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (BotList.SelectedIndex == -1) return;
            int idx = editorTab.Items.OfType<TabItem>().ToList().FindIndex(e => e.Name == "t" + BotList.SelectedIndex);
            if (idx != -1)
            {
                editorTab.SelectedIndex = idx;
                return;
            }

            //CloseTabCommandParameter = "{Binding RelativeSource={RelativeSource Self}, Path=Header}" >



            TabItem item = new TabItem()
            {
                Header = bots[BotList.SelectedIndex].Name,
                Name = "t" + BotList.SelectedIndex.ToString(),
                AllowDrop = true,
                //CloseTabCommand = BtnCmd,
                //CloseTabCommandParameter = BotList.SelectedIndex
            };
            item.MouseMove += TabItem_PreviewMouseMove;
            item.Drop += TabItem_Drop;

            TextEditor editor = new()
            {
                FontSize = 18,
                Name = "e" + BotList.SelectedIndex.ToString(),
                FontFamily = new FontFamily("D2Coding"),
                SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript"),
                Foreground = new SolidColorBrush(Colors.LightGray),
                ShowLineNumbers = true,
                Text = File.ReadAllText(bots[BotList.SelectedIndex].Path),
            };

            editor.TextChanged += Editor_TextChanged;

            using (Stream s = new MemoryStream(Encoding.Default.GetBytes(Properties.Resources.VS2019_Dark)))
            {
                using (XmlTextReader reader = new XmlTextReader(s))
                {
                    editor.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
                }
            }
            item.Content = editor;
            editorTab.Items.Add(item);
            editorTab.Items.Refresh();
            editorTab.SelectedIndex = editorTab.Items.Count - 1;
        }

        private void Editor_TextChanged(object? sender, EventArgs e)
        {
            bots[int.Parse(((TextEditor)sender).Name.Replace("e", ""))].isChanged = true;
        }
    }
}
