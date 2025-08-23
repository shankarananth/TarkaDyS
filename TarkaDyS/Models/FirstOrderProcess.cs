/*
 * File: FirstOrderProcess.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Simplified first-order process model
 * Date Created: 2025-08-23
 * Date Modified: 2025-08-23
 * 
 * Description: A simplified, high-performance first-order process model.
 * Represents the transfer function: K * e^(-Td*s) / (Tau*s + 1)
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace TarkaDyS.Models
{
    /// <summary>
    /// Simplified first-order process with dead time
    /// </summary>
    public class FirstOrderProcess
    {
        #region Private Fields
        private double _processGain = 1.0;     // K
        private double _timeConstant = 10.0;   // Tau (seconds)
        private double _deadTime = 1.0;        // Td (seconds)
        private double _input = 50.0;          // Process input (MV)
        private double _output = 50.0;         // Process output (PV)
        private double _disturbance = 0.0;     // External disturbance
        private double _timeStep = 0.1;        // Simulation time step
        
        // Dead time buffer
        private readonly Queue<DeadTimeValue> _deadTimeBuffer = new();
        #endregion

        #region Properties
        public double ProcessGain { get => _processGain; set => _processGain = value; }
        public double TimeConstant { get => _timeConstant; set => _timeConstant = Math.Max(0.1, value); }
        public double DeadTime { get => _deadTime; set { _deadTime = Math.Max(0.0, value); _deadTimeBuffer.Clear(); } }
        public double Input { get => _input; set => _input = value; }
        public double Output => _output;
        public double Disturbance { get => _disturbance; set => _disturbance = value; }
        public double TimeStep { get => _timeStep; set => _timeStep = Math.Max(0.001, value); }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize the process with steady-state values
        /// </summary>
        public void Initialize(double initialInput = 50.0, double initialOutput = 50.0)
        {
            _input = initialInput;
            _output = initialOutput;
            InitializeDeadTimeBuffer();
        }

        /// <summary>
        /// Update the process for one time step using Euler integration
        /// </summary>
        public void Update()
        {
            // Get delayed input
            double delayedInput = GetDelayedInput();
            
            // Calculate steady-state output
            double steadyStateOutput = _processGain * (delayedInput + _disturbance);
            
            // Apply first-order dynamics: dY/dt = (Yss - Y) / Tau
            double derivative = (steadyStateOutput - _output) / _timeConstant;
            _output += _timeStep * derivative;
            
            // Update dead time buffer
            UpdateDeadTimeBuffer();
        }

        /// <summary>
        /// Reset the process while preserving current values
        /// </summary>
        public void Reset()
        {
            _deadTimeBuffer.Clear();
            InitializeDeadTimeBuffer();
        }
        #endregion

        #region Private Methods
        private void InitializeDeadTimeBuffer()
        {
            _deadTimeBuffer.Clear();
            
            if (_deadTime > 0)
            {
                int bufferSize = Math.Max(1, (int)Math.Ceiling(_deadTime / _timeStep));
                for (int i = 0; i < bufferSize; i++)
                {
                    _deadTimeBuffer.Enqueue(new DeadTimeValue 
                    { 
                        Value = _input, 
                        Time = -_deadTime + (i * _timeStep)
                    });
                }
            }
        }

        private double GetDelayedInput()
        {
            if (_deadTime <= 0 || _deadTimeBuffer.Count == 0)
                return _input;

            var delayedValue = _deadTimeBuffer.LastOrDefault();
            return delayedValue?.Value ?? _input;
        }

        private void UpdateDeadTimeBuffer()
        {
            if (_deadTime <= 0) return;

            // Add current input
            _deadTimeBuffer.Enqueue(new DeadTimeValue { Time = 0, Value = _input });

            // Remove old values
            while (_deadTimeBuffer.Count > 0 && 
                   _deadTimeBuffer.Count > Math.Ceiling(_deadTime / _timeStep) + 1)
            {
                _deadTimeBuffer.Dequeue();
            }
        }
        #endregion

        #region Private Classes
        private class DeadTimeValue
        {
            public double Time { get; set; }
            public double Value { get; set; }
        }
        #endregion
    }
}