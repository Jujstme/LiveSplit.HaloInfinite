using LiveSplit.HaloInfinite;
using LiveSplit.Model;
using LiveSplit.UI.Components;
using System;
using System.Reflection;

[assembly: ComponentFactory(typeof(Factory))]

namespace LiveSplit.HaloInfinite
{
    public class Factory : IComponentFactory
    {
        public string ComponentName => "Halo Infinite - Autosplitter";
        public string Description => "Automatic splitting and load removal";
        public ComponentCategory Category => ComponentCategory.Control;
        public string UpdateName => this.ComponentName;
        public string UpdateURL => "https://raw.githubusercontent.com/Jujstme/LiveSplit.HaloInfinite/master/";
        public Version Version => Assembly.GetExecutingAssembly().GetName().Version;
        public string XMLURL => this.UpdateURL + "Components/update.LiveSplit.HaloInfinite.xml";
        public IComponent Create(LiveSplitState state) { return new Component(state); }
    }
}
