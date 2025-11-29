using System;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Input
{
    /// <summary>
    /// A class for taking input from a gamepad device
    /// </summary>
    public class GamepadInput : InputProfile
    {

        #region Constructors

        /// <summary>
        /// Creates a new KeyboardInput
        /// </summary>
        /// <param name="mappings">The mappings to use</param>
        /// <param name="device">The gamepad device to take input from</param>
        public GamepadInput(Dictionary<TetrisInput, GamepadButton[]> mappings, Gamepad device)
        {
            Device = device;
            CreateDictionary(mappings);
        }

        /// <summary>
        /// Sets up the dictionaries for InputControls and buffered inputs
        /// </summary>
        /// <param name="mappings">
        ///The mappings of DuckInputs to GamepadButtons
        /// </param>
        ///<remarks>
        ///GamepadButton does not have any handling for joystick inputs, so this method will add the respective 
        /// joystickControls to any inputs mapped to dPad buttons.
        /// </remarks>
        private void CreateDictionary(Dictionary<TetrisInput, GamepadButton[]> mappings)
        {
            Dictionary<TetrisInput, InputControl[]> toMap = new Dictionary<TetrisInput, InputControl[]>();
            
            foreach (KeyValuePair<TetrisInput, GamepadButton[]> map in mappings)
            {
                List<InputControl> controls = new List<InputControl>();
                foreach (GamepadButton key in map.Value)
                {
                    controls.Add(((Gamepad)Device)[key]);
                    //this is dumb but i can't use the GamepadButton enum to access stick direction controls,
                    //so it just checks if it's a dpad direction and if so, adds the corresponding stick direction
                    switch (key)
                    {
                        case GamepadButton.DpadLeft:
                            controls.Add(((Gamepad)Device).leftStick.left);
                            continue;
                        case GamepadButton.DpadDown:
                            controls.Add(((Gamepad)Device).leftStick.down);
                            continue;
                        case GamepadButton.DpadRight:
                            controls.Add(((Gamepad)Device).leftStick.right);
                            continue;
                        case GamepadButton.DpadUp:
                            controls.Add(((Gamepad)Device).leftStick.up);
                            continue;
                    }
                }
                toMap.Add(map.Key, controls.ToArray());
            }

            Mappings = toMap;
        }

        #endregion

        #region InputCollection

        /// <inheritdoc />
        public override void PushInputs(bool noEvents = false)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region InputRetreival

        /// <inheritdoc />
        public override bool Pressed(TetrisInput input)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool Released(TetrisInput input)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool Down(TetrisInput input)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool AnyPressed()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool AnyReleased()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override bool AnyDown()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}