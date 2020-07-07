using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AutoChildHider : MonoBehaviour
{
    void Awake()
    {
        /* var transforms = new HashSet<Transform>(GetComponentsInChildren<Transform>());
         transforms.Remove(this.transform);
         var objsToHide = transforms.ToArray();
         foreach (var obj in objsToHide)
         {
             obj.transform.gameObject.SetActive(false);
         }*/
        //Utils.SetAllChildrenActive(this.gameObject, false);
    }
}
