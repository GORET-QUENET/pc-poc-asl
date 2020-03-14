using System.Collections;
using System.Collections.Generic;
using System.Linq;
using HandPositionReader.Scripts;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

namespace HandPositionReader.Tests.Functional
{
    public class HandVizualizerTest
    {
        private readonly HandVizualizer _handLoader;
        private readonly string _mockPath;

        public HandVizualizerTest()
        {
            _mockPath = string.Format("{0}/Tests/MockAsset", Application.dataPath);
            _handLoader = new HandVizualizer(_mockPath, "Full_Hand_Positions_Mock");
        }

        #region DrawHand
        [Test]
        public void When_DrawHand_Work()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            List<Vector3> positions = GeneratePositions();

            _handLoader.DrawHand(positions, lineRenderer);

            Vector3[] generatedPositions = new Vector3[50];
            lineRenderer.GetPositions(generatedPositions);
            List<Vector3> expectedPositions = ExpectedLineRendererPositions();

            Assert.AreEqual(expectedPositions, generatedPositions.ToList());
        }

        [Test]
        public void When_DrawHand_With_BadLineRenderer1of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(9);
            List<Vector3> positions = GeneratePositions();

            _handLoader.DrawHand(positions, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 9).ToArray();
            expectedPositions[0] = positions[0];
            expectedPositions[1] = positions[1];
            Vector3[] generatedPositions = new Vector3[9];
            lineRenderer.GetPositions(generatedPositions);

