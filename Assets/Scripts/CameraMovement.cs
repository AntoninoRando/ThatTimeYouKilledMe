using UnityEngine;

/// <summary>
/// Scripts that manages the movement of an object if dragging the mouse.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    /// <summary>
    /// Speed of camera movement during drag.
    /// </summary>
    [Tooltip("Speed of camera movement during drag.")]
    [Range(MinDragSpeed, MaxDragSpeed)]
    public float DragSpeed = 2.0f;
    public const float MinDragSpeed = 0.0f;
    public const float MaxDragSpeed = 5.0f;

    FollowPoint controller;

    #region UNITY LIFECYCLE ----------------------------------------------------

    void Awake()
    {
        controller = new(MinDragSpeed, MaxDragSpeed)
        {
            DragSpeed = DragSpeed,
            PivotGenerator = () =>
            {
                return (Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);
            }
        };
    }

    void Update()
    {
        // Check for left mouse button press
        if (Input.GetMouseButtonDown(1))
        {
            controller.Start();
        }

        // Stop dragging when the left mouse button is released
        if (Input.GetMouseButtonUp(1))
        {
            controller.Stop();
        }

        if (controller.IsActive)
        {
            var move = controller.Update(Time.deltaTime);
            transform.Translate(new Vector3(-move.Item1, -move.Item2, move.Item3), Space.World);
        }
    }

    #endregion -----------------------------------------------------------------
}
