using RPG.Control;
using RPG.Resx;
using UnityEngine;

namespace RPG.Dialogue
{
  public class NPCConversation : MonoBehaviour, IRaycastable
  {
    [SerializeField] Dialogue _dialogue;
    Health _health;
    public string Name;
    public CursorType CursorType => CursorType.Dialogue;
    void Awake()
    {
      _health = GetComponent<Health>();
    }
    public bool HandleRaycast(PlayerController playerCtrl)
    {
      if (!_dialogue || (_health && _health.IsDead)) return false;
      if (Input.GetMouseButtonDown(0))
        playerCtrl.GetComponent<PlayerConversation>().StartDialogue(this, _dialogue);
      return true;
    }
  }

}