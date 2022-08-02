using UnityEngine;

namespace RPG.Core
{
  public class DestroyAfterFX : MonoBehaviour
  {
    void Update()
    {
      if (!GetComponent<ParticleSystem>().IsAlive())
        Destroy(gameObject);
    }
  }
}