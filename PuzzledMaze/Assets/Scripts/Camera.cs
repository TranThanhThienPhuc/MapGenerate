using UnityEngine;

public class Camera : MonoBehaviour
{
    public float viewSize = 90;
    public float followSize = 30;
    public Transform pl;
    public bool isFollowing = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isFollowing = !isFollowing;
        }


        if (pl == null)
        {
            pl = GameObject.FindGameObjectWithTag("Player").transform;
            return;
        }

        float speed = 10;
        Vector3 targetPosition = new Vector3(0, isFollowing ? followSize : viewSize, 0);
        
        //Toggle to make camera either follow player or fixed
        if (isFollowing) targetPosition = new Vector3(pl.position.x, targetPosition.y, pl.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPosition, isFollowing ? Time.deltaTime * speed : 1);
    }
}