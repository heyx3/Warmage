using System;
using System.Collections.Generic;
using UnityEngine;


public static class Extensions
{
	/// <summary>
	/// Gets the horizontal part of the given 3D vector (i.e. the X and Z components).
	/// </summary>
	public static Vector2 Horz(this Vector3 v)
	{
		return new Vector2(v.x, v.z);
	}
	/// <summary>
	/// Turns this horizontal vector into a 3D vector with the given height component.
	/// </summary>
	public static Vector3 Full3D(this Vector2 v, float height)
	{
		return new Vector3(v.x, height, v.y);
	}
	
	public static float DistanceSqr(this Vector3 v1, Vector3 other)
	{
		float x = v1.x - other.x,
			  y = v1.y - other.y,
			  z = v1.z - other.z;
		return (x * x) + (y * y) + (z * z);
	}
	public static float Distance(this Vector3 v1, Vector3 other)
	{
		return Mathf.Sqrt(v1.DistanceSqr(other));
	}

	public static float DistanceSqr(this Vector2 v1, Vector2 other)
	{
		float x = v1.x - other.x,
			  y = v1.y - other.y;
		return (x * x) + (y * y);
	}
	public static float Distance(this Vector2 v1, Vector2 other)
	{
		return Mathf.Sqrt(v1.DistanceSqr(other));
	}

	public static Vector2 Rotate(this Vector2 v, float radians)
	{
		float sin = Mathf.Sin(radians),
			  cos = Mathf.Cos(radians);

		return new Vector2((cos * v.x) - (sin * v.y),
						   (sin * v.x) + (cos * v.y));
	}


	/// <summary>
	/// Takes a collection of collections and compresses it all into one list.
	/// </summary>
	public static IEnumerator<T> Combine<T, TCollection>(this IEnumerable<TCollection> listOfLists)
		where TCollection : IEnumerable<T>
	{
		foreach (TCollection c in listOfLists)
			foreach (T t in c)
				yield return t;
	}
}