using UnityEngine;
using UnityEngine.UI;

public class AddPhoto : MonoBehaviour
{
    private Button _addPhotoButton;

    [SerializeField]
    private GameObject _captureButtonObject;

    [SerializeField]
    private GameObject _cam;

    private void Awake() {
        _addPhotoButton = GetComponent<Button>();
        
    }
    private void OnEnable() {
        _addPhotoButton.onClick.AddListener(delegate { ActivateCamView(true); });
        CapturePhoto.OnPhotoCaptured += OnPhotoCaptured;
        CapturePhoto.OnRetakePhoto += OnRetakePhoto;
    }
    private void OnDisable() {
        _addPhotoButton.onClick.RemoveListener(delegate { ActivateCamView(true); });
        CapturePhoto.OnPhotoCaptured -= OnPhotoCaptured;
        CapturePhoto.OnRetakePhoto -= OnRetakePhoto;
    }
    private void OnPhotoCaptured() {
        ActivateCamView(false);
    }
    private void OnRetakePhoto() { 
        ActivateCamView(true);
    }
    /// <summary>
    /// activates cam view on / off
    /// </summary>
    /// <param name="active"></param>
    public void ActivateCamView(bool active) {
        _cam.SetActive(active);
        _captureButtonObject.SetActive(active);
    }
}
