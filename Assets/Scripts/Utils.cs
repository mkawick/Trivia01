using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public static class Utils
{
    public static bool IsInList(string needle, string[] answers)
    {
        foreach (var answer in answers)
        {
            if (needle == answer)
                return true;
        }
        return false;
    }

    public static bool HasExpired(float timeStamp)
    {
        return timeStamp <= Time.time;
    }
    static public GameObject GetChildWithName(GameObject obj, string name)
    {
        Transform trans = obj.transform;
        Transform childTrans = trans.Find(name);
        if (childTrans != null)
        {
            return childTrans.gameObject;
        }
        else
        {
            return null;
        }
    }
    static public void DisableAllCameras()
    {
        Camera[] cameras = Camera.allCameras;
        for (int i = 1; i < cameras.Length; i++)
        {
            cameras[i].gameObject.SetActive(false);
        }
    }

    static public void SetAllChildrenActive(GameObject parent, bool isActive)
    {
        var transforms = new HashSet<Transform>(parent.GetComponentsInChildren<Transform>());
        transforms.Remove(parent.transform);
        var objsToHide = transforms.ToArray();
        foreach (var obj in objsToHide)
        {
            obj.transform.gameObject.SetActive(isActive);
        }
    }
}
