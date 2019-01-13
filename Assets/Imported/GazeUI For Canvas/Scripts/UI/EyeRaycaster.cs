using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;

public class EyeRaycaster : MonoBehaviour
{

	[SerializeField] private Text debugText;
	
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

	private bool planeDetected = false;

	[SerializeField] private Button startButton;
	
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
		try
		{
			// Centre of the screen
			PointerEventData pointer = new PointerEventData(EventSystem.current);
			pointer.position = new Vector2(Screen.width / 2, Screen.height / 2);
			pointer.button = PointerEventData.InputButton.Left;


			if (m_SessionOrigin.Raycast(pointer.position, s_Hits, TrackableType.PlaneWithinPolygon))
			{
				progress = Mathf.Lerp(1, 0, (endFocusTime - Time.time) / loadingTime);

				indicatorFillRT.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, curve.Evaluate(progress));
				indicatorFillRawImage.color = Color.Lerp(Color.clear, activeColor, curve.Evaluate(progress));
				centerRawImage.color = Color.Lerp(Color.black, Color.white, curve.Evaluate(progress));

				if (startButton.IsActive())
				{
					startButton.interactable = true;
				}
				
			}
			// No target -> reset
			else
			{
				if (startButton.IsActive())
				{
					startButton.interactable = false;
				}

				progress = Mathf.Lerp(0, 1, (Time.time - endFocusTime) / loadingTime * 2);
				indicatorFillRawImage.color = Color.Lerp(Color.white, Color.clear, curve.Evaluate(progress));
				centerRawImage.color = Color.Lerp(activeColor, Color.gray, curve.Evaluate(progress));

				if (progress >= 1f)
				{
					target = defaultValue;

					indicatorFillRT.localScale = Vector3.zero;
					centerRawImage.color = Color.gray;
				
					endFocusTime = Time.time + loadingTime;
				}

			}

		}
		catch (Exception e)
		{
			debugText.text = e.Message;
		}

	}
}
