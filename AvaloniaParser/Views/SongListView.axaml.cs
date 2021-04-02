using System.Collections.Generic;
using System.Collections.ObjectModel;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using AvaloniaParser.Models;

namespace AvaloniaParser.Views
{
    public class SongListView : UserControl
    {
        public ObservableCollection<SongModel> Songs { get; set; }
        public SongListView()
        {
            this.InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}