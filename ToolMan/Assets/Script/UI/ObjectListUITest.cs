using UnityEngine;
using System.Collections.Generic;

public class ObjectListUITest : MonoBehaviour
{
    public ObjectListUI ui;
    // Use this for initialization
    void Start()
    {
        string[] keys = {"a", "b"};
        ui.SetSprites(new List<string>(keys));
    }

    // Update is called once per frame
    void Update()
    {

    }
}
