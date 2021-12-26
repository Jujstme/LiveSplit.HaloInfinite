using System.Windows.Forms;

namespace LiveSplit.HaloInfinite
{
    enum GameVersion
    {
        v6_10020_17952_0 = 0x1263000,
        v6_10020_19048_0 = 0x133F000
    }

    static class Maps
    {
        public const string BanishedShip = "dungeon_banished_ship";
        public const string Foundation = "dungeon_underbelly";
        public const string ZetaHalo = "island01";
        public const string Conservatory = "dungeon_forerunner_dallas";
        public const string Spire01 = "dungeon_spire_01";
        public const string Nexus = "dungeon_forerunner_houston";
        public const string Spire02 = "dungeon_spire_02";
        public const string Repository = "dungeon_forerunner_austin";
        public const string HouseOfReckoning = "dungeon_boss_hq_interior";
        public const string SilentAuditorium = "dungeon_cortana_palace";
    }
	
    enum SplitTrigger
    {
        BanishedShip,
        Foundation,
        OutpostTremonius,
        FOBGolf,
        Tower,
        TravelToDigSite,
        Bassus,
        Conservatory,
        SpireApproach,
        AdjutantResolution,
        EastAAGun,
        NorthAAGun,
        WestAAGun,
        PelicanSpartanKillers,
        SequenceNorthernBeacon,
        SequenceSouthernBeacon,
        SequenceEasternBeacon,
        SequenceSouthwesternBeacon,
        SequenceEnterCommandSpire,
        Nexus,
        Spire2_reach_top,
        Spire2_deactivate,
        Repository,
        Road,
        HouseOfReckoning,
        SilentAuditorium
    }

    static class Messages
    {
        public static void UnsupportedGameVersion()
        {
            MessageBox.Show("This game version is not currently supported by the autosplitter.\n\n" +
                            "Load time removal and autosplitting functionality will be disabled.",
                            "LiveSplit - Halo Infinite", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
