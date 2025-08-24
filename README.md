# TarkaDyS - Process Dynamic Simulator

**A Professional PID Control System Simulation Platform**

*Author: Shankar Ananth Asokan*  
*Version: 0.2*  
*Date: 24-Aug-2025*

---

## Overview

In Sanskrit, the word "tarka" (तर्क) means reasoning. TarkaDyS (**Tarka** **Dy**namic **S**ystems) is a comprehensive PID control simulation platform designed for educational and professional use. It provides realistic process control scenarios with industrial-quality PID controllers featuring multiple algorithms and advanced capabilities.

## Key Features

### Industrial-Quality PID Controller
- **Three Algorithm Types:**
  - **BasicPID**: Traditional PID (fast response, has kicks)
  - **I-PD**: Integral on error, P&D on measurement (no kicks) 
  - **PI-D**: P&I on error, D on measurement (no derivative kick)
- **Velocity Form Implementation**: Inherently bumpless operation
- **Conservative Default Tuning**: Kp=0.5, Ki=0.1, Kd=0.0
- **Manual/Auto Modes**: Seamless bumpless transfers
- **Output Limiting**: Configurable min/max constraints

### First Order Tunable Process Model
- **First-Order Process Model**: Transfer function K×e^(-Td×s)/(Tau×s+1)
- **Dead Time Support**: Configurable transport delays
- **Dynamic Random Disturbance**: Realistic process noise
- **Configurable Parameters**: Gain, time constant, dead time

---

## Getting Started

### System Requirements
- Windows 10/11
- .NET 8.0 Runtime
- Display resolution: 1366×768 or higher
- Visual Studio 2022 (for development)

### Installation
1. Clone the repository:
   ```bash
   git clone https://github.com/shankarananth/TarkaDyS
   ```
2. Open `TarkaDyS.sln` in Visual Studio 2022
3. Build the solution (Ctrl+Shift+B)
4. Run the application (F5)

---

## User Guide

### Starting a Simulation
1. **Launch TarkaDyS** - Splash screen appears for 3 seconds
2. **Open Model** - Click menu to open "First Order Process with PID"
3. **Configure Process** - Set gain, time constant, dead time
4. **Tune PID** - Adjust Kp, Ki, Kd parameters
5. **Start Simulation** - Click START button
6. **Make Changes** - Adjust setpoint, parameters, or algorithm
7. **Observe Response** - Watch real-time trends

### PID Algorithm Comparison

| Algorithm | Response Type | Proportional Kick | Derivative Kick | Best For |
|-----------|---------------|------------------|------------------|----------|
| **BasicPID** | Fast, aggressive | Present | Present | Infrequent setpoint changes |
| **I-PD** | Smooth, gradual | Eliminated | Eliminated | Frequent setpoint changes |
| **PI-D** | Compromise | Present | Eliminated | General purpose control |

---

## Architecture

### Project Structure
```
TarkaDyS/
├── Controllers/
│   └── PidController.cs          # Velocity-form PID implementation
├── Models/
│   └── FirstOrderProcess.cs      # Process simulation with disturbance
├── Forms/
│   ├── MainForm.cs              # Application main window
│   ├── SplashScreen.cs          # Startup splash screen
│   └── FirstOrderProcessWithPidForm.cs  # Main simulation form
├── Utilities/
│   └── ResourceHelper.cs        # Asset loading utilities
├── Resources/
│   ├── README.md               # Resource folder instructions
│   ├── TarkaDyS_Logo.png      # Application logo (user provided)
│   └── TarkaDyS_Icon.ico      # Application icon (user provided)
└── Documentation/              # Additional documentation files
```

### Key Classes

#### PidController.cs
- Velocity-form PID implementation
- Three algorithm types with kick elimination
- Bumpless manual-to-auto transfers

#### FirstOrderProcess.cs
- First-order plus dead time model
- Dynamic random disturbance generation
- Realistic industrial process behavior
- Configurable parameters

#### FirstOrderProcessWithPidForm.cs
- Main simulation interface
- Real-time plotting and controls
- Parameter tuning capabilities
- Professional HD-optimized layout

---

## Development

### Technical Specifications
- **Framework**: .NET 8.0 Windows Forms
- **Language**: C# 12.0
- **Plot Library**: OxyPlot.WindowsForms 2.1.2
- **Architecture**: Model-View-Controller pattern
- **Design Pattern**: Single instance management

### Velocity Form PID Implementation
The PID controller uses velocity form algorithms for superior performance:

```csharp
// BasicPID Velocity Form
ΔOutput = Kp×(e[n]-e[n-1]) + Ki×e[n]×Δt + Kd×(e[n]-2×e[n-1]+e[n-2])/Δt

// I-PD Velocity Form (No Kicks)
ΔOutput = Ki×e[n]×Δt + Kp×(PV[n-1]-PV[n]) + Kd×(PV[n-2]-2×PV[n-1]+PV[n])/Δt

// PI-D Velocity Form  
ΔOutput = Kp×(e[n]-e[n-1]) + Ki×e[n]×Δt + Kd×(PV[n-2]-2×PV[n-1]+PV[n])/Δt
```

---

## License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## Support

- **Author**: Shankar Ananth Asokan
- **GitHub**: https://github.com/shankarananth/TarkaDyS
- **Issues**: Report bugs via GitHub Issues

---

*TarkaDyS - Bringing professional Dynamic Process simulation with Control for Education*