using HandPositionReader.Scripts.Enums;
using System.Collections.Generic;
using UnityEngine;

namespace HandPositionReader.Scripts.Algorithm
{
    public class HandJoint
    {
        // Ici c'est le patern Singleton le plus simple 
        private static HandJoint instance;
        public static HandJoint Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HandJoint();
                }
                return instance;
            }
        }

        public Dictionary<EHandJointWord, List<Vector3>> WordDico = new Dictionary<EHandJointWord, List<Vector3>>();
        public Dictionary<EHandJointWord, Dictionary<string, List<bool>>> TagDico = new Dictionary<EHandJointWord, Dictionary<string, List<bool>>>();

        #region Words
        public List<Vector3> One = new List<Vector3>
        {
            new Vector3(-0.07735085f,-6.024948f,48.64041f),
		    new Vector3(-0.4621073f,-9.589645f,47.83019f),
		    new Vector3(-1.750735f,-8.345339f,47.98904f),
		    new Vector3(-3.675894f,-6.476032f,49.54053f),
		    new Vector3(-4.640599f,-4.998193f,51.66546f),
		    new Vector3(-4.14852f,-4.6747f,52.68194f),
		    new Vector3(-1.008742f,-8.256534f,48.01378f),
		    new Vector3(-1.880658f,-3.425713f,48.17571f),
		    new Vector3(-2.059365f,0.4135721f,48.44834f),
		    new Vector3(-2.229239f,2.28071f,48.74523f),
		    new Vector3(-2.301105f,3.406398f,48.9671f),
		    new Vector3(-0.4863502f,-8.278262f,48.21367f),
		    new Vector3(-0.4371158f,-3.690816f,48.60718f),
		    new Vector3(-1.592187f,-2.035317f,51.59832f),
		    new Vector3(-2.337037f,-3.995913f,52.09548f),
		    new Vector3(-2.531886f,-5.203343f,51.58703f),
		    new Vector3(0.1645618f,-8.24425f,48.51218f),
		    new Vector3(0.4931346f,-4.255715f,49.4596f),
		    new Vector3(-0.3979388f,-3.234204f,52.12069f),
		    new Vector3(-1.201307f,-4.934945f,51.98523f),
		    new Vector3(-1.43723f,-6.154263f,51.05038f),
		    new Vector3(0.6114468f,-8.459711f,48.85916f),
		    new Vector3(1.558188f,-4.751776f,50.37713f),
		    new Vector3(0.8744919f,-4.234219f,51.75141f),
		    new Vector3(-0.07835843f,-5.174553f,52.02257f),
		    new Vector3(-0.7302031f,-6.021938f,51.54086f),
        };

        #endregion

        #region Tags
        public List<bool> WordOneFile1Tag = new List<bool>
        {
            false,
            false,
            false,
            true,
            true,
            true,
            true,
            true,
            true,
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
            false,
            false,
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
            false,
            false,
            false,
            false,
            true,
            true,

            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,

            true,
            true,
            true,
            true,
            true,
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
            false,
            false,

            false,
            false,
            false,
            false,
            false,
            false,
            false,
            true,
            true,
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
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
        };

        public List<bool> WordOneFile2Tag = new List<bool>
        {
            false,
            false,
            false,
            false,
            false,
            true,
            true,
            true,
            true,
            true,

            true,
            true,
            true,
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
            false,
            false,
            false,
            false,

            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,

            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,

            false,
            false,
            false,
            false,
            false,
            false,
            true,
            true,
            false,
            true,

            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,

            false,
            false,
            false,
            false,
            true,
            true,
            true,
            true,
            true,
            true,

            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
            true,
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
            true,
        };

        public List<bool> WordOneFile3Tag = new List<bool>
        {
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,

            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,

            false,
            false,
            false,
            false,
            true,
            false,
            false,
            false,
            false,
            true,

            false,
            false,
            false,
            false,
            true,
            false,
            false,
            false,
            false,
            true,

            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,

            false,
            false,
            false,
            true,
            true,
            true,
            true,
            true,
            true,
            true,

            true,
            true,
            true,
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
            false,
            false,
            false,
            false,

            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,

            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
            false,
        };
        #endregion

        public HandJoint()
        {
            WordDico.Add(EHandJointWord.One, One);
            TagDico.Add(EHandJointWord.One, new Dictionary<string, List<bool>>());
            TagDico[EHandJointWord.One].Add("Word_One_File1", WordOneFile1Tag);
            TagDico[EHandJointWord.One].Add("Word_One_File2", WordOneFile2Tag);
            TagDico[EHandJointWord.One].Add("Word_One_File3", WordOneFile3Tag);
        }
    }
}
