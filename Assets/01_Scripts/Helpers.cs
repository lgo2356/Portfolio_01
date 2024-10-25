using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ExtendTransformHelpers
{
    public static Transform FindChildByName(this Transform transform, string name)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();

        return children.FirstOrDefault(child => child.name.Equals(name));
    }

    public static List<Transform> FindChildrenByName(this Transform transform, string name, bool isWildcard = false)
    {
        Transform[] children = transform.GetComponentsInChildren<Transform>();
        List<Transform> results;

        if (isWildcard)
        {
            results = children
                .Where(t => Regex.IsMatch(t.name, @$"^{name}"))
                .ToList();
        }
        else
        {
            results = children
                .Where(t => t.name.Equals(name))
                .ToList();
        }

        return results;
    }
}
