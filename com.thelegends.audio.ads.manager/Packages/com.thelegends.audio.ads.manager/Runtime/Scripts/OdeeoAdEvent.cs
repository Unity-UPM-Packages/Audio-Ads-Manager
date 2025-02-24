using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum OdeeoAdEvent
{
    None,
    LoadRequest,
    LoadAvailable,
    ShowSuccess,
    ShowFail,
    ShowComplete,
    PopupSkip,
    PopupAppear,
    Close,
    Click,
    Pause,
    Resume,
    Mute
}