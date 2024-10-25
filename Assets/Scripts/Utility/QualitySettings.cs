using UnityEngine;

namespace Utility
{
    public class QualitySettings : MonoBehaviour
    {
        public bool targetIsRefreshRate;
        public int targetFrameRate;

        private void Start()
        {
            int maxRefresh = 60;
#if UNITY_ANDROID
            Resolution[] allResolutions = Screen.resolutions;
            foreach (var resolution in allResolutions)
            {
                maxRefresh = Mathf.Max(maxRefresh, (int)resolution.refreshRateRatio.value);
            }
#endif
            #region IOS
            maxRefresh = Mathf.Max(maxRefresh, (int)Screen.currentResolution.refreshRateRatio.value);
            #endregion

            // Clamp just in case bogus values are returned.
            maxRefresh = Mathf.Clamp(maxRefresh, 1, 1600);

            Application.targetFrameRate = targetIsRefreshRate ? maxRefresh : targetFrameRate;
        }
    }
}