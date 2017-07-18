using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using FTPUse_gui.Annotations;
using FTPUse_gui.Helpers;
using Microsoft.Win32;

namespace FTPUse_gui
{
    internal sealed class MainViewModel : INotifyPropertyChanged
    {
        #region Commands
        private void RunMethod()
        {
            if (DriveInfo.GetDrives().Any(d => d.Name.Contains(Letter.Key)) != (SelectedAction == MapUnmapDrive.Map))
            {
                var parameters = string.Empty;
                switch (SelectedAction)
                {
                    case MapUnmapDrive.Map:
                        if (string.IsNullOrEmpty(Hostname))
                        {
                            MessageBox.Show("Incorrect address/hostname input!");
                            return;
                        }
                        parameters = $"{Letter.Key} {Hostname}";
                        if (!string.IsNullOrEmpty(Remotepath))
                            parameters += "/" + Remotepath;
                        if (!string.IsNullOrEmpty(Password))
                            parameters += " " + parameters;
                        if (!string.IsNullOrEmpty(Login))
                            parameters += " /USER:" + Login;
                        if (!string.IsNullOrEmpty(Port))
                            parameters += " /PORT:" + Port;
                        if (SelectedMode == PassiveActive.Active)
                            parameters += " /NOPASSIVE";
                        break;
                    case MapUnmapDrive.Unmap:
                        parameters = $"{Letter.Key} /DELETE";
                        break;
                }
                if (!string.IsNullOrEmpty(parameters))
                {
                    var path = @"c:\Program Files\Ferro Software\FtpUse\ftpuse.exe";
                    if (!File.Exists(path))
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), "ftpuse.exe");
                        if (!File.Exists(path))
                        {
                            MessageBox.Show("Select path to ftpuse.exe");
                            var ofd = new OpenFileDialog
                            {
                                Filter = "ftpuse|ftpuse.exe",
                                InitialDirectory = path,
                                Multiselect = false
                            };
                            if (ofd.ShowDialog() != true)
                                return;
                            path = ofd.FileName;
                        }
                    }
                    using (
                        var process = new Process
                        {
                            StartInfo =
                                new ProcessStartInfo(path)
                                {
                                    Arguments = parameters,
                                    UseShellExecute = false,
                                    CreateNoWindow = true
                                }
                        })
                    {
                        process.Start();
                        process.WaitForExit();
                        Thread.Sleep(2500);
                        OnPropertyChanged(nameof(DrivesList));
                    }
                }
            }
            else
                MessageBox.Show("Bad drive letter choice!");
        }
        #endregion
        #region Data
        internal enum MapUnmapDrive { Map, Unmap, None }
        internal enum PassiveActive { Passive, Active }
        public Dictionary<string, string> DrivesList
            => Enumerable.Range(0, 26)
                    .Select(i =>
                            new KeyValuePair<string, string>($"{Convert.ToChar(i + 65)}:",
                                DriveInfo.GetDrives()
                                    .FirstOrDefault(d => d.Name.Equals($"{Convert.ToChar(i + 65)}:\\"))?.VolumeLabel))
                    .ToDictionary(x => x.Key, x => x.Value);
        #endregion

        #region Public Properties for View
        public bool IsEnabledInput => SelectedAction == MapUnmapDrive.Map;
        public bool IsEnabledButton => SelectedAction != MapUnmapDrive.None;
        private MapUnmapDrive _selectedAction = MapUnmapDrive.None;
        public MapUnmapDrive SelectedAction
        {
            get { return _selectedAction; }
            set
            {
                if (value != _selectedAction)
                {
                    _selectedAction = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(IsEnabledInput));
                    OnPropertyChanged(nameof(IsEnabledButton));
                }
            }
        }
        public KeyValuePair<string, string> Letter { get; set; }
        public string Hostname { get; set; }
        public string Remotepath { get; set; }
        public string Port { get; set; } = "21";
        public string Login { get; set; }
        public string Password { get; set; }
        public PassiveActive SelectedMode { get; set; } = PassiveActive.Passive;
        public bool Debug { get; set; }
        public ICommand Run => new ButtonCommandBinding(RunMethod);
        #endregion

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}