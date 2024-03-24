using System.Collections;
using System.Linq;
using UnityEngine;

public class NPC : MonoBehaviour {
    private SpriteRenderer _spriteRenderer;

    private void Awake() {
        _spriteRenderer = transform.Find("Sprite").GetComponent<SpriteRenderer>();
    }

    void Start() {
        var _roads = ConstructionManager.Instance.RoadMap.Constructions.Cast<Road>().ToArray();
        Road start = null;
        if (_roads.Length > 0) {
            start = _roads[Random.Range(0, _roads.Length)];
            transform.position = start.transform.position;
        }
        StartCoroutine(IEnuMove(start));
    }

    void Update() {
        _spriteRenderer.sortingOrder = 300 - Mathf.FloorToInt(transform.position.y * 10);
    }

    IEnumerator IEnuMove(Road start) {
        var _roads = ConstructionManager.Instance.RoadMap.Constructions.Cast<Road>().ToArray();
        Road target = null;
        if (_roads.Length > 0) {
            target = _roads[Random.Range(0, _roads.Length)];

            var path = PathFinder.SearchPath(start, target, _roads);

            int index = 0;

            while (index < path.Length) {
                while (Vector3.Distance(transform.position, path[index].transform.position) > 0.01) {
                    transform.position = Vector3.MoveTowards(transform.position, path[index].transform.position, Time.deltaTime);
                    yield return null;
                }
                index++;
            }
        }

        yield return new WaitForSeconds(1);

        StartCoroutine(IEnuMove(target));
    }
}
