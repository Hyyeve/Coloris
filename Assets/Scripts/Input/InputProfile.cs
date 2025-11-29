using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Input
{
    /// <summary>
    /// A abstract class that serves as a base for all DiU input classes
    /// </summary>
    public abstract class InputProfile
    {
        #region Constants

        public static readonly TetrisInput[] DuckInputs = (TetrisInput[]) Enum.GetValues(typeof(TetrisInput));

        #endregion
        
        #region Vars
        
        protected Dictionary<TetrisInput, InputControl[]> Mappings;

        protected Dictionary<TetrisInput, int> BufferedInputs;

        protected Dictionary<TetrisInput, int> BufferedEvents;

        protected InputDevice Device;

        #endregion
         
        #region Constructors

        /// <summary>
        /// Creates a Input Profile using the given Keyboard device
        /// </summary>
        /// <param name="mappings">The mappings to use</param>
        /// <param name="device">The device to use</param>
        /// <returns>A KeyboardInput cast to InputProfile with the given device and mappings</returns>
        public static InputProfile CreateKeyboardInput(Dictionary<TetrisInput, Key[]> mappings, Keyboard device)
        {
            return new KeyboardInput(mappings, device);
        }

        /// <summary>
        /// Creates a Input Profile using the given Gamepad device
        /// </summary>
        /// <param name="mappings">The mappings to use</param>
        /// <param name="device">The device to use</param>
        /// <returns>A GamepadInput cast to InputProfile with the given device and mappings</returns>
        public static InputProfile CreateGamepadInput(Dictionary<TetrisInput, GamepadButton[]> mappings, Gamepad device)
        {
             return new GamepadInput(mappings, device);
        }

        /// <summary>
        /// Creates a Input profile using the current keyboard device and default (built-in) mappings
        /// </summary>
        /// <returns>A KeyboardInput cast to InputProfile with the default mappings</returns>
        public static InputProfile CreateDefaultKeyboard()
        {
            //TODO - i need to make the default layout more intuitive
            return new KeyboardInput(
                new Dictionary<TetrisInput, Key[]>()
                {

                    {TetrisInput.Left, new []{Key.A}},
                    {TetrisInput.Right, new []{Key.D}},

                    {TetrisInput.SoftDrop, new[] {Key.S}},
                    {TetrisInput.HardDrop, new[] {Key.W}},

                    {TetrisInput.RotateR, new[] {Key.RightArrow}},
                    {TetrisInput.RotateL, new[] {Key.LeftArrow}},
                    {TetrisInput.Hold, new[] {Key.UpArrow}},
                }, Keyboard.current
            );
        }

        /// <summary>
        /// Creates a Input profile using the current keyboard device and default (built-in) mappings
        /// </summary>
        /// <returns>A GamepadInput cast to InputProfile with the default mappings</returns>
        public static InputProfile CreateDefaultGamepad()
        {
            return new GamepadInput(
                new Dictionary<TetrisInput, GamepadButton[]>()
                {
                    //sadly the unity GamepadButton enum doesn't contain any stick directions, so i'm treating
                    //dpad as if it's both dpad and left stick. The gamepad input class will add the actual left
                    //stick controls to it's input array.
                    /*
                    {TetrisInput.Left, new[]{GamepadButton.DpadLeft}},
                    {TetrisInput.Right, new[]{GamepadButton.DpadRight}},
                    {TetrisInput.Up, new[]{GamepadButton.DpadUp}},
                    {TetrisInput.Down, new[]{GamepadButton.DpadDown}},

                    {TetrisInput.Jump, new[]{GamepadButton.South, GamepadButton.East}},
                    {TetrisInput.Strafe, new[]{GamepadButton.LeftShoulder}},
                    {TetrisInput.Ragdoll, new[]{GamepadButton.LeftTrigger}},

                    {TetrisInput.Grab, new[]{GamepadButton.West}},
                    {TetrisInput.Use, new[]{GamepadButton.RightTrigger}},
                    {TetrisInput.Quack, new[]{GamepadButton.North}},
                    {TetrisInput.Pause, new[]{GamepadButton.Start, GamepadButton.Select}}
                    */
                }, Gamepad.current
            );
        }

        protected virtual void SetupBuffers()
        {
            BufferedInputs = new Dictionary<TetrisInput, int>();
            BufferedEvents = new Dictionary<TetrisInput, int>();

            foreach (TetrisInput input in DuckInputs)
            {
                BufferedInputs.Add(input, 0);
                BufferedEvents.Add(input, 0);
            }
        }
        
        #endregion

        #region InputCollection

        /// <summary>
        /// Pushes the current inputs into the input buffer.
        /// Should be called once every Update()
        /// </summary>
        /// <param name="noEvents">Whether to include pressed/released events in this push.
        /// Used by <c>FinaliseInputs</c> to make sure all inputs are available but pressed/released events are not duplicated
        /// if there are multiple input cycles called per frame
        /// </param>
        public abstract void PushInputs(bool noEvents = false);


        /// <summary>
        /// Should be called before input is polled, to make sure all inputs are available and no events are duplicated.
        /// </summary>
        public void FinaliseInputs() => PushInputs(true);


        /// <summary>
        /// Clears the input buffer. Should be called after you have finished checking inputs.
        /// </summary>
        public virtual void ClearBufferedInputs()
        {
            foreach(TetrisInput inp in DuckInputs)
            {
                BufferedInputs[inp] = 0;
            }
        }

        public virtual void ClearBufferedEvents(List<TetrisInput> toClear = null)
        {
            if (toClear == null)
            {
                foreach (TetrisInput inp in DuckInputs)
                {
                    BufferedEvents[inp] = 0;
                }

                return;
            }

            foreach (TetrisInput inp in toClear)
            {
                BufferedEvents[inp] = 0;
            }

        }
        
        #endregion

        #region InputRetreival
        
        /// <summary>
        /// Checks if a input was pressed during this update
        /// </summary>
        /// <param name="input"> The input to check</param>
        /// <returns>
        /// Whether the input was pressed this update
        /// </returns>
        /// <remarks>
        ///This method should be implemented to return true if ANY of the associated input controls were released during this update.
        /// </remarks>
        public abstract bool Pressed(TetrisInput input);

        /// <summary>
        /// Checks if a input was released during this update
        /// </summary>
        /// <param name="input"> The input to check</param>
        /// <returns>
        /// Whether the input was released this update
        /// </returns>
        /// <remarks>
        ///This method should be implemented to return true if ANY of the associated input controls were released during this update.
        /// </remarks>
        public abstract bool Released(TetrisInput input);

        /// <summary>
        /// Checks if a input is currently being pressed down
        /// </summary>
        /// <param name="input"> The input to check</param>
        /// <returns>
        /// Whether the input is pressed down
        /// </returns>
        /// <remarks>
        ///This method should be implemented to return true if ANY of the associated input controls are being pressed down.
        /// </remarks>
        public abstract bool Down(TetrisInput input);

        /// <summary>
        /// Checks if any input was pressed during this update
        /// </summary>
        public abstract bool AnyPressed();

        /// <summary>
        /// Checks if any input was released during this update
        /// </summary>
        public abstract bool AnyReleased();

        /// <summary>
        /// Checks if any input is currently pressed down
        /// </summary>
        public abstract bool AnyDown();
        
        #endregion
    }

    /// <summary>
    /// A enum containing all inputs DiU makes use of
    /// </summary>
    public enum TetrisInput
    {
        Left, Right, SoftDrop, HardDrop, RotateL, RotateR, Hold, Debug1, Debug2, Debug3
    }
    
}