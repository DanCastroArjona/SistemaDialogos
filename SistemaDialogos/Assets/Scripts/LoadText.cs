using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace DialogSystem
{
    public sealed class LoadText : MonoBehaviour
    {
        #region Variables

        List<DialogText> dialogs = new List<DialogText>();

        private TextAsset _dialogData;
        [BoxGroup("Dialog Files")]
        public TextAsset[] dialogFiles;

        private int _languageCode = 0;
        #endregion

        private void Start()
        {
            switch (_languageCode)
            {
                default:            //spanish, agregar tantos como idiomas a introducir
                    _dialogData = dialogFiles[0];
                    break;  
            }

            string[] dialogSplit = _dialogData.text.Split(new char[] { '\n' });
            for (int i = 1; i < dialogSplit.Length; i++)
            {
                string[] col = dialogSplit[i].Split(new char[] { ';' });
                if (col[1] != "")
                {
                    DialogText dialog = new DialogText(col[0], col[1], col[2], col[3], col[4], col[5], col[6], col[7]);
                    dialogs.Add(dialog);
                }
            }
        }

        public DialogText GetDialog(int id)
        {
            try
            {
                return dialogs[id];
            }
            catch
            {
                return null;
            }
        }
    }

    public class DialogText
    {
        private int _id, _ansA, _ansB, _evId;
        private string _cha, _dia;
        private bool _ques, _int;

        public int id { get { return _id; } }           //Id de cada linea de diálogo
        public int answerA { get { return _ansA; } }       //Id de la respuesta A, en caso de que exista, sino -1
        public int answerB { get { return _ansB; } }       //Id de la respuesta B, en caso de que exista, sino -1
        public int eventId { get { return _evId; } }       //Id del evento a ejecutar, en caso de que exista, sino -1
        
        public string character { get { return _cha; } }  //Nombre del emisor
        public string dialog { get { return _dia; } }         //Dialogo
        
        public bool question { get { return _ques; } }          //True si es pregunta
        public bool interactive { get { return _int; } }        //True si es respuesta

        public DialogText(string id, string character, string dialog, string question, string interactive,  //Constructor
            string answerA, string answerB, string eventId)
        {
            int.TryParse(id, out _id);
            int.TryParse(answerA, out _ansA);
            int.TryParse(answerB, out _ansB);
            int.TryParse(eventId, out _evId);
            _cha = character;
            _dia = dialog;
            _ques = (question == "t") ? true : false;
            _int = (interactive == "t") ? true : false;
        }
    }
}