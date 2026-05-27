using Avalonia.Interactivity;
using System.Diagnostics;
namespace Garage_With_Avalonia_UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public string Greeting { get; } = "Welcome to the Garage!";
    public string Instructions { get; } = "Let's create a garage. How many spots do you want?";
}
