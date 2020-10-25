
using UnityEngine;

public class RotateCamera : MonoBehaviour
{
    public float speed = 5f;
    private Transform Rotator;

    // Start is called before the first frame update
    void Start()
    {
        Rotator = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        Rotator.Rotate(0, speed * Time.deltaTime, 0); //rotating depends on frame quantity (deltaTime)
    }
}
