using UnityEngine;
using UnityEngine.InputSystem;

public class TouchPlayerControl : MonoBehaviour
{
    public float speed = 5f;

    [Header("Объект игрок")]
    private CharacterController controller;
    public FixedJoystick joystick;
    private Vector3 movementPatch;

    void Start() => controller = GetComponent<CharacterController>();

    void Update()
    {
        //Считываем джойстик
        Vector2 vectorMove = joystick.Direction;

        //Получаем направление в виде углов Эйлера 
        var vectorEuler = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);

        //Задаем направления по осям и суммируем
        Vector3 vectorForward = vectorEuler * Vector3.forward;
        Vector3 vectorOffset = vectorEuler * Vector3.right;
        Vector3 direction = vectorForward * vectorMove.y + vectorOffset * vectorMove.x;

        // Нормализуем итоговый вектор
        if (direction.magnitude > 0.001f)
        {
            direction.Normalize();
            movementPatch = direction * speed * Time.deltaTime;
        }
        else
            movementPatch = Vector3.zero;

        controller.Move(movementPatch);
    }
}
