using UnityEngine;
using UnityEngine.SceneManagement;

public class LetterCollaider: MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject); 
            ScoreManager.instance.ScoreUP();
        }
    }
}
