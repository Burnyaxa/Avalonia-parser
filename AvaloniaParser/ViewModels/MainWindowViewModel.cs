using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AvaloniaParser.Interfaces;
using AvaloniaParser.Models;
using AvaloniaParser.Parsers;
using ReactiveUI;

namespace AvaloniaParser.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private IParser _parser;
        public SongListViewModel Songs { get; set; }
        public ObservableCollection<SongModel> SongList { get; set; }
        
        public MainWindowViewModel(IParser parser)
        {
            Songs = new SongListViewModel(parser);
        }
    }
}