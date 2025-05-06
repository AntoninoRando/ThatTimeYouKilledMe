using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Scripts that manages the movement of an object if dragging the mouse.
/// </summary>
public class PawnMovement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{    
    FollowPoint controller;

    #region UNITY LIFECYCLE ----------------------------------------------------

    void Awake()
    {
        controller = new()
        {
            DragSpeed = float.NaN,
            PivotGenerator = () =>
            {
                var v = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                return (v.x, v.y, v.z);
            }
        };
    }

    // void Update() {}

    #endregion -----------------------------------------------------------------

    public void OnBeginDrag(PointerEventData eventData)
    {
        controller.Start();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var move = controller.Update(Time.deltaTime);
        transform.Translate(new Vector3(move.Item1, move.Item2, move.Item3), Space.World);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        controller.Stop();
    }
}
