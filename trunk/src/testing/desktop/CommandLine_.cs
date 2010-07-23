using System;
using System.Collections.Generic;
using System.Text;
using Nohros.Desktop;
using NUnit.Framework;

namespace Nohros.Test.Desktop
{
    [TestFixture]
    public class CommandLine_
    {
        const string kTestCommandLine = "nohros.exe loose_00 loose_01 --switch_00=00 --switch_01 -switch_02=02 --switch_03:J:\\sex\\bigass.avi";

        [Test]
        public void CommandLine_ctor()
        {
            CommandLine command_line = new CommandLine("nohros.exe");
            Assert.AreEqual("nohros.exe", command_line.Program);
        }

        [Test]
        public void CommandLine_ForCurrentProcess() {
            CommandLine command_line = CommandLine.ForCurrentProcess;
            Assert.AreEqual(Environment.CommandLine, command_line.CommandLineString);
        }

        [Test]
        public void CommandLine_LooseValues() {
            CommandLine command_line = new CommandLine("nohros.exe");
            command_line.ParseFromString(kTestCommandLine);
            Assert.AreEqual(2, command_line.LooseValues.Count);
            Assert.AreEqual("loose_00", command_line.LooseValues[0]);
            Assert.AreEqual("loose_01", command_line.LooseValues[1]);
        }

        [Test]
        public void CommandLine_GetSwitchValue() {
            CommandLine command_line = new CommandLine("nohros.exe");
            command_line.ParseFromString(kTestCommandLine);

            Assert.AreEqual("00", command_line.GetSwitchValue("switch_00"));
            Assert.AreEqual(string.Empty, command_line.GetSwitchValue("switch_01"));
            Assert.AreEqual("02", command_line.GetSwitchValue("switch_02"));
            Assert.AreEqual("J:\\sex\\bigass.avi", command_line.GetSwitchValue("switch_03"));
        }

        [Test]
        public void CommandLine_Fail() {
            CommandLine command_line = new CommandLine("nohros.exe");

            command_line.ParseFromString("nohros.exe \"");
            Assert.AreEqual(command_line.Program, "nohros.exe");
            Assert.AreEqual(1, command_line.LooseValues.Count);

            command_line.Reset();
            command_line.ParseFromString("nohros.exe \"galo doido --switch_00 /switch_01");
            Assert.AreEqual(command_line.Program, "nohros.exe");
            Assert.AreEqual(1, command_line.LooseValues.Count);

            command_line.Reset();
            command_line.ParseFromString("nohros.exe \"galo doido\" --switch_00 /switch_01");
            Assert.AreEqual(command_line.Program, "nohros.exe");
            Assert.AreEqual(1, command_line.LooseValues.Count);
        }
    }
}