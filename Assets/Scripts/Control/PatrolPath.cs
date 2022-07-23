using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
  public class PatrolPath : MonoBehaviour
  {
    int _cur;
    void OnDrawGizmos()
    {
      for (int i = 0; i < transform.childCount; ++i)
      {
        Gizmos.DrawSphere(GetPoint(i), .3f);
        Gizmos.DrawLine(GetPoint(i), GetPoint(GetNext(i)));
      }
    }
    public int GetNext(int i) => i == transform.childCount - 1 ? 0 : i + 1;
    public Vector3 GetPoint(int i) => transform.GetChild(i).position;
  }

}