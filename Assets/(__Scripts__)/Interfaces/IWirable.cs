using UnityEngine;

public interface IWirable : IColorable
{
    public GameObject Wire_GameObject { get; set; }
    public Vector3 GetHelperPointDirection();

    public GameObject GetItemGameObject();
    public Trait GetPinType();
    public int GetPinIndex();

    public LogicState LogicState { get; set; }
    public void UpdateVisual();
}
