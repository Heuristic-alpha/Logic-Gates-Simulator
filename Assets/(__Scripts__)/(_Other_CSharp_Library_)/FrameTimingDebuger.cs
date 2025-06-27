using UnityEngine;

namespace HSCL
{
    public class FrameTimingDebuger : MonoBehaviour
    {
        float _fps;
        string _fpsColor;
        double _cpuMainThreadTime;
        double _cpuRenderThreadTime;
        double _cpuFrameTime;
        double _gpuFrameTime;
        bool _showDebug;
        Rect _debugBoxRect = new Rect(4, 4, 220, 90);
        Rect _debugButtonRect = new Rect(4, 4, 90, 28);

        GUIStyle _closeButtonStyle;

        private void Awake()
        {
            if (!FrameTimingManager.IsFeatureEnabled())
            {
                Debug.LogError("FrameTimimgManager is disabled!");
                return;
            }

            DontDestroyOnLoad(this);
            _fpsColor = "white";
            _closeButtonStyle = new GUIStyle();
            _closeButtonStyle.richText = false;
            _closeButtonStyle.stretchHeight = true;
            _closeButtonStyle.stretchWidth = true;

        }

        private void OnGUI()
        {
            if (!FrameTimingManager.IsFeatureEnabled()) return;

            FrameTimingManager.CaptureFrameTimings();
            FrameTiming[] frameTimings = new FrameTiming[1];
            // update
            if (Time.frameCount % 10 == 0)
            {
                var count = FrameTimingManager.GetLatestTimings(1, frameTimings);
                _fps = 1 / Time.deltaTime;
                _cpuMainThreadTime = frameTimings[0].cpuMainThreadFrameTime;
                _cpuRenderThreadTime = frameTimings[0].cpuRenderThreadFrameTime;
                _cpuFrameTime = frameTimings[0].cpuFrameTime;
                _gpuFrameTime = frameTimings[0].gpuFrameTime;

                // set fps color:
                if (_fps <= 10) _fpsColor = "red";
                else if (_fps <= 30) _fpsColor = "orange";
                else if (_fps <= 50) _fpsColor = "yellow";
                else if (_fps <= 90) _fpsColor = "lime";
                else _fpsColor = "cyan";
            }

            // draw
            if (!_showDebug) { if (GUI.Button(_debugButtonRect, "Show Debug")) _showDebug = true; }
            else
            {
                GUI.Box(_debugBoxRect, $"FPS: <color={_fpsColor}>{_fps.ToString("F1")}</color>\n" +
                                       $"CPU main-thread Time: <color=yellow>{_cpuMainThreadTime.ToString("F1")}</color> ms\n" +
                                       $"CPU render-thread Time: <color=yellow>{_cpuRenderThreadTime.ToString("F1")}</color> ms\n" +
                                       $"CPU render Time: <color=yellow>{_cpuFrameTime.ToString("F1")}</color> ms\n" +
                                       $"GPU render Time: <color=yellow>{_gpuFrameTime.ToString("F1")}</color> ms");

                Rect closeButtonRect = new Rect(_debugBoxRect.x + _debugBoxRect.width - 22, _debugBoxRect.y + 2, 22, 18);
                if (GUI.Button(closeButtonRect, "<color=red>X</color>")) _showDebug = false;
            }

        }

    }
}
