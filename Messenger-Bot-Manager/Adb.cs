using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Messenger_Bot_Manager
{
    internal class Adb
    {
        private string adbPath;
        private string targetDeviceId;
        public Adb(string adbPath)
        {
            this.adbPath = adbPath;
        }


        public string[] getDeviceIds()
        {
            List<string> deviceIds = new();
            string[] result = runExec("devices").Split(new String[] {"\r\n"}, StringSplitOptions.None);
            foreach(string deviceId in result)
            {
                if(Regex.IsMatch(deviceId, "(\\S+)\t(device)"))
                {
                    deviceIds.Add(Regex.Split(deviceId, "\\s")[0]);
                }
            }
            return deviceIds.ToArray();
        }

        public void pullFiles(string targetPath, string clientPath)
        {
            runExec($"pull {targetPath} {clientPath}");
        }

        public void pullFolder(string targetPath, string clientPath)
        {
            string[] folders = getFileFolders($"shell ls {targetPath}");
            foreach(string folder in folders)
            {
                if (!Regex.IsMatch(folder, ".+\\..+"))
                {
                    // folder
                    pullFolder(folder, targetPath + "/" + folder + "/" + clientPath);
                }
                else
                {
                    // file
                    pullFiles(folder, clientPath);
                }
            }
        }

        public void setTargetDeviceId(string deviceId)
        {
            string[] deviceids = getDeviceIds();
            if(!Array.Exists(deviceids, (e) => e == deviceId))
            {
                throw new ArgumentException("Not exists Device ID");
            }else this.targetDeviceId = deviceId;
        }

        public string[] getFileFolders(string path)
        {
            path = path
                .Replace("=", "\\=")
                .Replace("[", "\\[")
                .Replace("#", "\\#")
                .Replace("&", "\\&")
                .Replace(")", "\\)")
                .Replace("(", "\\(")
                .Replace("'", "\\'")
                .Replace(";", "\\;")
                .Replace("`", "\\`")
                .Replace("{", "\\{")
                .Replace("}", "\\}")
                .Replace("$", "\\$");
            try
            {
                return runExec($"shell \"ls -1 {path}\"").Split(new string[] { "\r\n" }, StringSplitOptions.None);
            }catch
            {
                return new string[0];
            }
        }

        private string output = "";
        public string runExec(string Argments)
        {
            output = "";
            if(Argments != "devices" && string.IsNullOrEmpty(targetDeviceId))
            {
                throw new ArgumentException("targetDeviceId is must be setted before run command");
            }
            Process process = new Process();
            ProcessStartInfo startinfo = new()
            {
                FileName = adbPath,
                StandardOutputEncoding = Encoding.Default,
                Arguments = (!string.IsNullOrEmpty(targetDeviceId) ? $"-s {targetDeviceId} " : "") + Argments,
                CreateNoWindow = true,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
            };
            process.StartInfo = startinfo;
            process.Start();
            process.OutputDataReceived += Process_OutputDataReceived;
            process.BeginOutputReadLine();
            process.WaitForExit();
            //string output = process.StandardOutput.ReadToEnd();
            string errStr = process.StandardError.ReadToEnd();
            if (process.ExitCode != 0 && !string.IsNullOrWhiteSpace(errStr))
            {
                throw new Exception(errStr);
            }
            else
            {
                return output;
                //return process.StandardOutput.ReadToEnd();
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            output += e.Data + "\r\n";
        }
    }
}
