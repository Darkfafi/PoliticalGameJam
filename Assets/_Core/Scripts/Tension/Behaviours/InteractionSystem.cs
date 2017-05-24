using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class InteractionSystem : MonoBehaviour
{
    public enum InteractionType
    {
        Pushing,
        Fighting,
        Murder
    }

    public delegate void InteractionTypeDelegate(InteractionType interactionType);
    public event InteractionTypeDelegate InteractionEndedEvent;

    private List<InteractionProcess> _interactions = new List<InteractionProcess>();

	public void TryInteractionMatch(InteractionType interactionType, Vector2 interactionPosition)
    {
        Police police = Interactor.GetFreeInteractor(FindObjectsOfType<Police>());
        Protester protester = Interactor.GetFreeInteractor(FindObjectsOfType<Protester>());
        if (police == null || protester == null) { return; }
        _interactions.Add(new InteractionProcess(interactionType, protester, police, interactionPosition));
    }

    protected void Update()
    {
        for(int i = _interactions.Count - 1; i >= 0; i--)
        {
            _interactions[i].Update();

            if (_interactions[i].HasEnded)
            {
                if(InteractionEndedEvent != null)
                {
                    InteractionEndedEvent(_interactions[i].InteractionType);
                }
                _interactions.RemoveAt(i);
            }
        }
    }

    public class InteractionProcess
    {
        public bool HasEnded { get; private set; }

        public InteractionType InteractionType { get; private set; }
        private Protester _protester;
        private Police _police;
        private Vector2 _interactionPosition;
        private Vector2 _protesterPos;
        private Vector2 _policePos;

        public InteractionProcess(InteractionType type, Protester protester, Police police, Vector2 interactionPosition)
        {
            InteractionType = type;
            _protester = protester;
            _police = police;
            _interactionPosition = interactionPosition;

            _policePos = _interactionPosition;
            _protesterPos = _interactionPosition;

            _policePos.x += ((RectTransform)police.transform).rect.width / 2 + 5;
            _protesterPos.x -= ((RectTransform)protester.transform).rect.width / 2 + 5;

            FaceOneAnother();
        }

        public void Update()
        {
            if(((RectTransform)_police.transform).anchoredPosition.x == _policePos.x && ((RectTransform)_police.transform).anchoredPosition.y == _policePos.y
               && ((RectTransform)_protester.transform).anchoredPosition.x == _protesterPos.x && ((RectTransform)_protester.transform).anchoredPosition.y == _protesterPos.y)
            {
                doAnimationType();
            }
        }

        private void doAnimationType()
        {
            Debug.Log("Animation");
            HasEnded = true;
        }

        private void FaceOneAnother()
        {
            RectTransform t;
            _protester.WalkTo(_protesterPos, 10);
            _police.WalkTo(_policePos, 10);
        }
    }
}
