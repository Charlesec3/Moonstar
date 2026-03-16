using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] bool isBossKey;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            if(isBossKey == false)
            {
                ++Manager.instance.numberOfKeys;
            }
            else
            {
                Manager.instance.hasBossKey = true;
            }

            Destroy(this.gameObject);
        }
    }
}
