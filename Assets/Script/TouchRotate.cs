using UnityEngine;
using UnityEngine.EventSystems;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class TouchRotate: MonoBehaviour
{
    [Header("Персонаж")]
    public Transform target;

    [Header("Параметры вращения")]
    public float horizontalSpeed = 13f;
    public float verticalSpeed = 5f;

    private float maxPitch = 80f;
    private float minPitch = 80f;         

    // Внутренние состояния
    private float currentYaw;              
    private float currentPitch;            

    void Start()
    {
        Vector3 euler = transform.eulerAngles;
        currentYaw = euler.y;
        currentPitch = euler.x;
        if (currentPitch > 180f) currentPitch -= 360f;
    }

    public void RotateCamera(float deltaX, float deltaY)
    {
        currentYaw += deltaX * horizontalSpeed * Time.deltaTime;
        currentPitch -= deltaY * verticalSpeed * Time.deltaTime; 
        currentPitch = Mathf.Clamp(currentPitch, -minPitch, maxPitch);
        Quaternion rotation = Quaternion.Euler(currentPitch, currentYaw, 0f);
        transform.rotation = rotation;
    }
}
