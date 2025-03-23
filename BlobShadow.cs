using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobShadow : MonoBehaviour
{
    public GameObject shadow;
    public RaycastHit hit;
    public float offset;

    private void FixedUpdate()
    {
        Ray downray = new Ray(new Vector3(transform.position.x, transform.position.y - offset, transform.position.z), -Vector3.up);

        Vector3 hitpos = hit.point;
        shadow.transform.position = hitpos;

        if (Physics.Raycast(downray, out hit))
            print(hit.transform);
    }
}
