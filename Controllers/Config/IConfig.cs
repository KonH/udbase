﻿using UnityEngine;
using System;
using System.Collections;
using UDBase.Controllers;
using UDBase.Utils.Json;

namespace UDBase.Controllers.Config {
	public interface IConfig : IController {

		T GetNode<T>() where T:class, IJsonNode, new();
	}
}