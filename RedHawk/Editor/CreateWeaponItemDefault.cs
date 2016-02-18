using UnityEngine;
using System.Collections;
using UnityEditor;

namespace RedHawk{
	public class CreateWeaponItemDefault {
		[MenuItem("Tools/RedHawk/Create Weapon Item Defaults List Container")]
		public static WeaponItemDefaultsList Create(){
			WeaponItemDefaultsList asset = ScriptableObject.CreateInstance<WeaponItemDefaultsList>();
			AssetDatabase.CreateAsset(asset,"Assets/RedHawk/ScriptableObject/NewWeaponItemDefaultsList.asset");
			AssetDatabase.SaveAssets();
			return asset;
		}
	}
}
