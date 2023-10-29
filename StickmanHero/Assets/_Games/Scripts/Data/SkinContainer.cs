using UnityEditor;
using UnityEngine;
namespace SuperFight
{
    [CreateAssetMenu(fileName = "SkinContainer", menuName = "Data/SkinContainer", order = 0)]
    public class SkinContainer : ScriptableObject
    {
        public SkinObject[] container;
#if UNITY_EDITOR
        [ContextMenu("GenID")]
        void GenID()
        {
            for (int i = 0; i < container.Length; i++)
            {
                // container[i].id = i;
                // string assetPath = AssetDatabase.GetAssetPath(container[i].GetInstanceID());
                // string skinName = container[i].skinName;
                // int index = 0;
                // for (int j = 0; j < skinName.Length; j++)
                // {
                //     if (!char.IsLetterOrDigit(skinName[j]))
                //     {
                //         index = j;
                //     }
                // }
                // int Length = skinName.Length - index;
                // string newName = skinName.Substring(index, Length);
                // AssetDatabase.RenameAsset(assetPath, newName);
                // AssetDatabase.SaveAssets();
            }
        }
#endif

    }
}