            Assert.AreEqual(expectedPositions, generatedPositions);
        }

        [Test]
        public void When_DrawHand_With_BadLineRenderer2of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(1);
            List<Vector3> positions = GeneratePositions();

            _handLoader.DrawHand(positions, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 1).ToArray();
            Vector3[] generatedPositions = new Vector3[1];
            lineRenderer.GetPositions(generatedPositions);

            Assert.AreEqual(expectedPositions, generatedPositions);
        }

        [Test]
        public void When_DrawHand_With_BadLineRenderer3of3()
        {
            LineRenderer lineRenderer = null;
            List<Vector3> positions = GeneratePositions();

            _handLoader.DrawHand(positions, lineRenderer);

            Assert.Null(lineRenderer);
        }


        [Test]
        public void When_DrawHand_With_BadPositions1of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            List<Vector3> positions = new List<Vector3>{
                new Vector3(1, 1, 1),
                new Vector3(2, 2, 2),
                new Vector3(3, 3, 3)
            };

            _handLoader.DrawHand(positions, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 50).ToArray();
            expectedPositions[0] = positions[0];
            expectedPositions[1] = positions[1];
            Vector3[] generatedPositions = new Vector3[50];
            lineRenderer.GetPositions(generatedPositions);

            Assert.AreEqual(expectedPositions, generatedPositions);
        }

        [Test]
        public void When_DrawHand_With_BadPositions2of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            List<Vector3> positions = new List<Vector3>{
                new Vector3(1, 1, 1)
            };

            _handLoader.DrawHand(positions, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 50).ToArray();
            Vector3[] generatedPositions = new Vector3[50];
            lineRenderer.GetPositions(generatedPositions);

            Assert.AreEqual(expectedPositions, generatedPositions);
        }

        [Test]
        public void When_DrawHand_With_BadPositions3of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            List<Vector3> positions = null;

            _handLoader.DrawHand(positions, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 50).ToArray();
            Vector3[] generatedPositions = new Vector3[50];
            lineRenderer.GetPositions(generatedPositions);

            Assert.AreEqual(expectedPositions, generatedPositions);
        }
        #endregion

        //[UnityTest]
        //public IEnumerator When_StartVizualition_When_Work()
        //{
        //    _handLoader.LineRenderer = GenerateLineRenderer(50);
        //    _handLoader.slider = GenerateSlider(10);
        //    _handLoader.HandJoins = Resources.Load<GameObject>("Mock/Prefabs/Hand_Joins_Mock");

        //    _handLoader.ShowHand();
        //    yield return new WaitForSeconds(1f);
        //    _handLoader.PauseVizualisation();

        //    List<Vector3> generatedPositions = new List<Vector3>();

        //    for (int i = 0; i < _handLoader.HandJoins.transform.childCount; i++)
        //    {
        //        generatedPositions.Add(_handLoader.HandJoins.transform.GetChild(i).position);
        //    } 

        //    List<Vector3> expectedPositions = GeneratePositions();

        //    Assert.AreEqual(expectedPositions, generatedPositions);
        //}

        #region Generetors
        private LineRenderer GenerateLineRenderer(int size)
        {
            GameObject gameObject = new GameObject();
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.SetPosition(1, Vector3.zero);
            lineRenderer.positionCount = size;

            return lineRenderer;
        }

        private Slider GenerateSlider(float value)
        {
            GameObject gameObject = new GameObject();
            Slider slider = gameObject.AddComponent<Slider>();
            slider.minValue = 1;
            slider.maxValue = 10;
            slider.value = value;

            return slider;
        }

        private List<Vector3> GeneratePositions()
        {
            List<Vector3> positions = new List<Vector3>
            {
                new Vector3(1,1,1),
                new Vector3(2,2,2),
                new Vector3(3,3,3),
                new Vector3(4,4,4),
                new Vector3(5,5,5),
                new Vector3(6,6,6),
                new Vector3(7,7,7),
                new Vector3(8,8,8),
                new Vector3(9,9,9),
                new Vector3(10,10,10),
                new Vector3(11,11,11),
                new Vector3(12,12,12),
                new Vector3(13,13,13),
                new Vector3(14,14,14),
                new Vector3(15,15,15),
                new Vector3(16,16,16),
                new Vector3(17,17,17),
                new Vector3(18,18,18),
                new Vector3(19,19,19),
                new Vector3(20,20,20),
                new Vector3(21,21,21),
                new Vector3(22,22,22),
                new Vector3(23,23,23),
                new Vector3(24,24,24),
                new Vector3(25,25,25),
                new Vector3(26,26,26)
            };

            return positions;
        }

        private List<Vector3> ExpectedLineRendererPositions()
        {
            List<Vector3> positions = new List<Vector3>
            {
                new Vector3(1,1,1),
                new Vector3(2,2,2),

                //Doigt 1
                new Vector3(3,3,3),
                new Vector3(4,4,4),
                new Vector3(5,5,5),
                new Vector3(6,6,6),
                new Vector3(5,5,5),
                new Vector3(4,4,4),
                new Vector3(3,3,3),
                new Vector3(1,1,1),

                //Doigt 2
                new Vector3(7,7,7),
                new Vector3(8,8,8),
                new Vector3(9,9,9),
                new Vector3(10,10,10),
                new Vector3(11,11,11),
                new Vector3(10,10,10),
                new Vector3(9,9,9),
                new Vector3(8,8,8),
                new Vector3(7,7,7),
                new Vector3(1,1,1),

                //Doigt 3
                new Vector3(12,12,12),
                new Vector3(13,13,13),
                new Vector3(14,14,14),
                new Vector3(15,15,15),
                new Vector3(16,16,16),
                new Vector3(15,15,15),
                new Vector3(14,14,14),
                new Vector3(13,13,13),
                new Vector3(12,12,12),
                new Vector3(1,1,1),

                //Doigt 4
                new Vector3(17,17,17),
                new Vector3(18,18,18),
                new Vector3(19,19,19),
                new Vector3(20,20,20),
                new Vector3(21,21,21),
                new Vector3(20,20,20),
                new Vector3(19,19,19),
                new Vector3(18,18,18),
                new Vector3(17,17,17),
                new Vector3(1,1,1),

                //Doigt 5
                new Vector3(22,22,22),
                new Vector3(23,23,23),
                new Vector3(24,24,24),
                new Vector3(25,25,25),
                new Vector3(26,26,26),
                new Vector3(25,25,25),
                new Vector3(24,24,24),
                new Vector3(23,23,23),
                new Vector3(22,22,22),
                new Vector3(1,1,1)
            };

            return positions;
        }
        #endregion
    }
}
