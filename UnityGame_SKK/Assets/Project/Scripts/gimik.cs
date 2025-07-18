using System.Collections;
using UnityEngine;

public class gimik : MonoBehaviour
{
    [SerializeField] private float targetZ = 13.5f;
    [SerializeField] private float moveSpeed = 1.0f;

    private float startZ;
    private Transform myTransform;
    private int direction = -1;  // 初めは減る方向（-1）に移動
    private bool isMoving = true;

    void Start()
    {
        myTransform = transform;
        startZ = myTransform.position.z;

        //Debug.Log($"StartZ: {startZ}, TargetZ: {targetZ}, 初期方向: {direction}");
    }

    void Update()
    {
        if (!isMoving) return;

        float step = moveSpeed * Time.deltaTime;
        myTransform.position += Vector3.forward * step * direction;

        float z = myTransform.position.z;

        if (direction == -1 && z <= targetZ)
        {
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, targetZ);
            StartCoroutine(PauseThenReverse());
        }
        else if (direction == 1 && z >= startZ)
        {
            myTransform.position = new Vector3(myTransform.position.x, myTransform.position.y, startZ);
            StartCoroutine(PauseThenReverse());
        }
    }

    IEnumerator PauseThenReverse()
    {
        isMoving = false;
        yield return new WaitForSeconds(3.0f);
        direction *= -1; // 進行方向を反転
        isMoving = true;
    }
}
