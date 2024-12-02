using System.Collections;
using TMPro;
using UnityEngine;

public class PopupText : MonoBehaviour
{
    public static PopupText Instance;

    [SerializeField]
    private TMP_Text _popupTextObj;
    
    private Coroutine _popupCoroutine;
    private void Awake() {
        if(Instance == null) { 
            Instance = this;
        } else {
            Destroy(this);
        }
    }
    public void DisplayPopup(string popupText, float duration) {
        _popupTextObj.text = popupText;
        if(_popupCoroutine != null) { 
            StopCoroutine( _popupCoroutine );
        }
        _popupCoroutine = StartCoroutine(ClearTextField(duration));
    }

    private IEnumerator ClearTextField(float duration) {
        yield return new WaitForSeconds(duration);
        _popupTextObj.text = string.Empty;
    }
}
