using RPG.Saving;
using UnityEngine.AI;

namespace RPG.Extensions
{
  public static class NavmeshAgentExtension
  {
    public static void Warp(this NavMeshAgent agent, SerializableVector3 pos)
    {
      agent.enabled = false;
      agent.transform.position = pos.ToVector();
    }
  }

}