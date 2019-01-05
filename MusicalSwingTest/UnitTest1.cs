using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
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
                    var mapper = new NoteMappnig(eventObject, new List<int>() {5, 10}, 1, 1);
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
            var detect = new SwingDetector(1, 5);
            detect.AddPosition(1);
            detect.AddPosition(2);
            detect.AddPosition(3);
            detect.AddPosition(4);
            detect.AddPosition(5);
            detect.AddPosition(6);
            detect.AddPosition(5);
            Assert.AreEqual(MovingDir.Left, detect.Dir);
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
            Assert.AreEqual(MovingDir.Left, detect.Dir);
            detect.AddPosition(3);
            detect.AddPosition(3);
            detect.AddPosition(3);
            detect.AddPosition(3);
            detect.AddPosition(3);
            detect.AddPosition(3);
            Assert.AreEqual(MovingDir.None, detect.Dir);
        }
        [TestMethod]
        public void should_dispatch_positons()
        {
            var swingGenerator = new Mock<ISoundGenerator>();
            var dispatch = new SwingDispatch(new []{1,2,3,4,5,6}, swingGenerator.Object);
            dispatch.AddPositions("1,323,2,222,3,333,4,555,5,122,6,112");
            dispatch.AddPositions("1,324,2,222,3,333,4,555,5,122,6,112");
            dispatch.AddPositions("1,325,2,224,3,333,4,555,5,122,6,112");
            dispatch.AddPositions("1,326,2,222,3,333,4,555,5,122,6,111");
            dispatch.AddPositions("1,327,2,222,3,333,4,555,5,122,6,112");
            dispatch.AddPositions("1,326,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,325,2,222,3,333,4,555,5,122,6,112");
            dispatch.AddPositions("1,324,2,222,3,333,4,555,5,122,6,112");
            swingGenerator.Verify(c=>c.PlaySound(1, 327, It.IsAny<MovingDir>()),Times.Once);
            swingGenerator.Verify(c => c.PlaySound(2, 224, It.IsAny<MovingDir>()), Times.Once);
            swingGenerator.Verify(c => c.PlaySound(4, 590, It.IsAny<MovingDir>()), Times.Once);
            swingGenerator.Verify(c => c.PlaySound(6, 111, MovingDir.Left), Times.Once);
        }
    
}
}
