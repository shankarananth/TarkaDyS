# TarkaDyS - Process Dynamic Simulator

**A Professional PID Control System Simulation Platform**

*Author: Shankar Ananth Asokan*  
*Version: 2.0*  
*Date: 24-Aug-2025*

---

## Overview

TarkaDyS (Target Dynamic Systems) is a comprehensive PID control simulation platform designed for educational and professional use. It provides realistic process control scenarios with industrial-quality PID controllers featuring multiple algorithms and advanced capabilities.

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

### Realistic Process Simulation
- **First-Order Process Model**: Transfer function K×e^(-Td×s)/(Tau×s+1)
- **Dead Time Support**: Configurable transport delays
- **Dynamic Random Disturbance**: Realistic process noise
- **Configurable Parameters**: Gain, time constant, dead time

### Professional User Interface
- **Real-Time Plotting**: Multi-variable trend display
- **HD Screen Optimized**: 1600×768 layout
- **Parameter Tuning**: Live adjustment of all parameters
- **Simulation Controls**: Start/Stop/Reset functionality
- **Algorithm Switching**: Real-time algorithm comparison

### Advanced Capabilities
- **Splash Screen**: Professional startup with logo support
- **Debug Output**: Comprehensive algorithm verification
- **Single Instance Management**: Prevents duplicate forms
- **Resource Management**: Automatic asset loading

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

### Adding Your Logo
1. Create a `Resources` folder in the build output directory
2. Place your logo as `TarkaDyS_Logo.png` (300×200 recommended)
3. Optionally add `TarkaDyS_Icon.ico` for window icons
4. The application will automatically detect and use these assets

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

### Testing I-PD Kick Elimination
1. Start with BasicPID algorithm
2. Change setpoint 50% → 70% (observe immediate MV jump = kick)
3. Switch to I-PD algorithm  
4. Change setpoint 70% → 40% (observe smooth response = no kick)
5. Check debug output for algorithm confirmation

### Dynamic Disturbance Model
- **Disturbance Factor**: 0-100% from UI
- **Random Amplitude**: Factor × Random(0-1) each cycle
- **Bipolar Effect**: Can add or subtract from PV
- **Realistic Behavior**: Simulates industrial process noise

**Examples:**
- **5% Disturbance**: Light process variations
- **20% Disturbance**: Moderate noise (challenging)
- **50% Disturbance**: Heavy noise (extreme conditions)

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
- Industrial-standard features

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

### Benefits of Velocity Form
- **Inherently Bumpless**: No integral tracking required
- **Numerical Stability**: Better for discrete implementation  
- **No Windup Issues**: Delta output prevents integration buildup
- **Industrial Standard**: Used in modern DCS/PLC systems

### Adding New Models
1. Create new process model in `Models/` folder
2. Create corresponding form in `Forms/` folder
3. Add menu item to `MainForm.cs`
4. Implement single instance management
5. Update documentation

---

## Testing and Validation

### PID Tuning Verification
- **Default Values**: Kp=0.5, Ki=0.1, Kd=0.0 (conservative)
- **Stability Test**: Step response should be smooth
- **Disturbance Rejection**: Test with various noise levels
- **Setpoint Tracking**: Verify smooth response

### Algorithm Switching Test
1. Start simulation in steady state
2. Change algorithm via dropdown
3. Verify debug output confirms switch
4. Test setpoint response differences
5. Confirm no output bumps during switch

### Manual/Auto Transfer Test
1. Start in Auto mode
2. Switch to Manual (output tracks)
3. Change manual output value
4. Switch back to Auto (bumpless)
5. Verify no output discontinuity

---

## Troubleshooting

### Common Issues

**Logo Not Displaying**
- Check `Resources/TarkaDyS_Logo.png` exists in build output
- Verify file is copied to `bin/Debug/net8.0-windows/Resources/`
- Review debug output for file loading messages

**PID Not Responding**
- Verify process parameters are reasonable (Tau > 0.1)
- Check PID gains are not zero
- Ensure simulation is running (START clicked)
- Review integral limits and output limits

**Form Layout Issues**
- Application optimized for 1366×768 or higher
- Check display scaling settings in Windows
- Form uses fixed dialog style for consistent layout

**Algorithm Switching Not Working**
- Check debug output for algorithm change messages
- Verify dropdown selection is changing
- Reset controller after algorithm change

### Debug Output
The application provides comprehensive debug output:
```
PID Algorithm changed: BasicPID → I_PD
I-PD: ΔI=0.25 ΔP=-1.20 ΔD=0.00 = -0.95
Disturbance applied: 10.0% × 0.743 × +1 = 0.074
```

---

## Contributing

### Development Guidelines
1. Follow existing code style and patterns
2. Add comprehensive XML documentation
3. Include debug output for verification
4. Test on different screen resolutions
5. Update README.md for new features

### Submission Process
1. Fork the repository
2. Create feature branch
3. Implement changes with tests
4. Update documentation
5. Submit pull request with detailed description

---

## License

This project is licensed under the MIT License - see the LICENSE file for details.

---

## Support

- **Author**: Shankar Ananth Asokan
- **Email**: [Your contact email]
- **GitHub**: https://github.com/shankarananth/TarkaDyS
- **Issues**: Report bugs via GitHub Issues

---

## Acknowledgments

- OxyPlot team for excellent plotting library
- Industrial control engineers for algorithm insights

---

*TarkaDyS - Bringing professional process control simulation to education and industry*