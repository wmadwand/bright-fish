using UnityEngine;

public class SwipeTrail : MonoBehaviour
{
    #region Properties
    [SerializeField]
    Transform trailMouse;

    private float speedX;
    private float speedY;
    #endregion

    #region Basic methods
    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        speedX = Input.GetAxisRaw("Mouse X");
        speedY = Input.GetAxisRaw("Mouse Y");
        Vector2 speedMouse = new Vector2(speedX, speedY);

        if (Input.GetMouseButton(0))
        {
            if (speedMouse.magnitude > 1)
            {
                UpdateTrailPosition(mousePos);
            }
        }
    }
    #endregion

    #region Control methods
    private void UpdateTrailPosition(Vector2 pos)
    {
        trailMouse.position = new Vector3(pos.x, pos.y, 0);
    }
    #endregion
}