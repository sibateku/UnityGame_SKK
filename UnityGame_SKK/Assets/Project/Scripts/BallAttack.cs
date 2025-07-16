using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour

{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        transform.LookAt(player.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(0, 0, 1);
        Destroy(gameObject, 1);
    }
}
