using RPG.Control;
using UnityEngine;

namespace RPG.Dialogue
{
  public class NPCConversation : MonoBehaviour, IRaycastable
  {
    [SerializeField] Dialogue _dialogue;
    public string Name;
    public CursorType CursorType => CursorType.Dialogue;

    public bool HandleRaycast(PlayerController playerCtrl)
    {
      if (!_dialogue) return false;
      if (Input.GetMouseButtonDown(0))
        playerCtrl.GetComponent<PlayerConversation>().StartDialogue(this, _dialogue);
      return true;
    }
  }

}