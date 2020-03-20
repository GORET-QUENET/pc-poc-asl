using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace HandPositionReader.Scripts.Vizualizer
{
    public class HandVizualizer
    {
        private string _fileContent;
        private int _currentHandJoint = 0;
        private int _nbHandJoint;
        private string _assetPath;

        public event EventHandler ErrorDetected;


        public HandVizualizer(string assetPath)
        {
            _assetPath = assetPath;
        }
        
        /// <summary>
        /// Cette fonction va nous permettre de préparer l'affichage de la main et vérifier que le fichier indiqué existe.
        /// </summary>
        /// <param name="fileName"> Le nom du fichier à lire. </param>
        public void InitVizualisation(string fileName)
        {
            try
            {
                string filePath = string.Format("{0}/{1}.txt", _assetPath, fileName);
                using (StreamReader reader = new StreamReader(filePath))
                {
                    _fileContent = reader.ReadToEnd();
                }

                List<string> lines = _fileContent.Split('\n').ToList();
                _nbHandJoint = lines.Where(l => l.Contains("START")).Count();
            }
            // Cette erreur arrive lorsque le nom du fichier fileName n'existe pas.
            catch (Exception ex)
            {
                Debug.LogError(ex.Message);
                ErrorDetected?.Invoke(this, null);
            }
        }

        /// <summary>
        /// Cette fonction va afficher une main à l'aide d'un LineRenderer qui joindra tous les joints dans le handJointObject
        /// </summary>
        /// <param name="lineRenderer"> Ligne qui va relier les joints de la main. </param>
        /// <param name="handJointObject"> Object qui contient les 26 joints de la main. </param>
        public void ShowHand(LineRenderer lineRenderer, GameObject handJointObject)
        {
            if (lineRenderer == null || handJointObject == null)
                return;

            if (_currentHandJoint >= _nbHandJoint)
                _currentHandJoint = 0;

            List<Vector3> handJoint = FileReader.GetHandJoint(_fileContent, _currentHandJoint, 3);

            DrawHandLine(handJoint, lineRenderer);

            for (int i = 0; i < handJoint.Count; i++)
            {
                handJointObject.transform.GetChild(i).position = handJoint[i];
            }

            _currentHandJoint++;
        }

        /// <summary>
        /// Cette fonction va dessiner les traits entre chaque jonctions de le main avec un lineRenderer.
        /// </summary>
        /// <param name="handJoint"> Liste des positions des jonctions de la main. </param>
        /// <param name="lineRenderer"> lineRenderer qui sera afficher pour lier chaque jonctions. </param>
        public void DrawHandLine(List<Vector3> handJoint, LineRenderer lineRenderer)
        {
            if (handJoint == null || lineRenderer == null)
            {
                Debug.Log("Can't DrawHand of null values");
                return;
            }

            if (handJoint.Count < 2 || lineRenderer.positionCount < 2)
            {
                Debug.Log("Can't DrawHand of to small arrays");
                return;
            }

            // 0 : Position de la paume
            lineRenderer.SetPosition(0, handJoint[0]);
            // 1 : Postion du poignet
            lineRenderer.SetPosition(1, handJoint[1]);

            // Le premier doigt est le pouce qui est moins long
            DrawFinger(handJoint, lineRenderer, 4, 2, 2);
            DrawFinger(handJoint, lineRenderer, 5, 10, 6);
            DrawFinger(handJoint, lineRenderer, 5, 20, 11);
            DrawFinger(handJoint, lineRenderer, 5, 30, 16);
            DrawFinger(handJoint, lineRenderer, 5, 40, 21);
        }

        /// <summary>
        /// Cette fonction fait un allé / retour sur le doigt pour le dessin.
        /// Donc l'indexLine avance de 2 * length.
        /// Alors que l'indexPosition avance d'une fois length.
        /// </summary>
        /// <param name="handJoint"> La liste des positions des jonctions. </param>
        /// <param name="lineRenderer"> Le lineRenderer qui va permettre d'afficher le doigt. </param>
        /// <param name="length"> Le nombre de jonctions de ce doigt. </param>
        /// <param name="indexLine"> L'indexe de départ pour le lineRendere. </param>
        /// <param name="indexPosition"> L'indexe de départ pour le tables de positions. </param>
        public void DrawFinger(List<Vector3> handJoint, LineRenderer lineRenderer, int length, int indexLine, int indexPosition)
        {
            if (indexPosition + length > handJoint.Count
                || indexLine + (length * 2) > lineRenderer.positionCount)
                return;

            int direction = 1;
            int i;

            for (i = 0; i < length * 2; i++)
            {
                // Si nous arrivons au bout du doigt nous revenons en arrière.
                if (i == length - 1)
                    direction = -1;

                lineRenderer.SetPosition(indexLine + i, handJoint[indexPosition]);
                indexPosition += direction;
            }

            // Le dessin du doigt se fini toujours sur la paume donc index 0.
            lineRenderer.SetPosition(indexLine + i - 1, handJoint[0]);
        }
    }
}