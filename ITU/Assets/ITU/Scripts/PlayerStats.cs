using System;
using UnityEngine;

namespace ITU
{
	[Serializable]
	public class PlayerStats
	{
		[field: SerializeField] public float Attack { get; private set; } = 1.0f;
		[field: SerializeField] public float Defense { get; private set; } = 1.0f;

		[field: SerializeField] public float Reach { get; private set; } = 2.0f;
	}
}
