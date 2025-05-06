using UnityEngine;

public class IsAboveChecker : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 10f;
    [SerializeField] private bool debugRay = true;
    [SerializeField] private Vector3 raycastDirection = Vector3.back;

    public bool IsAboveCell { get; private set; }
    public GameObject UnderObject { get; private set; }
    public Color prevColor;

    #region UNITY LIFECYCLE ----------------------------------------------------

    void Update()
    {
        CheckIfAboveCell();
    }

    #endregion -----------------------------------------------------------------

    void CheckIfAboveCell()
    {
        // Cast ray downward from this object
        bool hitSomething = Physics.Raycast(transform.position, raycastDirection, out RaycastHit hit, raycastDistance);

        // Draw debug ray if enabled
        if (debugRay)
        {
            Debug.DrawRay(transform.position, raycastDirection * raycastDistance, hitSomething ? Color.green : Color.red);
        }

        // Check if we hit something with the "Cell" tag
        if (hitSomething && hit.collider.CompareTag("Cell"))
        {
           
            var hitObject = hit.collider.gameObject;
            if (hitObject == UnderObject)
            {
                return;
            }

            IsAboveCell = true;
            if (UnderObject != null)
            {
                UnderObject.GetComponent<Renderer>().material.color = prevColor;
            }
            prevColor = hitObject.GetComponent<Renderer>().material.color;
            prevColor = new Color(prevColor.r, prevColor.g, prevColor.b);
            UnderObject = hit.collider.gameObject;
            Color unityColor = new(0, 0, 255);
            UnderObject.GetComponent<Renderer>().material.color = unityColor;
        }
        else
        {
            IsAboveCell = false;
            if (UnderObject != null)
            {
                UnderObject.GetComponent<Renderer>().material.color = prevColor;
            }
            UnderObject = null;
        }
    }
}