using UnityEngine;
using UnityEngine.UI;
using MovementEffects;
using System.Collections;
using System.Collections.Generic;

/* TODO: Clean up later */

public class TurnTransition : MonoBehaviour {

	public Sprite playerBanner;
	public Sprite enemyBanner;
	public float pauseSeconds;

	public delegate void TransitionEndFunc();

	private FadeTransition fade;
	private Image overlayBG; // completely transparent and blocks raycasts during transition
	private Animator anim;
	private Dictionary<string, float> animationLengths;

	// Animation image and direction
	private Image bannerImage;
	private string enterAnimName;
	private string exitAnimName;
	private string enterMotion;
	private string exitMotion;
	private TransitionEndFunc transitionEndCallback;

	public bool IsRunning { get; private set; }

	void Awake() {
		anim = GetComponent<Animator>();
		fade = GetComponentInParent<FadeTransition>();
		overlayBG = fade.fadeImage;
		bannerImage = GetComponent<Image>();
		GetAnimationLengths();
	}

	private void GetAnimationLengths() {
		animationLengths = new Dictionary<string, float>();
		foreach(AnimationClip clip in anim.runtimeAnimatorController.animationClips) {
			Debug.Log(clip.name);
			animationLengths[clip.name] = clip.length;
		}
	}

	void Start() {
		overlayBG.enabled = false;
		anim.enabled = false;
		IsRunning = false;
	}

	public void PlayTransition(bool toEnemyTurn, TransitionEndFunc endCallback = null) {
		if (!IsRunning) {
            IsRunning = true;
            SelectionController.mode = SelectionMode.TurnTransition;
            transitionEndCallback = endCallback;
            Timing.RunCoroutine(AnimateTransition(toEnemyTurn));
        }
	}

	private IEnumerator<float> AnimateTransition(bool toEnemyTurn) {
		SetTurnAnimation(toEnemyTurn);
		overlayBG.enabled = true;
		anim.enabled = true;
		yield return Timing.WaitForSeconds(fade.BeginFade(FadeDirection.Out));
		anim.Play(enterAnimName);
		yield return Timing.WaitForSeconds(animationLengths[enterMotion]);
		yield return Timing.WaitForSeconds(pauseSeconds);
		anim.Play(exitAnimName);
		yield return Timing.WaitForSeconds(animationLengths[exitMotion]);
		yield return Timing.WaitForSeconds(fade.BeginFade(FadeDirection.In));
		anim.enabled = false;
		overlayBG.enabled = false;
		transitionEndCallback();
		IsRunning = false;
	}

	private void SetTurnAnimation(bool toEnemyTurn) {
		if (toEnemyTurn) {
			bannerImage.sprite = enemyBanner;
			enterAnimName = "TurnBannerEnterFromLeft";
			exitAnimName = "TurnBannerExitToRight";
			enterMotion = "TurnBannerEnter";
			exitMotion = "TurnBannerExit";
		} else {
			bannerImage.sprite = playerBanner;
			enterAnimName = "TurnBannerEnterFromRight";
			exitAnimName = "TurnBannerExitToLeft";
			enterMotion = "TurnBannerExit";
			exitMotion = "TurnBannerEnter";
		}
	}
}