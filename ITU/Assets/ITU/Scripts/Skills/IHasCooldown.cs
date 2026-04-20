namespace ITU
{
	public interface IHasCooldown
	{
		CooldownHandler Cooldown { get; }
	}
}
