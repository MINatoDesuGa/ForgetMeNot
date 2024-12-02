using UnityEngine;

namespace ForgetMeNotEnums {
    public class GlobalEnums : MonoBehaviour {
    }

    public enum ImageNameError {
        Duplicate, Empty, None
    }
    public enum ActiveScene {
        Home, Cam, ImageNameInput
    }
}