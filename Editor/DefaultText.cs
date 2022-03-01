using System.Linq;
using TMPro;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

namespace towerb.editor.localize
{
    /**
       ローカライズされたテキストを設定する
    */
    public static class DefaultText
    {
        /**
         */
        [MenuItem("Towerb/Localize/DefaultText")]
        public static void SetDefaultText()
        {
            foreach(var obj in Resources.FindObjectsOfTypeAll<LocalizeStringEvent>())
            {
                if(obj.GetComponent<Text>() is Text text)
                {
                    SetText(text, "m_text", obj.StringReference);
                }
                else if(obj.GetComponent<TextMeshProUGUI>() is TextMeshProUGUI textMesh)
                {
                    SetText(textMesh, "m_text", obj.StringReference);
                }
            }
        }

        /**
         */
        private static void SetText(Object component, string property, LocalizedString src)
        {
            if(!src.IsEmpty)
            {
                var collection = 
                    LocalizationEditorSettings.GetStringTableCollection(src.TableReference);
                if(collection != null)
                {
                    var entry = collection.GetRowEnumerator().
                        First(row => row.KeyEntry.Id == src.TableEntryReference.KeyId);
                    for(int i = 0; i < entry.TableEntriesReference.Length; i++)
                    {
                        var locale = 
                            LocalizationEditorSettings.GetLocale(entry.TableEntriesReference[i].Code);
                        if(locale.SortOrder == 0)
                        {
                            var serializedObject = new SerializedObject(component);
                            serializedObject.FindProperty(property).stringValue = 
                                entry.TableEntries[i].LocalizedValue;
                            serializedObject.ApplyModifiedProperties();
                            break;
                        }
                    }
                }
            }
        }
    }
}
