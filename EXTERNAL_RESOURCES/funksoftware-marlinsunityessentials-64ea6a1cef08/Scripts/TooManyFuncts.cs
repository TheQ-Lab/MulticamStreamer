using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TooManyFuncts
{
    public static bool Singletonize<T>(ref T Instance, T @this, bool persistence) where T : MonoBehaviour
    {
        if (Instance == null)
        {
            Instance = @this;
            if(persistence) UnityEngine.Object.DontDestroyOnLoad(@this.gameObject);
            return false;
        }
        else if (Instance != @this)
        {
            UnityEngine.Object.Destroy(@this.gameObject);
            return true;
        }
        return true;
        // If (Instance == this) already, no need to act
        // (probably: no scene was ever loaded OR last load is done and completed)
    }
    
    public static bool Singletonize<T>(ref T Instance, T @this) where T : MonoBehaviour
    {
        return Singletonize<T>(ref Instance, @this, false);
    }


#region Remaps
    public static float Remap(float value, int start1, int stop1, int start2, int stop2)
    {
        float outgoing =
            start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
        return outgoing;
    }

    public static float Remap(float value, float start1, float stop1, float start2, float stop2)
    {
        float outgoing =
            start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
        return outgoing;
    }

    public static double Remap(double value, double start1, double stop1, double start2, double stop2)
    {
        double outgoing =
            start2 + (stop2 - start2) * ((value - start1) / (stop1 - start1));
        return outgoing;
    }
#endregion


#region List/Array finders
    /// <summary>
    /// Find Item by String in Array
    /// </summary>
    /// <param name="target">String</param>
    /// <param name="array">Array</param>
    /// <returns></returns>
    public static GameObject FindInArray(string target, GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i].name == target)
                return array[i];
        }
        Debug.LogWarning("GameObject " + target + " could not be found in array");
        return null;
    }

    /// <summary>
    /// Iterate to the following Item in a List
    /// </summary>
    /// <typeparam name="T">Type of List</typeparam>
    /// <param name="list">List</param>
    /// <param name="current">Current Item</param>
    /// <param name="loop">Should we loop back if we got to the end of the List</param>
    /// <returns>Next Item in List</returns>
    public static T CycleThroughList<T>(List<T> list, T current, bool loop) where T : class
    {
        int iNext = list.IndexOf(current) + 1;
        if (iNext < list.Count)
            return list[iNext];

        // Last item was passed
        if (loop)
            return list[0];
        else
            return null;
    }
#endregion


#region GameObject/Transform getters
    [System.Obsolete("Use full TooManyFuncts.GetChildParametric() instead")]
    /// <summary>
    /// Find Child of given GameObject by Tag
    /// </summary>
    /// <param name="parent">Parent Transform</param>
    /// <param name="tag">List of all Tags in project is at UnityEditorInternal.InternalEditorUtility.tags</param>
    /// <param name="mustBeActive">false skips over inactive ones</param>
    /// <returns></returns>
    public static Transform FindChildWithTag(Transform parent, string tag, bool mustBeActive)
    {
        foreach(Transform c in parent)
        {
            if (mustBeActive && !c.gameObject.activeSelf)
                continue;
            if (c.gameObject.CompareTag(tag))
                return c;
        }
        return null;
    }

    [System.Obsolete("Use full TooManyFuncts.GetChildParametric() instead")]
    /// <summary>
    /// Find Children of given GameObject by Tag
    /// </summary>
    /// <param name="parent">Parent Transform</param>
    /// <param name="tag">List of all Tags in project is at UnityEditorInternal.InternalEditorUtility.tags</param>
    /// <param name="mustBeActive">false skips over inactive ones</param>
    /// <returns>List of all hits</returns>
    public static List<Transform> FindChildrenWithTag(Transform parent, string tag, bool mustBeActive)
    {
        List<Transform> res = new();
        foreach (Transform c in parent)
        {
            if (mustBeActive && !c.gameObject.activeSelf)
                continue;
            if (c.gameObject.CompareTag(tag))
                res.Add(c);
        }
        if (res.Count > 0)
            return res;

        return null;
    }

    [System.Obsolete("Use full TooManyFuncts.GetComponentInChildrenParametric<T>() instead")]
    /// <summary>
    /// Get Component of Type from given GameObjects' children, based on the wanted childs name
    /// Target Component can be Lower in Hierarchy than Child with nameContaining
    /// </summary>
    /// <typeparam name="T">Type of Component wanted</typeparam>
    /// <param name="gameObject">parent GameObject</param>
    /// <param name="nameContaining">Wanted Childs name contains this and has Component</param>
    /// <returns></returns>
    public static T GetComponentInChildNamed<T>(GameObject gameObject, string nameContaining) where T : Component
    {
        foreach (Transform c in gameObject.transform)
        {
            if (c.name.ToLower().Contains(nameContaining.ToLower()))
            {
                T comp = c.GetComponentInChildren<T>();
                if (comp is not null)
                    return comp;
            }
        }

        return gameObject.transform.GetComponentInChildren<T>();
    }

    /// <summary>
    /// searches for Child of 1st generation with criteria, THEN GetComponentInChildren for T
    /// ; therefore can find Component in Subchildren
    /// </summary>
    /// <typeparam name="T">Any Component</typeparam>
    /// <param name="parent">parent Transform</param>
    /// <param name="nameContaining">null, if no filter | not case sensitive, part of name is enough (contains)</param>
    /// <param name="tag">null, if no filter</param>
    /// <param name="mustBeActOrInactive">null, if no filter | true - must be active, false | must be inactive</param>
    /// <returns>returns T</returns>
