using System;
using System.Threading;

namespace LiveSplit.HaloInfinite
{
    class SplittingLogic
    {
        private Watchers watchers;

        public event EventHandler OnStartTrigger;
        public event EventHandler<bool> OnIsLoading;
        public event EventHandler<SplitTrigger> OnSplitTrigger;
        public delegate bool TimerCheckEventHandler(object sender, EventArgs e);
        public event TimerCheckEventHandler OnTimerCheck;

        public void Update()
        {
            if (watchers != null && watchers.GameHasExited) this.OnIsLoading?.Invoke(this, true);
            if (!VerifyOrHookGameProcess() || !watchers.IsAutosplitterEnabled) return;
            watchers.Update();
            if (TimerNotRunning()) watchers.SplitBools = new SplitBools();
            Start();
            IsLoading();
            Split();
        }

        void Start()
        {
            if (watchers.StartTrigger) this.OnStartTrigger?.Invoke(this, EventArgs.Empty);
        }


        void Split()
        {
            if (watchers.Plot_BanishedShip) { watchers.SplitBools.BanishedShip = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.BanishedShip); }
            else if (watchers.Plot_Foundation) { watchers.SplitBools.Foundation = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.Foundation); }
            else if (watchers.Plot_OutpostTremonius) { watchers.SplitBools.OutpostTremonius = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.OutpostTremonius); }
            else if (watchers.Plot_FOBGolf) { watchers.SplitBools.FOBGolf = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.FOBGolf); }
            else if (watchers.Plot_Tower) { watchers.SplitBools.Tower = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.Tower); }
            else if (watchers.Plot_TravelToDigSite) { watchers.SplitBools.TravelToDigSite = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.TravelToDigSite); }
            else if (watchers.Plot_Bassus) { watchers.SplitBools.Bassus = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.Bassus); }
            else if (watchers.Plot_Conservatory) { watchers.SplitBools.Conservatory = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.Conservatory); }
            else if (watchers.Plot_SpireApproach) { watchers.SplitBools.SpireApproach = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.SpireApproach); }
            else if (watchers.Plot_SpireAdjutantResolution) { watchers.SplitBools.AdjutantResolution = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.AdjutantResolution); }
            else if (watchers.Plot_PelicanEast) { watchers.SplitBools.EastAAGun = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.EastAAGun); }
            else if (watchers.Plot_PelicanNorth) { watchers.SplitBools.NorthAAGun = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.NorthAAGun); }
            else if (watchers.Plot_PelicanWest) { watchers.SplitBools.WestAAGun = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.WestAAGun); }
            else if (watchers.Plot_SpartanKillers) { watchers.SplitBools.PelicanSpartanKillers = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.PelicanSpartanKillers); }
            else if (watchers.Plot_SequenceNorthernBeacon) { watchers.SplitBools.SequenceNorthernBeacon = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.SequenceNorthernBeacon); }
            else if (watchers.Plot_SequenceSouthernBeacon) { watchers.SplitBools.SequenceSouthernBeacon = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.SequenceSouthernBeacon); }
            else if (watchers.Plot_SequenceEasternBeacon) { watchers.SplitBools.SequenceEasternBeacon = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.SequenceEasternBeacon); }
            else if (watchers.Plot_SequenceSouthwesternBeacon) { watchers.SplitBools.SequenceSouthwesternBeacon = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.SequenceSouthwesternBeacon); }
            else if (watchers.Plot_SequenceEnterCommandspire) { watchers.SplitBools.SequenceEnterCommandSpire = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.SequenceEnterCommandSpire); }
            else if (watchers.Plot_Nexus) { watchers.SplitBools.Nexus = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.Nexus); }
            else if (watchers.Plot_Spire2_Reach_Top) { watchers.SplitBools.Spire2_reach_top = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.Spire2_reach_top); }
            else if (watchers.Plot_Spire2_Deactivate) { watchers.SplitBools.Spire2_deactivate = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.Spire2_deactivate); }
            else if (watchers.Plot_Repository) { watchers.SplitBools.Repository = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.Repository); }
            else if (watchers.Plot_Road) { watchers.SplitBools.Road = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.Road); }
            else if (watchers.Plot_HouseOfReckoning) { watchers.SplitBools.HouseOfReckoning = true; this.OnSplitTrigger?.Invoke(this, SplitTrigger.HouseOfReckoning); }
            else if (watchers.Plot_SilentAuditorium) { watchers.SplitBools.SilentAuditorium = true; this.OnSplitTrigger?.Invoke(this,SplitTrigger.SilentAuditorium); }
        }

        void IsLoading()
        {
            this.OnIsLoading.Invoke(this, watchers.IsLoading.Current);
        }

        bool TimerNotRunning()
        {
            return this.OnTimerCheck.Invoke(this, EventArgs.Empty);
        }

        bool VerifyOrHookGameProcess()
        {
            if (watchers != null && watchers.IsGameHooked) return true;
            try { watchers = new Watchers(); } catch { watchers = null; Thread.Sleep(500); return false; }
            return true;
        }
    }
}
