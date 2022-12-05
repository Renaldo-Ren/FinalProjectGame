using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string header;

    [Multiline()]
    public string content;
    public void OnPointerEnter(PointerEventData eventData)
    {
        ToolTipManage.Show(content, header);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipManage.Hide();
    }
}
