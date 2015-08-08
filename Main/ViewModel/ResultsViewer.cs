using BuildingEditor.ViewModel;
using Genetics.Statistics;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Input;

namespace Main.ViewModel
{
    [ImplementPropertyChanged]
    public class ResultsViewer : INotifyPropertyChanged
    {
        private string resultsPath;

        public string ResultsPath
        {
            get { return resultsPath; }
            set { resultsPath = value; LoadResultsList(); }
        }
        public ObservableCollection<ResultSet> ResultSets { get; set; }
        public ResultSet SelectedResultSet { get; set; }
        public ICommand ChooseResultsPathCommand { get; set; }

        public ResultsViewer()
        {
            ChooseResultsPathCommand = new SimpleCommand(x => ChooseResultsPath());
            ResultSets = new ObservableCollection<ResultSet>();
            ResultsPath = @"D:\Results";
        }

        public void ChooseResultsPath()
        {
            var dialog = new FolderBrowserDialog();
            dialog.ShowDialog();
            ResultsPath = dialog.SelectedPath;
        }

        private void LoadResultsList()
        {
            if (!Directory.Exists(resultsPath))
                return;
            ResultSets.Clear();
            Directory.GetDirectories(resultsPath)
                .Select(x => new ResultSet(x))
                .Where(x => x.IsValid)
                .ToList()
                .ForEach(x => ResultSets.Add(x));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
