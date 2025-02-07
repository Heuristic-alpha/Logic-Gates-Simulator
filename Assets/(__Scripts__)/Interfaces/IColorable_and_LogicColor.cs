using UnityEngine;

public interface IColorable
{
    public LogicColor LogicColor { get; set; }
}

public readonly struct LogicColor
{
    private readonly Color _onColor;
    private readonly Color _offColor;
    public Color OnColor { get { return _onColor; } }
    public Color OffColor { get { return _offColor;} }

    public LogicColor(Color onColor, Color offColor)
    {
        _onColor = onColor;
        _offColor = offColor;
    }

    public override string ToString()
    {
        return $"LogicColor[OnColor({_onColor}), OffColor({_offColor})]";
    }

}