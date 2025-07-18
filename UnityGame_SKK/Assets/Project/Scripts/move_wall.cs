using System.Collections;
using UnityEngine;

public class move_wall : MonoBehaviour
{
    public float targetZ = 58.0f;
    public float moveSpeed = 15.0f;

    private float startZ;
    private Transform myTransform;
    private int direction = 1;
    private bool isMoving = true;

    void Start()
    {
        myTransform = transform;
        startZ = myTransform.position.z;
        //Debug.Log("StartZ: " + startZ + ", TargetZ: " + targetZ);
    }

    void Update()
    {
        if (isMoving)
        {
            myTransform.position += Vector3.forward * moveSpeed * direction * Time.deltaTime;
            float z = myTransform.position.z;

            if (direction == 1 && z >= targetZ)
            {
                myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, targetZ);
                StartCoroutine(PauseThenReverse());
            }
            else if (direction == -1 && z <= startZ)
            {
                myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, startZ);
                StartCoroutine(PauseThenReverse());
            }
        }
    }

    IEnumerator PauseThenReverse()
    {
        isMoving = false;
        //Debug.Log("一時停止");
        yield return new WaitForSeconds(1.0f);
        direction *= -1;
        isMoving = true;
        //Debug.Log("再開");
    }
}
