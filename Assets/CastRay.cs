using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CastRay : MonoBehaviour
{
	public float rotateFactor;

	public Canvas canvas;
	public Vector2 positionOnRect { get; set; }
	GraphicRaycaster uiRaycast;
	Ray ray;
	RaycastHit hitInfo;
	PointerEventData ped;
	List<RaycastResult> castRes;
	private bool catchTheTarget;
	private GameObject caughtTarget;

	public Camera cameraRenderer;


	public Vector2 startPosition;
	float previourCurrentXPosition;

	// Use this for initialization
	void Start()
	{
		ped = new PointerEventData(EventSystem.current);
		ped.position = Input.mousePosition;
		castRes = new List<RaycastResult>();
	}


	// Update is called once per frame
	void Update()
	{
		ped.position = Input.mousePosition;
		EventSystem.current.RaycastAll(ped, castRes);
		//uiRaycast = this.GetComponent<GraphicRaycaster>();

		if (Input.GetMouseButtonDown(0) && !catchTheTarget)
		{
			//Debug.Log(castRes.);
			var rect = castRes[0].gameObject.GetComponent<RectTransform>();
			if (rect.name != "RawImage") return;
			Vector3 rectPos = rect.position;
			Vector2 rectSize = rect.sizeDelta;
			Vector3 screenRectSize = rectSize * canvas.scaleFactor;
			Vector2 rectStartingPoint = new Vector2(rectPos.x - (screenRectSize.x / 2),
													rectPos.y - (screenRectSize.y / 2));

			positionOnRect = new Vector2(Input.mousePosition.x - rectStartingPoint.x,
										 Input.mousePosition.y - rectStartingPoint.y);

			float cameraRendererScaleFactor = cameraRenderer.pixelWidth / screenRectSize.x;

			ray = cameraRenderer.ScreenPointToRay(positionOnRect * cameraRendererScaleFactor);
			if (Physics.Raycast(ray, out hitInfo))
			{
				startPosition = Input.mousePosition;
				catchTheTarget = true;
				caughtTarget = hitInfo.transform.gameObject;
				//caughеTarget.SetActive(false);

				//Debug.Log(ray.origin);
				//Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 3.0f);
				Debug.Log(hitInfo.collider.name);
			}
		}

		if (catchTheTarget && Input.GetMouseButton(0))
		{
			if (previourCurrentXPosition == Input.mousePosition.x)
			{
				startPosition.x = previourCurrentXPosition;
				//return;
			}
			float xCurrentDifference = Input.mousePosition.x - startPosition.x;
			caughtTarget.transform.Rotate(new Vector3(0, -(xCurrentDifference * Time.deltaTime * rotateFactor), 0));
			previourCurrentXPosition = Input.mousePosition.x;
			Debug.Log("+");
		}

		if (Input.GetMouseButtonUp(0))
		{
			catchTheTarget = false;
		}

	}
}
