using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using LiveSplit.ComponentUtil;

namespace LiveSplit.HaloInfinite
{
    class Watchers : MemoryWatcherList
    {
        // Game process
        private readonly Process game;
        public bool IsGameHooked => game != null && !game.HasExited;
        public bool GameHasExited => game.HasExited;
        public bool IsAutosplitterEnabled = true;

        // Imported game data
        private MemoryWatcher<byte> LoadStatus { get; }
        private MemoryWatcher<byte> LoadStatusPercentage { get; }
        private StringWatcher StatusString { get; }
        private MemoryWatcher<bool> LoadScreen { get; }
        private MemoryWatcher<bool> LoadingIcon { get; }
        private MemoryWatcher<byte> OutpostTremonius { get; }
        private MemoryWatcher<byte> FOBGolf { get; }
        private MemoryWatcher<byte> Tower { get; }
        private MemoryWatcher<byte> TravelToDigSite { get; }
        private MemoryWatcher<byte> Spire { get; }
        private MemoryWatcher<byte> EastAAGun { get; }
        private MemoryWatcher<byte> NorthAAGun { get; }
        private MemoryWatcher<byte> WestAAGun { get; }
        private MemoryWatcher<byte> PelicanSpartanKillers { get; }
        private MemoryWatcher<byte> SequenceNorthernBeacon { get; }
        private MemoryWatcher<byte> SequenceSouthernBeacon { get; }
        private MemoryWatcher<byte> SequenceEasternBeacon { get; }
        private MemoryWatcher<byte> SequenceSouthwesternBeacon { get; }
        private MemoryWatcher<byte> SilentAuditorium { get; }

        // Fake MemoryWatchers: used to convert game data into more easily readable formats
        private FakeMemoryWatcher<string> Map => new FakeMemoryWatcher<string>(string.IsNullOrEmpty(this.StatusString.Old) ? "null" : this.StatusString.Old.Substring(this.StatusString.Old.LastIndexOf("\\") + 1),
            string.IsNullOrEmpty(this.StatusString.Current) ? "null" : this.StatusString.Current.Substring(this.StatusString.Current.LastIndexOf("\\") + 1));

        public FakeMemoryWatcher<bool> IsLoading => new FakeMemoryWatcher<bool>((string.IsNullOrEmpty(this.StatusString.Old) ? false : this.StatusString.Old.Substring(0, this.StatusString.Old.LastIndexOf(" ")) == "loading") ||
            this.LoadStatus.Old == 3 || this.LoadingIcon.Old || this.LoadScreen.Old || (this.LoadStatusPercentage.Old != 0 && this.LoadStatusPercentage.Old != 100) || (this.LoadStatusPercentage.Old == 100 && this.LoadStatus.Old < 4),
            (string.IsNullOrEmpty(this.StatusString.Old) ? false : this.StatusString.Current.Substring(0, this.StatusString.Current.LastIndexOf(" ")) == "loading") ||
            this.LoadStatus.Current == 3 || this.LoadingIcon.Current || this.LoadScreen.Current || (this.LoadStatusPercentage.Current != 0 && this.LoadStatusPercentage.Current != 100) || (this.LoadStatusPercentage.Current == 100 && this.LoadStatus.Current < 4));
			
        public SplitBools SplitBools = new SplitBools();
        public bool StartTrigger => this.Map.Current == Maps.BanishedShip && this.IsLoading.Old && !this.IsLoading.Current;
		
