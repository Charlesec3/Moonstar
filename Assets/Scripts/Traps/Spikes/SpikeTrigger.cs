using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour
{
    [SerializeField] bool isPressureSpikes;

    bool deployRunning = false;

    [SerializeField] float waitTime;
    [SerializeField] float triggerTime;

    [SerializeField] GameObject spikes;

    float t = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isPressureSpikes == false && deployRunning == false)
        {
            deployRunning = true;
            StartCoroutine(deploy());
        }

        if(spikes.activeSelf == false)
        {
            this.gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, Color.red,t);

            if(t < 1)
            {
                t += Time.deltaTime / waitTime;
            }
        }
    }

    IEnumerator deploy()
    {
        yield return new WaitForSeconds(waitTime);

        spikes.SetActive(true);

        yield return new WaitForSeconds(triggerTime);

        t = 0;

        spikes.SetActive(false);

        deployRunning = false;
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player" && isPressureSpikes == true)
        {
            spikes.SetActive(true);
        }
    }
}
