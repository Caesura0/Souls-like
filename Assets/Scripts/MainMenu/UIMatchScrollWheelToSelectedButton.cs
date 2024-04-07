using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JS
{
    public class UIMatchScrollWheelToSelectedButton : MonoBehaviour
    {
        [SerializeField] GameObject currentSelected;
        [SerializeField] GameObject previouslySelected;
        [SerializeField] RectTransform currnetlySelectedTransform;
        [SerializeField] RectTransform contentPanel;
        [SerializeField] ScrollRect scrollRect;


        private void Update()
        {
            currentSelected = EventSystem.current.currentSelectedGameObject;
            if(currentSelected != null )
            {
                if(currentSelected != previouslySelected )
                {
                    previouslySelected = currentSelected;
                    currnetlySelectedTransform = currentSelected.GetComponent<RectTransform>();
                    SnapTo(currnetlySelectedTransform);
                }
            }
        }

        void SnapTo(RectTransform target)
        {
            Canvas.ForceUpdateCanvases();

            Vector2 newPosition = (Vector2)scrollRect.transform.InverseTransformDirection(contentPanel.position) - (Vector2)scrollRect.transform.InverseTransformDirection(target.position);

            //only want to lock in the up and down
            newPosition.x = 0;

            contentPanel.anchoredPosition = newPosition;
        }
    }
}
