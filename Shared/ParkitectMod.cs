using System;

namespace Shared
{
    public abstract class ParkitectMod : AbstractMod
    {
        public const int LatestKnownVersion = 14290343;
        
        protected abstract int SupportedAppBuildId { get; }
        protected abstract Version Version { get; }
        protected abstract string Name { get; }
        protected abstract string Description { get; }

        public override string getName()
        {
            return this.Name;
        }

        public override string getDescription()
        {
            return this.Description;
        }

        public override string getVersionNumber()
        {
            var rev = this.Version.Revision == -1 ? 0 : this.Version.Revision;
            
            return
                $"{this.Version.Major}.{this.Version.Minor}.{rev}";
        }

        public override string getIdentifier()
        {
            return $"com.bengreenier.{this.Name.Replace(" ", "-").ToLower()}";
        }

        public bool IsSupported()
        {
            return Steamworks.SteamApps.GetAppBuildId() == LatestKnownVersion;
        }
    }
}