        // Plot events triggers management
        public bool Plot_BanishedShip => !this.SplitBools.BanishedShip && this.Map.Old == Maps.BanishedShip && this.Map.Current == Maps.Foundation;
        public bool Plot_Foundation => !this.SplitBools.Foundation && this.Map.Old == Maps.Foundation && this.Map.Current == Maps.ZetaHalo;
        public bool Plot_OutpostTremonius => !this.SplitBools.OutpostTremonius && this.Map.Current == Maps.ZetaHalo && this.OutpostTremonius.Changed && this.OutpostTremonius.Current == 6;
        public bool Plot_FOBGolf => !this.SplitBools.FOBGolf && this.Map.Current == Maps.ZetaHalo && this.FOBGolf.Changed && this.FOBGolf.Current == 10;
        public bool Plot_Tower => !this.SplitBools.Tower && this.Map.Current == Maps.ZetaHalo && this.Tower.Changed && this.Tower.Current == 10;
        public bool Plot_TravelToDigSite => !this.SplitBools.TravelToDigSite && this.Map.Current == Maps.ZetaHalo && this.TravelToDigSite.Changed && this.TravelToDigSite.Current == 10;
        public bool Plot_Bassus => !this.SplitBools.Bassus && this.Map.Old == Maps.ZetaHalo && this.Map.Current == Maps.Conservatory;
        public bool Plot_Conservatory => !this.SplitBools.Conservatory && this.Map.Old == Maps.Conservatory && this.Map.Current == Maps.ZetaHalo;
        public bool Plot_SpireApproach => !this.SplitBools.SpireApproach && this.Map.Old == Maps.ZetaHalo && this.Map.Current == Maps.Spire01;
        public bool Plot_SpireAdjutantResolution => !this.SplitBools.AdjutantResolution && this.Map.Current == Maps.ZetaHalo && this.Spire.Changed && this.Spire.Current == 10;
        public bool Plot_PelicanEast => !this.SplitBools.EastAAGun && this.Map.Current == Maps.ZetaHalo && this.PelicanSpartanKillers.Current != 10 && this.EastAAGun.Changed && this.EastAAGun.Current == 10;
        public bool Plot_PelicanNorth => !this.SplitBools.NorthAAGun && this.Map.Current == Maps.ZetaHalo && this.PelicanSpartanKillers.Current != 10 && this.NorthAAGun.Changed && this.NorthAAGun.Current == 10;
        public bool Plot_PelicanWest => !this.SplitBools.WestAAGun && this.Map.Current == Maps.ZetaHalo && this.PelicanSpartanKillers.Current != 10 && this.WestAAGun.Changed && this.WestAAGun.Current == 10;
        public bool Plot_SpartanKillers => !this.SplitBools.PelicanSpartanKillers && this.Map.Current == Maps.ZetaHalo && this.EastAAGun.Old == 10 && this.NorthAAGun.Old == 10 && this.WestAAGun.Old == 10 && this.PelicanSpartanKillers.Changed && this.PelicanSpartanKillers.Current == 10;
        public bool Plot_SequenceNorthernBeacon => !this.SplitBools.SequenceNorthernBeacon && this.Map.Current == Maps.ZetaHalo && this.SequenceNorthernBeacon.Changed && this.SequenceNorthernBeacon.Current == 10;
        public bool Plot_SequenceSouthernBeacon => !this.SplitBools.SequenceSouthernBeacon && this.Map.Current == Maps.ZetaHalo && this.SequenceSouthernBeacon.Changed && this.SequenceSouthernBeacon.Current == 10;
        public bool Plot_SequenceEasternBeacon => !this.SplitBools.SequenceEasternBeacon && this.Map.Current == Maps.ZetaHalo && this.SequenceEasternBeacon.Changed && this.SequenceEasternBeacon.Current == 10;
        public bool Plot_SequenceSouthwesternBeacon => !this.SplitBools.SequenceSouthwesternBeacon && this.Map.Current == Maps.ZetaHalo && this.SequenceSouthwesternBeacon.Changed && this.SequenceSouthwesternBeacon.Current == 10;
        public bool Plot_SequenceEnterCommandspire => !this.SplitBools.SequenceEnterCommandSpire && this.Map.Old == Maps.ZetaHalo && this.Map.Current == Maps.Nexus;
        public bool Plot_Nexus => !this.SplitBools.Nexus && this.Map.Old == Maps.Nexus && this.Map.Current == Maps.Spire02;
        public bool Plot_Spire2_Reach_Top => !this.SplitBools.Spire2_reach_top && this.Map.Old == Maps.Spire02 && this.Map.Current == Maps.ZetaHalo;
        public bool Plot_Spire2_Deactivate => !this.SplitBools.Spire2_deactivate && this.Map.Old == Maps.ZetaHalo && this.Map.Current == Maps.Repository;
        public bool Plot_Repository => !this.SplitBools.Repository && this.Map.Old == Maps.Repository && this.Map.Current == Maps.ZetaHalo;
        public bool Plot_Road => !this.SplitBools.Road && this.Map.Old == Maps.ZetaHalo && this.Map.Current == Maps.HouseOfReckoning;
        public bool Plot_HouseOfReckoning => !this.SplitBools.HouseOfReckoning && this.Map.Old == Maps.HouseOfReckoning && this.Map.Current == Maps.SilentAuditorium;
        public bool Plot_SilentAuditorium => !this.SplitBools.SilentAuditorium && this.Map.Current == Maps.SilentAuditorium && this.SilentAuditorium.Changed && this.SilentAuditorium.Current == 10;

