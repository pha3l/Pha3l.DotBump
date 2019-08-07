using System.IO;

namespace Pha3l.DotBump.Data
{
    public class VersionFile
    {
        public const string VersionFileName = ".version";
        public int MajorVersion { get; set; }
        
        public int MinorVersion { get; set; }
        
        public int PatchVersion { get; set; }

        public static VersionFile Load()
        {
            string contents = File.ReadAllText(VersionFileName);

            VersionFile versionFile = new VersionFile();
            versionFile.SetVersion(contents);
            return versionFile;
        }

        public void BumpMajor()
        {
            MajorVersion += 1;
            MinorVersion = 0;
            PatchVersion = 0;
        }

        public void BumpMinor()
        {
            MinorVersion += 1;
            PatchVersion = 0;
        }

        public void BumpPatch()
        {
            PatchVersion += 1;
        }

        public void SetVersion(string versionString)
        {
            SetVersionFromString(versionString);
        }

        private void SetVersionFromString(string versionString)
        {
            string[] parts = versionString.Split('.');
            
            if (!int.TryParse(parts[0], out int majorVersion))
                throw new InvalidDataException($"Could not parse version string: {versionString}");
            if (!int.TryParse(parts[1], out int minorVersion))
                throw new InvalidDataException($"Could not parse version string: {versionString}");
            if (!int.TryParse(parts[2], out int patchVersion))
                throw new InvalidDataException($"Could not parse version string: {versionString}");

            MajorVersion = majorVersion;
            MinorVersion = minorVersion;
            PatchVersion = patchVersion;
        }

        public void SerializeToFile()
        {
            File.WriteAllText(".version", string.Join(".", MajorVersion, MinorVersion, PatchVersion));
        }
    }
}