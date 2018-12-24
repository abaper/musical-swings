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
    }
}
