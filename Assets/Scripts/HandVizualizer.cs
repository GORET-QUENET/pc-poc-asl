using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HandPositionReader.Scripts
{
    public class HandVizualizer : MonoBehaviour
    {
        private string _fileContent;
        private int _currentPosition = 0;
        private int _nbPositions;
        private bool _isRunning = false;
        private string _assetPath;

        [Header("Jonctions de la main")]
        [Tooltip("Doit contenir 26 objets enfants.")]
        public GameObject HandJoins;

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

        private void Start()
        {
            _assetPath = Application.dataPath;
            HandJoins = Instantiate(HandJoins);
        }

        public HandVizualizer()
        {

        }

        public HandVizualizer(string assetPath) : base()
        {
            _assetPath = assetPath;
        }

        public HandVizualizer(string assetPath, string fileName) : base()
        {
            _assetPath = assetPath;
            string filePath = string.Format("{0}/{1}.txt", _assetPath, fileName);

            StreamReader reader = new StreamReader(filePath);
            _fileContent = reader.ReadToEnd();

            _isRunning = true;

            List<string> lines = _fileContent.Split('\n').ToList();
            _nbPositions = lines.Where(l => l.Contains("START")).Count();
        }

        /// <summary>
        /// Cette fonction est trigger par le click du bouton StartButton.
        /// </summary>
        public void StartVizualisation()
        {
            if (!_isRunning)
            {
                string filePath = string.Format("{0}/{1}.txt", _assetPath, FileName.text);
                try
                {
                    StreamReader reader = new StreamReader(filePath);
                    _fileContent = reader.ReadToEnd();
                    ErrorMessage.SetActive(false);

                    _isRunning = true;

                    List<string> lines = _fileContent.Split('\n').ToList();
                    _nbPositions = lines.Where(l => l.Contains("START")).Count();

                    StartCoroutine(ShowHand());
                }
                // Cette erreur arrive lorsque le nom du fichier dans l'InputField n'existe pas.
                catch (Exception ex)
                {
                    Debug.LogError(ex);
                    ErrorMessage.SetActive(true);
                }
            }
        }

        /// <summary>
        /// Cette fonction est trigger par le click sur le bouton PauseButton.
        /// </summary>
        public void PauseVizualisation()
        {
            _isRunning = false;
        }

        /// <summary>
        /// Cette fonction ce lance dans une Coroutine et va afficher une main animé.
        /// </summary>
        /// <returns></returns>
        public IEnumerator ShowHand()
        {
            Debug.Log("start " + _isRunning);
            while (_isRunning)
            {
                float time = (1f / slider.value) * 4;

                if (_currentPosition >= _nbPositions)
                    _currentPosition = 0;

                List<Vector3> positions = GetPositions(_fileContent, _currentPosition, 3);

                DrawHand(positions, LineRenderer);

                for (int i = 0; i < positions.Count; i++)
                {
                    HandJoins.transform.GetChild(i).position = positions[i];
                }

                _currentPosition++;
                yield return new WaitForSeconds(time);
            }
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

        /// <summary>
        /// Cette fonction va dessiner les traits entre chaque jonctions de le main avec un lineRenderer.
        /// </summary>
        /// <param name="positions"> Liste des positions des jonctions de la main. </param>
        /// <param name="lineRenderer"> lineRenderer qui sera afficher pour lier chaque jonctions. </param>
        public void DrawHand(List<Vector3> positions, LineRenderer lineRenderer)
        {
            if (positions == null || lineRenderer == null)
            {
                Debug.Log("Can't DrawHand of null values");
                return;
            }

            if (positions.Count < 2 || lineRenderer.positionCount < 2)
            {
                Debug.Log("Can't DrawHand of to small arrays");
                return;
            }

            //0 : Position de la paume
            lineRenderer.SetPosition(0, positions[0]);
            //1 : Postion du poignet
            lineRenderer.SetPosition(1, positions[1]);

            //Le premier doigt est le pouce qui est moins long
            DrawFinger(positions, lineRenderer, 4, 2, 2);
            DrawFinger(positions, lineRenderer, 5, 10, 6);
            DrawFinger(positions, lineRenderer, 5, 20, 11);
            DrawFinger(positions, lineRenderer, 5, 30, 16);
            DrawFinger(positions, lineRenderer, 5, 40, 21);
        }

        /// <summary>
        /// Cette fonction fait un allé / retour sur le doigt pour le dessin.
        /// Donc l'indexLine avance de 2 * length.
        /// Alors que l'indexPosition avance d'une fois length.
        /// </summary>
        /// <param name="positions"> La liste des positions des jonctions. </param>
        /// <param name="lineRenderer"> Le lineRenderer qui va permettre d'afficher le doigt. </param>
        /// <param name="length"> Le nombre de jonctions de ce doigt. </param>
        /// <param name="indexLine"> L'indexe de départ pour le lineRendere. </param>
        /// <param name="indexPosition"> L'indexe de départ pour le tables de positions. </param>
        public void DrawFinger(List<Vector3> positions, LineRenderer lineRenderer, int length, int indexLine, int indexPosition)
        {
            if (indexPosition + length > positions.Count
                || indexLine + (length * 2) > lineRenderer.positionCount)
                return;

            int direction = 1;
            int i;

            for (i = 0; i < length * 2; i++)
            {
                //Si nous arrivons au bout du doigt nous revenons en arrière.
                if (i == length - 1)
                    direction = -1;

                lineRenderer.SetPosition(indexLine + i, positions[indexPosition]);
                indexPosition += direction;
            }

            //Le dessin du doigt se fini toujours sur la paume donc index 0.
            lineRenderer.SetPosition(indexLine + i - 1, positions[0]);
        }
    }
}