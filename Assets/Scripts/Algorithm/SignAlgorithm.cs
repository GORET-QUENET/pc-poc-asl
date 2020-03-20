using HandPositionReader.Scripts.Enums;
using HandPositionReader.Scripts.Structs;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

namespace HandPositionReader.Scripts.Algorithm
{
    public class SignAlgorithm
    {
        private string _assetPath;

        public SignAlgorithm(string assetPath)
        {
            _assetPath = assetPath;
        }

        /// <summary>
        /// Cette fonction va exécuter l'algoritme pour tous les fichiers indiqués dans files.
        /// </summary>
        /// <param name="files"> La liste des fichiers à annalysé avec au mot recherché.</param>
        /// <param name="generateFile"> Indique si nous devons générer un fichier. </param>
        /// <param name="generateFileWord"> Indique le mot pour lequel nous générons le fichier. </param>
        /// <param name="generateFileIndex"> Indique l'index du handJoint du fichier à utiliser pour générer le fichier. </param>
        /// <returns> Un dictionnaire de métrics pour chaque mots annalysés </returns>
        public Dictionary<EHandJointWord, List<Metrics>> RunAlgorithmOnFiles(List<FileWord> files, bool generateFile, EHandJointWord generateFileWord, int generateFileIndex)
        {
            if (files == null || files.Count == 0)
                return null;

            string filePath;
            string fileContent;
            int nbHandJoint;
            List<string> lines = new List<string>();
            Metrics metrics;
            Dictionary<EHandJointWord, List<Metrics>> wordsMetrics = new Dictionary<EHandJointWord, List<Metrics>>();

            foreach (FileWord fileWord in files)
            {
                filePath = string.Format("{0}/{1}.txt", _assetPath, fileWord.FileName);
                using (StreamReader reader = new StreamReader(filePath))
                {
                    fileContent = reader.ReadToEnd();
                }

                lines = fileContent.Split('\n').ToList();
                nbHandJoint = lines.Where(l => l.Contains("START")).Count();

                // Ici nous pouvons générer un fichier qui contiendra une list<Vector3> des jointures d'un HandJoint.
                if (generateFile && fileWord.Word == generateFileWord)
                {
                    List<Vector3> handJoint = FileReader.GetHandJoint(fileContent, generateFileIndex, 100);
                    filePath = string.Format("{0}/GeneratedList.cs", _assetPath);
                    GenerateListFileFromHandJoint(handJoint, filePath);
                }

                metrics = AlgorithmOnWord(fileContent, nbHandJoint, fileWord.Word);

                if (!wordsMetrics.ContainsKey(fileWord.Word))
                {
                    wordsMetrics.Add(fileWord.Word, new List<Metrics>());
                }
                wordsMetrics[fileWord.Word].Add(metrics);
            }

            return wordsMetrics;
        }

        /// <summary>
        /// Cette fonction va générer un fichier cs qui contiendra une List<Vector3> avec les valeurs de handJoint. 
        /// </summary>
        /// <param name="handJoint"> Nous donne les valeurs à enregistre dans le fichier. </param>
        /// <param name="path"> Le chemin ou le fichier sera enregistrer avec son nom. </param>
        public void GenerateListFileFromHandJoint(List<Vector3> handJoint, string path)
        {
            if (handJoint?.Any() != true)
                return;

            // Nous devons utiliser une culture américaine pour la convertion d'un float en string
            // Car la culture france donne 2,3 or un float en c# est de la forme 2.3
            CultureInfo culture = new CultureInfo("en-US");

            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("using System.Collections.Generic;");
                writer.WriteLine("using System.Numerics;");
                writer.WriteLine("");
                writer.WriteLine("public static class GeneratedList");
                writer.WriteLine("{");
                writer.WriteLine("\tpublic static List<Vector3> positions = new List<Vector3>");
                writer.WriteLine("\t{");
                foreach (var joint in handJoint)
                {
                    string line = string.Format("\t\tnew Vector3({0}f,{1}f,{2}f),", joint.x.ToString(culture), joint.y.ToString(culture), joint.z.ToString(culture));
                    writer.WriteLine(line);
                }
                writer.WriteLine("\t};");
                writer.WriteLine("}");
                writer.Close();
            }
        }

        /// <summary>
        /// Cette fonction va exécuter l'algorithme sur le fileContent.
        /// </summary>
        /// <param name="fileContent"> Contenu du fichier qui contients tous les handjoints enregistrés et à comparer. </param>
        /// <param name="nbHandJoint"> Nombre de handJoints dans le fileContent que nous allons annalyser. </param>
        /// <param name="word"> Le mot qui devra etre reconue dans le fichier. </param>
        /// <returns> Les métrics de l'analyse sur tous les handJoints. </returns>
        public Metrics AlgorithmOnWord(string fileContent, int nbHandJoint, EHandJointWord word)
        {
            List<bool> validationList = new List<bool>();
            for (int i = 0; i < nbHandJoint; i++)
            {
                List<Vector3> handJoint = FileReader.GetHandJoint(fileContent, i, 100);
                List<Vector3> oneHandJoint = HandJoint.Instance.WordDico[word];

                if (IsThisWord(oneHandJoint, handJoint, 200))
                {
                    //Debug.Log("OK");
                    validationList.Add(true);
                }
                else
                {
                    //Debug.Log("NO");
                    validationList.Add(false);
                }
            }

            return GetAlgorithmMetrics(validationList, word);
        }

        /// <summary>
        /// Cette fonction est l'algorithme qui va nous dire si le handJoint expected est le mot voulue.
        /// </summary>
        /// <param name="expected"> Le handJoint que nous annalysons. </param>
        /// <param name="actual"> Le handJoint qui représente le mot que nous cherchons. </param>
        /// <param name="sqrDistanceLimit"> La distance au carré max entre les deux handJoint. </param>
        /// <returns> true si nous avons reconnue le mot et false sinon. </returns>
        public bool IsThisWord(List<Vector3> expected, List<Vector3> actual, float sqrDistanceLimit)
        {
            if (expected?.Any() != true || actual?.Any() != true)
                return false;

            for (int i = 0; i < actual.Count; i++)
            {
                if (Vector3.SqrMagnitude(actual[i] - expected[i]) > sqrDistanceLimit)
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Cette fonction va comparer les validations que nous avons obtenue avec les Tag voulue du mot en entrée et nous en retourner les métrics.
        /// </summary>
        /// <param name="validationList"> La liste des mot reconnue apres annalyse de l'algorithme sur un fichier. </param>
        /// <param name="word"> Le mot qui à été annalysé pour obtenir ces Tags voulue. </param>
        /// <returns> Les métrics de cette annalyse. </returns>
        public Metrics GetAlgorithmMetrics(List<bool> validationList, EHandJointWord word)
        {
            List<bool> wordTagList = HandJoint.Instance.TagDico[word];

            if (validationList?.Any() != true || validationList.Count != wordTagList.Count)
                return Metrics.Zero;

            Metrics metrics = Metrics.Zero;

            for (int i = 0; i < wordTagList.Count; i++)
            {
                if (validationList[i] && wordTagList[i])
                    metrics.TP++;
                if (validationList[i] && !wordTagList[i])
                    metrics.FN++;
                if (!validationList[i] && wordTagList[i])
                    metrics.FP++;
                if (!validationList[i] && !wordTagList[i])
                    metrics.TN++;
            }

            return metrics;
        }
    }
}
