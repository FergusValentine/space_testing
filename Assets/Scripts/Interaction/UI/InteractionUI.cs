using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text key;
    [SerializeField] private TMP_Text promptText;
    
    void Awake()
    {
        transform.gameObject.SetActive(false);
    }

    public void SetVisible(bool isVisible)
    {
        transform.gameObject.SetActive(isVisible);
    }

    public void SetPrompt(InteractionPrompt prompt)
    {
        this.promptText.text = prompt.promptText;

        SetVisible(true);
    }
}
