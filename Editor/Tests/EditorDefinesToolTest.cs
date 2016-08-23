﻿using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using NUnit.Framework;
using UDBase.Editor;

namespace UDBase.Editor.Tests {
	public class EditorDefinesToolTest {

		[Test]
		public void ConvertBuildTargetTest()
		{
			var currentBuildTargets = Enum.GetValues(typeof(BuildTarget));
			var iter = currentBuildTargets.GetEnumerator();
			while(iter.MoveNext()) {
				var target = (BuildTarget)iter.Current;
				if( IsCorrentTarget(target) ) {
					var group = BuildTargetGroup.Unknown;
					TestDelegate convertDelegate = delegate {
						group = EditorDefinesTool.ConvertBuildTarget(target);
					};
					Assert.DoesNotThrow(convertDelegate, "Failed to convert " + target); 
					Assert.AreNotEqual(BuildTargetGroup.Unknown, group, "Failed to convert " + target);
				}
			}
		}

		bool IsCorrentTarget(BuildTarget target) {
			var type = typeof(BuildTarget);
			var memInfo = type.GetMember(target.ToString());
			var attributes = memInfo[0].GetCustomAttributes(typeof(ObsoleteAttribute),
				false);
			return attributes.Length == 0;
		}
	}
}
