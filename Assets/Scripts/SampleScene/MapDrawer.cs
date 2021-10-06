using System.Collections.Generic;
using UnityEngine;

public class MapDrawer : MonoBehaviour {
    [SerializeField] private TextAsset _jsonMapSource;
    [SerializeField] private string _mapImagesFolder = "Images/Tiles";

    private MapData _map;

    private Dictionary<string, Sprite> _images = new Dictionary<string, Sprite>();
    [SerializeField] private Sprite _placeholder;

    private JsonSerrialization<MapData> _JsSer = new JsonSerrialization<MapData>();

    private Tile _leftBottomCorner;
    private Tile _rigthUpCorner;

    private void Awake(){
        Init();
        DrawMap();
    }

    private void Init() {
        if (!_JsSer.FromJason(_jsonMapSource, out _map)) {
            return;
        }

        _leftBottomCorner = _rigthUpCorner = _map.List[0];

        foreach (var item in _map.List) {
            CheckCorners(item);

            var image = Resources.Load<Sprite>($"{_mapImagesFolder}/{item.Id}");

            if (image == null) {
                continue;
            }

            if (_images.ContainsKey(item.Id)) {
                continue;
            }

            _images.Add(item.Id, image);
        }
    }

    private void DrawMap() {

        foreach (var item in _map.List) {
            Sprite sprite;

            var hasImage = _images.TryGetValue(item.Id, out sprite);

            if (!hasImage) {
                sprite = _placeholder;
            }

            var curentOnject    = new GameObject(item.Id);
            Transform transform = curentOnject.transform;

            transform.parent        = gameObject.transform;
            transform.position      = new Vector2(item.X, item.Y);
            transform.localScale    = new Vector2(item.Width, item.Height);

            curentOnject.AddComponent<SpriteRenderer>().sprite = sprite;
            curentOnject.AddComponent<BoxCollider2D>();
        }
    }

    private void CheckCorners(Tile item) {
        if (item.X < _leftBottomCorner.X || item.Y < _leftBottomCorner.Y) {
            _leftBottomCorner = item;
        }

        if (item.X > _rigthUpCorner.X || item.Y > _rigthUpCorner.Y) {
            _rigthUpCorner = item;
        }
    }

    public Vector2 MapLeftBottomCornerCoordinates() {
        return new Vector2(_leftBottomCorner.X - _leftBottomCorner.Width / 2, _leftBottomCorner.Y - _leftBottomCorner.Height / 2);
    }
    public Vector2 MapRightUpCornerCoordinates() {
        return new Vector2(_rigthUpCorner.X + _rigthUpCorner.Width / 2, _rigthUpCorner.Y + _rigthUpCorner.Height / 2);
    }
}
