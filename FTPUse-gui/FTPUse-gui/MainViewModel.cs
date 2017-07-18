using System;
using System.Collections.Generic;
using System.Security;
using System.IO;
using System.Linq;
using System.Windows.Input;
using FTPUse_gui.Helpers;

namespace FTPUse_gui
{
    internal sealed class MainViewModel
    {
        #region Command
        private void RunMethod()
        {
            //
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
        public MapUnmapDrive SelectedAction { get; set; } = MapUnmapDrive.None;
        public KeyValuePair<string, string> Letter { get; set; }
        public string Hostname { get; set; }
        public string Remotepath { get; set; }
        public string Port { get; set; }
        public string Login { get; set; }
        public SecureString Password { get; set; }
        public PassiveActive SelectedMode { get; set; } = PassiveActive.Passive;
        public bool Debug { get; set; }
        public ICommand Run => new ButtonCommandBinding(RunMethod);
        #endregion
    }
}