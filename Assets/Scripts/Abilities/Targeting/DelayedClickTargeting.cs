using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Control;
using UnityEngine;

namespace RPG.Abilities.Targeting
{
  [CreateAssetMenu(fileName = "Delayed Click Targeting", menuName = "RPG/Abilities/Targeting/Delayed Click Targeting", order = 0)]
  public class DelayedClickTargeting : TargetingStrategy
  {
    [SerializeField] Texture2D _cursorTexture;
    [SerializeField] Vector2 _cursorHotspot;
    [SerializeField] float _areaFxRadius;
    [SerializeField] LayerMask _layerMask;
    [SerializeField] Transform _targetingPrefab;
    Transform _targetingPrefabInstance;
    PlayerController _playerCtrl;
    public IEnumerable<GameObject> GetGameObjsInRange(Vector3 point)
    {
      var hits = Physics.SphereCastAll(point, _areaFxRadius, Vector3.up, 0);
      foreach (var hit in hits)
        yield return hit.collider.gameObject;
    }

    public override void StartTargeting(AbilityData data, Action finished)
    {
      if (!_playerCtrl) _playerCtrl = data.User.GetComponent<PlayerController>();
      _playerCtrl.StartCoroutine(Targeting(data, finished));
    }

    IEnumerator Targeting(AbilityData data, Action finished)
    {
      _playerCtrl.enabled = false;
      if (!_targetingPrefabInstance) _targetingPrefabInstance = Instantiate(_targetingPrefab);
      else _targetingPrefabInstance.gameObject.SetActive(true);
      _targetingPrefabInstance.localScale = new(_areaFxRadius * 2, _areaFxRadius * 2, 1);
      while (!data.IsCancelled)
      {
        Cursor.SetCursor(_cursorTexture, _cursorHotspot, CursorMode.Auto);
        if (Physics.Raycast(PlayerController.MouseRay, out var hit, 1000, _layerMask))
        {
          _targetingPrefabInstance.position = hit.point;
          if (Input.GetMouseButtonDown(0))
          {
            yield return new WaitWhile(() => Input.GetMouseButton(0));
            data.Targets = GetGameObjsInRange(hit.point);
            data.TargetPoint = hit.point;
            break;
          }
        }
        yield return null;
      }
      _playerCtrl.enabled = true;
      _targetingPrefabInstance.gameObject.SetActive(false);
      finished();
    }
  }
}
