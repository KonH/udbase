﻿using System;
using UDBase.Controllers.LogSystem;
using AssetBundles;

namespace UDBase.Controllers.ContentSystem {
	public sealed class AssetBundleContentController:IContent, ILogContext {
		
		[Serializable]
		public class Settings {
			public AssetBundleMode Mode = AssetBundleMode.StreamingAssets;
			public string Path;
		}

		readonly AssetBundleHelper _helper;

		readonly string _streamingAssetsPath;
		readonly string _baseUrl;

		public AssetBundleContentController(Settings settings, AssetBundleManager manager, AssetBundleHelper helper, ILog log) {
			if( settings.Mode == AssetBundleMode.StreamingAssets ) {
				_streamingAssetsPath = settings.Path;
			} else {
				if( string.IsNullOrEmpty(settings.Path) ) {
					log.Error(this, "For WebServer mode you need to provide path!");
				}
				_baseUrl = settings.Path;
			}
			_helper = helper;
			if( _helper ) {
				_helper.Init(log, manager, _streamingAssetsPath, _baseUrl);
			}
		}

		public bool CanLoad(ContentId id) {
			return id && (id.LoadType == ContentLoadType.AssetBundle);
		}

		public void LoadAsync<T>(ContentId id, Action<T> callback) where T:UnityEngine.Object {
			_helper.StartLoadAsync(id.BundleName, id.AssetName, callback);
		}
	}
}