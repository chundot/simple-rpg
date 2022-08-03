using UnityEngine;

namespace RPG.Core
{
  public class PersistObjSpawner : MonoBehaviour
  {
    [SerializeField] GameObject _objPrefab;
    static bool _hasSpawned = false;
    void Awake()
    {
      if (_hasSpawned) return;
      SpawnObj();
      _hasSpawned = true;
    }

    private void SpawnObj()
    {
      var obj = Instantiate(_objPrefab);
      DontDestroyOnLoad(obj);
    }
  }

}