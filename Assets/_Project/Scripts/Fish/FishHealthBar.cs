using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FishHealthBar : MonoBehaviour
{
	public Transform indicatorPivotPoint;
	public Image green;
	public GameObject bar;

	//--------------------------------------------------------

	public void Init(Transform indicatorPivotPoint)
	{
		this.indicatorPivotPoint = indicatorPivotPoint;
	}

	public void SetVisible(bool value)
	{
		bar.SetActive(value);
	}

	public void UpdateState(int value)
	{
		green.fillAmount = value * .01f; /* Mathf.Lerp(green.fillAmount, value * .01f, Time.deltaTime);*/
	}

	//--------------------------------------------------------

	private void Update()
	{
		Vector3 vec2 = Camera.main.WorldToScreenPoint(indicatorPivotPoint.position);
		bar.transform.position = vec2;
	}

	//private void Start()
	//{
	//	bar.SetActive(false);
	//}
}