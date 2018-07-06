using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace TheGame
{
    public class FileScanner
    {
        private static List<string> _files;
        private static List<Task> _scanningTasks = new List<Task>();
        private static Dictionary<int?, Display> _displays = new Dictionary<int?, Display>();
        private static int _displayToUpdate;

        private static ScanningComputerUserControl _scanningUC;


        public static List<string> Scan()
        {
            _scanningUC = new ScanningComputerUserControl();

            Window scanningDisplay = new Window
            {
                Title = "Scanning Computer",
                Content = _scanningUC,
                SizeToContent = SizeToContent.WidthAndHeight
            };

            scanningDisplay.Show();

            setupFilesList();

            Task.Run(async () =>
            {
                while (true)
                {
                    if (_scanningTasks.All(task => task.IsCompleted))
                        break;

                    foreach(var key in _displays.Keys)
                        _displays[key].ShouldUpdate = true;

                    await Task.Delay(800);
                }
                scanningDisplay.Close();
            });

            return  _files;
        }

        //[PrincipalPermission(SecurityAction.Demand, Role = @"BUILTIN\Administrators")]
        private static List<Task> setupFilesList()
        {
            _files = new List<string>();

            var temp = DriveInfo.GetDrives();

            foreach (var drive in DriveInfo.GetDrives())
            {
                int displayToUpdate = _displayToUpdate++;
                Task t = Task.Run(() =>
                {
                    var files = _getAllSystemObjectsInDirectory(drive.RootDirectory.FullName);

                    _files.AddRange(files.ToArray());
                });
                _scanningTasks.Add(t);
                _displays.Add(t.Id, new Display(displayToUpdate));
            }
            return _scanningTasks;
        }

        private static IEnumerable<string> _getAllSystemObjectsInDirectory(string rootDirectory)
        {
            IEnumerable<string> files = Enumerable.Empty<string>();
            IEnumerable<string> directories = Enumerable.Empty<string>();
            try
            {
                // The test for UnauthorizedAccessException.
                var permission = new FileIOPermission(FileIOPermissionAccess.PathDiscovery, rootDirectory);
                permission.Demand();

                files = Directory.GetFiles(rootDirectory);
                directories = Directory.GetDirectories(rootDirectory);
            }
            catch
            {
                // Ignore folder (access denied).
                rootDirectory = null;
            }

            if (rootDirectory != null)
                yield return rootDirectory;

            foreach (var file in files)
            {
                if (_displays.ContainsKey(Task.CurrentId) && _displays[Task.CurrentId].ShouldUpdate)
                    _displayScanningProgress(_displays[Task.CurrentId].DisplayCode, file);
                yield return file;
            }

            // Recursive call for SelectMany.
            var subdirectoryItems = directories.SelectMany(_getAllSystemObjectsInDirectory);
            foreach (var result in subdirectoryItems)
            {
                if (_displays.ContainsKey(Task.CurrentId) && _displays[Task.CurrentId].ShouldUpdate)
                    _displayScanningProgress(_displays[Task.CurrentId].DisplayCode, result);
                yield return result;
            }
        }

        private static void _displayScanningProgress(int displayToUpdate, string filePath)
        {
            int? taskId = Task.CurrentId;
            Application.Current.Dispatcher.Invoke(() =>
            {
                switch (displayToUpdate)
                {
                    case 0:
                        _scanningUC.lblCurrentlyScanning1.Content = filePath;
                        _displays[taskId].ShouldUpdate = false;
                        break;
                    case 1:
                        _scanningUC.lblCurrentlyScanning2.Content = filePath;
                        _displays[taskId].ShouldUpdate = false;
                        break;
                    case 2:
                        _scanningUC.lblCurrentlyScanning3.Content = filePath;
                        _displays[taskId].ShouldUpdate = false;
                        break;
                    case 3:
                        _scanningUC.lblCurrentlyScanning4.Content = filePath;
                        _displays[taskId].ShouldUpdate = false;
                        break;
                }

            });
        }

        private class Display
        {
            public int DisplayCode { get; private set; }

            public bool ShouldUpdate { get; set; }

            public Display(int displayCode)
            {
                DisplayCode = displayCode;
                ShouldUpdate = false;
            }
        }
    }
}