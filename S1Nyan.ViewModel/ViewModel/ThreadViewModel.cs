﻿using System;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using S1Nyan.Model;
using S1Nyan.Utils;
using S1Parser;

namespace S1Nyan.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ThreadViewModel : S1NyanViewModelBase
    {
        IDataService _dataService;
        /// <summary>
        /// Initializes a new instance of the ThreadViewModel class.
        /// </summary>
        public ThreadViewModel(IDataService dataService) : base()
        {
            _dataService = dataService;
            //if (IsInDesignMode) _dataService.GetThreadData(null, 0, (item, error) => { TheThread = item; Title = TheThread.Title; });
        }

        private string _title = null;

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get { return _title; }
            set
            {
                if (_title == value) return;

                _title = value;
                RaisePropertyChanged(() => Title);
            }
        }

        private string _tid = null;
        public void OnChangeTID(string tid, string title, int page)
        {
            Title = title;
            if(tid!=null && tid != _tid)
            {
                _tid = tid;
                TotalPage = 0;
                CurrentPage = page;
            }
        }

        private S1ThreadPage _threadpage = null;
        /// <summary>
        /// Sets and gets the TheThread property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public S1ThreadPage TheThread
        {
            get { return _threadpage; }
            set
            {
                if (_threadpage == value) return;

                _threadpage = value;

                RaisePropertyChanged(() => TheThread);
            }
        }

        public override async void RefreshData()
        {
            TheThread = null;
            if (null == _tid) return;
            Util.Indicator.SetLoading();
            try
            {
                var temp = await _dataService.GetThreadDataAsync(_tid, CurrentPage);
                if (temp.CurrentPage == CurrentPage)
                {
                    TheThread = temp;
                    TotalPage = TheThread.TotalPage;
                    Title = TheThread.Title;

                    if (PageChanged != null)
                    {
                        PageChanged(CurrentPage, TotalPage);
                    }
                    Util.Indicator.SetBusy(false);
                }
            }
            catch (Exception e)
            {
                if (!HandleUserException(e))
                    Util.Indicator.SetError(e);
            }
        }

        public bool IsShowPage { get { return TotalPage > 1; } }

        int totalPage;
        public int TotalPage
        {
            get { return totalPage; }
            set
            {
                if (totalPage == value) return;
                totalPage = value;
                RaisePropertyChanged(() => TotalPage);
                RaisePropertyChanged(() => IsShowPage);
            }
        }

        //may change during async actions, disable optimization
        volatile int currentPage;
        public int CurrentPage
        {
            get { return currentPage; }
            set
            {
                if (value > 0 && 
                    value != currentPage &&
                    ((totalPage > 0 && value <= totalPage) || totalPage == 0))
                {
                    currentPage = value;
                    RefreshData();
                    RaisePropertyChanged(() => CurrentPage);
                }
            }
        }
        public Action<int, int> PageChanged { get; set; }

        public override void Cleanup()
        {
            base.Cleanup();
            TheThread = null;
            PageChanged = null;
        }

        #region Reply
        private string _replyText = "";

        /// <summary>
        /// Sets and gets the ReplyText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ReplyText
        {
            get { return _replyText; }

            set
            {
                if (_replyText == value) return;

                _replyText = value;
                RaisePropertyChanged(() => ReplyText);
                SendCommand.RaiseCanExecuteChanged();
            }
        }

        private bool isSending = false;
        private bool IsSending
        {
            get { return isSending; }
            set
            {
                isSending = value; SendCommand.RaiseCanExecuteChanged();
            }
        }

        private string _replyResult = "";

        /// <summary>
        /// Sets and gets the Result property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string ReplyResult
        {
            get { return _replyResult; }

            set
            {
                if (_replyResult == value) return;

                _replyResult = value;
                RaisePropertyChanged(() => ReplyResult);
            }
        }

        private RelayCommand _sendCommand;

        /// <summary>
        /// Gets the SendCommand.
        /// </summary>
        public RelayCommand SendCommand
        {
            get
            {
                return _sendCommand
                    ?? (_sendCommand = new RelayCommand(
                        () => SendReply(),
                        () => ReplyText.Length > 0 && !IsSending));
            }
        }

        private async void SendReply()
        {
#if UseLocalhost
            var replyLink = "post.php?action=reply&fid=2&tid=1";
#else
            var replyLink = TheThread.ReplyLink;
#endif
            System.Diagnostics.Debug.WriteLine("Send Reply: " + replyLink + "\r\n" + ReplyText);

            IsSending = true;
            var result = await SendPostService.DoSendPost(replyLink, ReplyText.Replace("\r","\r\n"));
            IsSending = false;
            if (result == null)
            {
                ReplyText = "";
                ReplyResult = Utils.Util.ErrorMsg.GetExceptionMessage(S1Parser.User.S1UserException.ReplySuccess);
            }
            else
            {
                ReplyResult = result;
            }
        }

        private ISendPostService SendPostService
        {
            get { return SimpleIoc.Default.GetInstance<ISendPostService>(); }
        }
        #endregion
    }
}