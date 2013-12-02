using Genetics.Specialized;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace Main.ViewModel.GeneticsConfiguration
{
    [ImplementPropertyChanged]
    public class TournamentSelectionConfiguration : IGUIConfiguration, ISelectionConfiguration
    {
        public Genetics.Generic.ISelector BuildSelector()
        {
            return new TournamentSelector();
        }

        public string Name
        {
            get { return "Tournament selection"; }
        }

        public System.Windows.FrameworkElement GUI { get; set; }
    }
}
