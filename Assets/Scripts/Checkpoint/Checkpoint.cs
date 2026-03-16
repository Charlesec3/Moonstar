using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] int checkpointNum;
    Vector3 pos;

    void Start()
    {
        pos = this.transform.position;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("CHECKPOINT: collided with player");
            if(Manager.instance.checkpointNum < checkpointNum)
            {
                Debug.Log("CHECKPOINT: new checkpoint");

                Manager.instance.checkpointNum = checkpointNum;

                Manager.instance.checkpointPositions[checkpointNum] = this.transform;

                File.WriteAllText(Application.persistentDataPath + "/CheckpointData.txt", Manager.instance.checkpointNum.ToString());

                StartCoroutine(CheckpointDisplay.instance.displayCheckpoint());
            }
            /*else if(Manager.instance.checkpointNum == 0 && checkpointNum == 0)
            {
                Debug.Log("CHECKPOINT: first checkpoint");

                Manager.instance.checkpointNum = checkpointNum;

                Manager.instance.checkpointPositions[checkpointNum] = this.transform;

                File.WriteAllText(Application.persistentDataPath + "/CheckpointData.txt", Manager.instance.checkpointNum.ToString());
            }*/
        }
    }
}
