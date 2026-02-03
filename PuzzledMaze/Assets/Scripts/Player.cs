using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Range(0, 10)]
    public float speed;

    [Range(0, 10)]
    public float acceleration;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        Vector3 velosety = new Vector3 
        {
            x = Input.GetAxisRaw("Horizontal"),
            y = 0,
            z = Input.GetAxisRaw("Vertical")
        } * speed;
        rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, velosety, Time.deltaTime * acceleration);
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SceneManager.LoadScene("Lobby");
        }
    }
}
