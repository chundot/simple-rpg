using UnityEngine;

public static class GameObjectExtension
{
  public static void DestroyAllChildren(this MonoBehaviour mb)
  {
    foreach (Transform transform in mb.transform)
      Object.Destroy(transform.gameObject);
  }
  public static void DestroyAllChildren(this Transform transform)
  {
    foreach (Transform child in transform)
      Object.Destroy(child.gameObject);
  }
}
