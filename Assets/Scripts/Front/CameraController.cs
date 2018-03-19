using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Used in options //
    public bool allowMouseScroll;
    public float scrollSpeed = 5;

    private float scrollZone = 15;
    private Vector3 desiredPosition;

    private void Start()
    {
        desiredPosition = transform.position;
    }

    private void Update()
    {
        float posX = 0, posZ = 0;
        float speed = scrollSpeed * Time.deltaTime;

        if (allowMouseScroll)
        {
            if (Input.mousePosition.x < scrollZone || Input.GetKey("a"))
                posX -= speed;
            else if (Input.mousePosition.x > Screen.width - scrollZone || Input.GetKey("d"))
                posX += speed;

            if (Input.mousePosition.y < scrollZone || Input.GetKey("s"))
                posZ -= speed;
            else if (Input.mousePosition.y > Screen.height - scrollZone || Input.GetKey("w"))
                posZ += speed;
        }
        else
        {
            if (Input.GetKey("a"))
                posX -= speed;
            else if (Input.GetKey("d"))
                posX += speed;

            if (Input.GetKey("s"))
                posZ -= speed;
            else if (Input.GetKey("w"))
                posZ += speed;
        }

        if (posX != 0 && posZ != 0)
        {
            posX /= 1.33f;
            posZ /= 1.33f;
        }

        Vector3 move = new Vector3(posX, 0, posZ) + desiredPosition;
        desiredPosition = move;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.2f);
    }

    private void LateUpdate()
    {

    }
}
