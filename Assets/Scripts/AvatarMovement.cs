using System.Collections;
using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public IEnumerator MoveRoutine(Path path)
    {
        var gridmap = GameManager.Instance.GetSystem<ConstructionGridmap>();

        int index = 0;
        var speed = 1f;
        while (index < path.Length)
        {
            var newSpeed = GameManager.Instance.GetSystem<TimeSystem>().TimeScale * 0.3f;
            var road = gridmap.GetConstructionAt(path.Nodes[index].Position)?.GetComponent<Road>();
            if (road != null)
            {
                newSpeed *= road.Speed;
            }
            else if (gridmap.GetConstructionAt(path.Nodes[index].Position))
            {
                newSpeed = speed;
            }
            else
            {
                newSpeed *= 0.5f;
            }
            speed = newSpeed;

            var worldPos = gridmap.CellToWorld(path.Nodes[index].Position);
            while (Vector2.Distance(transform.position, worldPos) > 0.1f)
            {
                var dir = (worldPos - (Vector2)transform.position).normalized;
                var velocity = speed * Time.deltaTime * dir;

                transform.localScale = new Vector3(velocity.x < 0 ? 1 : -1, 1, 1);

                transform.position += (Vector3)velocity;


                _animator.SetFloat("RunState", 0.5f);

                yield return null;
            }
            index++;
            path.Location = index;
        }

        _animator.SetFloat("RunState", 0f);
    }
}
