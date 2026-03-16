using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private UnityEngine.Camera cam;
    [SerializeField] private float threshold;

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        Vector3 targetPos = (Player.instance.transform.position + mousePos) / 2f;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + Player.instance.transform.position.x, threshold + Player.instance.transform.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + Player.instance.transform.position.y, threshold + Player.instance.transform.position.y);

        this.transform.position = targetPos;
    }
}
