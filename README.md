# TarkaDyS - Process Dynamic Simulation Software

TarkaDyS is a comprehensive Process Dynamic Simulation Software built with .NET 8 and Windows Forms. It provides a platform for simulating various process models including PID controllers, first-order processes, and other control systems.

## Features

### Core Architecture
- **MDI Parent-Child Interface**: Main application window hosts multiple simulation child forms
- **Extensible Process Models**: Modular architecture supporting various process types
- **Real-time Plotting**: Interactive charts using OxyPlot library
- **Well-documented Code**: Comprehensive inline documentation serving as technical documentation

### Available Process Models

1. **PID Controller Simulation** (Based on VB PID Simulator)
   - Complete closed-loop PID control system
   - Tunable PID parameters (Kp, Ki, Kd)
   - First-order process with dead time
   - Real-time plotting of setpoint, process variable, controller output, and error
   - Manual/Automatic mode switching
   - Disturbance injection capability

2. **First-Order Process Model**
   - Process gain, time constant, and dead time parameters
   - Step response analysis
   - Disturbance handling

### Planned Models
- Level Control Process (Tank level simulation)
- Temperature Control Process (Heat exchanger)
- Flow Control Process
- Distillation Column Process

## Architecture Overview

### Core Components

```
TarkaDyS/
??? Core/
?   ??? Interfaces/
?   ?   ??? IProcessModel.cs          # Base interface for all process models
?   ?   ??? IController.cs            # Base interface for controllers
?   ??? Models/
?   ?   ??? ProcessModelBase.cs       # Abstract base class with common functionality
?   ??? Controllers/
?   ?   ??? PidController.cs          # Complete PID controller implementation
?   ??? Factories/
?       ??? ProcessModelFactory.cs    # Factory for creating process model instances
??? ProcessModels/
?   ??? FirstOrderProcessModel.cs     # First-order process with dead time
?   ??? PidProcessModel.cs            # Combined PID controller + process
??? Forms/
?   ??? MainForm.cs                   # MDI parent form
?   ??? PidSimulatorForm.cs           # PID simulation child form
??? Program.cs                        # Application entry point
```

### Key Design Patterns
- **Factory Pattern**: ProcessModelFactory for model creation
- **Observer Pattern**: Event-driven parameter changes
- **Template Method**: ProcessModelBase with virtual/abstract methods
- **MDI Pattern**: Multiple document interface for child forms

## Getting Started

### Prerequisites
- .NET 8.0 or later
- Windows operating system
- Visual Studio 2022 (recommended) or VS Code

### Building and Running

1. Clone or download the project
2. Open `TarkaDyS.sln` in Visual Studio
3. Restore NuGet packages (OxyPlot.WindowsForms)
4. Build the solution (Ctrl+Shift+B)
5. Run the application (F5)

### Using the PID Simulator

1. From the main window, select **File > New > First Order Process with PID**
2. Adjust PID parameters (Kp, Ki, Kd) in the PID Parameters panel
3. Configure process parameters (Gain, Tau, Td) in the Process Parameters panel
4. Set desired setpoint in the Control panel
5. Click **Start** to begin simulation
6. Watch real-time plotting of system response
7. Apply disturbances or change setpoint during simulation
8. Switch between Manual/Automatic modes

## Technical Details

### Process Models
- **Sampling Time**: Configurable (default 0.1 seconds)
- **Integration Method**: Euler integration for differential equations
- **Dead Time Implementation**: Circular buffer with interpolation
- **Thread Safety**: All models are thread-safe with proper locking

### PID Controller Features
- **Anti-windup Protection**: Prevents integral windup
- **Derivative on Measurement**: Avoids derivative kick on setpoint changes
- **Configurable Limits**: Output limits and integral limits
- **Bumpless Transfer**: Smooth transitions between manual and automatic modes

### Error Handling
- Comprehensive exception handling with user-friendly messages
- Model error events for runtime issues
- Application-level unhandled exception catching
- Graceful degradation when components fail

## Extending the System

### Adding New Process Models

1. Create a new class inheriting from `ProcessModelBase`
2. Implement abstract methods: `InitializeModel()`, `UpdateModel()`, `GetOutputValue()`
3. Register the model in `ProcessModelFactory`
4. Create corresponding UI form if needed

### Example:
```csharp
public class LevelProcessModel : ProcessModelBase
{
    protected override void InitializeModel() { /* Initialize parameters */ }
    protected override void UpdateModel() { /* Update simulation */ }
    protected override double GetOutputValue(string parameterName) { /* Return output */ }
}
```

## Dependencies

- **.NET 8.0**: Target framework
- **OxyPlot.WindowsForms 2.1.2**: Real-time plotting library
- **Windows Forms**: UI framework

## License

This project is provided as-is for educational and development purposes.

## Contributing

This is a demonstration project showing modern C# architecture for process simulation software. Feel free to extend and modify according to your needs.

## Acknowledgments

- Based on the VB PID Simulator repository: https://github.com/shankarananth/VB_PID_Simulator
- Converted to modern C# with extensive architecture improvements
- Designed for extensibility and maintainability