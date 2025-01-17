using UnityEngine;
using UnityEngine.UIElements;

public class ExitPanel : MonoBehaviour
{
    public GameObject Target;
    public RectTransform fromPosition;
    public RectTransform toPosition;

    private void OnEnable()
    {
        // Animate only the Y position from `fromPosition` to `toPosition`
        LeanTween.value(gameObject, fromPosition.position.y, toPosition.position.y, 0.2f)
                 .setEase(LeanTweenType.easeOutQuad) // Set ease-out effect
                 .setOnUpdate((float newY) =>
                 {
                     MovePanel(newY);
                 });
    }

    void MovePanel(float newY)
    {
        // Update the target's position
        Vector3 newPosition = Target.transform.position;
        newPosition.y = newY;
        Target.transform.position = newPosition;
    }


    public void ExitGame() // For Main Menu
    {
#if UNITY_EDITOR
        // Exit play mode in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // Quit the application on mobile or standalone builds
        Application.Quit();
#endif
    }

    public void ExitLobby() // for gameplay 
    {

    }

}
