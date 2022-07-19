using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.MPE;
using UnityEngine;

namespace ARPG.Dialogue.Editor
{
  public class DialogueEditor : EditorWindow
  {
    Dialogue _selectedDialogue;
    [NonSerialized]
    GUIStyle _nodeStyle, _playerNodeStyle;
    [NonSerialized]
    DialogueNode _draggingNode, _creatingNode, _deletingNode, _linkingParentNode, _linkingNode, _unlinkingNode;
    [NonSerialized]
    Vector2 _draggingOffset, _scrollPos, _draggingCanvasOffset;
    bool _draggingCanvas;
    int _bgSize = 50, _canvasSize = 4000;

    [MenuItem("Window/Dialogue Editor")]
    public static void ShowEditorWindow()
    {
      GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
    }
    [OnOpenAsset(1)]
    public static bool OpenDialogue(int instanceID, int _)
    {
      if (EditorUtility.InstanceIDToObject(instanceID) is Dialogue)
      {
        ShowEditorWindow();
        return true;
      }
      return false;
    }
    private void OnEnable()
    {
      Selection.selectionChanged += () =>
      {
        if (Selection.activeObject is Dialogue dialogue)
        {
          _selectedDialogue = dialogue;
          Repaint();
        }
      };
      _nodeStyle = new();
      _nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
      _nodeStyle.normal.textColor = Color.white;
      _nodeStyle.padding = new RectOffset(20, 20, 20, 20);
      _nodeStyle.border = new RectOffset(12, 12, 12, 12);

      _playerNodeStyle = new();
      _playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
      _playerNodeStyle.normal.textColor = Color.white;
      _playerNodeStyle.padding = new RectOffset(20, 20, 20, 20);
      _playerNodeStyle.border = new RectOffset(12, 12, 12, 12);
    }

    private void OnGUI()
    {
      if (_selectedDialogue == null)
      {
        EditorGUILayout.LabelField("No dialogue selected");
      }
      else
      {
        ProcessEvent();
        _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);
        var canvas = GUILayoutUtility.GetRect(_canvasSize, _canvasSize);
        GUI.DrawTextureWithTexCoords(canvas, EditorGUIUtility.Load("gridbg.png") as Texture2D, new(0, 0, _canvasSize / _bgSize, _canvasSize / _bgSize));
        foreach (var node in _selectedDialogue.Nodes)
          DrawNode(node);
        foreach (var node in _selectedDialogue.Nodes)
          DrawConnections(node);
        EditorGUILayout.EndScrollView();
      }
      if (_creatingNode != null)
      {
        _selectedDialogue.CreateNode(_creatingNode);
        _creatingNode = null;
      }
      if (_deletingNode != null)
      {
        _selectedDialogue.DeleteNode(_deletingNode);
        _deletingNode = null;
      }
      if (_linkingNode != null)
      {
        _linkingParentNode.AddChild(_linkingNode);
        _linkingNode = _linkingParentNode = null;
      }
      if (_unlinkingNode != null)
      {
        _linkingParentNode.RemoveChild(_unlinkingNode);
        _unlinkingNode = _linkingParentNode = null;
      }
    }

    private void DrawConnections(DialogueNode p)
    {
      Vector2 startPos = new(p.Rect.xMax, p.Rect.center.y);
      foreach (var node in _selectedDialogue.GetAllChildren(p))
      {
        Vector2 endPos = new(node.Rect.xMin, node.Rect.center.y);
        var ctrlPointOffset = endPos - startPos;
        ctrlPointOffset.y = 0;
        Handles.DrawBezier(startPos, endPos, startPos + ctrlPointOffset, endPos - ctrlPointOffset, Color.white, null, 4f);
      }
    }

    private void ProcessEvent()
    {
      if (Event.current.type is EventType.MouseDown && _draggingNode == null)
      {
        _draggingNode = GetNodeAtPos(Event.current.mousePosition);
        if (_draggingNode == null)
        {
          _draggingCanvas = true;
          _draggingCanvasOffset = Event.current.mousePosition + _scrollPos;
          Selection.activeObject = _selectedDialogue;
        }
        else Selection.activeObject = _draggingNode;
      }
      else if (Event.current.type is EventType.MouseDrag)
      {
        if (_draggingNode != null)
        {
          _draggingNode.SetPos(Event.current.mousePosition + _draggingOffset);
        }
        else if (_draggingCanvas)
          _scrollPos = _draggingCanvasOffset - Event.current.mousePosition;
        GUI.changed = true;
      }
      else if (Event.current.type is EventType.MouseUp)
      {
        if (_draggingNode != null)
          _draggingNode = null;
        else if (_draggingCanvas)
          _draggingCanvas = false;
      }
    }
    private void DrawNode(DialogueNode node)
    {
      if (node.IsPlayerSpeaking)
        GUILayout.BeginArea(node.Rect, _playerNodeStyle);
      else
        GUILayout.BeginArea(node.Rect, _nodeStyle);
      node.SetText(EditorGUILayout.TextField(node.Text));
      GUILayout.BeginHorizontal();
      if (GUILayout.Button("x"))
        _deletingNode = node;
      if (_linkingParentNode == null)
      {
        if (GUILayout.Button("link"))
          _linkingParentNode = node;
      }
      else
      {
        if (node == _linkingParentNode)
        {
          if (GUILayout.Button("cancel"))
            _linkingParentNode = null;
        }
        else
        {
          if (_linkingParentNode.Children.Contains(node.name))
          {
            if (GUILayout.Button("unlink"))
              _unlinkingNode = node;
          }
          else if (GUILayout.Button("child"))
            _linkingNode = node;
        }
      }
      if (GUILayout.Button("+"))
        _creatingNode = node;
      GUILayout.EndHorizontal();

      GUILayout.EndArea();
    }
    private DialogueNode GetNodeAtPos(Vector2 mousePosition)
    {
      foreach (var node in _selectedDialogue.Nodes)
        if (node.Rect.Contains(mousePosition + _scrollPos))
        {
          _draggingOffset = node.Rect.position - mousePosition;
          return node;
        }
      return null;
    }
  }
}