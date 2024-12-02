using ForgetMeNotEnums;
using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapturePhoto : MonoBehaviour
{
    public static Action OnPhotoCaptured;
    public static Action OnRetakePhoto;

    private bool _isCamRotated = false;
    private Texture2D _capturedImageTex;

    private WebCamTexture _webCamTexture;
    [SerializeField]
    private RawImage _camView;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter;
    [SerializeField]
    private Button _captureButton;

    [Space(10)]
    [Header("Image Prefab")]
    [SerializeField]
    private Transform _imagePrefabsHolder;

    [SerializeField]
    private TMP_InputField _imageNameInput;
    [SerializeField]
    private GameObject _imageNamePanel;

    private Quaternion _baseRotation;
    private void OnEnable() {
        GameManager.Instance.SetActiveScene(ActiveScene.Cam);

        _captureButton.onClick.AddListener(OnCapturePress);
        InitCam();
        
    }
    private void OnDisable() {
        _captureButton?.onClick.RemoveListener(OnCapturePress);

        _webCamTexture.Stop();
    }
    private void Update() {
        float ratio = (float) _webCamTexture.width / (float) _webCamTexture.height;
        _aspectRatioFitter.aspectRatio = ratio;

        float scaleY = _webCamTexture.videoVerticallyMirrored ? -1f : 1f;
        _camView.rectTransform.localScale = new Vector3(1f, scaleY, 1f);
        //_camView.rectTransform.localEulerAngles = new Vector3(0, 0, -_webCamTexture.videoRotationAngle);
        _camView.transform.rotation = _baseRotation * Quaternion.AngleAxis(_webCamTexture.videoRotationAngle, Vector3.back);
    }
    private void InitCam() {
        if (!_isCamRotated) {
            _baseRotation = _camView.transform.rotation;
            _isCamRotated = true;
        }

        //TODO: Cam access permission popup if denied access
        if (_webCamTexture == null) {
            _webCamTexture = new WebCamTexture();
        }

        _camView.texture = _webCamTexture;

        _webCamTexture.Play();
    }
    private void OnCapturePress() {
        _captureButton.gameObject.SetActive(false);
        _capturedImageTex = ScreenCapture.CaptureScreenshotAsTexture(ScreenCapture.StereoScreenCaptureMode.RightEye);

        PreviewImage.Instance.UpdatePreviewImageTexture(_capturedImageTex);
        PreviewImage.Instance.gameObject.SetActive(true);

        _captureButton.gameObject.SetActive(true);
        _imageNamePanel.SetActive(true);
        OnPhotoCaptured?.Invoke();

        GameManager.Instance.SetActiveScene(ActiveScene.ImageNameInput);
    }

    //============ INPUT NAME PANEL BUTTON ACTIONS =======================//

    /// <summary>
    /// On ok press after inputting image name, starts input string validation
    /// and on valid string saving to local drive and generating new image item
    /// </summary>
    public void SaveImage() {
        string imageName = _imageNameInput.text;

        ImageNameError imageValidationResult = GameManager.Instance.ValidateImageName(imageName);
        //validate image name 
        switch (imageValidationResult) {
            case ImageNameError.Duplicate:
                PopupText.Instance.DisplayPopup("Duplicate Name Exists!", 2);
                break;
            case ImageNameError.Empty:
                PopupText.Instance.DisplayPopup("Name field empty!", 2);
                break;
            case ImageNameError.None:
                GameManager.Instance.AddImageName(imageName);

                var capturedImage = _capturedImageTex.EncodeToPNG();
                var savePath = Path.Combine(Application.persistentDataPath, imageName + ".png");

                var imageObj = Instantiate(GameManager.Instance.ImagePrefab, GameManager.Instance.ImagePrefabsHolder);
                CapturedImage capturedImageInfo = imageObj.GetComponentInChildren<CapturedImage>();

                capturedImageInfo.ImageName.text = imageName;

                PopupText.Instance.DisplayPopup("saved successfully", 2);

                File.WriteAllBytes(savePath, capturedImage);

                _imageNamePanel.SetActive(false);
                PreviewImage.Instance.gameObject.SetActive(false);

                GameManager.Instance.SetActiveScene(ActiveScene.Home);
                break;
            
            
        }
        _imageNameInput.text = string.Empty;        
    }
    /// <summary>
    /// On retake press from input image name panel, close input name panel and 
    /// starts cam again
    /// </summary>
    public void RetakePhoto() {
        GameManager.Instance.SetActiveScene(ActiveScene.Cam);

        CloseImageNameInputPanel();
        OnRetakePhoto?.Invoke();
    }
    /// <summary>
    /// Cancel image save disabling input panel and preview image
    /// </summary>
    public void CloseImageNameInputPanel() {
        _imageNamePanel.SetActive(false);
        PreviewImage.Instance.gameObject .SetActive(false);
    }

    //============================================================//
}


