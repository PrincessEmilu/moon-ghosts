/// <summary>
/// Interface for a behavior that can be triggered by a switch. Only intended to use with the pylons, but it could be used anywhere.
/// </summary>
public interface ISwitchTriggerable
{
    public void OnTriggerActivate();
}
