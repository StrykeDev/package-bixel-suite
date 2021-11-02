using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Blaze.Views
{
    public partial class MonitorDim : UserControl
    {
        bool loaded = false;


        public MonitorDim()
        {
            InitializeComponent();

            sldOpacity.Value = Properties.Settings.Default.Dim_Opacity;
            sldFadeSpeed.Value = Properties.Settings.Default.Dim_Speed;

            loaded = true;

            SldOpacity_ValueChanged(null, null);
        }


        public static readonly RoutedEvent UpdatedEvent = EventManager.RegisterRoutedEvent(
            "Updated", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MonitorDim));

        public event RoutedEventHandler Updated
        {
            add { AddHandler(UpdatedEvent, value); }
            remove { RemoveHandler(UpdatedEvent, value); }
        }

        void RaiseUpdatedEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(MonitorDim.UpdatedEvent);
            RaiseEvent(newEventArgs);
        }


        private void SldOpacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (loaded)
            {
                rectDimPreview.Opacity = sldOpacity.Value;
            }
        }

        private void BtnApply_Click(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.Dim_Opacity = sldOpacity.Value;
            Properties.Settings.Default.Dim_Speed = sldFadeSpeed.Value;
            Properties.Settings.Default.Save();

            RaiseUpdatedEvent();
        }
    }
}
