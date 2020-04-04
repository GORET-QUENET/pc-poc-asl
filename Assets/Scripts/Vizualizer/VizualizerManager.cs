using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HandPositionReader.Scripts.Vizualizer
{
    public class VizualizerManager : MonoBehaviour
    {
        private HandVizualizer _handVizualizer;
        private bool _isRunning = false;

        [Header("Jonctions de la main")]
        [Tooltip("Doit contenir 26 objets enfants.")]
        public GameObject HandJointObject;

        [Header("Components du GameObject")]
        [Tooltip("Permet d'afficher les traits entre chaque jonctions de la main.")]
        public LineRenderer LineRenderer;

        [Header("Objets de l'UI")]
        [Tooltip("Slider qui permet de controler la vitesse d'affichage.")]
        public Slider slider;

        [Tooltip("InputField qui permet d'entre le nom du fichier à lire pour récupérer les positions des jonctions de la main.")]
        public TMP_InputField FileName;

        [Tooltip("Message à afficher lorsque le nom de fichier dans l'InputField n'existe pas.")]
        public GameObject ErrorMessage;

        [Tooltip("Un texte pour afficher la position actuellement visualisé.")]
        public TMP_Text PositionIndexe;

        // Start is called before the first frame update
        void Start()
        {
            HandJointObject = Instantiate(HandJointObject);
            _handVizualizer = new HandVizualizer(Application.dataPath);
            _handVizualizer.ErrorDetected += ErrorTrigerred;
        }

        public void StartButtonClicked()
        {
            if (!_isRunning)
            {
                ErrorMessage.SetActive(false);
                _isRunning = true;
                _handVizualizer.InitVizualisation(FileName.text);

                StartCoroutine(HandLoop());
            }
        }

        public void PauseButtonClicked()
        {
            _isRunning = false;
        }

        public IEnumerator HandLoop()
        {
            while (_isRunning)
            {
                float time = (1f / slider.value) * 4;

                string text = string.Format("Position : {0}",_handVizualizer.ShowHand(LineRenderer, HandJointObject));
                PositionIndexe.text = text;

                yield return new WaitForSeconds(time);
            }
        }

        public void ErrorTrigerred(object sender, EventArgs e)
        {
            _isRunning = false;
            ErrorMessage.SetActive(true);
        }
    }
}
