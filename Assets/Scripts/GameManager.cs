using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem
{
    public sealed class GameManager : MonoBehaviour
    {
        #region Variables & References

        private DialogBlock _dialogBlock;
        private LoadText _loadText;

        public UnityEvent[] events;
        #endregion

        private void Awake()
        {
            _dialogBlock = FindObjectOfType<DialogBlock>();
            _loadText = GetComponent<LoadText>();
        }

        private void Start()
        {
            NewDialog(0);
        }

        private void Update()
        {

            float horizontal = Input.GetAxisRaw("Horizontal");
            if (horizontal > 0f)
                QuestionDialog(true);
            else if (horizontal < 0f)
                QuestionDialog(false);


            if (Input.GetKeyDown(KeyCode.Space))
                NextDialog();
        }

        public void NewDialog(int id)
        {

            _dialogBlock.NewLineDialog(GetDialog(id));

            int eventID = _dialogBlock.GetEventID();
            if (eventID != -1 && eventID < events.Length)
                events[eventID].Invoke();
        }

        public void NextDialog()
        {
            if (_dialogBlock.GetQuestionState())
                return;     
            
            if(_dialogBlock.GetNextDialogState() != DialogOption.EndDialog)
                NewDialog(_dialogBlock.GetNextDialogID());
        }

        public void QuestionDialog(bool election)
        {
            if (_dialogBlock.GetQuestionState())
            {
                DialogText text = GetDialog(_dialogBlock.GetCurrentDialogID());
                _dialogBlock.NewLineDialog((election) ? GetDialog(GetDialog(text.answerB).answerA) : GetDialog(GetDialog(text.answerA).answerA));
            }
        }

        public DialogText GetDialog(int id)
        {
            return _loadText.GetDialog(id);
        }
    }
}