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

    private Path _path;
    private SpriteRenderer[] _renderers;

    public Path Path => _path;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<SpriteRenderer>();
    }

    private void Update()
    {
        if (_path != null)
        {
            for (int i = 0; i < _path.Length; i++)
            {
                var renderer = _renderers[i];
                renderer.enabled = _path.Location <= i;
            }
        }
    }

    public void DrawPath(Path path)
    {

        if (path.Nodes.Length == 0)
        {
            return;
        }

        _path = path;

        var gridmap = GameManager.Instance.GetSystem<ConstructionGridmap>();

        for (int i = 0; i < path.Nodes.Length; i++)
        {
            var renderer = _renderers[i];
            renderer.enabled = true;
            renderer.color = Color.red;
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
        }

        for (int i = path.Nodes.Length; i < _renderers.Length; i++)
        {
            _renderers[i].enabled = false;
        }
    }

    public void RemovePath()
    {
        foreach (var renderer in _renderers)
        {
            renderer.enabled = false;
        }
        _path = null;
    }
}
