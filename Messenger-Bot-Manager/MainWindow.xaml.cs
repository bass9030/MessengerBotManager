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
using Ookii.Dialogs.Wpf;
using ControlzEx.Automation.Peers;

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

        List<Bot> bots = new List<Bot>();
        public MainWindow()
        {
            new Loading().ShowDialog();
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = this;
            BotList.ItemsSource = bots;
            editorTab.ClosingItemCallback = EditorTab_PreviewTabClosing;

            refreshBotList();
        }

        private void refreshBotList()
        {
            bots.Clear();
            string botPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MessengerBotManager");
            if (!Directory.Exists(botPath)) Directory.CreateDirectory(botPath);
            foreach (string bot in Directory.GetDirectories(botPath))
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
            TabItem selectedItem = (TabItem)args.Owner.SelectedItem;
            if (bots[int.Parse(selectedItem.Name.Replace("t", ""))].isChanged)
            {
                TaskDialog dialog = new();
                dialog.MainIcon = TaskDialogIcon.Warning;
                dialog.WindowTitle = "저장되지 않은 봇";
                dialog.Content = $"{selectedItem.Header}을(를) 저장하시겠습니까?";
                dialog.Buttons.Add(new TaskDialogButton()
                {
                    ButtonType = ButtonType.Yes,
                    Text = "저장"
                });
                dialog.Buttons.Add(new TaskDialogButton()
                {
                    ButtonType = ButtonType.No,
                    Text = "저장하지 않음"
                });
                dialog.Buttons.Add(new TaskDialogButton()
                {
                    ButtonType = ButtonType.Cancel,
                    Text = "취소"
                });
                TaskDialogButton result = dialog.ShowDialog();
                if (result.ButtonType == ButtonType.Yes) SaveBot(int.Parse(selectedItem.Name.Replace("t", "")));
                else if (result.ButtonType == ButtonType.No) return;
                else if (result.ButtonType == ButtonType.Cancel) args.Cancel();
            }
        }

        private void SaveBot(int idx)
        {
            // TODO: save bot
            File.WriteAllText(bots[idx].Path, ((TextEditor)editorTab.SelectedContent).Text);
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
                FontSize = 16,
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
            //TODO: 미저장후 앱 종료시 경고창 생성
            int idx = int.Parse(((TextEditor)sender).Name.Replace("e", ""));
            bots[idx].isChanged = true;
            TabItem item = editorTab.Items.OfType<TabItem>().SingleOrDefault(n => n.Name == "t" + idx);
            if(!item.Header.ToString().EndsWith(" *"))item.Header = item.Header + " *";
        }

        private void CreateNewBot_Click(object sender, RoutedEventArgs e)
        {
            CreateBotWindow window = new CreateBotWindow();
            window.Closing += (sender, e) =>
            {
                if (window.bot != null)
                {
                    Bot bot = window.bot;
                    Directory.CreateDirectory(bot.Path);
                    JObject infoJson = new JObject();
                    /*
                     * Type = (BotType)Enum.Parse(typeof(BotType), info["type"].ToString()),
                     * Name = info["name"].ToString(),
                     * Path = Path.Combine(bot, info["scriptName"].ToString()),
                     * isOn = info["isOn"].ToObject<bool>()
                    */
                    infoJson.Add("type", bot.Type.ToString());
                    infoJson.Add("name", bot.Name);
                    infoJson.Add("scriptName", bot.Path.Split('\\').Last() + ".js");
                    infoJson.Add("isOn", bot.isOn);
                    File.WriteAllText(Path.Combine(bot.Path, "bot.json"), infoJson.ToString());
                    File.WriteAllText(Path.Combine(bot.Path, bot.Name + ".js"), Properties.Settings.Default.defaultCode);
                    refreshBotList();
                }
            };
            window.ShowDialog();
        }

        private void OpenBot_Click(object sender, RoutedEventArgs e)
        {
            //TODO: bot.json 
        }
    }
}
