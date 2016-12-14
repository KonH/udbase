﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UDBase.Controllers.InventorySystem.UI {
	public abstract class ItemControl : MonoBehaviour {
		
		public abstract void Init(HolderItemsView owner, InventoryItem item);
	}
}