using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using AvaloniaParser.Interfaces;
using AvaloniaParser.Models;
using DynamicData;
using ReactiveUI;

namespace AvaloniaParser.ViewModels
{
    public class SongListViewModel : ViewModelBase
    {
        private const string DefaultUrl = "https://www.genie.co.kr/chart/top200";
        private readonly IParser _parser;
        private string _info;
        public string Info
        {
            get => _info;
            set
            {
                _info = value;
                this.RaisePropertyChanged(nameof(Info));
            }
        }

        public ObservableCollection<SongModel> SongList { get; set; }
        
        public SongListViewModel(IParser parser)
        {
            Search = ReactiveCommand.Create<string>(OnClickCommand);
            SongList = new ObservableCollection<SongModel>();
            _parser = parser;
            Info = "Put your URL here or leave it empty to load first top 50 songs";
        }
        
        public ReactiveCommand<string, Unit> Search { get; }
        
        public async void OnClickCommand(string parameter)
        {
            Info = "Loading...";
            SongList.Clear();
            if (parameter == string.Empty || parameter is null)
            {
                parameter = DefaultUrl;
            }

            try
            {
                List<SongModel> list = await Task.Run(() => _parser.Parse(parameter));
                SongList.AddRange(list);
                Info = "Loaded successfully";
            }
            catch (Exception e)
            {
                Info = "Oops, something went wrong :(";
                Console.WriteLine(e);
            } 
        }
    }
}