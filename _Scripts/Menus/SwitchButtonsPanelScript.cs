using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchButtonsPanelScript : MonoBehaviour
{
    [SerializeField] private Button[] buttons;

    public void SetAllButtonsInteractable()
    {
        foreach (Button button in buttons)
        {
            button.interactable = true;
        }
    }

    public void SetAllButtonsDisabled()
    {
        foreach (Button button in buttons)
        {
            button.interactable = false;
        }
    }

    public void OnButtonClicked_DisableAll(Button clickedButton)
    {
        int buttonIndex = System.Array.IndexOf(buttons, clickedButton);

        if (buttonIndex == -1)
            return;

        SetAllButtonsDisabled();

        KeepColorHighlited(clickedButton);
    }

    public void OnButtonClicked_DisableItself(Button clickedButton)
    {
        int buttonIndex = System.Array.IndexOf(buttons, clickedButton);

        if (buttonIndex == -1)
            return;

        SetAllButtonsInteractable();

        clickedButton.interactable = false;

        KeepColorHighlited(clickedButton);
    }

    public void KeepColorHighlited(Button button)
    {
        ColorBlock colors = button.colors;

        colors.disabledColor = button.colors.pressedColor;
        button.colors = colors;
    }

    public Button[] Buttons
    {
        get{return buttons;}
    }
}
