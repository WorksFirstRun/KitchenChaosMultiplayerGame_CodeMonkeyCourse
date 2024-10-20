
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconScriptUi : MonoBehaviour
{
    [SerializeField] private Image spriteIcon;

    public void ChangeSprite(KitchenObjectSO kitchenObjectSo)
    {
        spriteIcon.sprite = kitchenObjectSo.sprite;
    }

}
