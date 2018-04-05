using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ButtonHover : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    // When highlighted with mouse.
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<Button>().onClick.Invoke();
    }

    // When selected.
    public void OnSelect(BaseEventData eventData)
    {
        GetComponent<Button>().onClick.Invoke();
    }
}