using HandPositionReader.Scripts.Vizualizer;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TestTools;

namespace HandPositionReader.Tests.Unit
{
    public class HandVizualizerTest
    {
        private readonly HandVizualizer _handVizualiser;
        private readonly string _mockPath;

        public HandVizualizerTest()
        {
            _mockPath = string.Format("{0}/Tests/MockAsset", Application.dataPath);
            _handVizualiser = new HandVizualizer(_mockPath);
        }

        #region DrawFinger
        [Test]
        public void When_DrawFinger_Draw_1of2()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(8);
            List<Vector3> handJoint = GenerateHandJoint();

            _handVizualiser.DrawFinger(handJoint, lineRenderer, 2, 0, 1);

            Assert.AreEqual(handJoint[1], lineRenderer.GetPosition(0));
            Assert.AreEqual(handJoint[2], lineRenderer.GetPosition(1));
            Assert.AreEqual(handJoint[1], lineRenderer.GetPosition(2));
            Assert.AreEqual(handJoint[0], lineRenderer.GetPosition(3));
        }

        [Test]
        public void When_DrawFinger_Draw_2of2()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(8);
            List<Vector3> handJoint = GenerateHandJoint();

            _handVizualiser.DrawFinger(handJoint, lineRenderer, 2, 4, 3);

