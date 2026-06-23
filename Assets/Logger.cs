using UnityEngine;

public class Logger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool logToConsole = true;

    public void Log(string message)
    {
        if (logToConsole)
        {
            Debug.Log(message);
        }
    }
}
