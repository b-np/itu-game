namespace ITU
{
	public interface IHasCooldown
	{
		float Cooldown { get; }
		float ElapsedCooldown { get; }
	}
}