            Assert.AreEqual(handJoint[3], lineRenderer.GetPosition(4));
            Assert.AreEqual(handJoint[4], lineRenderer.GetPosition(5));
            Assert.AreEqual(handJoint[3], lineRenderer.GetPosition(6));
            Assert.AreEqual(handJoint[0], lineRenderer.GetPosition(7));
        }

        [Test]
        public void When_DrawFinger_Draw_3of2()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(8);
            List<Vector3> handJoint = GenerateHandJoint();

            _handVizualiser.DrawFinger(handJoint, lineRenderer, 2, 8, 5);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 8).ToArray();
            Vector3[] actualPositions = new Vector3[8];
            lineRenderer.GetPositions(actualPositions);

            Assert.AreEqual(expectedPositions, actualPositions);
        }
        #endregion

        #region DrawHandLine
        [Test]
        public void When_DrawHandLine_Work()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            List<Vector3> handJoint = GenerateFullHandJoint(1);

            _handVizualiser.DrawHandLine(handJoint, lineRenderer);

            Vector3[] actualPositions = new Vector3[50];
            lineRenderer.GetPositions(actualPositions);
            List<Vector3> expectedPositions = ExpectedLineRendererPositions(1);

            Assert.AreEqual(expectedPositions, actualPositions.ToList());
        }

        [Test]
        public void When_DrawHandLine_With_BadLineRenderer1of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(9);
            List<Vector3> handJoint = GenerateFullHandJoint(1);

            _handVizualiser.DrawHandLine(handJoint, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 9).ToArray();
            expectedPositions[0] = handJoint[0];
            expectedPositions[1] = handJoint[1];
            Vector3[] actualPositions = new Vector3[9];
            lineRenderer.GetPositions(actualPositions);

            Assert.AreEqual(expectedPositions, actualPositions);
        }

        [Test]
        public void When_DrawHandLine_With_BadLineRenderer2of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(1);
            List<Vector3> handJoint = GenerateFullHandJoint(1);

            _handVizualiser.DrawHandLine(handJoint, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 1).ToArray();
            Vector3[] actualPositions = new Vector3[1];
            lineRenderer.GetPositions(actualPositions);

            Assert.AreEqual(expectedPositions, actualPositions);
        }

        [Test]
        public void When_DrawHandLine_With_BadLineRenderer3of3()
        {
            LineRenderer lineRenderer = null;
            List<Vector3> handJoint = GenerateFullHandJoint(1);

            _handVizualiser.DrawHandLine(handJoint, lineRenderer);

            Assert.Null(lineRenderer);
        }


        [Test]
        public void When_DrawHandLine_With_BadHandJoint1of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            List<Vector3> handJoint = new List<Vector3>{
                new Vector3(1, 1, 1),
                new Vector3(2, 2, 2),
                new Vector3(3, 3, 3)
            };

            _handVizualiser.DrawHandLine(handJoint, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 50).ToArray();
            expectedPositions[0] = handJoint[0];
            expectedPositions[1] = handJoint[1];
            Vector3[] actualPositions = new Vector3[50];
            lineRenderer.GetPositions(actualPositions);

            Assert.AreEqual(expectedPositions, actualPositions);
        }

        [Test]
        public void When_DrawHandLine_With_BadHandJoint2of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            List<Vector3> handJoint = new List<Vector3>{
                new Vector3(1, 1, 1)
            };

            _handVizualiser.DrawHandLine(handJoint, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 50).ToArray();
            Vector3[] actualPositions = new Vector3[50];
            lineRenderer.GetPositions(actualPositions);

            Assert.AreEqual(expectedPositions, actualPositions);
        }

        [Test]
        public void When_DrawHandLine_With_BadHandJoint3of3()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            List<Vector3> handJoint = null;

            _handVizualiser.DrawHandLine(handJoint, lineRenderer);

            Vector3[] expectedPositions = Enumerable.Repeat(Vector3.zero, 50).ToArray();
            Vector3[] actualPositions = new Vector3[50];
            lineRenderer.GetPositions(actualPositions);

            Assert.AreEqual(expectedPositions, actualPositions);
        }
        #endregion

        #region InitVizualisation & ShowHand
        [Test]
        public void When_ShowHand_Work_OnGameObject()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            GameObject handJointObject = Resources.Load<GameObject>("Prefabs/Hand_Joint");

            _handVizualiser.InitVizualisation("Full_Hand_Positions_Mock");
            _handVizualiser.ShowHand(lineRenderer, handJointObject);

            List<Vector3> expectedPositions = GenerateFullHandJoint(3);
            List<Vector3> actualPositions = new List<Vector3>();

            for(int i = 0; i < handJointObject.transform.childCount; i++)
            {
                actualPositions.Add(handJointObject.transform.GetChild(i).position);
            }

            Assert.AreEqual(expectedPositions, actualPositions);
        }

        [Test]
        public void When_ShowHand_Work_OnLineRenderer()
        {
            LineRenderer lineRenderer = GenerateLineRenderer(50);
            GameObject handJointObject = Resources.Load<GameObject>("Prefabs/Hand_Joint");

            _handVizualiser.InitVizualisation("Full_Hand_Positions_Mock");
            _handVizualiser.ShowHand(lineRenderer, handJointObject);

            Vector3[] actualPositions = new Vector3[50];
            lineRenderer.GetPositions(actualPositions);
            List<Vector3> expectedPositions = ExpectedLineRendererPositions(3);

            Assert.AreEqual(expectedPositions, actualPositions.ToList());
        }

        [Test]
        public void When_ShowHand_With_NullInputs()
        {
            _handVizualiser.ShowHand(null, null);

            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void When_InitVizualisation_With_NullInput()
        {
            string errorMessage = string.Format("Could not find file \"{0}\\.txt\"", _mockPath);
            errorMessage = errorMessage.Replace('/', '\\');

            LogAssert.Expect(LogType.Error, errorMessage);
            _handVizualiser.InitVizualisation(null);
            LogAssert.NoUnexpectedReceived();
        }

        [Test]
        public void When_InitVizualisation_With_BadFileName()
        {
            string errorMessage = string.Format("Could not find file \"{0}\\test.txt\"", _mockPath);
            errorMessage = errorMessage.Replace('/', '\\');

            LogAssert.Expect(LogType.Error, errorMessage);
            _handVizualiser.InitVizualisation("test");
            LogAssert.NoUnexpectedReceived();
        }
        #endregion

        #region Local Methods
        private LineRenderer GenerateLineRenderer(int size)
        {
            GameObject gameObject = new GameObject();
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.SetPosition(1, Vector3.zero);
            lineRenderer.positionCount = size;

            return lineRenderer;
        }

        private List<Vector3> GenerateHandJoint()
        {
            List<Vector3> handJoint = new List<Vector3>
            {
                new Vector3(0,0,1),
                new Vector3(0,1,0),
                new Vector3(0,1,1),
                new Vector3(1,0,0),
                new Vector3(1,0,1)
            };

            return handJoint;
        }

        private List<Vector3> GenerateFullHandJoint(int scale)
        {
            List<Vector3> handJoint = new List<Vector3>
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

            for (int i = 0; i < handJoint.Count; i++)
            {
                handJoint[i] *= scale;
            }

            return handJoint;
        }

        private List<Vector3> ExpectedLineRendererPositions(int scale)
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

            for (int i = 0; i < positions.Count; i++)
            {
                positions[i] *= scale;
            }

            return positions;
        }
        #endregion
    }
}
