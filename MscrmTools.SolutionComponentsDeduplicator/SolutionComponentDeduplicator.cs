using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using XrmToolBox.Extensibility;
using XrmToolBox.Extensibility.Interfaces;

namespace MscrmTools.SolutionComponentsDeduplicator
{
    // Do not forget to update version number and author (company attribute) in AssemblyInfo.cs class
    // To generate Base64 string for Images below, you can use https://www.base64-image.de/
    [Export(typeof(IXrmToolBoxPlugin)),
        ExportMetadata("Name", "Solution Components Deduplicator"),
        ExportMetadata("Description", "A tool to identify components present in multiple unmanaged solutions in order to keep them in only one solution"),
        // Please specify the base64 content of a 32x32 pixels image
        ExportMetadata("SmallImageBase64", "iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAMAAABEpIrGAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAHyUExURV9fYF5eYGBgYFBQZxUVhhQUhhERhzw8ckxMagAAkTIyd0xMaQEBkDMzdgAAkF1dZWVli2pqkGlpj2lpkWtrfWlpaezs7P///7y8vOnp6bu7u+7u7r6+vmZmZri4uMfHx8XFxcrKypqamllZYyQkfhwcgh0dgiAdgUgoemwxc3c0cWcwdD8lexsbgzY2dVZWZAYGjT8Wgq49ad5OX+BPX9lMYJw3bikOhyEhfwkJjGEie9FJYkAWgiMjfkMXgd9FWN9AVOBOXuBQX+BLXN8/U+BJWs5IYiQMiAcCj7Q/aN9GWOmYnu66vd9DVuFcafDDxuWAiOBIWpAycDsVg99BVfHHyvDAw+FXZfXX2eyqrxsJigcHjWgkeeBMXeBTYvPP0f/+/vrs7e65vN9FVz4VgyIif1xcYUdHdkZGekZGeZdKa+BQYOFfbPvx8vff4d88UXlIcFFRcsvLy/X19fHx8emmruFbaPXW2Pnm5/DBxOBJW+/Hy9fX1/fS1t9BVPLKze63u/PQ0uytst8+U+JdbPvs7tXV1f76+uZ1gt9HWeeQlu2wtO+5veR5guuSnPbQ1OBSYt89UuNicPrn6fO8wuBTY+NgbvfV2ffW2eh/iuFVZOFZaOuOmPrm6MbGxu3t7ejo6OfN0OW0ueWssua4vefT1bPJQC4AAAAJcEhZcwAADsMAAA7DAcdvqGQAAAAYdEVYdFNvZnR3YXJlAFBhaW50Lk5FVCA1LjEuOWxu2j4AAAC2ZVhJZklJKgAIAAAABQAaAQUAAQAAAEoAAAAbAQUAAQAAAFIAAAAoAQMAAQAAAAIAAAAxAQIAEAAAAFoAAABphwQAAQAAAGoAAAAAAAAAYAAAAAEAAABgAAAAAQAAAFBhaW50Lk5FVCA1LjEuOQADAACQBwAEAAAAMDIzMAGgAwABAAAAAQAAAAWgBAABAAAAlAAAAAAAAAACAAEAAgAEAAAAUjk4AAIABwAEAAAAMDEwMAAAAABMz8BIJY/XoAAAAXdJREFUOE9jYGDEBxiYGJhZWHEDNnYmBg5OfICLiYGbB10QGfDSSwEfTgBSwC8gKIQTCIswMYiKieMBEkwMopLogshAij4KpNEFkYEME4OsnLwCTqCoxMSgrKKqhgbUNTS1tHVALF09JgZ9A/TgNTQyNjExMTUz5+TktGBi0LdEk7cCSYOAtQ0npy2mAjsTE3sHRydnF1cnN3csCjw8Tby8fbx8/fwDAk2CsCgINjEJCRUPC48Qj3QxsY4CKYhGURBjYhIbF5+QmJCU7GhikpLKxJCWnpGJBLJMTLIdcnLF8/JdTUxMCgqZGJiKikuQQKmJiXNZeUKFeGWVo4lJdRETA1MNStjWmpjU1Ys3VDWKNzWbtLTWMDEwtaEoaO8w6ezq9vUK7+mtMukTb8NQIN4/wcnLxdEpdmKV06TJ2BSIT5kKDclp08XBClDdIC4uPmPmLBMTk9lz5oqLi4PcMG++JBpYsHDR4iVLQaz585gYmGRF8QBZJgDgboZbGctcfQAAAABJRU5ErkJggg=="),
        // Please specify the base64 content of a 80x80 pixels image
        ExportMetadata("BigImageBase64", "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"),
        ExportMetadata("BackgroundColor", "#606060"),
        ExportMetadata("PrimaryFontColor", "White"),
        ExportMetadata("SecondaryFontColor", "White")]
    public class SolutionComponentDeduplicator : PluginBase, IPayPalPlugin
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SolutionComponentDeduplicator()
        {
            // If you have external assemblies that you need to load, uncomment the following to
            // hook into the event that will fire when an Assembly fails to resolve
            // AppDomain.CurrentDomain.AssemblyResolve += new ResolveEventHandler(AssemblyResolveEventHandler);
        }

        public string DonationDescription => "Donation for Solution Components Deduplicator";

        public string EmailAccount => "tanguy92@hotmail.com";

        public override IXrmToolBoxPluginControl GetControl()
        {
            return new SolutionComponentDeduplicatorControl();
        }

        /// <summary>
        /// Event fired by CLR when an assembly reference fails to load
        /// Assumes that related assemblies will be loaded from a subfolder named the same as the Plugin
        /// For example, a folder named Sample.XrmToolBox.MyPlugin
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        private Assembly AssemblyResolveEventHandler(object sender, ResolveEventArgs args)
        {
            Assembly loadAssembly = null;
            Assembly currAssembly = Assembly.GetExecutingAssembly();

            // base name of the assembly that failed to resolve
            var argName = args.Name.Substring(0, args.Name.IndexOf(","));

            // check to see if the failing assembly is one that we reference.
            List<AssemblyName> refAssemblies = currAssembly.GetReferencedAssemblies().ToList();
            var refAssembly = refAssemblies.Where(a => a.Name == argName).FirstOrDefault();

            // if the current unresolved assembly is referenced by our plugin, attempt to load
            if (refAssembly != null)
            {
                // load from the path to this plugin assembly, not host executable
                string dir = Path.GetDirectoryName(currAssembly.Location).ToLower();
                string folder = Path.GetFileNameWithoutExtension(currAssembly.Location);
                dir = Path.Combine(dir, folder);

                var assmbPath = Path.Combine(dir, $"{argName}.dll");

                if (File.Exists(assmbPath))
                {
                    loadAssembly = Assembly.LoadFrom(assmbPath);
                }
                else
                {
                    throw new FileNotFoundException($"Unable to locate dependency: {assmbPath}");
                }
            }

            return loadAssembly;
        }
    }
}