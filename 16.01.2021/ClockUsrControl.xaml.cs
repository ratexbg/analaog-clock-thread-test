using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace _16._01._2021
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        private bool suspended;
        public UserControl1()
        {
            InitializeComponent();
            Thread clock = new Thread(TicTac); // suzdaden nov thread no ne e startiran
            clock.Start();  // startirane na nishkata
            suspended = true;
        }

        private void TicTac()
        {
            if(SecondHand == null || 
                MinuteHand == null || 
                HourHand == null) 
                return;
            while (true)
            {
                SetTime();
                Thread.Sleep(1000);
                lock (this)
                {
                    while (suspended)
                    {
                        Monitor.Wait(this);
                    }

                }
            }
        }
        private void SetTime()
        {
            this.Dispatcher.Invoke(
                new Action(
                    ()=>
                    {
                        int secs, mins, hrs;
                        secs = DateTime.Now.Second;
                        mins = DateTime.Now.Minute;
                        hrs = DateTime.Now.Hour;
                        SecondHand.Angle = secs * 6; // 6 e gradusite na premestvane zashtoto 360gradusa/60minuti
                        MinuteHand.Angle = mins * 6; // same kato gornoto
                        HourHand.Angle = hrs * 30 + mins*0.5 ; // 30 gradusa v sluchaq za chasovete + polovinata na minutite za plavno preminavane na chasa.
                    }
                ));
        }

        public void Start()
        {
            lock (this)
            {
                suspended = false;
                Monitor.PulseAll(this);

            }
           
        }
        public void Stop()
        {
            suspended = true;
        }

    }
}
