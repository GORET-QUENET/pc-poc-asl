using HandPositionReader.Scripts.Enums;
using HandPositionReader.Scripts.Structs;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using System.Diagnostics;

namespace HandPositionReader.Scripts.Algorithm
{
    public class AlgorithmManager : MonoBehaviour
    {
        private SignAlgorithm _signAlgorithm;
        private Dictionary<EHandJointWord, List<Metrics>> _wordsMetrics;

        [Header("Tests Files")]
        [Tooltip("Liste des fichier à tester et des mots associés.")]
        // Ici je passe par un struct car on ne peut pas afficher un Dictionary dans l'inspecteur
        public List<FileWord> Files;

        [Header("File Generator")]
        [Tooltip("Indique si nous generons le fichier ou non.")]
        public bool GenerateFile;

        [Tooltip("Indique le mot et le fichier pour lequel on génère le fichier.")]
        public FileWord GenerateFileWord;

        [Tooltip("Indique l'index de la HandJoint du fichier à utiliser pour la génération.")]
        public int GenerateFileIndex;

        [Header("Objets de l'UI")]
        [Tooltip("Permettra de choisir les stats affichés selon le mot sélectionné.")]
        public TMP_Dropdown WordDropDown;

        [Tooltip("Objet qui contient le dropdown et le tritre qu'on activera et désactivera.")]
        public GameObject FileSelection;

        [Tooltip("Permettra de choisir les stats affichés selon le fichier sélectionné.")]
        public TMP_Dropdown FileDropDorwn;

        public TMP_Text TP;
        public TMP_Text FP;
        public TMP_Text FN;
        public TMP_Text TN;

        public TMP_Text Precision;
        public TMP_Text Sensivity;
        public TMP_Text NPV;
        public TMP_Text Specificity;
        public TMP_Text Accuracy;

        // Start is called before the first frame update
        void Start()
        {
                    
            _signAlgorithm = new SignAlgorithm(Application.dataPath);

            RunAlgorithm();
            
            foreach (KeyValuePair<EHandJointWord, List<Metrics>> word in _wordsMetrics)
            {
                WordDropDown.options.Add(new TMP_Dropdown.OptionData(word.Key.ToString()));
            }
            WordDropDown.RefreshShownValue();

            OnWordChanged();
        }

        public void RunAlgorithm()
        {
            Stopwatch stopWatch = new Stopwatch();

            stopWatch.Start();
            _wordsMetrics = _signAlgorithm.RunAlgorithmOnFiles(Files, GenerateFile, GenerateFileWord, GenerateFileIndex);
            stopWatch.Stop();

            // Get the elapsed time as a TimeSpan value.
            TimeSpan ts = stopWatch.Elapsed;
            // Format and display the TimeSpan value.
            string elapsedTime = string.Format("{0:00}min {1:00},{2:00}s",
                ts.Minutes, ts.Seconds, ts.Milliseconds / 10);
            UnityEngine.Debug.Log("RunTime " + elapsedTime);
        }

        /// <summary>
        /// Cette méthode est call lorsque le WordDropDown change de valeur dans l'inspecteur Unity.
        /// </summary>
        public void OnWordChanged()
        {
            int index = WordDropDown.value;

            List<Metrics> metricsList = _wordsMetrics[(EHandJointWord)index];

            if (metricsList.Count > 1)
            {
                FileSelection.SetActive(true);
                FileDropDorwn.value = 0;
                FileDropDorwn.options = new List<TMP_Dropdown.OptionData>();
                List<FileWord> files = Files.Where(p => p.Word == (EHandJointWord)index).ToList();

                foreach (FileWord file in files)
                {
                    FileDropDorwn.options.Add(new TMP_Dropdown.OptionData(file.FileName));
                }
                FileDropDorwn.RefreshShownValue();
            }
            else
            {
                FileSelection.SetActive(false);
            }

            ShowMetrics(metricsList[0]);
        }

        /// <summary>
        /// Cette méthode est call lorsque le FileDropDown change de valeur dans l'inspecteur Unity.
        /// </summary>
        public void OnFileChanged()
        {
            int wordIndex = WordDropDown.value;
            int fileIndex = FileDropDorwn.value;

            Metrics metrics = _wordsMetrics[(EHandJointWord)wordIndex][fileIndex];

            ShowMetrics(metrics);
        }

        public void ShowMetrics(Metrics metrics)
        {
            TP.text = metrics.TP.ToString();
            FP.text = metrics.FP.ToString();
            FN.text = metrics.FN.ToString();
            TN.text = metrics.TN.ToString();

            double precision = Math.Round((double)metrics.TP / (double)(metrics.TP + metrics.FP), 2);
            double sensivity = Math.Round((double)metrics.TP / (double)(metrics.TP + metrics.FN), 2);
            double negativePredictiveValue = Math.Round((double)metrics.TN / (double)(metrics.TN + metrics.FN), 2);
            double specificity = Math.Round((double)metrics.TN / (double)(metrics.TN + metrics.FP), 2);
            double accuracy = Math.Round((double)(metrics.TP + metrics.TN) / (double)(metrics.TP + metrics.TN + metrics.FP + metrics.FN), 2);

            Precision.text = precision.ToString();
            Sensivity.text = sensivity.ToString();
            NPV.text = negativePredictiveValue.ToString();
            Specificity.text = specificity.ToString();
            Accuracy.text = accuracy.ToString();
        }
    }
}