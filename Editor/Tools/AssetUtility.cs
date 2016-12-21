﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace UDBase.Editor {
	public static class AssetUtility {

		public static T CreateAsset<T>() where T : ScriptableObject {
			T asset = ScriptableObject.CreateInstance<T>();
			string path = AssetDatabase.GetAssetPath (Selection.activeObject);
			if ( path == "" ) {
				path = "Assets";
			} else if ( Path.GetExtension (path) != "" ) {
				path = path.Replace(Path.GetFileName(AssetDatabase.GetAssetPath(Selection.activeObject)), "");
			}
			string assetPathAndName = path + "/" + typeof(T).ToString() + ".asset";
			assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(assetPathAndName);
			AssetDatabase.CreateAsset(asset, assetPathAndName);
			SaveAndFocusAsset(asset);
			return asset;
		}

		public static T AddSubAsset<T>(ScriptableObject parent, bool select) where T:ScriptableObject {
			T asset = ScriptableObject.CreateInstance<T>();
			AssetDatabase.AddObjectToAsset(asset, parent);
			if( select ) {
				SaveAndFocusAsset(asset);
			} else {
				SaveAssets();
			}
			return asset;
		}

		public static void SaveAndFocusAsset<T>(T asset) where T:ScriptableObject {
			SaveAssets();
			FocusAsset(asset);
		}

		public static void SaveAssets() {
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
		}

		public static void FocusAsset<T>(T asset) where T:ScriptableObject {
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = asset;
		}

		public static void RemoveSubAsset<T>(T subasset) where T:ScriptableObject {
			ScriptableObject.DestroyImmediate(subasset, true);
			SaveAssets();
		}
	}
}
