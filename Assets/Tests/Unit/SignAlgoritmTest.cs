using HandPositionReader.Scripts.Algorithm;
using HandPositionReader.Scripts.Enums;
using HandPositionReader.Scripts.Structs;
using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

namespace HandPositionReader.Tests.Unit
{
    public class SignAlgoritmTest
    {
        private readonly SignAlgorithm _signAlgoritm;
        private readonly string _mockPath;

        public SignAlgoritmTest()
        {
            _mockPath = string.Format("{0}/Tests/MockAsset", Application.dataPath);
            _signAlgoritm = new SignAlgorithm(_mockPath);
        }

        #region GetAlgorithmMetrics
        [Test]
        public void When_GetAlgorithmMetrics_Work()
        {
            List<bool> validationList = GeneratePerfectValidationlist();

            Metrics actualMetrics = _signAlgoritm.GetAlgorithmMetrics(validationList, EHandJointWord.One);
            Metrics expectedMetrics = GeneratePerfectMetrics();

            Assert.AreEqual(expectedMetrics.TP, actualMetrics.TP);
            Assert.AreEqual(expectedMetrics.FP, actualMetrics.FP);
            Assert.AreEqual(expectedMetrics.FN, actualMetrics.FN);
            Assert.AreEqual(expectedMetrics.TN, actualMetrics.TN);
        }

        [Test]
        public void When_GetAlgorithmMetrics_With_EmptyValidation()
        {
            List<bool> validationList = new List<bool>();

            Metrics actualMetrics = _signAlgoritm.GetAlgorithmMetrics(validationList, EHandJointWord.One);

            Assert.AreEqual(Metrics.Zero, actualMetrics);
        }

        [Test]
        public void When_GetAlgorithmMetrics_With_NullValidation()
        {
            Metrics actualMetrics = _signAlgoritm.GetAlgorithmMetrics(null, EHandJointWord.One);

            Assert.AreEqual(Metrics.Zero, actualMetrics);
        }

        [Test]
        public void When_GetAlgorithmMetrics_With_BadSizeValidation()
        {
            List<bool> validationList = new List<bool>
            {
                true,
                false,
                true
            };

            Metrics actualMetrics = _signAlgoritm.GetAlgorithmMetrics(validationList, EHandJointWord.One);

            Assert.AreEqual(Metrics.Zero, actualMetrics);
        }
        #endregion

        #region IsThisWord
        // TODO
        #endregion

        #region AlgorithmOnWord
        // TODO
        #endregion

        #region GenerateListFileFromHandJoint
        // TODO
        #endregion

        #region RunAlgorithmOnFiles
        // TODO
        #endregion

        #region Local Methods
        private List<bool> GeneratePerfectValidationlist()
        {
            List<bool> validationList = new List<bool>
            {
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                true,
                false,
                false,

                false,
                false,
                false,
                true,
                true,
                true,
                false,
                true,
                true,
                true,

                true,
                true,
                false,
                false,
                false,
                false,
                false,
                false,
                false,
                false
            };
            return validationList;
        }

        private Metrics GeneratePerfectMetrics()
        {
            Metrics metrics;
            metrics.TP = 16;
            metrics.FP = 0;
            metrics.FN = 0;
            metrics.TN = 14;
            return metrics;
        }
        #endregion
    }
}
