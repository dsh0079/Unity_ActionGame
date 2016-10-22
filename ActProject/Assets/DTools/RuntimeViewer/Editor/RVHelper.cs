using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public static class RVHelper
{

    public static Component[] GetComponent(GameObject data)
    {
        Component[] c = data.GetComponents<Component>();

        if (c == null || c.Length == 0)
            return null;

        return c;
    }
}
