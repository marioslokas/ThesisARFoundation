﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class EyeRaycaster : MonoBehaviour
{
	[SerializeField] private ARSessionOrigin m_SessionOrigin;
	List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();
	
	[SerializeField]
	float loadingTime;
	[SerializeField]
	float sliderIncrement;
	[SerializeField]
	Color activeColor;
	[SerializeField]
	AnimationCurve curve;
	[SerializeField]
	bool forceActive;

	float endFocusTime;
	float progress;

	RectTransform indicatorFillRT;
	RawImage indicatorFillRawImage;
	RawImage centerRawImage;

	ARRaycastHit lastActivatedTarget;
	ARRaycastHit target;

	private ARRaycastHit defaultValue;
	
	void Start()
	{
		indicatorFillRT = transform.Find("IndicatorFill").GetComponent<RectTransform>();
		indicatorFillRawImage = transform.Find("IndicatorFill").GetComponent<RawImage>();
		centerRawImage = transform.Find("Center").GetComponent<RawImage>();

		gameObject.SetActive(UnityEngine.XR.XRSettings.enabled || forceActive);

		endFocusTime = Time.time + loadingTime;
	}

	void Update()
	{
		// Centre of the screen
		PointerEventData pointer = new PointerEventData(EventSystem.current);
		pointer.position = new Vector2(Screen.width / 2, Screen.height / 2);
		pointer.button = PointerEventData.InputButton.Left;
		
		RaycastHit hitInfo;

		if (m_SessionOrigin.Raycast(pointer.position, s_Hits, TrackableType.PlaneWithinPolygon))
		{
			// Target is being activating -> fade in anim
			if (target == s_Hits[0] && target != lastActivatedTarget)
			{
				progress = Mathf.Lerp(1, 0, (endFocusTime - Time.time) / loadingTime);

				indicatorFillRT.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, curve.Evaluate(progress));
				indicatorFillRawImage.color = Color.Lerp(Color.clear, activeColor, curve.Evaluate(progress));
				centerRawImage.color = Color.Lerp(Color.black, Color.white, curve.Evaluate(progress));

//				if (target.GetComponent<Selectable>())
//					target.GetComponent<Selectable>().OnPointerEnter(pointer);

				if (Time.time >= endFocusTime && target != lastActivatedTarget)
				{
					lastActivatedTarget = target;

//					if (target.GetComponent<ISubmitHandler>() != null)
//						target.GetComponent<ISubmitHandler>().OnSubmit(pointer);
//					else if (target.GetComponentInParent<ISubmitHandler>() != null)
//						target.GetComponentInParent<ISubmitHandler>().OnSubmit(pointer);
//					else if (target.GetComponentInParent<Slider>() != null)
//					{
//						lastActivatedTarget = null;
//						endFocusTime = Time.time + loadingTime;
//
//						if (target.GetComponentInParent<Slider>().normalizedValue < 1f - sliderIncrement)
//							target.GetComponentInParent<Slider>().normalizedValue += sliderIncrement;
//						else if(target.GetComponentInParent<Slider>().normalizedValue != 1)
//							target.GetComponentInParent<Slider>().normalizedValue = 1;
//						else
//							target.GetComponentInParent<Slider>().normalizedValue = 0;
//					}
				}
			}

			// Target activated -> fade out anim
			else
			{
//				if (target && target.GetComponent<Selectable>()) 
//					target.GetComponent<Selectable>().OnPointerExit(pointer);

				if(target != s_Hits[0])
				{
					target = s_Hits[0];
					endFocusTime = Time.time + loadingTime;
				}

				progress = Mathf.Lerp(0, 1, (Time.time - endFocusTime) / loadingTime * 2);

				indicatorFillRawImage.color = Color.Lerp(Color.white, Color.clear, curve.Evaluate(progress));
				centerRawImage.color = Color.Lerp(activeColor, Color.gray, curve.Evaluate(progress));
			}
		}

		// No target -> reset
		else
		{
			lastActivatedTarget = defaultValue;

//			if (target && target.GetComponent<ARPlane>())
//				target.GetComponent<Selectable>().OnPointerExit(pointer);

			target = defaultValue;

			indicatorFillRT.localScale = Vector3.zero;
			centerRawImage.color = Color.gray;
			endFocusTime = Time.time + loadingTime;
		}

	}
}
