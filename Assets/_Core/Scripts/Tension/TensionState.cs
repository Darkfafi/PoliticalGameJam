
public delegate void TensionStateDelegate(TensionState tensionState);

public enum TensionState
{
    Idle,
    Pushy,
    Aggression,
    Outbreak
}