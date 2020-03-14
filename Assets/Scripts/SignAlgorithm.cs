using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HandPositionReader.Scripts
{
    public class SignAlgorithm : MonoBehaviour
    {
        private string _assetPath;
        private string _fileContent;
        private int _nbPositions;

        public string TestFileName;

        // Start is called before the first frame update
        void Start()
        {
            _assetPath = Application.dataPath;
            string filePath = string.Format("{0}/{1}.txt", _assetPath, TestFileName);
            StreamReader reader = new StreamReader(filePath);
            _fileContent = reader.ReadToEnd();
            List<string> lines = _fileContent.Split('\n').ToList();
            _nbPositions = lines.Where(l => l.Contains("START")).Count();

            GenerateListFromPosition(_fileContent, 0, 100);

            TestingLoop();
        }

        public void GenerateListFromPosition(string fileContent, int targetPosition, int scale)
        {
            List<Vector3> positions = GetPositions(fileContent, targetPosition, scale);
            string filePath = string.Format("{0}/{1}.cs", _assetPath, "GeneratedList");
            StreamWriter writer = new StreamWriter(filePath);
            

            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine("using System.Numerics;");
            writer.WriteLine("");
            writer.WriteLine("public static class GeneratedList");
            writer.WriteLine("{");
            writer.WriteLine("\tpublic static List<Vector3> positions = new List<Vector3>");
            writer.WriteLine("\t{");
            foreach (var position in positions)
            {
                string line = string.Format("\t\tnew Vector3({0}f|{1}f|{2}f)|", position.x, position.y, position.z);
                line = line.Replace(',', '.');
                line = line.Replace('|', ',');
                writer.WriteLine(line);
            }
            writer.WriteLine("\t};");
            writer.WriteLine("}");
            writer.Close();
        }

        public void TestingLoop()
        {
            List<bool> verification = new List<bool>();
            for (int i = 0; i < _nbPositions; i++)
            {
                List<Vector3> positions = GetPositions(_fileContent, i, 100);
                List<Vector3> onePosition = Position.One;

                if (IsThisPosition(positions, onePosition, 200))
                {
                    //Debug.Log("OK");
                    verification.Add(true);
                }
                else
                {
                    //Debug.Log("NO");
                    verification.Add(false);
                }
            }

            //Metrics values
            int TP = 0; // True positives
            int FP = 0; // False positives
            int FN = 0; // False negatives
            int TN = 0; // True negatives
            for (int i = 0; i < Position.OneValue.Count; i++)
            {
                if (verification[i] && Position.OneValue[i])
                    TP++;
                if (verification[i] && !Position.OneValue[i])
                    FN++;
                if (!verification[i] && Position.OneValue[i])
                    FP++;
                if (!verification[i] && !Position.OneValue[i])
                    TN++;
            }

            float precision = (float)TP / (float)(TP + FP);
            float sensivity = (float)TP / (float)(TP + FN);
            float negativePredictiveValue = (float)TN / (float)(TN + FN);
            float specificity = (float)TN / (float)(TN + FP);
            float accuracy = (float)(TP + TN) / (float)(TP + TN + FP + FN);

            Debug.Log("TP " + TP + " | FP " + FP + " | FN " + FN + " | TN " + TN);
            Debug.Log("Precision : " + precision);
            Debug.Log("Sensivity : " + sensivity);
            Debug.Log("Negative Predictive Value : " + negativePredictiveValue);
            Debug.Log("Specificity : " + specificity);
            Debug.Log("Accuracy : " + accuracy);
        }

        public bool IsThisPosition(List<Vector3> positions, List<Vector3> onePosition, float sqrDistanceLimit)
        {
            for (int i = 0; i < positions.Count; i++)
            {
                if (Vector3.SqrMagnitude(positions[i] - onePosition[i]) > sqrDistanceLimit)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Cette fonction va parcourir chaques lignes du fichier file et retourner les positions entre #START# et #END#.
        /// </summary>
        /// <param name="file"> Contenue du fichier à parcourir. </param>
        /// <param name="targetPosition"> Index de la position voulue. </param>
        /// <param name="scale"> Un facteur permettant d'agrandir l'espace entre chaque points. </param>
        /// <returns> La liste des positions récupérées du fichier. </returns>
        public List<Vector3> GetPositions(string file, int targetPosition, int scale)
        {
            List<string> lines = file.Split('\n').ToList();
            List<Vector3> positions = new List<Vector3>();

            int currentPosition = 0;
            bool canReadPosition = false;

            foreach (var line in lines)
            {
                if (line.Contains("#START#"))
                {
                    if (currentPosition == targetPosition)
                        canReadPosition = true;
                }
                else if (line.Contains("#END#"))
                {
                    currentPosition++;

                    //Stop la lecture du fichier car nous venons de lire toutes les positions.
                    if (canReadPosition)
                        break;
                }
                else if (canReadPosition)
                {
                    positions.Add(ToVector3(line, '|', scale));
                }
            }

            return positions;
        }

        /// <summary>
        /// Cette fonction va convertir en Vector3 un string de la forme "2.3|3.3|2.0".
        /// </summary>
        /// <param name="line"> String à convertir en Vector3. </param>
        /// <param name="separator"> Séparateur entre chaque valeurs. </param>
        /// <param name="scale"> Un facteur permettant d'agrandir l'espace entre chaque points. </param>
        /// <returns> Le Vector3 obtenue du string en param. </returns>
        public Vector3 ToVector3(string line, char separator, int scale)
        {
            if (!line.Contains(separator))
                return Vector3.zero;

            string[] Vector3Values = line.Split(separator);

            try
            {
                float x = float.Parse(Vector3Values[0]) * scale;
                float y = float.Parse(Vector3Values[1]) * scale;
                float z = float.Parse(Vector3Values[2]) * scale;

                return new Vector3(x, y, z);
            }
            catch (Exception ex)
            {
                Debug.Log(ex);
                return Vector3.zero;
            }
        }
    }
}
