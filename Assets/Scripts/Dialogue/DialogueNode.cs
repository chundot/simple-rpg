using System.Collections.Generic;
using RPG.Core;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
  public class DialogueNode : ScriptableObject
  {
    [SerializeField] bool _isPlayerSpeaking;
    [SerializeField] string _text;
    [SerializeField] List<string> _children = new();
    [SerializeField] Rect _rect = new(0, 0, 200, 100);
    [SerializeField] string _onEnterAction, _onExitAction;
    [SerializeField] Condition _condition;
    [SerializeField] int _order = 0;
    public string OnEnterAction { get => _onEnterAction; }
    public string OnExitAction { get => _onExitAction; }
    public string Text { get => _text; }
    public Rect Rect { get => _rect; }
    public bool IsPlayerSpeaking { get => _isPlayerSpeaking; }
    public List<string> Children { get => _children; }
    public int Order => _order;
    public bool CheckCondition(IEnumerable<IPredicateEvaluator> evaluators)
    {
      return _condition.Check(evaluators);
    }
#if UNITY_EDITOR
    public void SetPos(Vector2 pos)
    {
      Undo.RecordObject(this, "Move Dialogue Node");
      _rect.position = pos;
      EditorUtility.SetDirty(this);
    }
    public void SetText(string text)
    {
      Undo.RecordObject(this, "Update Dialgoue Text");
      _text = text;
      EditorUtility.SetDirty(this);
    }
    public void AddChild(DialogueNode childNode)
    {
      Undo.RecordObject(this, "Link Dialogue Nodes");
      _children.Add(childNode.name);
      EditorUtility.SetDirty(this);
    }
    public void RemoveChild(DialogueNode childNode)
    {
      Undo.RecordObject(this, "Unlink Dialogue Nodes");
      _children.Remove(childNode.name);
      EditorUtility.SetDirty(this);
    }
    public void SetIsPlayer(bool isPlayer)
    {
      Undo.RecordObject(this, "Change Dialogue Speaker");
      _isPlayerSpeaking = isPlayer;
      EditorUtility.SetDirty(this);
    }

#endif
  }
}