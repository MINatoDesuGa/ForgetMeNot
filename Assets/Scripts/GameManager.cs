using System.Collections.Generic;
using UnityEngine;
using ForgetMeNotEnums;
using System.Collections;
using TMPro;
using UnityEngine.Android;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    public GameObject ImagePrefab;
    public Transform ImagePrefabsHolder;

    [HideInInspector]
    public Texture2D PreviewImageTexture;

    [HideInInspector]
    public List<string> ImageNamesList = new List<string>();

    public ActiveScene CurrentActiveScene;

    [SerializeField]
    private AddPhoto _addPhotoComponent;
    [SerializeField]
    private CapturePhoto _capturePhotoComponent;
    [SerializeField]
    private GameObject _promptObj;
    [SerializeField]
    private TMP_Text _promptText;

    private Coroutine _promptCoroutine;
    private WaitForSeconds _promptDuration = new(GameConfig.BACK_PROMPT_ENABLE_DURATION);
    private void Awake() {
        if(Instance == null) { 
            Instance = this; 
        } else {
            Destroy(this);
        }
    }

    private void Start() {
        SetActiveScene(ActiveScene.Home);
        PreviewImageTexture = new Texture2D(Screen.width, Screen.height);

        if(!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
            Permission.RequestUserPermission(Permission.Camera);
        }
    }
    private void Update() { 
        if(Input.GetKeyDown(KeyCode.Escape)) { //back press in android system UI
            HandleOnBackPress();
        }
    }
    private void HandleOnBackPress() {
        switch(CurrentActiveScene) { 
            case ActiveScene.Home:
                HandleAppExit();
                break;
            case ActiveScene.Cam: //if in cam scene, go back to home screen
                _addPhotoComponent.ActivateCamView(false);
                SetActiveScene(ActiveScene.Home);
                break;
            case ActiveScene.ImageNameInput: 
                HandleBackToHomeFromNameInput();
                break;
        }
    }
    private void HandleBackToHomeFromNameInput() {
        //on double tap back close input image name panel to display main menu
        if(_promptObj.activeInHierarchy) {
            _promptObj.SetActive(false);
            _capturePhotoComponent.CloseImageNameInputPanel();
            SetActiveScene(ActiveScene.Home);
            return;
        }
        //on single tap back display back to menu prompt
        InitPromptText("Press back again to go mainmenu");

    }
    private void HandleAppExit() {
        //on double tap back exit app
        if(_promptObj.activeInHierarchy) {
            Application.Quit();
            return;
        }

        //on single tap back display app exit prompt
        InitPromptText("Press back again to exit app");
    }
    private void InitPromptText(string promptText) {
        _promptText.text = promptText;
        _promptObj.SetActive(true);
        if (_promptCoroutine != null) {
            StopCoroutine(_promptCoroutine);
        }
        _promptCoroutine = StartCoroutine(DelayPromptDisable());
    }
    public void SetActiveScene(ActiveScene scene) {
        CurrentActiveScene = scene;
    }
    public void AddImageName(string imageName) {
        ImageNamesList.Add(imageName);
    }
    public void DeleteImageName(string imageName) {
        ImageNamesList.Remove(imageName);
    }
    /// <summary>
    /// Validates image name by comparing with existing image names
    /// </summary>
    /// <param name="imageName"></param> inputted image name to compare with
    /// <returns></returns>
    public ImageNameError ValidateImageName(string imageName) {
        if(imageName == null || imageName == "") { 
            return ImageNameError.Empty;
        }

        if( ImageNamesList.Contains(imageName)) {
            return ImageNameError.Duplicate;
        }

        return ImageNameError.None;
    }
    private IEnumerator DelayPromptDisable() {
        yield return _promptDuration;
        _promptObj.SetActive(false);
    }
}
