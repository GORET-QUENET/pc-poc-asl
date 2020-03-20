using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HandPositionReader.Scripts
{
    public static class FileReader
    {
        /// <summary>
        /// Cette fonction va parcourir chaque ligne du fichier file et retourner le squelette entre #START# et #END#.
        /// </summary>
        /// <param name="file"> Contenue du fichier à parcourir. </param>
        /// <param name="target"> Index du squelette voulue. </param>
        /// <param name="scale"> Un facteur permettant d'agrandir l'espace entre chaque points. </param>
        /// <returns> La liste le squelette demandé du fichier. </returns>
        public static List<Vector3> GetHandJoint(string file, int target, int scale)
        {
            List<string> lines = file.Split('\n').ToList();
            List<Vector3> handJoint = new List<Vector3>();

            int currentHandJoint = 0;
            bool canReadHandJoint = false;

            foreach (var line in lines)
            {
                if (line.Contains("#START#"))
                {
                    if (currentHandJoint == target)
                        canReadHandJoint = true;
                }
                else if (line.Contains("#END#"))
                {
                    currentHandJoint++;

                    // Stop la lecture du fichier car nous venons de lire toutes les positions.
                    if (canReadHandJoint)
                        break;
                }
                else if (canReadHandJoint)
                {
                    handJoint.Add(ToVector3(line, '|', scale));
                }
            }

            return handJoint;
        }

        /// <summary>
        /// Cette fonction va convertir en Vector3 un string de la forme "2.3|3.3|2.0".
        /// </summary>
        /// <param name="line"> String à convertir en Vector3. </param>
        /// <param name="separator"> Séparateur entre chaque valeurs. </param>
        /// <param name="scale"> Un facteur permettant d'agrandir l'espace entre chaque points. </param>
        /// <returns> Le Vector3 obtenue du string en param. </returns>
        public static Vector3 ToVector3(string line, char separator, int scale)
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
                Debug.LogError(ex.Message);
                return Vector3.zero;
            }
        }
    }
}
