using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SynSensorTrigger : MonoBehaviour
{
    public GameObject playerprefub;
    public float triggerDistance = 0.5f;

    public SyncSensor SyncSensorManager; // 共通のマネージャ参照

    void Update()
    {
        float distance = Vector3.Distance(playerprefub.transform.position, transform.position);

        if (distance <= triggerDistance)
        {
            SyncSensorManager.TriggerFromSensor();
        }
    }
}
