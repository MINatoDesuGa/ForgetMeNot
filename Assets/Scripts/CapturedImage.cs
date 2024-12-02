using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CapturedImage : MonoBehaviour
{
    public TMP_Text ImageName;
    public static Action OnImageLoad;

    /// <summary>
    /// On Image item button press, load image from drive w.r.t name
    /// [Image name is the ID to fetch from drive] 
    /// </summary>
    public void LoadImage() {
        var imagePath = Path.Combine(Application.persistentDataPath, ImageName.text+".png");
        var imageData = File.ReadAllBytes(imagePath);

        PopupText.Instance.DisplayPopup("loading image", 0.5f);

        GameManager.Instance.PreviewImageTexture.LoadImage(imageData);

        OnImageLoad?.Invoke();
        PreviewImage.Instance.UpdatePreviewImageTexture(GameManager.Instance.PreviewImageTexture);
        PreviewImage.Instance.gameObject.SetActive(true);
    }
    /// <summary>
    /// On delete image item button press, deletes image from drive
    /// and disables image item object from image item scroll list
    /// </summary>
    public void DeleteImage() {
        var imageName = ImageName.text;
        GameManager.Instance.DeleteImageName(imageName);

        //delete image file from internal storage
        File.Delete(Path.Combine(Application.persistentDataPath, imageName + ".png"));

        PopupText.Instance.DisplayPopup("deleted image", 0.5f);
        //disable ingame element (destroy not done to avoid GC process)
        transform.parent.gameObject.SetActive(false);
    }
}
