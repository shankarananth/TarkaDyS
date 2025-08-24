/*
 * File: FirstOrderProcess.cs
 * Author: Shankar Ananth Asokan
 * Purpose: First-order process model with dynamic random disturbance
 * Date Created: 24-Aug-2025
 * Date Modified: 24-Aug-2025
 * 
 * Description: A first-order process model with realistic random disturbance.
 * Represents the transfer function: K * e^(-Td*s) / (Tau*s + 1)
 * Features dynamic disturbance: Factor (0-100%) × Random(0-1) each cycle
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace TarkaDyS.Models
{
    /// <summary>
    /// First-order process with dead time and dynamic random disturbance
    /// Simulates industrial process with realistic noise characteristics
    /// </summary>
    public class FirstOrderProcess
    {
        #region Private Fields
        private double _processGain = 1.0;         // Process gain (K)
        private double _timeConstant = 10.0;       // Time constant (Tau) in seconds
        private double _deadTime = 1.0;            // Dead time (Td) in seconds
        private double _input = 50.0;              // Process input (MV) in %
        private double _output = 50.0;             // Process output (PV) in %
        private double _disturbanceFactor = 0.0;   // Disturbance factor (0-100%)
        private double _timeStep = 0.1;            // Simulation time step in seconds
        
        // Static random generator to avoid reseeding issues
        private static readonly Random _random = new Random();
        
        // Dead time implementation using queue buffer
        private readonly Queue<DeadTimeValue> _deadTimeBuffer = new();
        #endregion

        #region Public Properties
        /// <summary>Process gain - ratio of output change to input change</summary>
        public double ProcessGain { get => _processGain; set => _processGain = value; }
        
        /// <summary>Time constant in seconds - time to reach 63.2% of final value</summary>
        public double TimeConstant { get => _timeConstant; set => _timeConstant = Math.Max(0.1, value); }
        
        /// <summary>Dead time in seconds - delay before process responds to input changes</summary>
        public double DeadTime { get => _deadTime; set { _deadTime = Math.Max(0.0, value); _deadTimeBuffer.Clear(); } }
        
        /// <summary>Process input (MV) in percentage (0-100%)</summary>
        public double Input { get => _input; set => _input = value; }
        
        /// <summary>Process output (PV) in percentage (0-100%) - read only</summary>
        public double Output => _output;
        
        /// <summary>Disturbance factor (0-100%) - multiplied by random number each cycle</summary>
        public double Disturbance { get => _disturbanceFactor; set => _disturbanceFactor = Math.Max(0.0, Math.Min(100.0, value)); }
        
        /// <summary>Simulation time step in seconds</summary>
        public double TimeStep { get => _timeStep; set => _timeStep = Math.Max(0.001, value); }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize the process to steady-state conditions
        /// </summary>
        /// <param name="initialInput">Initial input value (MV) in %</param>
        /// <param name="initialOutput">Initial output value (PV) in %</param>
        public void Initialize(double initialInput = 50.0, double initialOutput = 50.0)
        {
            _input = initialInput;
            _output = initialOutput;
            InitializeDeadTimeBuffer();
            
            System.Diagnostics.Debug.WriteLine($"FirstOrderProcess initialized - Input: {_input:F2}%, Output: {_output:F2}%, Disturbance: {_disturbanceFactor:F1}%");
        }

        /// <summary>
        /// Update process for one simulation time step
        /// Applies first-order dynamics with dead time and random disturbance
        /// </summary>
        public void Update()
        {
            // Apply dead time to input signal
            double delayedInput = GetDelayedInput();
            
            // Calculate steady-state response to delayed input
            double steadyStateOutput = _processGain * delayedInput;
            
            // Apply first-order lag dynamics using Euler integration
            // Differential equation: dY/dt = (Yss - Y) / Tau
            double derivative = (steadyStateOutput - _output) / _timeConstant;
            _output += _timeStep * derivative;
            
            // Apply dynamic random disturbance if enabled
            ApplyRandomDisturbance();
            
            // Update the dead time buffer for next cycle
            UpdateDeadTimeBuffer();
        }

        /// <summary>
        /// Reset process while maintaining current steady-state values
        /// Clears dead time buffer and reinitializes
        /// </summary>
        public void Reset()
        {
            _deadTimeBuffer.Clear();
            InitializeDeadTimeBuffer();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Apply dynamic random disturbance to process output
        /// Disturbance = Factor% × Random(0-1) × Sign(±1)
        /// </summary>
        private void ApplyRandomDisturbance()
        {
            if (_disturbanceFactor <= 0.0) return;
            
            // Generate random amplitude (0 to disturbance factor)
            double randomValue = _random.NextDouble();
            double disturbanceAmplitude = (_disturbanceFactor / 100.0) * randomValue;
            
            // Apply random sign (bipolar disturbance)
            double disturbanceSign = _random.NextDouble() > 0.5 ? 1.0 : -1.0;
            double actualDisturbance = disturbanceAmplitude * disturbanceSign;
            
            // Add disturbance to process output
            _output += actualDisturbance;
            
            // Occasional debug output (1 in 50 cycles to avoid spam)
            if (_random.Next(50) == 0)
            {
                System.Diagnostics.Debug.WriteLine($"Disturbance applied: {_disturbanceFactor:F1}% × {randomValue:F3} × {disturbanceSign:+0;-0} = {actualDisturbance:F3}");
            }
        }

        /// <summary>
        /// Initialize dead time buffer with current input value
        /// Buffer size calculated based on dead time and time step
        /// </summary>
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

        /// <summary>
        /// Get input value delayed by dead time
        /// Returns current input if no dead time configured
        /// </summary>
        /// <returns>Delayed input value</returns>
        private double GetDelayedInput()
        {
            if (_deadTime <= 0 || _deadTimeBuffer.Count == 0)
                return _input;

            var delayedValue = _deadTimeBuffer.LastOrDefault();
            return delayedValue?.Value ?? _input;
        }

        /// <summary>
        /// Update dead time buffer with current input
        /// Maintains proper buffer size based on dead time
        /// </summary>
        private void UpdateDeadTimeBuffer()
        {
            if (_deadTime <= 0) return;

            // Add current input to buffer
            _deadTimeBuffer.Enqueue(new DeadTimeValue { Time = 0, Value = _input });

            // Remove excess entries to maintain correct buffer size
            int maxBufferSize = (int)Math.Ceiling(_deadTime / _timeStep) + 1;
            while (_deadTimeBuffer.Count > maxBufferSize)
            {
                _deadTimeBuffer.Dequeue();
            }
        }
        #endregion

        #region Private Classes
        /// <summary>
        /// Data structure for dead time buffer entries
        /// </summary>
        private class DeadTimeValue
        {
            public double Time { get; set; }    // Time stamp
            public double Value { get; set; }   // Input value
        }
        #endregion
    }
}