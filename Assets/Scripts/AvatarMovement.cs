using System.Collections;
using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
    private Animator _animator;
    private Vector2 _startScale;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        _startScale = transform.localScale;
    }

    public IEnumerator MoveRoutine(Path path, float speed = 1)
    {
        var gridmap = GameManager.Instance.GetSystem<ConstructionGridmap>();

        int index = 0;
        while (index < path.Length)
        {
            var worldPos = gridmap.CellToWorld(path.Nodes[index].Position);
            while (Vector2.Distance(transform.position, worldPos) > 0.1f)
            {
                var newSpeed = GameManager.Instance.GetSystem<TimeSystem>().TimeScale * 0.3f * speed;
                var road = gridmap.GetConstructionAt(path.Nodes[index].Position)?.GetComponent<Road>();
                if (road != null)
                {
                    newSpeed *= road.Speed;
                }
                else
                {
                    newSpeed *= 0.5f;
                }

                var dir = (worldPos - (Vector2)transform.position).normalized;
                var velocity = newSpeed * Time.deltaTime * dir;

                transform.localScale = new Vector3(velocity.x < 0 ? _startScale.x : -_startScale.x, _startScale.y, _startScale.y);

                transform.position += (Vector3)velocity;

                _animator.SetFloat("RunState", 1);

                yield return null;
            }
            index++;
            path.Location = index;
        }

        _animator.SetFloat("RunState", 0f);
    }
}
