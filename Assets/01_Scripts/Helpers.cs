using UnityEngine;

public static class ExtendTransformHelpers
{
    public static Transform FindChildByName(this Transform transform, string name)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();

        foreach (Transform child in children)
        {
            if (child.name.Equals(name))
            {
                return child;
            }
        }

        return null;
    }
}
