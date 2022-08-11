using System;
using RPG.Manager;
using UnityEngine;

namespace RPG.Quests
{
  public class QuestGiver : MonoBehaviour
  {
    [SerializeField] Quest _quest;
    public void GiveQuest()
    {
      SceneMgr.Self.Player.GetComponent<QuestList>().AddQuest(_quest);
    }

  }
}