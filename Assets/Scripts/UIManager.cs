using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private InteractionUI interactionPromptUI;
    
    public void SetPromptUI(InteractionPrompt prompt)
    {
        interactionPromptUI.SetPrompt(prompt);
    }

    public void SetPromptUIVisible(bool isVisible)
    {
        interactionPromptUI.SetVisible(isVisible);
    }
}
