using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class PathDrawer : MonoBehaviour
{
    [SerializeField] private Sprite _arrowWE;
    [SerializeField] private Sprite _arrowNS;
    [SerializeField] private Sprite _arrowWS;
    [SerializeField] private Sprite _arrowEN;
    [SerializeField] private Sprite _arrowWN;
    [SerializeField] private Sprite _arrowES;

    [SerializeField] private Sprite _arrowStartW;
    [SerializeField] private Sprite _arrowStartE;
    [SerializeField] private Sprite _arrowStartN;
    [SerializeField] private Sprite _arrowStartS;

    [SerializeField] private Sprite _arrowEndW;
    [SerializeField] private Sprite _arrowEndE;
    [SerializeField] private Sprite _arrowEndN;
    [SerializeField] private Sprite _arrowEndS;

    private Dictionary<Path, Arrow> _pathArrows = new();

    private struct Arrow
    {
        private Transform _container;
        private List<SpriteRenderer> _renderers;

        public Arrow(Transform container)
        {
            _container = container;
            _renderers = new List<SpriteRenderer>();
        }

        public Transform Container => _container;
        public List<SpriteRenderer> Renderers => _renderers;
    }

    private void Update()
    {
        foreach (var pathArrow in _pathArrows)
        {
            for (int i = 0; i < pathArrow.Value.Renderers.Count; i++)
            {
                var renderer = pathArrow.Value.Renderers[i];
                renderer.enabled = pathArrow.Key.Location <= i;
            }
        }
    }

    public void DrawPath(Path path)
    {
        var gridmap = GameManager.Instance.GetSystem<ConstructionGridmap>();

        if (path.Nodes.Length == 0)
        {
            return;
        }

        var newArrow = new Arrow(new GameObject().transform);
        newArrow.Container.SetParent(transform);
        _pathArrows[path] = newArrow;

        var color = Color.HSVToRGB(Random.Range(0f, 1f), 1, 1);

        for (int i = 0; i < path.Nodes.Length; i++)
        {
            SpriteRenderer renderer = new GameObject().AddComponent<SpriteRenderer>();

            renderer.sortingOrder = -9;
            renderer.color = color;
            renderer.transform.SetParent(newArrow.Container);
            renderer.transform.position = gridmap.CellToWorld(path.Nodes[i].Position);

            if (i == 0)
            {
                var node = path.Nodes[i];

                if (node.Direction == Direction.East)
                {
                    renderer.sprite = _arrowStartE;
                }
                else if (node.Direction == Direction.West)
                {
                    renderer.sprite = _arrowStartW;
                }
                else if (node.Direction == Direction.North)
                {
                    renderer.sprite = _arrowStartN;
                }
                else if (node.Direction == Direction.South)
                {
                    renderer.sprite = _arrowStartS;
                }
            }
            else if (i == path.Nodes.Length - 1)
            {
                var prevNode = path.Nodes[i - 1];

                if (prevNode.Direction == Direction.East)
                {
                    renderer.sprite = _arrowEndE;
                }
                else if (prevNode.Direction == Direction.West)
                {
                    renderer.sprite = _arrowEndW;
                }
                else if (prevNode.Direction == Direction.North)
                {
                    renderer.sprite = _arrowEndN;
                }
                else if (prevNode.Direction == Direction.South)
                {
                    renderer.sprite = _arrowEndS;
                }
            }
            else
            {
                var prevNode = path.Nodes[i - 1];
                var node = path.Nodes[i];

                if ((prevNode.Direction == Direction.East && node.Direction == Direction.East)
                || (prevNode.Direction == Direction.West && node.Direction == Direction.West))
                {
                    renderer.sprite = _arrowWE;
                }
                else if ((prevNode.Direction == Direction.North && node.Direction == Direction.North)
                || (prevNode.Direction == Direction.South && node.Direction == Direction.South))
                {
                    renderer.sprite = _arrowNS;
                }
                else if ((prevNode.Direction == Direction.East && node.Direction == Direction.South)
                || (prevNode.Direction == Direction.North && node.Direction == Direction.West))
                {
                    renderer.sprite = _arrowWS;
                }
                else if ((prevNode.Direction == Direction.West && node.Direction == Direction.South)
                || (prevNode.Direction == Direction.North && node.Direction == Direction.East))
                {
                    renderer.sprite = _arrowES;
                }
                else if ((prevNode.Direction == Direction.East && node.Direction == Direction.North)
                || (prevNode.Direction == Direction.South && node.Direction == Direction.West))
                {
                    renderer.sprite = _arrowWN;
                }
                else if ((prevNode.Direction == Direction.West && node.Direction == Direction.North)
                || (prevNode.Direction == Direction.South && node.Direction == Direction.East))
                {
                    renderer.sprite = _arrowEN;
                }
            }

            _pathArrows[path].Renderers.Add(renderer);
        }
    }

    public void RemovePath(Path path)
    {
        if (_pathArrows.ContainsKey(path))
        {
            foreach (var renderer in _pathArrows[path].Renderers)
            {
                Destroy(renderer.gameObject);
            }

            Destroy(_pathArrows[path].Container.gameObject);
            _pathArrows.Remove(path);
        }
    }
}
