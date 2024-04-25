using Microsoft.VisualStudio.TestTools.UnitTesting;
using AGVROSEmulator.AGVRos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AGVROSEmulator.AGVRos.Tests
{
    [TestClass()]
    public class AGVEmuTests
    {
        [TestMethod()]
        public void InitializeTest()
        {
            AGVEmu emu = new AGVEmu();
            emu.Initialize();
        }
    }
}