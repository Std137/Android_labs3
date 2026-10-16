using UnityEngine;

public class ExitCollaider: MonoBehaviour
{
    public LevelController LevelController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelController.OnWin();
        }
    }
}
