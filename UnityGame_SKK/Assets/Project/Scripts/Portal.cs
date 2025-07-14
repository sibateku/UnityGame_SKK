using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    private Vector3 warpTargetPosition;
    private GameObject player;
    private GameObject exitPortal;

    public void SetWarpTarget(Vector3 target, GameObject playerObject, GameObject Portal)
    {
        warpTargetPosition = target;
        player = playerObject;
        exitPortal = Portal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            Debug.Log("Player touched warp portal. Warping to: " + warpTargetPosition);
            player.transform.position = warpTargetPosition;

            // 両方のポータルを削除
            if (exitPortal != null)
            {
                Destroy(exitPortal);
                Debug.Log("Exit portal destroyed.");
            }

            Destroy(gameObject); // 自分自身（入口）を削除
            Debug.Log("Entrance portal destroyed.");
        }
    }
}