        public Watchers()
        {
            game = Process.GetProcessesByName("HaloInfinite").OrderByDescending(x => x.StartTime).FirstOrDefault(x => !x.HasExited);
            if (game == null) throw new Exception("Couldn't connect to the game!");
			
            switch ((GameVersion)game.ModulesWow64Safe().Where(x => x.ModuleName == "Arbiter.dll").FirstOrDefault().ModuleMemorySize)
            {
                case GameVersion.v6_10020_19048_0:
                    this.LoadStatus = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x4FFDD04));
                    this.LoadStatusPercentage = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x4FFDD08));
                    this.StatusString = new StringWatcher(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x4CA11B0), 255);
                    this.LoadScreen = new MemoryWatcher<bool>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x47E73E0));
                    this.LoadingIcon = new MemoryWatcher<bool>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x522A6D0));
                    this.OutpostTremonius = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB5558));
                    this.FOBGolf = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB746C));
                    this.Tower = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB55B0));
                    this.TravelToDigSite = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB72BC));
                    this.Spire = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB5308));
                    this.EastAAGun = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB7344));
                    this.NorthAAGun = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB7354));
                    this.WestAAGun = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB7364));
                    this.PelicanSpartanKillers = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB7384));
                    this.SequenceNorthernBeacon = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB9370));
                    this.SequenceSouthernBeacon = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB9378));
                    this.SequenceEasternBeacon = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB9380));
                    this.SequenceSouthwesternBeacon = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB9388));
                    this.SilentAuditorium = new MemoryWatcher<byte>(new DeepPointer(game.MainModuleWow64Safe().BaseAddress + 0x482C908, 0xB740C));
                    break;

                default:
                    // If game version is not known, then try to find the memory addresses through signature scanning
                    IntPtr ptr;
                    SignatureScanner scanner;
                    Dictionary<string, bool> FoundVars = new Dictionary<string, bool>{
                        { "LoadStatusPercentage", false },
                        { "StatusString", false },
                        { "LoadScreen", false },
                        { "CampaignData", false }
                    };

                    Thread.Sleep(3000);
                    // Find the require memory addresses through sigScanning
                    foreach (var page in game.MemoryPages(true).Where(m => (long)m.BaseAddress >= (long)game.MainModuleWow64Safe().BaseAddress))
                    {
                        scanner = new SignatureScanner(game, page.BaseAddress, (int)page.RegionSize);

                        if (!FoundVars["LoadStatusPercentage"])
                        {
                            ptr = scanner.Scan(new SigScanTarget(2, "89 05 ???????? 48 81 C4 ???????? 41 5F"));
                            if (ptr != IntPtr.Zero)
                            {
                                this.LoadStatusPercentage = new MemoryWatcher<byte>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr)));
                                // If LoadStatus breaks in future game updates, we can scan for it using this alternative sigscan: 89 45 94 8B 05 ???????? 89 45 98
                                this.LoadStatus           = new MemoryWatcher<byte>(new DeepPointer(ptr + game.ReadValue<int>(ptr)));
                                this.LoadingIcon          = new MemoryWatcher<bool>(new DeepPointer(ptr + 4 + game.ReadValue<int>(ptr) + 0x22C9C8)); // Will probably break on next game update
                                FoundVars["LoadStatusPercentage"] = true;
                            }
                        }

                        if (!FoundVars["StatusString"])
                        {
                            ptr = scanner.Scan(new SigScanTarget(12, "00 00 00 00 00 00 00 00 00 00 00 00 6C 6F 61 64"));
                            if (ptr != IntPtr.Zero)
                            {
                                this.StatusString = new StringWatcher(new DeepPointer(ptr), 255);
                                FoundVars["StatusString"] = true;
                            }
                        }

                        if (!FoundVars["LoadScreen"])
                        {
                            ptr = scanner.Scan(new SigScanTarget(2, "80 3D ???????? 00 74 17 48 8D 0D ???????? E8 ???????? 84 C0"));
                            if (ptr != IntPtr.Zero)
                            {
                                this.LoadScreen = new MemoryWatcher<bool>(new DeepPointer(ptr + 5 + game.ReadValue<int>(ptr)));
                                FoundVars["LoadScreen"] = true;
                            }
                        }

                        if (!FoundVars["CampaignData"])
                        {
                            ptr = scanner.Scan(new SigScanTarget(3, "4C 8D 35 ???????? 48 8D 0D ???????? 66"));
                            if (ptr != IntPtr.Zero)
                            {
                                IntPtr address = ptr + 4 + game.ReadValue<int>(ptr);
                                this.OutpostTremonius           = new MemoryWatcher<byte>(new DeepPointer(address, 0xB5558));
                                this.FOBGolf                    = new MemoryWatcher<byte>(new DeepPointer(address, 0xB746C));
                                this.Tower                      = new MemoryWatcher<byte>(new DeepPointer(address, 0xB55B0));
                                this.TravelToDigSite            = new MemoryWatcher<byte>(new DeepPointer(address, 0xB72BC));
                                this.Spire                      = new MemoryWatcher<byte>(new DeepPointer(address, 0xB5308));
                                this.EastAAGun                  = new MemoryWatcher<byte>(new DeepPointer(address, 0xB7344));
                                this.NorthAAGun                 = new MemoryWatcher<byte>(new DeepPointer(address, 0xB7354));
                                this.WestAAGun                  = new MemoryWatcher<byte>(new DeepPointer(address, 0xB7364));
                                this.PelicanSpartanKillers      = new MemoryWatcher<byte>(new DeepPointer(address, 0xB7384));
                                this.SequenceNorthernBeacon     = new MemoryWatcher<byte>(new DeepPointer(address, 0xB9370));
                                this.SequenceSouthernBeacon     = new MemoryWatcher<byte>(new DeepPointer(address, 0xB9378));
                                this.SequenceEasternBeacon      = new MemoryWatcher<byte>(new DeepPointer(address, 0xB9380));
                                this.SequenceSouthwesternBeacon = new MemoryWatcher<byte>(new DeepPointer(address, 0xB9388));
                                this.SilentAuditorium           = new MemoryWatcher<byte>(new DeepPointer(address, 0xB740C));
                                FoundVars["CampaignData"] = true;
                            }
						}

						// Once we found all the required addresses, we can exit the loop
						if (FoundVars["LoadStatusPercentage"] && FoundVars["StatusString"] && FoundVars["LoadScreen"] && FoundVars["CampaignData"]) break;
					}
					
					// If, for some reason, we didn't find all the addresses, this code
					// will disable autosplitting functionality
					if (!FoundVars["LoadStatusPercentage"] || !FoundVars["StatusString"] || !FoundVars["LoadScreen"] || !FoundVars["CampaignData"])
					{
						Messages.UnsupportedGameVersion();
						IsAutosplitterEnabled = false;
						return;
					}
					break;	
			}

            this.AddRange(this.GetType().GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(p => !p.GetIndexParameters().Any()).Select(p => p.GetValue(this, null) as MemoryWatcher).Where(p => p != null));
        }

        public void Update()
        {
            this.UpdateAll(game);
        }
    }

    class FakeMemoryWatcher<T>
    {
        public T Current { get; set; }
        public T Old { get; set; }
        public bool Changed => !this.Old.Equals(this.Current);
        public FakeMemoryWatcher(T old, T current)
        {
            this.Old = old;
            this.Current = current;
        }
    }

    class SplitBools
    {
        public bool BanishedShip = false;
        public bool Foundation = false;
        public bool OutpostTremonius = false;
        public bool FOBGolf = false;
        public bool Tower = false;
        public bool TravelToDigSite = false;
        public bool Bassus = false;
        public bool Conservatory = false;
        public bool SpireApproach = false;
        public bool AdjutantResolution = false;
        public bool EastAAGun = false;
        public bool NorthAAGun = false;
        public bool WestAAGun = false;
        public bool PelicanSpartanKillers = false;
        public bool SequenceNorthernBeacon = false;
        public bool SequenceSouthernBeacon = false;
        public bool SequenceEasternBeacon = false;
        public bool SequenceSouthwesternBeacon = false;
        public bool SequenceEnterCommandSpire = false;
        public bool Nexus = false;
        public bool Spire2_reach_top = false;
        public bool Spire2_deactivate = false;
        public bool Repository = false;
        public bool Road = false;
        public bool HouseOfReckoning = false;
        public bool SilentAuditorium = false;
    }
}
