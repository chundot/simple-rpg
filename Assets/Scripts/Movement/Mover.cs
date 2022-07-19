using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
  public class Mover : MonoBehaviour
  {
    NavMeshAgent _agent;
    Animator _animator;
    void Start()
    {
      _agent = GetComponent<NavMeshAgent>();
      _animator = GetComponent<Animator>();
    }
    void Update()
    {
      if (Input.GetMouseButton(0))
      {
        MoveToCursor();
      }
      UpdateAnimator();
    }

    private void MoveToCursor()
    {
      var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
      var hasHit = Physics.Raycast(ray, out var hit);
      if (hasHit)
      {
        _agent.destination = hit.point;
      }
    }
    void UpdateAnimator()
    {
      var velocity = _agent.velocity;
      var localVelocity = transform.InverseTransformDirection(velocity);
      var spd = localVelocity.z;
      _animator.SetFloat("ZMove", spd);
    }
  }
}