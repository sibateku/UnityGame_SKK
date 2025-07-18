using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallAttack : MonoBehaviour
{
    private GameObject player;
    public float speed = 20f;
    public bool hasHitPlayer = false;

    void Start()
    {
        player = GameObject.Find("Player");
        transform.LookAt(player.transform);
        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    
}