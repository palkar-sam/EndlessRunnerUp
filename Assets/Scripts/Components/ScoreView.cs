using UnityEngine;
using System.Collections;
using TMPro;

public class ScoreView : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI distanceText;


	public void AddDistance(int distance)
	{
		distanceText.text = distance.ToString();
    }
}

