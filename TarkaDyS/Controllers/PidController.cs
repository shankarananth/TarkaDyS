/*
 * File: PidController.cs
 * Author: Shankar Ananth Asokan
 * Purpose: Velocity-form PID controller with multiple algorithms and kick elimination
 * Date Created: 24-Aug-2025
 * Date Modified: 24-Aug-2025
 * 
 * Description: Industrial-quality PID controller supporting three algorithms:
 * - BasicPID: Traditional PID (proportional kick present)
 * - I-PD: Integral on error, P&D on measurement (no kicks)
 * - PI-D: P&I on error, D on measurement (no derivative kick)
 * Uses velocity form for inherent bumpless operation and numerical stability.
 */

using System;

namespace TarkaDyS.Controllers
{
    /// <summary>
    /// PID algorithm enumeration
    /// </summary>
    public enum PidAlgorithm
    {
        /// <summary>Traditional PID - all terms calculated on error (has proportional and derivative kicks)</summary>
        BasicPID = 0,
        /// <summary>I-PD algorithm - Integral on error, Proportional and Derivative on measurement (no kicks)</summary>
        I_PD = 1,
        /// <summary>PI-D algorithm - Proportional and Integral on error, Derivative on measurement (no derivative kick)</summary>
        PI_D = 2
    }

    /// <summary>
    /// Velocity-form PID controller with kick elimination capabilities
    /// Implements industrial-standard algorithms for smooth control operation
    /// </summary>
    public class PidController
    {
        #region Private Fields
        // PID parameters with conservative default tuning
        private double _kp = 0.5;                    // Proportional gain
        private double _ki = 0.1;                    // Integral gain (1/time units)
        private double _kd = 0.0;                    // Derivative gain (time units)
        
        // Process variables
        private double _setpoint = 50.0;             // Desired value (SP) in %
        private double _processVariable = 50.0;      // Measured value (PV) in %
        private double _output = 50.0;               // Controller output (MV) in %
        private double _manualOutput = 50.0;         // Manual mode output in %
        
        // Operating mode and algorithm
        private bool _autoMode = true;               // True = Auto, False = Manual
        private PidAlgorithm _algorithm = PidAlgorithm.BasicPID;
        
        // Historical values for velocity form calculations
        private double _previousError = 0.0;         // Error from previous scan
        private double _previousPV = 50.0;           // PV from previous scan
        private double _previousPV2 = 50.0;          // PV from two scans ago
        private double _previousSetpoint = 50.0;     // SP from previous scan
        
        // Output limits
        private double _outputMin = 0.0;             // Minimum output limit
        private double _outputMax = 100.0;           // Maximum output limit
        #endregion

        #region Public Properties
        /// <summary>Proportional gain - determines response strength to current error</summary>
        public double Kp { get => _kp; set => _kp = value; }
        
        /// <summary>Integral gain - determines response strength to accumulated error</summary>
        public double Ki { get => _ki; set => _ki = value; }
        
        /// <summary>Derivative gain - determines response strength to error rate of change</summary>
        public double Kd { get => _kd; set => _kd = value; }
        
        /// <summary>Setpoint (desired value) in percentage</summary>
        public double Setpoint { get => _setpoint; set => _setpoint = value; }
        
        /// <summary>Process variable (measured value) in percentage - read only</summary>
        public double ProcessVariable => _processVariable;
        
        /// <summary>Controller output (manipulated variable) in percentage - read only</summary>
        public double Output => _output;
        
        /// <summary>Manual mode output value in percentage</summary>
        public double ManualOutput { get => _manualOutput; set => _manualOutput = value; }
        
        /// <summary>Controller operating mode - true for Auto, false for Manual</summary>
        public bool AutoMode { get => _autoMode; set => _autoMode = value; }
        
        /// <summary>Current control error (SP - PV)</summary>
        public double Error => _setpoint - _processVariable;
        
