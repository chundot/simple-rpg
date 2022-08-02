using UnityEngine;

namespace RPG.Manager.Common
{
  public class BaseMgr<T> : MonoBehaviour where T : MonoBehaviour
  {
    public static T Self;
    void Awake()
    {
      if (Self != this as T)
        Clean();
      Self = this as T;
    }
    public void Clean() => Destroy(Self);
  }
}