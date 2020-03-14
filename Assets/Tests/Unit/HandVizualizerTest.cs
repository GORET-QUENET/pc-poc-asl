using HandPositionReader.Scripts;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HandPositionReader.Tests.Unit
{
    public class HandVizualizerTest
    {
        private readonly HandVizualizer _handLoader;
        private readonly string _mockPath;

        public HandVizualizerTest()
        {
            _mockPath = string.Format("{0}/Tests/MockAsset", Application.dataPath);
            _handLoader = new HandVizualizer();
        }

        #region ToVector3
        [Test]
        public void When_ToVector3_With_Scale1()
        {
            Vector3 result = _handLoader.ToVector3("3,0|2,9|3,2", '|', 1);

            Assert.AreEqual(new Vector3(3, 2.9f, 3.2f), result);
        }

        [Test]
        public void When_ToVector3_With_Scale10()
        {
            Vector3 result = _handLoader.ToVector3("3,0|2,9|3,2", '|', 10);

            Assert.AreEqual(new Vector3(30, 29, 32), result);
        }

        [Test]
        public void When_ToVector3_With_DotSeparator()
        {
            Vector3 result = _handLoader.ToVector3("3,0.2,9.3,2", '.', 1);

            Assert.AreEqual(new Vector3(3, 2.9f, 3.2f), result);
        }

        [Test]
        public void When_ToVector3_With_BasSeparator()
        {
            Vector3 result = _handLoader.ToVector3("3,0|2,9|3,2", '.', 1);

            Assert.AreEqual(Vector3.zero, result);
        }

        [Test]
        public void When_ToVector3_When_CantConvert()
        {
            Vector3 result = _handLoader.ToVector3("3,0|2,9", '|', 1);

            Assert.AreEqual(Vector3.zero, result);
        }
        #endregion

        #region GetPositions
        [Test]
        public void When_GetPositions_Read_Position1of2()
        {
            string fileContent = ReadFile("Hand_Positions_Mock");
            List<Vector3> positions = _handLoader.GetPositions(fileContent, 0, 1);

            List<Vector3> positionsExpected = new List<Vector3>
            {
                new Vector3(1,2,1),
                new Vector3(2,1,2),
                new Vector3(-0.3f,2.0f,-0.001f),
                new Vector3(0.3f,-2.0f,0.001f),
                new Vector3(0,0,0)
            };

            Assert.AreEqual(positionsExpected, positions);
        }

        [Test]
        public void When_GetPositions_Read_Position2of2()
        {
            string fileContent = ReadFile("Hand_Positions_Mock");
            List<Vector3> positions = _handLoader.GetPositions(fileContent, 1, 1);

            List<Vector3> positionsExpected = new List<Vector3>
            {
                new Vector3(2,1,2),
                new Vector3(1,2,1),
                new Vector3(0,0,0),
                new Vector3(0.3f,-2.0f,0.001f),
                new Vector3(-0.3f,2.0f,-0.001f)
            };

            Assert.AreEqual(positionsExpected, positions);
        }

        [Test]
        public void When_GetPositions_Read_Position3of2()
        {
            string fileContent = ReadFile("Hand_Positions_Mock");
            List<Vector3> positions = _handLoader.GetPositions(fileContent, 2, 1);

            Assert.AreEqual(new List<Vector3>(), positions);
        }
        #endregion

        #region DrawFinger
        [Test]
        public void When_DrawFinger_Draw_1of2()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(8);
            List<Vector3> positions = GeneratePositions();

            _handLoader.DrawFinger(positions, lineRenderer, 2, 0, 1);

            Assert.AreEqual(positions[1], lineRenderer.GetPosition(0));
            Assert.AreEqual(positions[2], lineRenderer.GetPosition(1));
            Assert.AreEqual(positions[1], lineRenderer.GetPosition(2));
            Assert.AreEqual(positions[0], lineRenderer.GetPosition(3));
        }

        [Test]
        public void When_DrawFinger_Draw_2of2()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(8);
            List<Vector3> positions = GeneratePositions();

            _handLoader.DrawFinger(positions, lineRenderer, 2, 4, 3);

            Assert.AreEqual(positions[3], lineRenderer.GetPosition(4));
            Assert.AreEqual(positions[4], lineRenderer.GetPosition(5));
            Assert.AreEqual(positions[3], lineRenderer.GetPosition(6));
            Assert.AreEqual(positions[0], lineRenderer.GetPosition(7));
        }

        [Test]
        public void When_DrawFinger_Draw_3of2()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(8);
            List<Vector3> positions = GeneratePositions();

            _handLoader.DrawFinger(positions, lineRenderer, 2, 8, 5);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 8).ToArray();
            Vector3[] generatedPositions = new Vector3[8];
            lineRenderer.GetPositions(generatedPositions);

            Assert.AreEqual(expectedPositions, generatedPositions);
        }
        #endregion

        #region Generetors
        private LineRenderer GenerateLineRenderer(int size)
        {
            GameObject gameObject = new GameObject();
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.SetPosition(1, Vector3.zero);
            lineRenderer.positionCount = size;

            return lineRenderer;
        }

        private List<Vector3> GeneratePositions()
        {
            List<Vector3> positions =  new List<Vector3>
            {
                new Vector3(0,0,1),
                new Vector3(0,1,0),
                new Vector3(0,1,1),
                new Vector3(1,0,0),
                new Vector3(1,0,1)
            };

            return positions;
        }

        #endregion

        private string ReadFile(string name)
        {
            string filePath = string.Format("{0}/{1}.txt", _mockPath, name);
            StreamReader reader = new StreamReader(filePath);
            return reader.ReadToEnd();
        }
    }
}