        /// <summary>Active PID algorithm</summary>
        public PidAlgorithm Algorithm 
        { 
            get => _algorithm; 
            set 
            {
                if (_algorithm != value)
                {
                    System.Diagnostics.Debug.WriteLine($"PID Algorithm changed: {_algorithm} ? {value}");
                    _algorithm = value;
                    System.Diagnostics.Debug.WriteLine($"PID Algorithm confirmed: {_algorithm}");
                }
            }
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Initialize controller to steady-state conditions
        /// Sets all variables to consistent initial values
        /// </summary>
        /// <param name="initialOutput">Initial output value in %</param>
        /// <param name="initialPV">Initial process variable value in %</param>
        public void Initialize(double initialOutput = 50.0, double initialPV = 50.0)
        {
            _output = initialOutput;
            _processVariable = initialPV;
            _previousPV = initialPV;
            _previousPV2 = initialPV;
            _manualOutput = initialOutput;
            _previousError = 0.0;  // Start with zero error
            _previousSetpoint = _setpoint;
            
            System.Diagnostics.Debug.WriteLine($"PID Controller initialized - Algorithm: {_algorithm}, Output: {_output:F2}%, PV: {_processVariable:F2}%");
        }

        /// <summary>
        /// Update controller with new process variable and calculate output
        /// Uses velocity form for inherent bumpless operation
        /// </summary>
        /// <param name="processVariable">Current process variable value</param>
        /// <param name="deltaTime">Time since last update in seconds</param>
        /// <returns>Controller output value</returns>
        public double Update(double processVariable, double deltaTime)
        {
            _processVariable = processVariable;
            
            // Handle manual mode operation
            if (!_autoMode)
            {
                _output = _manualOutput;
                UpdateHistoryForManualMode();
                System.Diagnostics.Debug.WriteLine($"MANUAL MODE - Output: {_output:F2}%");
                return _output;
            }
            
            // Auto mode - calculate velocity form output change
            double deltaOutput = CalculateVelocityOutput(deltaTime);
            
            // Apply output change to previous output
            _output += deltaOutput;
            
            // Apply output limits
            _output = Math.Max(_outputMin, Math.Min(_outputMax, _output));
            
            // Update historical values for next scan
            UpdateHistory();
            
            System.Diagnostics.Debug.WriteLine($"VELOCITY {_algorithm}: ?Output={deltaOutput:F3}, Output={_output:F2}%");
            
            return _output;
        }

        /// <summary>
        /// Reset controller while preserving current steady-state values
        /// Clears historical data for clean restart
        /// </summary>
        public void Reset()
        {
            _previousError = 0.0;
            _previousPV2 = _previousPV;
            _previousPV = _processVariable;
            _previousSetpoint = _setpoint;
        }

        /// <summary>
        /// Set output limits for controller
        /// </summary>
        /// <param name="min">Minimum output limit</param>
        /// <param name="max">Maximum output limit</param>
        public void SetOutputLimits(double min, double max)
        {
            _outputMin = min;
            _outputMax = max;
        }
        #endregion

        #region Private Methods - Algorithm Implementation
        /// <summary>
        /// Calculate velocity form output change based on selected algorithm
        /// </summary>
        /// <param name="deltaTime">Time step in seconds</param>
        /// <returns>Change in output (delta output)</returns>
        private double CalculateVelocityOutput(double deltaTime)
        {
            return _algorithm switch
            {
                PidAlgorithm.BasicPID => CalculateVelocityBasicPID(deltaTime),
                PidAlgorithm.I_PD => CalculateVelocityI_PD(deltaTime),
                PidAlgorithm.PI_D => CalculateVelocityPI_D(deltaTime),
                _ => CalculateVelocityBasicPID(deltaTime)
            };
        }

        /// <summary>
        /// Velocity form BasicPID algorithm
        /// All terms calculated on error - has proportional and derivative kicks
        /// Formula: ?Output = Kp×(e[n]-e[n-1]) + Ki×e[n]×?t + Kd×(e[n]-2×e[n-1]+e[n-2])/?t
        /// </summary>
        private double CalculateVelocityBasicPID(double deltaTime)
        {
            double error = _setpoint - _processVariable;
            double previousError = _previousError;
            
            // Proportional term - responds to error change
            double deltaProportional = _kp * (error - previousError);
            
            // Integral term - responds to current error
            double deltaIntegral = _ki * error * deltaTime;
            
            // Derivative term - responds to second difference of error
            double deltaDerivative = 0.0;
            if (deltaTime > 0)
            {
                double errorMinus2 = _previousSetpoint - _previousPV2;
                deltaDerivative = _kd * (error - 2 * previousError + errorMinus2) / deltaTime;
            }
            
            double deltaOutput = deltaProportional + deltaIntegral + deltaDerivative;
            
            System.Diagnostics.Debug.WriteLine($"BasicPID: ?P={deltaProportional:F2} ?I={deltaIntegral:F2} ?D={deltaDerivative:F2} = {deltaOutput:F2}");
            
            return deltaOutput;
        }
        
        /// <summary>
        /// Velocity form I-PD algorithm - eliminates proportional and derivative kicks
        /// Integral on error, Proportional and Derivative on measurement
        /// Formula: ?Output = Ki×e[n]×?t + Kp×(PV[n-1]-PV[n]) + Kd×(PV[n-2]-2×PV[n-1]+PV[n])/?t
        /// </summary>
        private double CalculateVelocityI_PD(double deltaTime)
        {
            double error = _setpoint - _processVariable;
            
            // Integral term - responds to current error (only term that responds to setpoint changes)
            double deltaIntegral = _ki * error * deltaTime;
            
            // Proportional term - responds to PV changes only (eliminates proportional kick)
            double deltaProportional = _kp * (_previousPV - _processVariable);
            
            // Derivative term - responds to PV second difference (eliminates derivative kick)
            double deltaDerivative = 0.0;
            if (deltaTime > 0)
            {
                deltaDerivative = _kd * (_previousPV2 - 2 * _previousPV + _processVariable) / deltaTime;
            }
            
            double deltaOutput = deltaIntegral + deltaProportional + deltaDerivative;
            
            System.Diagnostics.Debug.WriteLine($"I-PD: ?I={deltaIntegral:F2} ?P={deltaProportional:F2} ?D={deltaDerivative:F2} = {deltaOutput:F2}");
            
            return deltaOutput;
        }
        
        /// <summary>
        /// Velocity form PI-D algorithm - eliminates derivative kick only
        /// Proportional and Integral on error, Derivative on measurement
        /// Formula: ?Output = Kp×(e[n]-e[n-1]) + Ki×e[n]×?t + Kd×(PV[n-2]-2×PV[n-1]+PV[n])/?t
        /// </summary>
        private double CalculateVelocityPI_D(double deltaTime)
        {
            double error = _setpoint - _processVariable;
            double previousError = _previousError;
            
            // Proportional term - responds to error change
            double deltaProportional = _kp * (error - previousError);
            
            // Integral term - responds to current error
            double deltaIntegral = _ki * error * deltaTime;
            
            // Derivative term - responds to PV second difference (eliminates derivative kick)
            double deltaDerivative = 0.0;
            if (deltaTime > 0)
            {
                deltaDerivative = _kd * (_previousPV2 - 2 * _previousPV + _processVariable) / deltaTime;
            }
            
            double deltaOutput = deltaProportional + deltaIntegral + deltaDerivative;
            
            System.Diagnostics.Debug.WriteLine($"PI-D: ?P={deltaProportional:F2} ?I={deltaIntegral:F2} ?D={deltaDerivative:F2} = {deltaOutput:F2}");
            
            return deltaOutput;
        }

        /// <summary>
        /// Update historical values for next scan cycle
        /// </summary>
        private void UpdateHistory()
        {
            _previousError = _setpoint - _processVariable;
            _previousPV2 = _previousPV;
            _previousPV = _processVariable;
            _previousSetpoint = _setpoint;
        }

        /// <summary>
        /// Update historical values during manual mode for bumpless transfer
        /// </summary>
        private void UpdateHistoryForManualMode()
        {
            _previousError = _setpoint - _processVariable;
            _previousPV2 = _previousPV;
            _previousPV = _processVariable;
            _previousSetpoint = _setpoint;
        }
        #endregion
    }
}