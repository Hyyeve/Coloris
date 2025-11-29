using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Input
{
    /// <summary>
    /// A class for taking input from a keyboard device
    /// </summary>
    public class KeyboardInput : InputProfile
    {
        #region Constructors

        /// <summary>
        /// Creates a new KeyboardInput
        /// </summary>
        /// <param name="mappings">The mappings to use</param>
        /// <param name="device">The keyboard device to take input from</param>
        public KeyboardInput(Dictionary<TetrisInput, Key[]> mappings, Keyboard device)
        {
            Device = device;
            CreateDictionary(mappings);
        }
        
        /// <summary>
        /// Sets up the dictionaries for InputControls and buffered inputs
        /// </summary>
        /// <param name="mappings">
        ///The mappings of DuckInputs to keys
        /// </param>
        private void CreateDictionary(Dictionary<TetrisInput, Key[]> mappings)
        {
            Dictionary<TetrisInput, InputControl[]> toMap = new Dictionary<TetrisInput, InputControl[]>();
            
            foreach (KeyValuePair<TetrisInput, Key[]> map in mappings)
            {
                List<InputControl> controls = new List<InputControl>();
                foreach (Key key in map.Value)
                {
                    controls.Add(((Keyboard)Device)[key]);
                }
                toMap.Add(map.Key, controls.ToArray());
            }

            Mappings = toMap;

            SetupBuffers();
        }

        #endregion

        #region InputCollection

        /// <inheritdoc />
        public override void PushInputs(bool noEvents = false)
        {
            foreach (TetrisInput input in Mappings.Keys)
            {
                foreach (InputControl control in Mappings[input])
                {
                    if (((KeyControl) control).isPressed) BufferedInputs[input] = 1;

                    if (noEvents) continue;
                    if (((KeyControl) control).wasPressedThisFrame) BufferedEvents[input] = 1;
                    else if (((KeyControl) control).wasReleasedThisFrame) BufferedEvents[input] = 2;
                }
            }
        }

        #endregion

        #region InputRetreival

        /// <inheritdoc />
        public override bool Pressed(TetrisInput input)
        {
            return BufferedEvents[input] == 1;
        }

        /// <inheritdoc />
        public override bool Released(TetrisInput input)
        {
            return BufferedEvents[input] == 2;
        }

        /// <inheritdoc />
        public override bool Down(TetrisInput input)
        {
            return BufferedInputs[input] == 1;
        }

        /// <inheritdoc />
        public override bool AnyPressed()
        {
            return BufferedEvents.Any(predicate => predicate.Value == 1);
        }

        /// <inheritdoc />
        public override bool AnyReleased()
        {
            return BufferedEvents.Any(predicate => predicate.Value == 2);
        }

        /// <inheritdoc />
        public override bool AnyDown()
        {
            return BufferedInputs.Any(predicate => predicate.Value == 1);
        }

        #endregion
    }
}