public interface IBorderable
{
    public bool IsMoving { get; set; }
    public Border GetBorder();
    //{
    //    return GetComponentInChildren<Border>();
    //}
}