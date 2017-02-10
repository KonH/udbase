﻿using UnityEngine;
using DG.Tweening;
using UDBase.Utils;

namespace UDBase.UI.Common {
    public class UIScaleAnimation : UIShowHideAnimation
    {
		public float Duration = 1.0f;
		
		Vector3 _originalScale = Vector3.zero;
		Sequence _seq          = null;

		void Awake() {
			_originalScale = transform.localScale;
			if( HasShowAnimation ) {
				transform.localScale = Vector3.zero;
			}
		}

        public override void Show(UIElement element) {
			if( !HasShowAnimation ) {
				transform.localScale = _originalScale;
				element.OnShowComplete();
				return;
			}
			_seq = TweenHelper.Replace(_seq);
			_seq.Append(transform.DOScale(_originalScale, Duration));
			_seq.AppendCallback(() => element.OnShowComplete());
        }

		public override void Hide(UIElement element) {
			if( !HasHideAnimation ) {
				element.OnHideComplete();
				return;
			}
			_seq = TweenHelper.Replace(_seq);
			_seq.Append(transform.DOScale(Vector3.zero, Duration));
			_seq.AppendCallback(() => element.OnHideComplete());
        }
    }
}
