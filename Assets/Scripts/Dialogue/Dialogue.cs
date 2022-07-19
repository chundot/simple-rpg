using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ARPG.Dialogue
{
  [CreateAssetMenu(fileName = "Dialogue", menuName = "ARPG/Dialogue", order = 0)]
  public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
  {
    [SerializeField]
    List<DialogueNode> _nodes = new();
    Vector2 _newNodeOffset = new(250, 50);
    readonly Dictionary<string, DialogueNode> _nodeDic = new();
    private void OnValidate()
    {
      _nodeDic.Clear();
      foreach (var node in _nodes)
        if (node != null)
          _nodeDic[node.name] = node;
    }
#if UNITY_EDITOR
    public IEnumerable<DialogueNode> Nodes
    {
      get => _nodes;
    }
    public DialogueNode GetRootNode() => _nodes[0];
    public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parent)
    {
      List<DialogueNode> res = new();
      foreach (var childID in parent.Children)
        if (_nodeDic.TryGetValue(childID, out var val))
          res.Add(val);
      return res;
    }
    public void CreateNode(DialogueNode node = null, bool record = true)
    {
      var newNode = NewNode(node);
      if (record)
      {
        Undo.RegisterCreatedObjectUndo(newNode, "Create Dialogue Node");
        Undo.RecordObject(this, "Add Dialogue Node");
      }
      _nodes.Add(newNode);
      OnValidate();
    }
    public void DeleteNode(DialogueNode node)
    {
      Undo.RecordObject(this, "Delete Dialogue Node");
      foreach (var n in Nodes)
        n.RemoveChild(node);
      _nodes.Remove(node);
      OnValidate();
      Undo.DestroyObjectImmediate(node);
    }
    DialogueNode NewNode(DialogueNode parent)
    {
      var newNode = CreateInstance<DialogueNode>();
      newNode.name = Guid.NewGuid().ToString();
      if (parent != null)
      {
        newNode.SetPos(parent.Rect.position + _newNodeOffset);
        parent.AddChild(newNode);
      }
      return newNode;
    }
#endif
    public void OnBeforeSerialize()
    {
#if UNITY_EDITOR
      if (_nodes.Count == 0)
        CreateNode(null, false);
      if (AssetDatabase.GetAssetPath(this) != "")
        foreach (var node in Nodes)
          if (AssetDatabase.GetAssetPath(node) == "")
            AssetDatabase.AddObjectToAsset(node, this);
#endif
    }
    public void OnAfterDeserialize()
    {
    }
  }
}