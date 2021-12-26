using System;
using System.Windows.Forms;
using System.Xml;
using LiveSplit.Model;
using LiveSplit.UI;
using LiveSplit.UI.Components;

namespace LiveSplit.HaloInfinite
{
    class Component : LogicComponent
    {
        public override string ComponentName => "Halo Infinite - Autosplitter";
        private Settings Settings { get; set; }
        private readonly TimerModel timer;
        private readonly System.Timers.Timer update_timer;
        private readonly SplittingLogic SplittingLogic;

        public Component(LiveSplitState state)
        {
            timer = new TimerModel { CurrentState = state };
            Settings = new Settings();

            SplittingLogic = new SplittingLogic();
            SplittingLogic.OnTimerCheck += OnTimerCheck;
            SplittingLogic.OnStartTrigger += OnStartTrigger;
            SplittingLogic.OnIsLoading += OnIsLoading;
            SplittingLogic.OnSplitTrigger += OnSplitTrigger;

            update_timer = new System.Timers.Timer() { Interval = 15, Enabled = true, AutoReset = false };
            update_timer.Elapsed += delegate { SplittingLogic.Update(); update_timer.Start(); };
        }

        void OnStartTrigger(object sender, EventArgs e)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.NotRunning) return;
            timer.Start();

        }

        void OnIsLoading(object sender, bool type)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.Running) return;
            timer.CurrentState.IsGameTimePaused = type;
        }

        void OnSplitTrigger(object sender, SplitTrigger type)
        {
            if (timer.CurrentState.CurrentPhase != TimerPhase.Running) return;
            switch (type)
            {
                case SplitTrigger.BanishedShip: Split(Settings.BanishedShip); break;
                case SplitTrigger.Foundation: Split(Settings.Foundation); break;
                case SplitTrigger.OutpostTremonius: Split(Settings.OutpostTremonius); break;
                case SplitTrigger.FOBGolf: Split(Settings.FOBGolf); break;
                case SplitTrigger.Tower: Split(Settings.Tower); break;
                case SplitTrigger.TravelToDigSite: Split(Settings.ReachTheDigSite); break;
                case SplitTrigger.Bassus: Split(Settings.Bassus); break;
                case SplitTrigger.Conservatory: Split(Settings.Conservatory); break;
                case SplitTrigger.SpireApproach: Split(Settings.ApproachTheCommandSpire); break;
                case SplitTrigger.AdjutantResolution: Split(Settings.AdjutantResolution); break;
                case SplitTrigger.EastAAGun: Split(Settings.EastAAGun); break;
                case SplitTrigger.NorthAAGun: Split(Settings.NorthAAGun); break;
                case SplitTrigger.WestAAGun: Split(Settings.WestAAGun); break;
                case SplitTrigger.PelicanSpartanKillers: Split(Settings.SpartanKillers); break;
                case SplitTrigger.SequenceEasternBeacon: Split(Settings.SequenceEasternBeacon); break;
                case SplitTrigger.SequenceSouthernBeacon: Split(Settings.SequenceSouthernBeacon); break;
                case SplitTrigger.SequenceNorthernBeacon: Split(Settings.SequenceNorthernBeacon); break;
                case SplitTrigger.SequenceSouthwesternBeacon: Split(Settings.SequenceSouthwesternBeacon); break;
                case SplitTrigger.SequenceEnterCommandSpire: Split(Settings.EnterTheNexus); break;
                case SplitTrigger.Nexus: Split(Settings.Nexus); break;
                case SplitTrigger.Spire2_reach_top: Split(Settings.ReachTheTop); break;
                case SplitTrigger.Spire2_deactivate: Split(Settings.DeactivateTheCommandSpire); break;
                case SplitTrigger.Repository: Split(Settings.Repository); break;
                case SplitTrigger.Road: Split(Settings.Road); break;
                case SplitTrigger.HouseOfReckoning: Split(Settings.HouseOfReckoning); break;
                case SplitTrigger.SilentAuditorium: Split(Settings.SilentAuditorium); break;
            }
        }

        private bool OnTimerCheck(object sender, EventArgs e) // Returns true if the timer is NOT running
        {
            return timer.CurrentState.CurrentPhase == TimerPhase.NotRunning;
        }

        void Split(bool statement)
        {
            if (statement) timer.Split();
        }

        public override void Dispose()
        {
            Settings.Dispose();
            update_timer?.Dispose();
        }

        public override XmlNode GetSettings(XmlDocument document) { return this.Settings.GetSettings(document); }

        public override Control GetSettingsControl(LayoutMode mode) { return this.Settings; }

        public override void SetSettings(XmlNode settings) { this.Settings.SetSettings(settings); }

        public override void Update(IInvalidator invalidator, LiveSplitState state, float width, float height, LayoutMode mode) { }
    }
}
