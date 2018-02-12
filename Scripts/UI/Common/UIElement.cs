﻿using System.Collections.Generic;
using UnityEngine;
using UDBase.Controllers.EventSystem;
using Zenject;

namespace UDBase.UI.Common {
	public class UIElement : MonoBehaviour {
		public static List<UIElement> Instances = new List<UIElement>();

		public enum UIElementState {
			None,
			Showing,
			Shown,
			Hiding,
			Hidden
		}

		public bool   AutoShow       = true;
		public bool   InitialActive  = true;
		public bool   DisableOnHide  = true;
		public bool   CacheAnimation = true;
		public bool   Ordered       = false;
		public string Group          = null;
		
		public List<UIElement> Childs = new List<UIElement>();

		public bool HasChilds {
			get {
				return Childs.Count > 0;
			}
		}

		public bool HasParent {
			get {
				return _parent;
			}
		}

		public UIElementState State { get; private set; }

		public bool IsInteractable {
			get {
				return _isInteractable;
			}
			set {
				_isInteractable = value;
				UpdateInteractable(_isInteractable);
				if( HasChilds ) {
					for( int i = 0; i < Childs.Count; i++ ) {
						Childs[i].IsInteractable = _isInteractable;
					}
				}
			}
		}

		IShowAnimation  _showAnimation  = null;
		IHideAnimation  _hideAnimation  = null;
		IClearAnimation _clearAnimation = null;
		bool            _isInteractable = false;
		bool            _groupChecked   = false;
		CanvasGroup     _group          = null;
		UIElement       _parent         = null;

		IEvent _events;

		[Inject]
		public void Init(IEvent events) {
			_events = events;
			Instances.Add(this);
			if( CacheAnimation ) {
				AssingAnimation(true);
			}
			if( HasChilds ) {
				for( int i = 0; i < Childs.Count; i++ ) {
					Childs[i].SetParent(this);
				}
			}
			_events.Subscribe<UI_ElementHidden>(this, OnElementHidden);
		}

		bool IsChild(UIElement element) {
			return Childs.Contains(element);
		}

		bool IsAllChildsHidden() {
			for( int i = 0; i < Childs.Count; i++ ) {
				if( Childs[i].State != UIElementState.Hidden ) {
					return false;
				}
			}
			return true;
		}

		void OnElementHidden(UI_ElementHidden e) {
			if( Ordered && IsChild(e.Element) && IsAllChildsHidden() ) {
				PerformHide();
			}
		}

		void SetParent(UIElement parent) {
			_parent = parent;
		}

		void Start() {
			if( !HasParent ) {
				IsInteractable = InitialActive;
				if( AutoShow ) {
					Show();
				}
			}
		}

		void AssingAnimation(bool firstTime = false) {
			if( firstTime || !CacheAnimation ) {
				_showAnimation = GetComponent<IShowAnimation>();
				_hideAnimation = GetComponent<IHideAnimation>();
				_clearAnimation = GetComponent<IClearAnimation>();
			}
		}

		bool CanShow() {
			return 
				(State != UIElementState.Showing) &&
				(State != UIElementState.Shown);
		}

		public void SetHidden() {
			if( _clearAnimation != null ) {
				_clearAnimation.Clear();
			}
			if( HasChilds ) {
				for( int i = 0; i < Childs.Count; i++ ) {
					Childs[i].SetHidden();
				}
			}
		}

		public void SetShown() {
			if( _clearAnimation != null ) {
				_clearAnimation.Clear();
			}
			if( HasChilds ) {
				for( int i = 0; i < Childs.Count; i++ ) {
					Childs[i].SetShown();
				}
			}
		}

		[ContextMenu("Show")]
		public void Show() {
			State = UIElementState.Showing;
			gameObject.SetActive(true);
			AssingAnimation();
			SetHidden();
			if( _showAnimation != null ) {
				_showAnimation.Show(this, () => OnShowComplete());
			} else {
				OnShowComplete();
			}
			if( HasChilds ) {
				if( !Ordered ) {
					for( int i = 0; i < Childs.Count; i++ ) {
						Childs[i].Show();
					}
				}
			}
		}

		public void OnShowComplete() {
			State = UIElementState.Shown;
			_events.Fire(new UI_ElementShown(this));
			if( Ordered ) {
				for( int i = 0; i < Childs.Count; i++ ) {
					Childs[i].Show();
				}
			}
		}

		bool CanHide() {
			return 
				(State != UIElementState.Hiding) &&
				(State != UIElementState.Hidden);
		}

		[ContextMenu("Hide")]
		public void Hide() {
			State = UIElementState.Hiding;
			AssingAnimation();
			SetShown();
			if( !Ordered ) {
				PerformHide();
			}
			if( HasChilds ) {
				for( int i = 0; i < Childs.Count; i++ ) {
					Childs[i].Hide();
				}
			} else {
				PerformHide();
			}
		}

		void PerformHide() {
			AssingAnimation();
			if( _hideAnimation != null ) {
				_hideAnimation.Hide(this, () => OnHideComplete());
			} else {
				OnHideComplete();
			}
		}

		public void OnHideComplete() {
			if( DisableOnHide ) {
				gameObject.SetActive(false);
			}
			State = UIElementState.Hidden;
			_events.Fire(new UI_ElementHidden(this));
		}

		[ContextMenu("Activate")]
		public void Activate() {
			IsInteractable = true;
		}

		[ContextMenu("Deactivate")]
		public void Deactivate() {
			IsInteractable = false;
		}

		void UpdateInteractable(bool isInteractable) {
			if( !_groupChecked ) {
				_group = GetComponent<CanvasGroup>();
				_groupChecked = true;
			}
			if( _group ) {
				_group.interactable = _isInteractable;
			}
		}

		void OnDestroy() {
			Instances.Remove(this);
			_events?.Unsubscribe<UI_ElementHidden>(OnElementHidden);
		}
	}
}
