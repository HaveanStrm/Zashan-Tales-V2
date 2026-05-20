using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LocalesSettings : MonoBehaviour
{
    private bool active = false;

    public void ChangeLocale(int localeID)
    {
        if (active == true)
            return;
        //StartCoroutine(SetLocale(localeID));
    }
    //IEnumerator SetLocale(int _localeID)
    ///{
    //active = true;
    //yield return LocalizationSettings.InitializationOperation;
    //LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
    // = false;
    //}
}

