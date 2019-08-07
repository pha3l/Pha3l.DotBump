using System;
using System.IO;
using System.Linq;
using CommandLine;
using Pha3l.DotBump.Data;

namespace Pha3l.DotBump
{
    public class Program
    {
        [Verb("init", HelpText = "Initialize versioning for this project")]
        public class InitOptions
        {
            [Option('i', "initial-version", Required = false, Default = "1.0.0", HelpText = "The initial version")]
            public string InitialVersion { get; set; }
        }

        [Verb("bump", HelpText = "Bump the version")]
        public class BumpOptions
        {
            private const string SetName = "BumpOptions";
            
//            [Option('c', "component", Required = false, Default = VersionComponent.Patch, HelpText = "The semver version component to bump", SetName = SetName)]
//            public VersionComponent Component { get; set; }
            
            [Value(0, Required = false, Default = VersionComponent.Patch, HelpText = "The semver version component to bump")]
            public VersionComponent Component { get; set; }
            
            [Option('s', "set", Required = false, HelpText = "Set the version explicitly")]
            public string Version { get; set; }
            
            public enum VersionComponent
            {
                Major, Minor, Patch
            }
        }
        
        [Verb("set", HelpText = "Set the version")]
        public class SetOptions
        {
            [Value(0, Required = true, HelpText = "Set the exact version")]
            public string Version { get; set; }
        }
        
        public static int Main(string[] args)
        {
            return new Parser(with =>
                {
                    with.CaseInsensitiveEnumValues = true;
                    with.EnableDashDash = true;
                    with.CaseSensitive = true;
                })
                .ParseArguments<InitOptions, BumpOptions, SetOptions>(args)
                .MapResult(
                    (InitOptions opts) => HandleInit(opts),
                    (BumpOptions opts) => HandleBump(opts),
                    (SetOptions opts) => HandleSet(opts),
                    errs =>
                    {
                        foreach (var error in errs)
                        {
                            Console.WriteLine(error.ToString());
                        }

                        return 1;
                    });
        }

        private static int HandleSet(SetOptions options)
        {
            VersionFile versionFile = new VersionFile();
            versionFile.SetVersion(options.Version);
            versionFile.SerializeToFile();
            return 0;
        }

        private static int HandleInit(InitOptions options)
        {
            VersionFile versionFile = new VersionFile();
            versionFile.SetVersion(options.InitialVersion);
            versionFile.SerializeToFile();
            return 0;
        }
        
        private static int HandleBump(BumpOptions options)
        {
            VersionFile versionFile = VersionFile.Load();
            switch (options.Component)
            {
                case BumpOptions.VersionComponent.Major:
                    versionFile.BumpMajor();
                    break;
                case BumpOptions.VersionComponent.Minor:
                    versionFile.BumpMinor();
                    break;
                case BumpOptions.VersionComponent.Patch:
                    versionFile.BumpPatch();
                    break;
                default:
                    throw new ArgumentException("Must specify a component to bump");
            }
            
            versionFile.SerializeToFile();
            return 0;
        }
    }
}