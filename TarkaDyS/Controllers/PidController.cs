/*
 * File: PidController.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Simplified PID controller for process control simulation
 * Date Created: 2025-08-23
 * Date Modified: 2025-08-23
 * 
 * Description: A simplified, high-performance PID controller that supports multiple algorithms.
 * This controller is designed for reliability and performance in simulation environments.
 */

using System;

namespace TarkaDyS.Controllers
{
    /// <summary>
    /// PID algorithm types
    /// </summary>
    public enum PidAlgorithm
    {
        /// <summary>Basic PID - Traditional algorithm</summary>
        BasicPID = 0,
        /// <summary>I-PD - Integral on error, P and D on measurement</summary>
        I_PD = 1,
        /// <summary>PI-D - P and I on error, D on measurement</summary>
        PI_D = 2
    }

    /// <summary>
    /// Simplified, high-performance PID controller
    /// </summary>
    public class PidController
    {
        #region Private Fields
        private double _kp = 1.0;
        private double _ki = 0.1;
        private double _kd = 0.05;
        private double _setpoint = 50.0;
        private double _processVariable = 50.0;
        private double _output = 50.0;
        private double _manualOutput = 50.0;
        private bool _autoMode = true;
        
        // Internal calculation variables
        private double _integralSum = 0.0;
        private double _previousError = 0.0;
        private double _previousPV = 50.0;
        
        // Output limits
        private double _outputMin = 0.0;
        private double _outputMax = 100.0;
        
        // Algorithm type
        private PidAlgorithm _algorithm = PidAlgorithm.BasicPID;
        #endregion

        #region Properties
        public double Kp { get => _kp; set => _kp = value; }
        public double Ki { get => _ki; set => _ki = value; }
        public double Kd { get => _kd; set => _kd = value; }
        public double Setpoint { get => _setpoint; set => _setpoint = value; }
        public double ProcessVariable => _processVariable;
        public double Output => _output;
        public double ManualOutput { get => _manualOutput; set => _manualOutput = value; }
        public bool AutoMode { get => _autoMode; set => _autoMode = value; }
        public double Error => _setpoint - _processVariable;
        public PidAlgorithm Algorithm { get => _algorithm; set => _algorithm = value; }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize the controller with steady-state values
        /// </summary>
        public void Initialize(double initialOutput = 50.0, double initialPV = 50.0)
        {
            _output = initialOutput;
            _processVariable = initialPV;
            _previousPV = initialPV;
            _manualOutput = initialOutput;
            
            // Calculate initial integral sum for steady state
            if (_autoMode && Math.Abs(_ki) > 1e-6)
            {
                // For steady state: Output = Kp*0 + Ki*IntegralSum + Kd*0
                // Therefore: IntegralSum = Output / Ki
                _integralSum = _output / _ki;
                
                // Clamp to prevent windup
                double maxIntegral = _outputMax / Math.Max(_ki, 1e-6);
                double minIntegral = _outputMin / Math.Max(_ki, 1e-6);
                _integralSum = Math.Max(minIntegral, Math.Min(maxIntegral, _integralSum));
            }
            else
            {
                _integralSum = 0.0;
            }
            
            _previousError = 0.0;
        }

        /// <summary>
        /// Update the controller with new process variable and calculate output
        /// </summary>
        public double Update(double processVariable, double deltaTime)
        {
            _processVariable = processVariable;
            
            if (!_autoMode)
            {
                _output = _manualOutput;
                return _output;
            }
            
            double calculatedOutput = _algorithm switch
            {
                PidAlgorithm.BasicPID => CalculateBasicPID(deltaTime),
                PidAlgorithm.I_PD => CalculateI_PD(deltaTime),
                PidAlgorithm.PI_D => CalculatePI_D(deltaTime),
                _ => CalculateBasicPID(deltaTime)
            };
            
            // Apply output limits
            _output = Math.Max(_outputMin, Math.Min(_outputMax, calculatedOutput));
            
            // Update previous values
            _previousPV = processVariable;
            
            return _output;
        }

        /// <summary>
        /// Reset the controller to initial state while preserving steady-state values
        /// </summary>
        public void Reset()
        {
            _previousError = 0.0;
            _integralSum = 0.0;
            _previousPV = _processVariable;
            // Don't reset _output, _processVariable, or _manualOutput to maintain steady state
        }

        /// <summary>
        /// Set output limits
        /// </summary>
        public void SetOutputLimits(double min, double max)
        {
            _outputMin = min;
            _outputMax = max;
        }
        #endregion

        #region Private Methods
        private double CalculateBasicPID(double deltaTime)
        {
            double error = _setpoint - _processVariable;
            
            // Proportional term
            double proportional = _kp * error;
            
            // Integral term with windup protection
            _integralSum += error * deltaTime;
            double integralOutput = _ki * _integralSum;
            if (integralOutput > _outputMax)
                _integralSum = _outputMax / Math.Max(_ki, 1e-6);
            else if (integralOutput < _outputMin)
                _integralSum = _outputMin / Math.Max(_ki, 1e-6);
            double integral = _ki * _integralSum;
            
            // Derivative term
            double derivative = 0.0;
            if (deltaTime > 0)
            {
                derivative = _kd * (error - _previousError) / deltaTime;
            }
            _previousError = error;
            
            return proportional + integral + derivative;
        }

        private double CalculateI_PD(double deltaTime)
        {
            double error = _setpoint - _processVariable;
            
            // Proportional on measurement
            double proportional = _kp * (_setpoint - _processVariable);
            
            // Integral on error
            _integralSum += error * deltaTime;
            double integralOutput = _ki * _integralSum;
            if (integralOutput > _outputMax)
                _integralSum = _outputMax / Math.Max(_ki, 1e-6);
            else if (integralOutput < _outputMin)
                _integralSum = _outputMin / Math.Max(_ki, 1e-6);
            double integral = _ki * _integralSum;
            
            // Derivative on measurement
            double derivative = 0.0;
            if (deltaTime > 0)
            {
                derivative = -_kd * (_processVariable - _previousPV) / deltaTime;
            }
            
            return proportional + integral + derivative;
        }

        private double CalculatePI_D(double deltaTime)
        {
            double error = _setpoint - _processVariable;
            
            // Proportional on error
            double proportional = _kp * error;
            
            // Integral on error
            _integralSum += error * deltaTime;
            double integralOutput = _ki * _integralSum;
            if (integralOutput > _outputMax)
                _integralSum = _outputMax / Math.Max(_ki, 1e-6);
            else if (integralOutput < _outputMin)
                _integralSum = _outputMin / Math.Max(_ki, 1e-6);
            double integral = _ki * _integralSum;
            
            // Derivative on measurement
            double derivative = 0.0;
            if (deltaTime > 0)
            {
                derivative = -_kd * (_processVariable - _previousPV) / deltaTime;
            }
            
            return proportional + integral + derivative;
        }
        #endregion
    }
}