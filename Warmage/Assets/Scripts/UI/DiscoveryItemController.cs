using System;
using UnityEngine;


[RequireComponent(typeof(Renderer))]
public class DiscoveryItemController : MonoBehaviour
{
	public float FadeInTime = 0.5f,
				 DisplayTime = 2.0f,
				 FadeOutTime = 1.0f;
	[NonSerialized]
	public float Elapsed = 0.0f;


	public float DisplayTimeEnd { get { return FadeInTime + DisplayTime; } }
	public float FadeOutEnd { get { return FadeInTime + DisplayTime + FadeOutTime; } }


	private Renderer rnd;
	private Color baseCol;


	void Awake()
	{
		rnd = GetComponent<Renderer>();
		baseCol = rnd.material.color;
	}
	void Update()
	{
		if (Elapsed <= FadeInTime)
		{
			rnd.material.color = Color.Lerp(new Color(baseCol.r, baseCol.g, baseCol.b, 0.0f), baseCol,
											Elapsed / FadeInTime);
		}
		else if (Elapsed <= DisplayTimeEnd)
		{
			rnd.material.color = baseCol;
		}
		else if (Elapsed <= FadeOutEnd)
		{
			rnd.material.color = Color.Lerp(baseCol, new Color(baseCol.r, baseCol.g, baseCol.b, 0.0f),
											(Elapsed - DisplayTimeEnd) / FadeOutTime);
		}
		else
		{
			Destroy(gameObject);
		}

		Elapsed += Time.deltaTime;
	}
}