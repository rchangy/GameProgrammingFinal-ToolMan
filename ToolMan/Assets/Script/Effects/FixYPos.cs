using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixYPos : MonoBehaviour
{
    public EnemyWhale parent;
    public float y;

    // Update is called once per frame
    void Update()
    {
        if (!parent)
            return;
        transform.position = new Vector3(parent.transform.position.x, y, parent.transform.position.z);
        Debug.Log("warning pos = " + transform.position);
    }
}
