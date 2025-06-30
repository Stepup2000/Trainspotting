using UnityEngine;

public class VolumetricParticle : MonoBehaviour
{
    private Transform targetCam;
    private Vector3 initialPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //targetCam = Camera.main.transform;
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = Camera.allCameras[0].transform.position;
        target.y = initialPos.y;
        Vector3 dir = target - initialPos;
        transform.position = initialPos + dir.normalized * 0.4f;
        transform.LookAt(target);
    }
}
