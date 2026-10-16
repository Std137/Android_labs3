using UnityEngine;
using UnityEngine.InputSystem.XR;

public class TouchPlayerControl : MonoBehaviour
{
    private float speed = 15f;

    [Header("Объект игрок")]
    private CharacterController controller;
    public FixedJoystick joystick;
    
    private Vector3 movementPatch;

    private bool stopGame = true;

    void Start() => controller = GetComponent<CharacterController>();

    void Update()
    {
        Vector2 vectorMove = joystick.Direction;
        var vectorEuler = Quaternion.Euler(0f, transform.eulerAngles.y, 0f);
        float moveSpeed = speed * joystick.Direction.magnitude;
        Vector3 vectorForward = vectorEuler * Vector3.forward;
        Vector3 vectorOffset = vectorEuler * Vector3.right;
        Vector3 direction = vectorForward * vectorMove.y + vectorOffset * vectorMove.x;
        if (direction.magnitude > 0.001f)
        {
            direction.Normalize();
            movementPatch = direction * moveSpeed * Time.deltaTime;
        }
        else
            movementPatch = Vector3.zero;
        if (!stopGame) controller.Move(movementPatch);
    }

    public void PlayerTeleport(Vector3 position, Vector3 lookPoint)
    {
        controller.enabled = false;
        transform.position = position;
        Physics.SyncTransforms();
        controller.enabled = true;
        Vector3 direction = lookPoint - transform.position;
        if (direction.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        
    }
    public void startMove() => stopGame = false;
    public void stopMove() => stopGame = true;
}
