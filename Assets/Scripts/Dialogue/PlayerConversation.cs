using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
  public class PlayerConversation : MonoBehaviour
  {
    [SerializeField] Dialogue _curDialogue;
    DialogueNode _curNode;
    public string Text => _curDialogue == null ? "" : _curNode.Text;
    public bool HasNext => _curDialogue.GetAllChildren(_curNode).Count() > 0;
    public bool IsChoosing { get; private set; }
    void Awake()
    {
      _curNode = _curDialogue.RootNode;
      IsChoosing = false;
    }
    public void Next()
    {
      IsChoosing = _curDialogue.GetPlayerChildren(_curNode).Count(n => n.IsPlayerSpeaking) > 0;
      if (IsChoosing)
        return;
      if (HasNext)
        _curNode = _curDialogue.GetAIChildren(_curNode).ToArray()[0];
    }
    public IEnumerable<string> Choices
    {
      get
      {
        foreach (var child in _curDialogue.GetPlayerChildren(_curNode))
          yield return child.Text;
      }
    }
  }

}