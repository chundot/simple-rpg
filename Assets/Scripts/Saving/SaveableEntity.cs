using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Saving
{
  [ExecuteAlways]
  public class SaveableEntity : MonoBehaviour
  {
    [SerializeField] string _uniqueIdentifier = "";
    static Dictionary<string, SaveableEntity> _globalLookup = new();

    public string GetUniqueIdentifier()
    {
      return _uniqueIdentifier;
    }

    public object CaptureState()
    {
      Dictionary<string, object> state = new();
      foreach (var saveable in GetComponents<ISaveable>())
      {
        state[saveable.GetType().ToString()] = saveable.CaptureState();
      }
      return state;
    }

    public void RestoreState(object state)
    {
      Dictionary<string, object> stateDict = (Dictionary<string, object>)state;
      foreach (ISaveable saveable in GetComponents<ISaveable>())
      {
        string typeString = saveable.GetType().ToString();
        if (stateDict.ContainsKey(typeString))
        {
          saveable.RestoreState(stateDict[typeString]);
        }
      }
    }

#if UNITY_EDITOR
    private void Update()
    {
      if (Application.IsPlaying(gameObject)) return;
      if (string.IsNullOrEmpty(gameObject.scene.path)) return;

      SerializedObject serializedObject = new(this);
      var property = serializedObject.FindProperty("_uniqueIdentifier");
      if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
      {
        property.stringValue = System.Guid.NewGuid().ToString();
        serializedObject.ApplyModifiedProperties();
      }

      _globalLookup[property.stringValue] = this;
    }
#endif

    private bool IsUnique(string candidate)
    {
      if (!_globalLookup.ContainsKey(candidate)) return true;

      if (_globalLookup[candidate] == this) return true;

      if (!_globalLookup[candidate])
      {
        _globalLookup.Remove(candidate);
        return true;
      }

      if (_globalLookup[candidate].GetUniqueIdentifier() != candidate)
      {
        _globalLookup.Remove(candidate);
        return true;
      }

      return false;
    }
  }
}