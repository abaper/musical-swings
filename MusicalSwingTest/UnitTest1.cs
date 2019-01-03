using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using WpfMusicalSwingPlayer;

namespace MusicalSwingTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            using (var file = File.OpenText(@"..\..\Data\swing samples.txt"))
            {
                while (true)
                {
                    var line = file.ReadLine();
                    if (string.IsNullOrEmpty(line))
                        break;
                    var eventObject = JsonConvert.DeserializeObject<SwingEvent>(line);
                    var mapper = new NoteMappnig(eventObject,new List<int>() { 5, 10} ,1,1);
                    if (mapper.IsPlayable)
                    {
                        Console.WriteLine(mapper.GetNote());
                    }
                    else
                    {
                        Console.WriteLine("not playable");
                    }
                }
                
            }
          //  var mapping = new NoteMappnig()
        }
        [TestMethod]
        public void should_play_sound_when_swings()
        {
            var detect = new SwingDetector(5);
            detect.AddPosition(1);
            detect.AddPosition(2);
            detect.AddPosition(3);
            detect.AddPosition(4);
            detect.AddPosition(5);
            detect.AddPosition(6);
            detect.AddPosition(5);
            Assert.AreEqual(MovingDir.Left,detect.Dir);
            detect.AddPosition(4);
            detect.AddPosition(3);
            detect.AddPosition(2);
            detect.AddPosition(1);
            detect.AddPosition(2);
            Assert.AreEqual(MovingDir.Right, detect.Dir);
            detect.AddPosition(3);
            detect.AddPosition(3);
            detect.AddPosition(3);
            detect.AddPosition(3);
            detect.AddPosition(3);
            detect.AddPosition(3);
            detect.AddPosition(4);
            detect.AddPosition(5);
            detect.AddPosition(4);
            detect.AddPosition(3);
            Assert.AreEqual(MovingDir.Left, detect.Dir);
        }
    }
}
