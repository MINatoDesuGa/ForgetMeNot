using UnityEngine;
using UnityEngine.UI;

public class PreviewImage : MonoBehaviour
{
    public static PreviewImage Instance;

    [SerializeField]
    private RawImage _image;
    [SerializeField]
    private GameObject _backBtnObj;

    private void Awake() {
        if(Instance == null) { 
            Instance = this;
            CapturedImage.OnImageLoad += UpdateUIOnImageLoad;
            CapturePhoto.OnPhotoCaptured += UpdateUIOnPhotoCaptured;
        } else {
            Destroy(this);
        }
        gameObject.SetActive(false);
    }

    public void UpdatePreviewImageTexture(Texture2D newTexture) {
        _image.texture = newTexture;
    }
    private void UpdateUIOnImageLoad() {
        _backBtnObj.SetActive(true);
    }
    private void UpdateUIOnPhotoCaptured() { 
        _backBtnObj.gameObject.SetActive(false);
    }
}
