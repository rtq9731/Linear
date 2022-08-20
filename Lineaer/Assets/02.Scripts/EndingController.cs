using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingController : MonoSingleton<EndingController>
{
    public void Ending()
    {
        ScreenFader.Instance.ScreenFade(1f);
    }
}
