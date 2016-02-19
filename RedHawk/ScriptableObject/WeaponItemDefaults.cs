using UnityEngine;
using System.Collections;
using UnityEditor;


namespace RedHawk{
	[System.Serializable]
	public enum weaponTypes {//JUST DO RANGED add Melee later
		Ranged
	}
[System.Serializable]
	public class WeaponItemDefaults {
		public string weaponDefaultName = "NewWeapon";
		public Texture2D weaponIcon = null;
		public weaponTypes weaponType = weaponTypes.Ranged;
		public vp_ItemType weaponItemType = null;
		//Add Weapon Setup Values
		public float renderingFieldOfView = 35.0f;
		public float renderingZoomDamping = 0.5f;
		public Vector3 weaponPositionSprings = new Vector3(-0.15f,-0.3f,-0.15f);
		public Vector3 weaponRotationSprings = new Vector3(-0.15f,-0.3f,-0.15f);
		public TextAsset weaponReloadState, weaponZoomState, weaponOutOfControlState, weaponSlideState, weaponCrouchState, weaponRunState, weaponAttackState;
		public TextAsset weaponShooterZoomState;
		public AudioClip weaponWieldSound, weaponUnWieldSound;
		public AnimationClip weaponWieldAnim, weaponUnwieldAnim;
		//Shooter Values
		public Vector3 shooterPositionRecoil = new Vector3(0f,0f,-0.04f);
		public Vector3 shooterRotationRecoil = new Vector3(-0.25f,0f,1f);
		//Projectile
		public float prjctlFireRate=1.2f;//t_FPShoot.ProjectileFiringRate = FLOAT FIRE RATE
		public float prjctlTapFire=1.0f;//t_FPShoot.ProjectileTapFiringRate = FLOAT TAP FIRE RATE - FASTEST YOU CAN FIRE
		public GameObject prjctlPreab=null;//t_FPShoot.ProjectilePrefab = GAMEOBJECT WITH vp_HitscanBullet
		public int prjctlCount=1;//t_FPShoot.ProjectileCount = INT BULLET COUNT per SHOT
		public float prjctlSpread=5.5f;//t_FPShoot.ProjectileSpread = FLOAT 0 to 360
		//MOTION - USE DEFAULTS
		//MUZZLE FLASH - if not listed, using defaults
		public GameObject mzlFlash=null;//t_FPShoot.MuzzleFlashPrefab = GAMEOBJECT WITH vp_MuzzleFlash
		//SHELL - if not listed, using defaults
		public GameObject shell=null;//t_FPShoot.ShellPrefab = GAMEOBJECT WITH vp_Shell
		public float shellSize=1f;
		//SOUND - if not listed, using defaults
		public AudioClip sndFire=null;//t_FPShoot.SoundFire = AUDIO CLIP
		public AudioClip sndDryFire=null;//t_FPShoot.SoundDryFire = AUDIO CLIP
		//ANIMATION - if not listed, using defaults
		//DO WE NEED TO CHECK IF NULL??
		public AnimationClip animFire=null;//t_FPShoot.AnimationFire = ANIMATION CLIP
		//RELOAD
		public AudioClip reloadSound=null;//t_FPReload.SoundReload = RELOAD AUDIOCLIP
		public float rldDuration=1f;//t_FPReload.ReloadDuration = 1f;//USE DEFAULS??
		public AnimationClip rldAnim=null;//t_FPReload.AnimationReload = RELOAD ANIMATION

	}
}