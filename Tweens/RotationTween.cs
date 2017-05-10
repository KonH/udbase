﻿using UnityEngine;
using DG.Tweening;
using UDBase.Utils;

namespace UDBase.Tweens {
	public class RotationTween : MonoBehaviour {

		public Vector3    EndValue = Vector3.zero;
		public float      Duration = 0;
		public RotateMode Mode     = RotateMode.Fast;
		public int        Loops    = -1;
		public Ease       Ease     = Ease.Unset;

		Sequence _seq = null;

		void OnEnable() {
			_seq = TweenHelper.Replace(_seq, false);
			_seq.Append(transform.DORotate(EndValue, Duration, Mode));
			_seq.SetLoops(Loops);
			_seq.SetEase(Ease);
		}

		void OnDisable() {
			_seq = TweenHelper.Reset(_seq);
		}
	}
}