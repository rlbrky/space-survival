using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private void LateUpdate()
    {
        transform.position = PlayerSC.instance.transform.position;
    }
}
