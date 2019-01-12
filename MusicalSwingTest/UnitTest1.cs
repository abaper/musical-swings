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
        public void should_play_c_when_swing_at_85()
       {
            var playingDevice = new Mock<IPlayingDevice>();
            var mapper = new NoteMapper(90, playingDevice.Object);
            mapper.Map(0, 85, MovingDir.Left);
            playingDevice.Verify(c => c.PlayNote(Midi.Pitch.C0));
        }
        [TestMethod]
        public void should_play_nothing_below_85()
        {
            var playingDevice = new Mock<IPlayingDevice>();
            var mapper = new NoteMapper(90, playingDevice.Object);
            mapper.Map(0, 80, MovingDir.Left);
            playingDevice.Verify(c => c.PlayNote(Midi.Pitch.C0),Times.Never);
        }

        [TestMethod]
        public void should_play_sound_when_swings()
        {
            var detect = new SwingDetector(1, 5);
            detect.AddPosition(5);
            detect.AddPosition(4);
            detect.AddPosition(3);
            detect.AddPosition(2);
            detect.AddPosition(1);
            detect.AddPosition(2);
            Assert.AreEqual(MovingDir.Right, detect.Dir);
            
            detect.AddPosition(4);
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
            var swingGenerator = new Mock<INoteMapper>();
            var dispatch = new SwingDispatch(new []{1,2,3,4,5,6}, swingGenerator.Object);
            dispatch.AddPositions("1,90,2,222,3,333,4,555,5,122,6,112");
            dispatch.AddPositions("1,98,2,222,3,333,4,555,5,122,6,112");
            dispatch.AddPositions("1,85,2,224,3,333,4,555,5,122,6,112");
            dispatch.AddPositions("1,70,2,222,3,333,4,555,5,122,6,111");
            dispatch.AddPositions("1,65,2,222,3,333,4,555,5,122,6,112");
            dispatch.AddPositions("1,40,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,41,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,50,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,60,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,80,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,70,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,60,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,50,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,39,2,222,3,333,4,590,5,122,6,112");
            dispatch.AddPositions("1,45,2,222,3,333,4,590,5,122,6,112");
            swingGenerator.Verify(c=>c.Map(1, 40, It.IsAny<MovingDir>()),Times.Once);
            swingGenerator.Verify(c => c.Map(1, 39, It.IsAny<MovingDir>()), Times.Once);
            swingGenerator.Verify(c => c.Map(6, 111, It.IsAny<MovingDir>()), Times.Once);
        }
    
}
}
