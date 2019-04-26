using UnityEngine;
using UnityEngine.EventSystems;

public delegate void SwipeEventHandler(Vector2 dir);

public class SwipeController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public static event SwipeEventHandler OnSwipe;

    #region Properties
    [SerializeField]
    float smoothing;

    private Vector2 origin;
    private Vector2 direction;
    private Vector2 smoothDirection;
    private bool touched;
    private int pointerID;
    #endregion

    #region Basic methods
    private void Awake()
    {
        touched = false;
        direction = Vector2.zero;
    }
    #endregion

    #region Control methods
    public void OnPointerDown(PointerEventData data)
    {
        if (!touched)
        {
            touched = true;
            pointerID = data.pointerId;
            origin = data.position;
        }
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.pointerId == pointerID)
        {
            Vector2 currentPosition = data.position;
            Vector2 directionRaw = currentPosition - origin;
            direction = directionRaw.normalized;
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        Debug.LogFormat("Swipe {0}, {1}", GetDirection().x, GetDirection().y);
		//OnSwipe(GetDirection());

		Debug.Log("released");

		GetComponent<Bubble>().AddForceDirection(GetDirection());

        if (data.pointerId == pointerID)
        {
            direction = Vector2.zero;
            touched = false;
        }
    }

    public Vector2 GetDirection()
    {
        smoothDirection = Vector2.MoveTowards(smoothDirection, direction, smoothing * Time.deltaTime);
        return smoothDirection;
    }
    #endregion
}