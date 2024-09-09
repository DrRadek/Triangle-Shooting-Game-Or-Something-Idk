using Godot;
using System;

public partial class GameManager : Node
{
    private Input.MouseModeEnum mouseMode1 = Input.MouseModeEnum.Visible;
    private Input.MouseModeEnum mouseMode2 = Input.MouseModeEnum.Captured;

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEvent keyEvent && keyEvent.IsPressed())
        {
            
            if (@event.IsActionPressed(GameInput.MouseModeSwitch))
            {
                GD.Print(@event);
                Input.MouseMode = Input.MouseMode == mouseMode1 ? mouseMode2 : mouseMode1;
            }
        }
    }
}
