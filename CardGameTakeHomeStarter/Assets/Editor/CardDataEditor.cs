using UnityEngine;
using UnityEditor;

namespace CardGame {
    [CustomEditor(typeof(CardData))]
    [CanEditMultipleObjects]
    public class CardDataEditor : Editor {
        // Overriding editor to include a button to fix asset naming.
        public override void OnInspectorGUI() {
            DrawDefaultInspector();

            if ( GUILayout.Button("Fix Asset Name")) {
                // Batch everything under one asset database write.
                AssetDatabase.StartAssetEditing();

                // Loop through all selected objects, for multi-editing.
                foreach ( Object obj in targets ) {
                    // Get card data, path and name.
                    CardData data = (CardData)obj;
                    string path = AssetDatabase.GetAssetPath(data);
                    string name = data.GetName();

                    // Rename asset appropriately.
                    AssetDatabase.RenameAsset( path , name );
                }

                AssetDatabase.StopAssetEditing();
                AssetDatabase.SaveAssets();
            }
        }
    }
}