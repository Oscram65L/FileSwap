using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TheGame.PointAllocation;

/* TODO:
 * Creation dates shouldn't matter as much
 * Older files mean you remember how to restore them less, so the older the better? up to a point - or older the better but get exponentially less per day for example.
 * More/less points based of file types? - research how many files types exist, ignore custom file types or should they be worth even more?
 * 
 * */

namespace TheGame.Deletion
{
    /// <summary>
    /// Interaction logic for GameUserControl.xaml
    /// </summary>
    public partial class GameUserControl : UserControl
    {
        private List<string> _files;
        private Player _currentPlayer;

        public GameUserControl()
        {
            InitializeComponent();
            _currentPlayer = new Player("Adam", 0);

            _files = FileScannerWinSearchIndex.Scan();

            _selectFile();
        }

        private void _selectFile()
        {
            int pointsToGain = 0;
            string selectedFilePath = _files[new Random().Next(_files.Count)];
            if (File.Exists(selectedFilePath))
                pointsToGain = PointsCalculator.CalculatePointsForFile(new FileInfo(selectedFilePath));
            else if (Directory.Exists(selectedFilePath))
                pointsToGain = PointsCalculator.CalculatePointsForDirectory(new DirectoryInfo(selectedFilePath));
            else
                throw new Exception("File path selected for deletion is not recognized as a File or Directory");

            lblFileName.Content = selectedFilePath;
            lblScoreToGain.Content = pointsToGain;
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            _delete();
        }

        private void buttonSkip_Click(object sender, RoutedEventArgs e)
        {
            _skip();
        }

        private void _delete()
        {

        }

        private void _skip()
        {
            _selectFile();
        }
    }
}
