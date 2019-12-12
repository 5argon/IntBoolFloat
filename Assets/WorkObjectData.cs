using Unity.Entities;

public struct WorkObjectData : IComponentData
{
    public int integer;
    public int timesUpdated;
    public float floating;
    public bool boolean;
}