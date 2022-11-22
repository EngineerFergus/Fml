using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fml.Tests
{
    [TestClass]
    public class FmlMapTests
    {
        private readonly string _globalInputs = "version = 5 \n" +
            "framework = dotnet 6\n" +
            " database =  sqlite \n";

        private FmlMap MapInput(string input)
        {
            using TextReader textReader = new StringReader(input);
            return FmlMap.ReadStream(textReader);
        }

        [TestMethod]
        public void MapsGlobals_Length()
        {
            FmlMap map = MapInput(_globalInputs);
            Assert.AreEqual(3, map.GlobalPairs.Count);
        }

        [TestMethod]
        public void MapsGlobals_Pairs()
        {
            FmlMap map = MapInput(_globalInputs);

            KeyValuePair<string, string>[] pairs =
            {
                new KeyValuePair<string, string>("version", "5"),
                new KeyValuePair<string, string>("framework", "dotnet 6"),
                new KeyValuePair<string, string>("database", "sqlite"),
            };

            foreach (KeyValuePair<string, string> pair in pairs)
            {
                Assert.IsTrue(map.GlobalPairs.Contains(pair));
            }
        }

        [TestMethod]
        public void MapsGlobals_NoSections()
        {
            FmlMap map = MapInput(_globalInputs);
            Assert.AreEqual(0, map.Sections.Count);
        }

        private readonly string _sectionInputs = "[app]\n" +
            "version = 5\n" +
            "database = sqlite\n" +
            "[database]\n" +
            "version = 0.1\n" +
            "type = sqlite\n";

        [TestMethod]
        public void MapsSections_NumSections()
        {
            FmlMap map = MapInput(_sectionInputs);
            Assert.AreEqual(2, map.Sections.Count);
        }

        [TestMethod]
        public void MapsSections_NumPerSection()
        {
            FmlMap map = MapInput(_sectionInputs);
            foreach(var section in map.Sections)
            {
                Assert.AreEqual(2, section.Value.Count);
            }
        }

        [TestMethod]
        public void MapsSections_NoGlobals()
        {
            FmlMap map = MapInput(_sectionInputs);
            Assert.AreEqual(0, map.GlobalPairs.Count);
        }

        [TestMethod]
        public void MapsSections_SectionNames()
        {
            FmlMap map = MapInput(_sectionInputs);
            string[] sectionNames = { "app", "database" };

            foreach (string sectionName in sectionNames)
            {
                Assert.IsTrue(map.Sections.ContainsKey(sectionName));
            }
        }

        [TestMethod]
        public void MapsSections_AppSectionPairs()
        {
            FmlMap map = MapInput(_sectionInputs);
            KeyValuePair<string, string>[] sectionPairs =
            {
                new KeyValuePair<string, string>("version", "5"),
                new KeyValuePair<string, string>("database", "sqlite")
            };

            var section = map.Sections["app"];

            foreach (var pair in sectionPairs)
            {
                Assert.IsTrue(section.Contains(pair));
            }
        }

        [TestMethod]
        public void MapsSections_DatabaseSectionPairs()
        {
            FmlMap map = MapInput(_sectionInputs);
            KeyValuePair<string, string>[] sectionPairs =
            {
                new KeyValuePair<string, string>("version", "0.1"),
                new KeyValuePair<string, string>("type", "sqlite")
            };

            var section = map.Sections["database"];

            foreach (var pair in sectionPairs)
            {
                Assert.IsTrue(section.Contains(pair));
            }
        }

        [TestMethod]
        public void MapsGlobalsAndSections_GlobalCount()
        {
            FmlMap map = MapInput(_globalInputs + _sectionInputs);

            Assert.AreEqual(3, map.GlobalPairs.Count);
        }

        [TestMethod]
        public void MapsGlobalsAndSections_SectionCount()
        {
            FmlMap map = MapInput(_globalInputs + _sectionInputs);

            foreach (var section in map.Sections)
            {
                Assert.AreEqual(2, section.Value.Count);
            }
        }

        [TestMethod]
        public void ThrowExceptionWithRepeatedGlobal()
        {
            string input = _globalInputs + "version = 6";

            Assert.ThrowsException<Exception>(() => _ = MapInput(input));
        }

        [TestMethod]
        public void ThrowsExceptionWithRepeatedSection()
        {
            string input = _sectionInputs + "type = mySQL";

            Assert.ThrowsException<Exception>(() => _ = MapInput(input));
        }
    }
}
