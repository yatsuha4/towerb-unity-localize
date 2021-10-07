using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace Towerb.Localize
{
  /**
     <summary>ロケールセレクタ</summary>
  */
  public class LocaleSelector
    : MonoBehaviour
  {
    [Serializable]
    public class OnChangeLocale : UnityEvent<Locale> {}

    [SerializeField]
    private OnChangeLocale onChangeLocale;

    /**
     */
    IEnumerator Start()
    {
      while(!LocalizationSettings.InitializationOperation.IsDone)
      {
        yield return null;
      }
      var dropdown = GetComponent<Dropdown>();
      var options = new List<Dropdown.OptionData>();
      int current = 0;
      foreach(var locale in LocalizationSettings.AvailableLocales.Locales)
      {
        if(LocalizationSettings.SelectedLocale == locale)
        {
          current = options.Count;
        }
        var name = locale.LocaleName;
        if(locale.Metadata.GetMetadata<Comment>() is Comment comment)
        {
          name = comment.CommentText;
        }
        options.Add(new Dropdown.OptionData(name));
      }
      dropdown.options = options;
      dropdown.value = current;
    }

    /**
     */
    public void OnValueChanged(int index)
    {
      var locale = LocalizationSettings.AvailableLocales.Locales[index];
      LocalizationSettings.SelectedLocale = locale;
      this.onChangeLocale.Invoke(locale);
    }
  }
}
