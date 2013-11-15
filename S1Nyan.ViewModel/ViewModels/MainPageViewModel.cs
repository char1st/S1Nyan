﻿using System;
using System.Collections.Generic;
using Caliburn.Micro;
using S1Nyan.Model;
using S1Nyan.Utils;
using S1Parser;

namespace S1Nyan.ViewModels
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainPageViewModel : S1NyanViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly INavigationService _navigationService;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainPageViewModel(IDataService dataService, 
            IEventAggregator eventAggregator, 
            INavigationService navigationService, 
            IServerModel serverModel) //TODO: move serverModel init somewhere else
            : base(eventAggregator)
        {
            _dataService = dataService;
            _navigationService = navigationService;
            MainListData = _dataService.GetMainListCache();
            //RefreshData();
            //if (IsInDesignMode) _dataService.GetMainListData((item, error) => { MainListData = item; });
        }

        private IEnumerable<S1ListItem> _data = null;
        /// <summary>
        /// Sets and gets the ListData property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public IEnumerable<S1ListItem> MainListData
        {
            get { return _data; }
            set
            {
                if (_data == value) return;

                _data = value;
                NotifyOfPropertyChange(() => MainListData);
            }
        }

        public override async void RefreshData()
        {
            Util.Indicator.SetLoading();
            try
            {
                MainListData = await _dataService.UpdateMainListAsync();
                Util.Indicator.SetBusy(false);
                _dataService.GetMainListDone();
            }
            catch (Exception e)
            {
                _dataService.GetMainListDone(false);
                if (!HandleUserException(e))
                    Util.Indicator.SetError(e);
            }
        }

        public void DoNavigation(object o)
        {
            S1ListItem item = o as S1ListItem;
            if (item != null)
                _navigationService
                    .UriFor<ThreadListViewModel>()
                    .WithParam(vm => vm.Fid, item.Id)
                    .WithParam(vm => vm.Title, item.Title)
                    .Navigate();
        }
        ////public override void Cleanup()
        ////{
        ////    // Clean up if needed

        ////    base.Cleanup();
        ////}
    }
}