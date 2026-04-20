using System;

namespace ITU
{
	public class Health
	{
		public float Current { get; private set; }
		public float Max { get; private set; }

		public Health(float current, float max)
		{
			Current = current;
			Max = max;
		}

		public Health(float value) : this(value, value)
		{

		}

		public bool IsAlive => Current > 0.0f;

		public void Increase(float value)
		{
			Current = Math.Min(Current + value, Max);
		}

		public void Decrease(float value)
		{
			Current = Math.Max(Current - value, 0.0f);
		}
	}
}
