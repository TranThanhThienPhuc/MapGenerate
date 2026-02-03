using Unity.VisualScripting;
using UnityEngine;

public class Turtorial : MonoBehaviour
{
    public bool turtorialOn;
    public void TurtorialOn()
    {
        gameObject.SetActive(true);
    }
}
