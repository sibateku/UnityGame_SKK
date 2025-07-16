using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform player;
    public GameObject ball;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(targetPosition);

            count++;
            Debug.Log(count);
            if (count % 75 == 0)
            {
                Instantiate(ball, transform.position, Quaternion.identity);
            }
        // transform.position += transform.forward * 0.01f;
        }
    }

    // Update is called once per frame
        void Update()
    {
        
    }
}
