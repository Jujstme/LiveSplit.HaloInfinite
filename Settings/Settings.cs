using System;
using System.Xml;
using System.Windows.Forms;

namespace LiveSplit.HaloInfinite
{
    public partial class Settings : UserControl
    {
        public bool RunStart { get; set; }
        public bool BanishedShip { get; set; }
        public bool Foundation { get; set; }
        public bool OutpostTremonius { get; set; }
        public bool FOBGolf { get; set; }
        public bool Tower { get; set; }
        public bool ReachTheDigSite { get; set; }
        public bool Bassus { get; set; }
        public bool Conservatory { get; set; }
        public bool ApproachTheCommandSpire { get; set; }
        public bool AdjutantResolution { get; set; }
        public bool EastAAGun { get; set; }
        public bool NorthAAGun { get; set; }
        public bool WestAAGun { get; set; }
        public bool SpartanKillers { get; set; }
        public bool SequenceEasternBeacon { get; set; }
        public bool SequenceSouthernBeacon { get; set; }
        public bool SequenceNorthernBeacon { get; set; }
        public bool SequenceSouthwesternBeacon { get; set; }
        public bool EnterTheNexus { get; set; }
        public bool Nexus { get; set; }
        public bool ReachTheTop { get; set; }
        public bool DeactivateTheCommandSpire { get; set; }
        public bool Repository { get; set; }
        public bool Road { get; set; }
        public bool HouseOfReckoning { get; set; }
        public bool SilentAuditorium { get; set; }

