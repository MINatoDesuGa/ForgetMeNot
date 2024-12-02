using System.IO;
using UnityEngine;
/// <summary>
/// On start, goes through persistent path for saved images and 
/// populates the recent image scroll with saved images.
/// </summary>
public class ImageFetcher : MonoBehaviour
{
    private void Start() {
        FetchImagesFromPersistentPath();
    }
    private void FetchImagesFromPersistentPath() {
        //gets all image file paths in persistent path of app
        string[] savedImagePaths = Directory.GetFiles(Application.persistentDataPath);


        foreach (string filePath in savedImagePaths) {
            //create image item object in image scroll panel
            var imageObj = Instantiate(GameManager.Instance.ImagePrefab, GameManager.Instance.ImagePrefabsHolder);
            
            CapturedImage capturedImageInfo = imageObj.GetComponentInChildren<CapturedImage>();
            var imageName = Path.GetFileNameWithoutExtension(filePath); //get image name from path
            capturedImageInfo.ImageName.text = imageName; //assign to capturedImage script to be used as ID to load it on image item press
            
            GameManager.Instance.ImageNamesList.Add(imageName); //add to image name list (will be used for validation on image name input)
        }
    }
}
