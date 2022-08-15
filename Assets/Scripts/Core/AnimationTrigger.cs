using UnityEngine;

public class AnimationTrigger : MonoBehaviour
{
  [SerializeField] int _initState = 0;
  Animator _animator;
  void Awake()
  {
    _animator = GetComponent<Animator>();
    _animator.SetInteger("state", _initState);
  }


}
