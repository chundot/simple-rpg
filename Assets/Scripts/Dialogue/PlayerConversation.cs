using System;
using System.Collections.Generic;
using System.Linq;
using RPG.Core;
using UnityEngine;

namespace RPG.Dialogue
{
  public class PlayerConversation : MonoBehaviour
  {
    [SerializeField] string _playerName = "龙鸣";
    NPCConversation _curConversation;
    Dialogue _curDialogue;
    DialogueNode _curNode;
    DialogueNode CurNode
    {
      get => _curNode;
      set
      {
        if (_curNode == value) return;
        if (_curNode)
          TriggerExitAction();
        _curNode = value;
        if (_curNode)
          TriggerEnterAction();
        OnConversationUpdate?.Invoke();
      }
    }
    public string Text => _curDialogue == null ? "" : CurNode.Text;
    public bool HasNext => FilteredNodes(_curDialogue.GetAllChildren(CurNode)).Count() > 0;
    public bool IsChoosing { get; set; }
    public string CurConversationName
    {
      get
      {
        if (IsChoosing)
          return _playerName;
        else
          return _curConversation.Name;
      }
    }
    public bool IsActive => _curDialogue != null;
    public event Action OnConversationUpdate;
    public void StartDialogue(NPCConversation conversation, Dialogue dialogue)
    {
      _curConversation = conversation;
      _curDialogue = dialogue;
      CurNode = _curDialogue.RootNode;
    }
    public void Next()
    {
      UpdateChoosing();
      if (IsChoosing)
      {
        OnConversationUpdate?.Invoke();
        return;
      }
      if (HasNext)
        CurNode = FilteredNodes(_curDialogue.GetAIChildren(CurNode)).First();
    }
    public IEnumerable<DialogueNode> Choices
    {
      get
      {
        foreach (var child in FilteredNodes(_curDialogue.GetPlayerChildren(CurNode)))
          yield return child;
      }
    }

    public IEnumerable<IPredicateEvaluator> Evaluators => GetComponents<IPredicateEvaluator>();

    public void SelectChoice(DialogueNode chosenNode)
    {
      UpdateChoosing(chosenNode);
      CurNode = chosenNode;
      Next();
    }

    public void Quit()
    {
      _curDialogue = null;
      CurNode = null;
      _curConversation = null;
      IsChoosing = false;
    }

    IEnumerable<DialogueNode> FilteredNodes(IEnumerable<DialogueNode> nodes)
    {
      foreach (var node in nodes)
        if (node.CheckCondition(Evaluators))
          yield return node;
    }

    public void UpdateChoosing(DialogueNode node = null) => IsChoosing = FilteredNodes(_curDialogue.GetPlayerChildren(node ? node : CurNode)).Count() > 1;
    void TriggerEnterAction()
    {
      TriggerAction(CurNode.OnEnterAction);
    }
    void TriggerExitAction()
    {
      TriggerAction(CurNode.OnExitAction);
    }

    void TriggerAction(string action)
    {
      if (action != "")
      {
        foreach (var trigger in _curConversation.GetComponents<DialogueTrigger>())
          trigger.Trigger(action);
      }
    }
  }

}