using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace Input
{
    public class InputComponent : MonoBehaviour
    {
        [SerializeField] private InputType defaultInput = InputType.Keyboard;
        [SerializeField] private float InputEventLeewayMilliseconds;
        
        private Stopwatch _timer;
        private int lastFrameCount;
        private List<TetrisInput> _eventsToClear;
        
        
        private InputProfile _inputProfile;
        public bool disableInput;

        private void Start()
        {
            Setup();
        }

        private void OnValidate()
        {
            if (Application.isPlaying) Setup();
        }

        private void Setup()
        {
            _timer = new Stopwatch();
            _timer.Start();
            _eventsToClear = new List<TetrisInput>();

            _inputProfile = defaultInput switch
            {
                InputType.Keyboard => InputProfile.CreateDefaultKeyboard(),
                InputType.Controller => InputProfile.CreateDefaultGamepad(),
                _ => InputProfile.CreateDefaultKeyboard()
            };
        }

        public void PushInputs()
        {
            if (lastFrameCount == Time.frameCount) return;
            _inputProfile.PushInputs();
            lastFrameCount = Time.frameCount;
        } 

        public void FinaliseInputs() => _inputProfile.FinaliseInputs();

        public void PopInputs()
        {
            _inputProfile.ClearBufferedInputs();
            _inputProfile.ClearBufferedEvents(_eventsToClear);
            _eventsToClear.Clear();
            if (!(_timer.ElapsedMilliseconds >= InputEventLeewayMilliseconds)) return;
            _inputProfile.ClearBufferedEvents();
            _timer.Restart();
        }

        public void ForcePopInputs()
        {
            _inputProfile.ClearBufferedInputs();
            _inputProfile.ClearBufferedEvents();
        }

        public bool Pressed(TetrisInput input)
        {
            bool flag = !disableInput && _inputProfile.Pressed(input);
            if (flag) _eventsToClear.Add(input);
            return flag;
        }
        public bool Released(TetrisInput input)
        {
            bool flag = !disableInput && _inputProfile.Released(input);
            if (flag) _eventsToClear.Add(input);
            return flag;
        }

        public bool Down(TetrisInput input) => !disableInput && _inputProfile.Down(input);

        public bool AnyPressed()
        {
            bool flag = !disableInput && _inputProfile.AnyPressed();
            if (flag) _eventsToClear.AddRange(InputProfile.DuckInputs);
            return flag;
        }

        public bool AnyReleased()
        {
            bool flag = !disableInput && _inputProfile.AnyReleased();
            if (flag) _eventsToClear.AddRange(InputProfile.DuckInputs);
            return flag;
        }

        public bool AnyDown() => !disableInput && _inputProfile.AnyDown();

        private enum InputType
        {
            Keyboard, Controller
        }
    }
}