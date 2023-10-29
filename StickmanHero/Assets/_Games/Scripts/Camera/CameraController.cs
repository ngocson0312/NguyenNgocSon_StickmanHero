using UnityEngine;
using DG.Tweening;

namespace SuperFight
{
    public class CameraController : Singleton<CameraController>
    {
        public Vector3 offset;
        public Camera cameraMain { get; private set; }
        private Vector3 velocity;
        private Transform targetFollow;
        public float cameraSizeMultiply
        {
            get { return PlayerPrefs.GetFloat("cam_size_value", 0); }
            private set { PlayerPrefs.SetFloat("cam_size_value", value); }
        }
        public bool hasBound = false;

        protected override void Awake()
        {
            base.Awake();
            cameraMain = GetComponentInChildren<Camera>();
        }
        private void Start()
        {
            targetFollow = PlayerManager.Instance.transform;
            CameraOnMain(Vector3.zero);
        }

        void Update()
        {
            HandleFollow();
            if (Input.GetKeyDown(KeyCode.K))
            {
                CameraController.Instance.ShakeCamera(.5f, 1f, 10, 90, true);
            }
        }
        void HandleFollow()
        {
            if (targetFollow == null) return;
            transform.position = Vector3.SmoothDamp(transform.position, targetFollow.position + offset, ref velocity, 0.1f);
        }

        Tween KillCameraShake;
        public void ShakeCamera(float dur, float stre, int vib, int randness, bool smooth = true)
        {
            if (KillCameraShake != null)
            {
                KillCameraShake.Kill();
                cameraMain.transform.localPosition = Vector3.zero;
            }
            KillCameraShake = cameraMain.DOShakePosition(dur, stre, vib, randness, smooth).OnComplete(() =>
            {
                cameraMain.transform.localPosition = Vector3.zero;
            });
        }
        public void SetTargetFollow(Transform target)
        {
            targetFollow = target;
        }
        public void SetOrthoSize(Vector2 size, float duration, bool lockSettingSize)
        {
            float screenRatio = (float)Screen.width / (float)Screen.height;
            float targetRatio = size.x / size.y;
            PlayScreenCtl playScreen = ScreenUIManager.Instance.GetScreen<PlayScreenCtl>(ScreenName.PLAYSCREEN);
            playScreen.SetActiveCameraSlider(!lockSettingSize);
            if (orthoTween != null)
            {
                orthoTween.Kill();
            }
            if (screenRatio >= targetRatio)
            {
                orthoTween = cameraMain.DOOrthoSize(size.y / 2f, duration);
            }
            else
            {
                float differenceSize = targetRatio / screenRatio;
                orthoTween = cameraMain.DOOrthoSize(size.y / 2f * differenceSize, duration);
            }
        }
        public void SetOffset(Vector3 newOffset)
        {
            offset = newOffset;
            offset.z = -500;
        }
        Tweener orthoTween;
        public void SetOrthoSize(float size, float duration, bool lockSettingSize)
        {
            PlayScreenCtl playScreen = ScreenUIManager.Instance.GetScreen<PlayScreenCtl>(ScreenName.PLAYSCREEN);
            playScreen.SetActiveCameraSlider(!lockSettingSize);
            if (orthoTween != null)
            {
                orthoTween.Kill();
            }
            orthoTween = cameraMain.DOOrthoSize(size, duration);
        }
        public void SetCamMultySize(float size)
        {
            cameraSizeMultiply = size;
            cameraMain.orthographicSize = 5f + (cameraSizeMultiply * 3f);
        }
        public void CamOnWin()
        {
            orthoTween = cameraMain.DOOrthoSize(2.6f, .7f);
        }
        public void CamOnLose()
        {
            orthoTween = cameraMain.DOOrthoSize(2.6f, .7f);
        }
        public void CamOnStart(Vector3 posCamStart)
        {
            PlayScreenCtl playScreen = ScreenUIManager.Instance.GetScreen<PlayScreenCtl>(ScreenName.PLAYSCREEN);
            playScreen.SetActiveCameraSlider(true);
            Vector3 currPos = transform.position;
            currPos.x = posCamStart.x + offset.x;
            currPos.y = posCamStart.y + offset.y;
            currPos.z = offset.z;
            offset = new Vector3(0, 2f, -500);
            transform.position = currPos;
            cameraMain.orthographicSize = 5f + (cameraSizeMultiply * 1.5f);
        }
        public void CameraOnMain(Vector3 posCamStart)
        {
            SetTargetFollow(PlayerManager.Instance.transform);
            offset = new Vector3(0, .8f, -500);
            Vector3 currPos = transform.position;
            currPos.x = posCamStart.x + offset.x;
            currPos.y = posCamStart.y + offset.y;
            currPos.z = offset.z;
            transform.position = currPos;
            cameraMain.orthographicSize = 5f;
        }
    }


}

