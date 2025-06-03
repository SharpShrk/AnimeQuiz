using UnityEngine;

public class QuitGame : MonoBehaviour
{
    [SerializeField] private QuestionDisplay _questionDisplay;

    public void ExitGame()
    {
        if (_questionDisplay != null)
            _questionDisplay.ReportSessionCompleted();
        else
            Debug.LogError("Объект отсутствует");
    } 
}