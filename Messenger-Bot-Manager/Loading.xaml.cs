using MahApps.Metro.Controls;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Messenger_Bot_Manager
{
    /// <summary>
    /// Loading.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class Loading : MetroWindow
    {
        BackgroundWorker worker;
        public Loading()
        {
            InitializeComponent();
            worker = new();
            worker.DoWork += Worker_DoWork;
            Loaded += Loading_Loaded;
        }

        private void checkADB()
        {
            if (!File.Exists("adb.exe") || !File.Exists("AdbWinApi.dll") || !File.Exists("AdbWinUsbApi.dll"))
            {
                Dispatcher.Invoke(() => comment.Content = "adb 다운로드중...");
                using (HttpClient client = new())
                {
                    client.BaseAddress = new Uri("https://dl.google.com");
                    using (ZipArchive archive = new ZipArchive(client.GetStreamAsync("/android/repository/platform-tools-latest-windows.zip?hl=ko").Result))
                    {
                        foreach (ZipArchiveEntry entry in archive.Entries)
                        {
                            if (entry.Name.StartsWith("adb", StringComparison.OrdinalIgnoreCase))
                            {
                                entry.ExtractToFile(Path.Combine(".\\", entry.Name));
                            }
                        }
                    }
                }
            }
        }

        private void Worker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if(string.IsNullOrEmpty(Properties.Settings.Default.programPath))
            {
                Properties.Settings.Default.programPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "MessengerBotManager");
                Properties.Settings.Default.Save();
            }
            Dispatcher.Invoke(() => comment.Content = "adb 파일 유무 확인중...");
            checkADB();

            Adb adb = new Adb(".\\adb.exe");
            Dispatcher.Invoke(() => comment.Content = "adb 연결 확인중...");

            bool isAdbConnect = false;
            string[] devices;
            while (true)
            {
                devices = adb.getDeviceIds();
                if (devices.Length == 0)
                {
                    TaskDialog dialog = new();
                    dialog.MainIcon = TaskDialogIcon.Warning;
                    dialog.WindowTitle = "ADB 연결 없음!";
                    dialog.Content = "ADB에 연결된 기기가 없습니다!\n일부 기능이 제한됩니다. 계속하시겠습니까?";
                    dialog.Buttons.Add(new TaskDialogButton()
                    {
                        ButtonType = ButtonType.Cancel
                    });
                    dialog.Buttons.Add(new TaskDialogButton()
                    {
                        ButtonType = ButtonType.Retry
                    });
                    if (dialog.ShowDialog().ButtonType != ButtonType.Retry) break;
                    else continue;
                }
            }
            if(devices.Length != 0)
            {
                isAdbConnect = true;
                if(string.IsNullOrEmpty(Properties.Settings.Default.deviceId))
                {
                    if (devices.Length == 1)
                    {
                        Properties.Settings.Default.deviceId = devices[0];
                        Properties.Settings.Default.Save();
                        adb.setTargetDeviceId(devices[0]);
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            Window deviceSelect = new deviceSelect(devices);
                            deviceSelect.Closed += (s,e) => adb.setTargetDeviceId(Properties.Settings.Default.deviceId);
                            deviceSelect.ShowDialog();
                        });
                        return;
                    }
                }
                else
                {
                    adb.setTargetDeviceId(Properties.Settings.Default.deviceId);
                }
            }

            Dispatcher.Invoke(() => comment.Content = "봇파일 동기화중...");

            if(isAdbConnect)
            {
                syncBot(adb);
            }

            Dispatcher.Invoke(() => Close());
        }

        private void syncBot(Adb adb)
        {
            // 메신저봇 경로 확인
            if (Properties.Settings.Default.msgbotPath != "None")
            {
                if (string.IsNullOrEmpty(Properties.Settings.Default.msgbotPath))
                {
                    Properties.Settings.Default.msgbotPath = "None";
                    string[] folders = adb.getFileFolders("/sdcard/");
                    foreach (string folder in folders)
                    {
                        //Debug.WriteLine(folder);
                        if (!Regex.IsMatch(folder, ".+\\..+"))
                        {
                            string[] innerFiles = adb.getFileFolders("/sdcard/" + folder);
                            string[] matchFiles = new string[] { "GLOBAL_LOG.json", "editor_shortcuts.txt", "global_modules", "Bots" };
                            int matchCount = 0;
                            foreach (string innerFile in innerFiles)
                            {
                                //Debug.WriteLine("└" + innerFile);
                                if (Array.IndexOf(matchFiles, innerFile) != -1)
                                {
                                    matchCount++;
                                }
                            }
                            if (matchCount == 4)
                            {
                                string path = $"/sdcard/{folder}/";
                                MessageBox.Show(path);
                                Properties.Settings.Default.msgbotPath = path;
                                Properties.Settings.Default.Save();
                            }
                        }
                    }
                }
            }

            // 채팅 자동응답 봇 경로 확인
            if (Properties.Settings.Default.chatbotPath != "None")
            {
                //TODO: 채팅 자동응답봇 폴더 존재 확인
            }
        }

        private void Loading_Loaded(object sender, RoutedEventArgs e)
        {
            worker.RunWorkerAsync();
        }
    }
}
