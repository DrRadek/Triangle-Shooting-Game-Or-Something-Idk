using System.Collections.Generic;
using System.Linq;
using Godot;

public partial class GameInput
{
    Godot.Collections.Dictionary<string, Godot.Collections.Array<InputEvent>> defaultInputs = new();
    Godot.Collections.Dictionary<string, Godot.Collections.Array<InputEvent>> customInputs = new();

    void CreateDefaultInputs()
    {
        // Movement
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.W),
            };
        }, MovementForward));

        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.S),
            };
        }, MovementBackward));

        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.A),
            };
        }, MovementLeft));

        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.D),
            };
        }, MovementRight));

        // MouseModeSwitch
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreatePhysicalInputEventKey(actionName, Key.M),
            };
        }, MouseModeSwitch));

        // Shoot
        AddAction(CreateActionLambda(actionName =>
        {
            return new(){
                CreateInputEventMouseButton(actionName, MouseButton.Left),
            };
        }, Shoot));
    }
}