#nullable enable
    public static T? GetComponentInChildrenParametric<T>(Transform parent, string? nameContaining, string? tag, bool? mustBeActOrInactive) where T : Component
    {
        T comp;
        foreach (Transform c in parent)
        {
            if (nameContaining is string && !c.name.ToLower().Contains(nameContaining.ToLower()))
                continue;
            if (tag is string && !c.gameObject.CompareTag(tag))
                continue;
            if (mustBeActOrInactive is bool && mustBeActOrInactive != c.gameObject.activeSelf)
                continue;
            comp = c.GetComponentInChildren<T>(true);
            if (comp is not null)
                return c.GetComponentInChildren<T>();
        }

        return null;
    }
#nullable disable

#nullable enable
    public static List<T>? GetComponentsInChildrenParametric<T>(Transform parent, string? nameContaining, string? tag, bool? mustBeActOrInactive) where T : Component
    {
        List<T> res = new();
        List<T> comp = new();
        foreach (Transform c in parent)
        {
            if (nameContaining is string && !c.name.ToLower().Contains(nameContaining.ToLower()))
                continue;
            if (tag is string && !c.gameObject.CompareTag(tag))
                continue;
            if (mustBeActOrInactive is bool && mustBeActOrInactive != c.gameObject.activeSelf)
                continue;
            c.GetComponentsInChildren<T>(true, comp);
            if (comp.Count > 0)
                res.AddRange(comp);
        }
        if (res.Count > 0)
            return res;
        return null;
    }
#nullable disable

    /// <summary>
    /// Gets you a child, with optional parameters to sort by
    /// </summary>
    /// <param name="parent">parent Transform</param>
    /// <param name="nameContaining">null, if no filter | not case sensitive, part of name is enough (contains)</param>
    /// <param name="tag">null, if no filter</param>
    /// <param name="mustBeActOrInactive">null, if no filter | true - must be active, false | must be inactive</param>
    /// <returns>returns Transform (save Transform directly if the result could be null, converting null to .GameObject throws errors)</returns>
#nullable enable
    public static Transform? GetChildParametric(Transform parent, string? nameContaining, string? tag, bool? mustBeActOrInactive)
    {
        foreach (Transform c in parent)
        {
            if (nameContaining is string && !c.name.ToLower().Contains(nameContaining.ToLower()))
                continue;
            if (tag is string && !c.gameObject.CompareTag(tag))
                continue;
            if (mustBeActOrInactive is bool && mustBeActOrInactive != c.gameObject.activeSelf)
                continue;
            return c;
        }
        return null;
    }
#nullable disable

#nullable enable
    public static List<Transform>? GetChildrenParametric(Transform parent, string? nameContaining, string? tag, bool? mustBeActOrInactive)
    {
        List<Transform> res = new();
        foreach (Transform c in parent)
        {
            if (nameContaining is string && !c.name.ToLower().Contains(nameContaining.ToLower()))
                continue;
            if (tag is string && !c.gameObject.CompareTag(tag))
                continue;
            if (mustBeActOrInactive is bool && mustBeActOrInactive != c.gameObject.activeSelf)
                continue;
            res.Add(c);
        }
        if (res.Count > 0)
            return res;
        return null;
    }
#nullable disable
    #endregion
}