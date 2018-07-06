using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace TheGame
{
    /// <summary>
    /// Interaction logic for ScanningComputerUserControl.xaml
    /// </summary>
    public partial class ScanningComputerUserControl : UserControl
    {
        public string CurrentFilePath { get; set; }

        public ScanningComputerUserControl()
        {
            InitializeComponent();
        }
    }
}
