using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using AvaloniaParser.Interfaces;
using AvaloniaParser.Models;
using AvaloniaParser.Parsers;
using AvaloniaParser.ViewModels;
using AvaloniaParser.Views;

namespace AvaloniaParser
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                IParser parser = new GenieParser();
                
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(parser)
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}