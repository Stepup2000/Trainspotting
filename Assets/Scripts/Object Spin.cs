using UnityEngine;

public class ObjectSpin : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(0f, 20 * Time.deltaTime, 0f, Space.Self);
    }
}
