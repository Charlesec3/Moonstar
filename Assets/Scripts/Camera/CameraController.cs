using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    [SerializeField] float xDiff;
    float finalXDiff;
    [SerializeField] float yMin;
    [SerializeField] float yMax;



    [SerializeField] bool moveableCamera;
    public GameObject target;

    void Awake()
    {
        instance = this;
    }

    void LateUpdate()
    {
        if(moveableCamera == false)
        {
            float y = this.transform.position.y;
            y = Mathf.Clamp(y, Player.instance.transform.position.y - yMin, Player.instance.transform.position.y + yMax);
         
            //transform.position = new Vector3(Player.instance.transform.position.x + xDiff, Player.instance.transform.position.y + yDiff, -10);
            if(Player.instance.facingRight == true)
            {
                if(finalXDiff < xDiff)
                {
                    finalXDiff += Time.deltaTime;
                }
            }
            else
            {
                if(finalXDiff > -xDiff)
                {
                    finalXDiff -= Time.deltaTime;
                }
            }

            transform.position = new Vector3(Player.instance.transform.position.x + finalXDiff, y, -10); 
        }
        
    }

    void FixedUpdate()
    {
        if(moveableCamera == true)
        {
            this.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, -10);
        }
    }


    public void setMoveableCamera(bool b)
    {
        moveableCamera = b;
    }
    
}
