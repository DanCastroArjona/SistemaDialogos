using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace DialogSystem
{
    public enum DialogOption
    {
        Interactive,
        EndDialog,
        Normal,
    }

    public class DialogBlock : MonoBehaviour
    {
        #region Variables & References

        private AudioSource _audioSource;
        private DialogText currentText;

        public GameManager _gameManager;

        public AudioClip clipChar;
        public TextMeshProUGUI characterText, dialogText, answerA, answerB;
        public Image selectionImage;

        private DialogOption _dialogOption;
        
        #endregion

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            _gameManager = FindObjectOfType<GameManager>();
        }

        private void Start()
        {
            _audioSource.clip = clipChar;
            ImageEnabled(false);
            AllTextVoid();
        }

        public void NewLineDialog(DialogText text)
        {
            if (text == null)
            {
                AllTextVoid();
                _dialogOption = DialogOption.EndDialog;
                return;
            } 

            StopAllCoroutines();

            currentText = text;
            
            AllTextVoid();
            selectionImage.enabled = currentText.question;

            if (GetQuestionState())
            {
                answerA.text = _gameManager.GetDialog(currentText.answerA).dialog;
                answerB.text = _gameManager.GetDialog(currentText.answerB).dialog;
            }

            StartCoroutine(NewLine());
        }

        IEnumerator NewLine()
        {
            characterText.text = currentText.character;

            foreach (char c in currentText.dialog)
            {
                dialogText.text = dialogText.text + c;
                if (!_audioSource.isPlaying)
                    _audioSource.Play();
                yield return new WaitForSeconds(_audioSource.clip.length/4);
            }
        }

        public DialogOption GetNextDialogState()
        {
            if (currentText.answerA != -1)
            {
                if (currentText.interactive)
                    _dialogOption = DialogOption.Interactive;
                else
                    _dialogOption = DialogOption.Normal;
            }
            else
                _dialogOption = DialogOption.EndDialog;

            return _dialogOption;
        }

        public int GetNextDialogID()
        { return currentText.answerA; }

        public int GetEventID()
        { return currentText.eventId; }

        public bool InteractiveCurrentDialog()
        { return currentText.interactive; }

        public bool GetQuestionState()
        { return currentText.question; }

        public int GetCurrentDialogID()
        { return currentText.id; }

        public void ImageEnabled(bool state)
        { selectionImage.enabled = state; }

        public void AllTextVoid()
        {
            TextVoid(characterText);
            TextVoid(dialogText);
            TextVoid(answerA);
            TextVoid(answerB);
            ImageEnabled(false);
        }

        void TextVoid(TextMeshProUGUI textMesh)
        { textMesh.text = ""; }

        
    }
}