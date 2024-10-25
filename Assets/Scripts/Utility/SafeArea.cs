using UnityEngine;

    [RequireComponent(typeof(RectTransform))]
    public class SafeArea : MonoBehaviour
    {
        [SerializeField]
        private bool conformX = true; 

        [SerializeField]
        private bool conformY = true; 

        private ScreenOrientation _lastOrientation = ScreenOrientation.AutoRotation;
        private Rect _lastSafeArea = new(0, 0, 0, 0);
        private Vector2Int _lastScreenSize = new(0, 0);
        private RectTransform _panel;

        private void Awake()
        {
            _panel = GetComponent<RectTransform>();

            if (_panel == null)
            {
                Debug.LogError("Cannot apply safe area - no RectTransform found on " + name);
                Destroy(this);
            }

            Refresh();
        }

        private void Update()
        {
            Refresh();
        }

        private void Refresh()
        {
            if (GetSafeArea() != _lastSafeArea || Screen.width != _lastScreenSize.x ||
                Screen.height != _lastScreenSize.y || Screen.orientation != _lastOrientation)
            {
                _lastScreenSize.x = Screen.width;
                _lastScreenSize.y = Screen.height;
                _lastOrientation = Screen.orientation;

                ApplySafeArea(GetSafeArea());
            }
        }

        private Rect GetSafeArea()
        {
            return Screen.safeArea;
        }

        private void ApplySafeArea(Rect r)
        {
            _lastSafeArea = r;

            if (!conformX)
            {
                r.x = 0;
                r.width = Screen.width;
            }

            if (!conformY)
            {
                r.y = 0;
                r.height = Screen.height;
            }

            if (Screen.width > 0 && Screen.height > 0)
            {
                var anchorMin = r.position;
                var anchorMax = r.position + r.size;
                anchorMin.x /= Screen.width;
                anchorMin.y /= Screen.height;
                anchorMax.x /= Screen.width;
                anchorMax.y /= Screen.height;

                if (anchorMin is {x: >= 0, y: >= 0} && anchorMax is {x: >= 0, y: >= 0})
                {
                    _panel.anchorMin = anchorMin;
                    _panel.anchorMax = anchorMax;
                }
            }
        }
    }