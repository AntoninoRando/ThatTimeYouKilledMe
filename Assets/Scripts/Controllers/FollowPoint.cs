using System;
using System.Diagnostics;

public class FollowPoint
{  
    #region FIELDS -------------------------------------------------------------
    public float DragSpeed;
    public float MinDragSpeed;
    public float MaxDragSpeed;

    (float, float, float) dragOrigin;
    bool isDragging;

    public bool IsActive => isDragging;
    public Func<(float, float, float)> PivotGenerator;
    #endregion -----------------------------------------------------------------



    #region CONSTRUCTORS -------------------------------------------------------
    public FollowPoint(float MinDragSpeed, float MaxDragSpeed)
    {
        this.MinDragSpeed = MinDragSpeed;
        this.MaxDragSpeed = MaxDragSpeed;
        DragSpeed = (MinDragSpeed + MaxDragSpeed) / 2;
    }

    public FollowPoint()
    {
        MinDragSpeed = float.NaN;
        MaxDragSpeed = float.NaN;
        DragSpeed = float.NaN;
    }
    #endregion -----------------------------------------------------------------


    
    /// <summary>
    /// Starts the movement of an origin point.
    /// </summary>
    /// <param name="origin"></param>
    public void Start((float, float, float) origin)
    {
        Debug.Assert(!isDragging, "Movement already ongoing");
        dragOrigin = origin;
        isDragging = true;
    }

    public void Start()
    {
        Debug.Assert(!isDragging, "Movement already ongoing");
        dragOrigin = PivotGenerator();
        isDragging = true;
    }

    /// <summary>
    /// Ends the movement of an ongoing movement.
    /// </summary>
    public void Stop()
    {
        Debug.Assert(isDragging, "No movement ongoing");
        isDragging = false;
    }

    /// <summary>
    /// Makes the movement progress.
    /// </summary>
    public (float, float, float) Update((float, float, float) pivotPoint, float deltaTime)
    {
        // Get the mouse movement delta
        (float, float, float) pointDelta = (
            pivotPoint.Item1 - dragOrigin.Item1,
            pivotPoint.Item2 - dragOrigin.Item2,
            pivotPoint.Item3 - dragOrigin.Item3
        );

        // Calculate movement based on drag speed and invert direction
        (float, float, float) move = (
            -pointDelta.Item1 * DragSpeed * deltaTime,
            -pointDelta.Item2 * DragSpeed * deltaTime,
            0f
        );

        // Update the drag origin to the current mouse position
        dragOrigin = pivotPoint;

        return move;
    }

    public (float, float, float) Update(float deltaTime)
    {
        var pivotPoint = PivotGenerator();

        // Get the mouse movement delta
        (float, float, float) pointDelta = (
            pivotPoint.Item1 - dragOrigin.Item1,
            pivotPoint.Item2 - dragOrigin.Item2,
            pivotPoint.Item3 - dragOrigin.Item3
        );

        // Calculate movement based on drag speed and invert direction
        var speed = float.IsNaN(DragSpeed) ? 1 : DragSpeed * deltaTime;
        (float, float, float) move = (
            pointDelta.Item1 * speed,
            pointDelta.Item2 * speed,
            0f
        );

        // Update the drag origin to the current mouse position
        dragOrigin = pivotPoint;

        return move;
    }
}