using HandPositionReader.Scripts;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.TestTools;

namespace HandPositionReader.Tests.Unit
{
    public class FileReaderTest
    {
        private readonly string _mockPath;

        public FileReaderTest()
        {
            _mockPath = string.Format("{0}/Tests/MockAsset", Application.dataPath);
        }

        #region ToVector3
        [Test]
        public void When_ToVector3_With_Scale1()
        {
            Vector3 result = FileReader.ToVector3("3,0|2,9|3,2", '|', 1);

            Assert.AreEqual(new Vector3(3, 2.9f, 3.2f), result);
        }

        [Test]
        public void When_ToVector3_With_Scale10()
        {
            Vector3 result = FileReader.ToVector3("3,0|2,9|3,2", '|', 10);

            Assert.AreEqual(new Vector3(30, 29, 32), result);
        }

        [Test]
        public void When_ToVector3_With_DotSeparator()
        {
            Vector3 result = FileReader.ToVector3("3,0.2,9.3,2", '.', 1);

            Assert.AreEqual(new Vector3(3, 2.9f, 3.2f), result);
        }

        [Test]
        public void When_ToVector3_With_BadSeparator()
        {
            Vector3 result = FileReader.ToVector3("3,0|2,9|3,2", '.', 1);

            Assert.AreEqual(Vector3.zero, result);
        }

        [Test]
        public void When_ToVector3_When_CantConvert()
        {
            LogAssert.Expect(LogType.Error, @"Index was outside the bounds of the array.");
            Vector3 result = FileReader.ToVector3("3,0|2,9", '|', 1);
            LogAssert.NoUnexpectedReceived();

            Assert.AreEqual(Vector3.zero, result);
        }
        #endregion

        #region GetHandJoint
        [Test]
        public void When_GetHandJoint_Read_Position1of2()
        {
            string fileContent = ReadFile("Hand_Positions_Mock");
            List<Vector3> handJoint = FileReader.GetHandJoint(fileContent, 0, 1);

            List<Vector3> handJointExpected = new List<Vector3>
            {
                new Vector3(1,2,1),
                new Vector3(2,1,2),
                new Vector3(-0.3f,2.0f,-0.001f),
                new Vector3(0.3f,-2.0f,0.001f),
                new Vector3(0,0,0)
            };

            Assert.AreEqual(handJointExpected, handJoint);
        }

        [Test]
        public void When_GetHandJoint_Read_Position2of2()
        {
            string fileContent = ReadFile("Hand_Positions_Mock");
            List<Vector3> handJoint = FileReader.GetHandJoint(fileContent, 1, 1);

            List<Vector3> handJointExpected = new List<Vector3>
            {
                new Vector3(2,1,2),
                new Vector3(1,2,1),
                new Vector3(0,0,0),
                new Vector3(0.3f,-2.0f,0.001f),
                new Vector3(-0.3f,2.0f,-0.001f)
            };

            Assert.AreEqual(handJointExpected, handJoint);
        }

        [Test]
        public void When_GetHandJoint_Read_Position3of2()
        {
            string fileContent = ReadFile("Hand_Positions_Mock");
            List<Vector3> handJoint = FileReader.GetHandJoint(fileContent, 2, 1);

            Assert.AreEqual(new List<Vector3>(), handJoint);
        }
        #endregion

        #region Local Methods
        private string ReadFile(string name)
        {
            string filePath = string.Format("{0}/{1}.txt", _mockPath, name);
            StreamReader reader = new StreamReader(filePath);
            return reader.ReadToEnd();
        }
        #endregion
    }
}
