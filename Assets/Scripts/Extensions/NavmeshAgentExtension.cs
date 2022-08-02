using System.Collections;
using System.Collections.Generic;
using RPG.Saving;
using UnityEngine;
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