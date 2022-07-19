using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ARPG.Dialogue
{
  public class DialogueNode : ScriptableObject
  {
    [SerializeField]
    bool isPlayerSpeaking;
    [SerializeField]
    string text;
    [SerializeField]
    List<string> children = new();
    [SerializeField]
    Rect rect = new(0, 0, 200, 100);
    public string Text { get => text; }
    public Rect Rect { get => rect; }
    public bool IsPlayerSpeaking { get => isPlayerSpeaking; }
    public List<string> Children { get => children; }
#if UNITY_EDITOR
    public void SetPos(Vector2 pos)
    {
      Undo.RecordObject(this, "Move Dialogue Node");
      rect.position = pos;
      EditorUtility.SetDirty(this);
    }
    public void SetText(string text)
    {
      Undo.RecordObject(this, "Update Dialgoue Text");
      this.text = text;
      EditorUtility.SetDirty(this);
    }
    public void AddChild(DialogueNode childNode)
    {
      Undo.RecordObject(this, "Link Dialogue Nodes");
      children.Add(childNode.name);
      EditorUtility.SetDirty(this);
    }
    public void RemoveChild(DialogueNode childNode)
    {
      Undo.RecordObject(this, "Unlink Dialogue Nodes");
      children.Remove(childNode.name);
      EditorUtility.SetDirty(this);
    }
    public void SetIsPlayer(bool isPlayer)
    {
      Undo.RecordObject(this, "Change Dialogue Speaker");
      isPlayerSpeaking = isPlayer;
      EditorUtility.SetDirty(this);
    }
#endif
  }
}