        public Settings()
        {
            InitializeComponent();

            // General settings
            this.chkRunStart.DataBindings.Add("Checked", this, "RunStart", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkBanishedShip.DataBindings.Add("Checked", this, "BanishedShip", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkFoundation.DataBindings.Add("Checked", this, "Foundation", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkOutpostTremonius.DataBindings.Add("Checked", this, "OutpostTremonius", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkFOBGolf.DataBindings.Add("Checked", this, "FOBGolf", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkTower.DataBindings.Add("Checked", this, "Tower", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkReachTheDigSite.DataBindings.Add("Checked", this, "ReachTheDigSite", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkBassus.DataBindings.Add("Checked", this, "Bassus", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkConservatory.DataBindings.Add("Checked", this, "Conservatory", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkApproachSpire.DataBindings.Add("Checked", this, "ApproachTheCommandSpire", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkAdjutantResolution.DataBindings.Add("Checked", this, "AdjutantResolution", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkEastAAGun.DataBindings.Add("Checked", this, "EastAAGun", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkNorthAAGun.DataBindings.Add("Checked", this, "NorthAAGun", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkWestAAGun.DataBindings.Add("Checked", this, "WestAAGun", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSpartanKillers.DataBindings.Add("Checked", this, "SpartanKillers", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSequenceEasternBeacon.DataBindings.Add("Checked", this, "SequenceEasternBeacon", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSequenceSouthernBeacon.DataBindings.Add("Checked", this, "SequenceSouthernBeacon", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSequenceNorthernBeacon.DataBindings.Add("Checked", this, "SequenceNorthernBeacon", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSequenceSouthwesternBeacon.DataBindings.Add("Checked", this, "SequenceSouthwesternBeacon", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkEnterTheNexus.DataBindings.Add("Checked", this, "EnterTheNexus", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkNexus.DataBindings.Add("Checked", this, "Nexus", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkReachTheTop.DataBindings.Add("Checked", this, "ReachTheTop", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkDeactivateTheCommandSpire.DataBindings.Add("Checked", this, "DeactivateTheCommandSpire", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkRepository.DataBindings.Add("Checked", this, "Repository", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkRoad.DataBindings.Add("Checked", this, "Road", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkHouseOfReckoning.DataBindings.Add("Checked", this, "HouseOfReckoning", false, DataSourceUpdateMode.OnPropertyChanged);
            this.chkSilentAuditorium.DataBindings.Add("Checked", this, "SilentAuditorium", false, DataSourceUpdateMode.OnPropertyChanged);

            // Default Values
            this.RunStart = true;
            this.BanishedShip = true;
            this.Foundation = true;
            this.OutpostTremonius = true;
            this.FOBGolf = true;
            this.Tower = true;
            this.ReachTheDigSite = true;
            this.Bassus = true;
            this.Conservatory = true;
            this.ApproachTheCommandSpire = true;
            this.AdjutantResolution = true;
            this.EastAAGun = true;
            this.NorthAAGun = true;
            this.WestAAGun = true;
            this.SpartanKillers = true;
            this.SequenceEasternBeacon = true;
            this.SequenceSouthernBeacon = true;
            this.SequenceNorthernBeacon = true;
            this.SequenceSouthwesternBeacon = true;
            this.EnterTheNexus = true;
            this.Nexus = true;
            this.ReachTheTop = true;
            this.DeactivateTheCommandSpire = true;
            this.Repository = true;
            this.Road = true;
            this.HouseOfReckoning = true;
            this.SilentAuditorium = true;
        }

        public XmlNode GetSettings(XmlDocument doc)
        {
            XmlElement settingsNode = doc.CreateElement("settings");
            settingsNode.AppendChild(ToElement(doc, "RunStart", this.RunStart));
            settingsNode.AppendChild(ToElement(doc, "BanishedShip", this.BanishedShip));
            settingsNode.AppendChild(ToElement(doc, "Foundation", this.Foundation));
            settingsNode.AppendChild(ToElement(doc, "OutpostTremonius", this.OutpostTremonius));
            settingsNode.AppendChild(ToElement(doc, "FOBGolf", this.FOBGolf));
            settingsNode.AppendChild(ToElement(doc, "Tower", this.Tower));
            settingsNode.AppendChild(ToElement(doc, "ReachTheDigSite", this.ReachTheDigSite));
            settingsNode.AppendChild(ToElement(doc, "Bassus", this.Bassus));
            settingsNode.AppendChild(ToElement(doc, "Conservatory", this.Conservatory));
            settingsNode.AppendChild(ToElement(doc, "ApproachTheCommandSpire", this.ApproachTheCommandSpire));
            settingsNode.AppendChild(ToElement(doc, "AdjutantResolution", this.AdjutantResolution));
            settingsNode.AppendChild(ToElement(doc, "EastAAGun", this.EastAAGun));
            settingsNode.AppendChild(ToElement(doc, "NorthAAGun", this.NorthAAGun));
            settingsNode.AppendChild(ToElement(doc, "WestAAGun", this.WestAAGun));
            settingsNode.AppendChild(ToElement(doc, "SpartanKillers", this.SpartanKillers));
            settingsNode.AppendChild(ToElement(doc, "SequenceEasternBeacon", this.SequenceEasternBeacon));
            settingsNode.AppendChild(ToElement(doc, "SequenceSouthernBeacon", this.SequenceSouthernBeacon));
            settingsNode.AppendChild(ToElement(doc, "SequenceNorthernBeacon", this.SequenceNorthernBeacon));
            settingsNode.AppendChild(ToElement(doc, "SequenceSouthwesternBeacon", this.SequenceSouthwesternBeacon));
            settingsNode.AppendChild(ToElement(doc, "EnterTheNexus", this.EnterTheNexus));
            settingsNode.AppendChild(ToElement(doc, "Nexus", this.Nexus));
            settingsNode.AppendChild(ToElement(doc, "ReachTheTop", this.ReachTheTop));
            settingsNode.AppendChild(ToElement(doc, "DeactivateTheCommandSpire", this.DeactivateTheCommandSpire));
            settingsNode.AppendChild(ToElement(doc, "Repository", this.Repository));
            settingsNode.AppendChild(ToElement(doc, "Road", this.Road));
            settingsNode.AppendChild(ToElement(doc, "HouseOfReckoning", this.HouseOfReckoning));
            settingsNode.AppendChild(ToElement(doc, "SilentAuditorium", this.SilentAuditorium));
            return settingsNode;
        }

        public void SetSettings(XmlNode settings)
        {
            this.RunStart = ParseBool(settings, "RunStart", true);
            this.BanishedShip = ParseBool(settings, "BanishedShip", true);
            this.Foundation = ParseBool(settings, "Foundation", true);
            this.OutpostTremonius = ParseBool(settings, "OutpostTremonius", true);
            this.FOBGolf = ParseBool(settings, "FOBGolf", true);
            this.Tower = ParseBool(settings, "Tower", true);
            this.ReachTheDigSite = ParseBool(settings, "ReachTheDigSite", true);
            this.Bassus = ParseBool(settings, "Bassus", true);
            this.Conservatory = ParseBool(settings, "Conservatory", true);
            this.ApproachTheCommandSpire = ParseBool(settings, "ApproachTheCommandSpire", true);
            this.AdjutantResolution = ParseBool(settings, "AdjutantResolution", true);
            this.EastAAGun = ParseBool(settings, "EastAAGun", true);
            this.NorthAAGun = ParseBool(settings, "NorthAAGun", true);
            this.WestAAGun = ParseBool(settings, "WestAAGun", true);
            this.SpartanKillers = ParseBool(settings, "SpartanKillers", true);
            this.SequenceEasternBeacon = ParseBool(settings, "SequenceEasternBeacon", true);
            this.SequenceSouthernBeacon = ParseBool(settings, "SequenceSouthernBeacon", true);
            this.SequenceNorthernBeacon = ParseBool(settings, "SequenceNorthernBeacon", true);
            this.SequenceSouthwesternBeacon = ParseBool(settings, "SequenceSouthwesternBeacon", true);
            this.EnterTheNexus = ParseBool(settings, "EnterTheNexus", true);
            this.Nexus = ParseBool(settings, "Nexus", true);
            this.ReachTheTop = ParseBool(settings, "ReachTheTop", true);
            this.DeactivateTheCommandSpire = ParseBool(settings, "DeactivateTheCommandSpire", true);
            this.Repository = ParseBool(settings, "Repository", true);
            this.Road = ParseBool(settings, "Road", true);
            this.HouseOfReckoning = ParseBool(settings, "HouseOfReckoning", true);
            this.SilentAuditorium = ParseBool(settings, "SilentAuditorium", true);
        }

        static bool ParseBool(XmlNode settings, string setting, bool default_ = false)
        {
            bool val;
            return settings[setting] != null ? (Boolean.TryParse(settings[setting].InnerText, out val) ? val : default_) : default_;
        }

        static XmlElement ToElement<T>(XmlDocument document, string name, T value)
        {
            XmlElement str = document.CreateElement(name);
            str.InnerText = value.ToString();
            return str;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://haloruns.com/discord");
        }
    }
}
