﻿using System;
using System.Text;
using TwitchLeecher.Core.Enums;
using TwitchLeecher.Shared.Notification;

namespace TwitchLeecher.Core.Models
{
    public class TwitchVideoDownload : BindableBase
    {
        #region Fields

        private string _id;

        private DownloadParameters _downloadParams;

        private DownloadState _downloadState;
        private object _downloadStateLockObject;

        private StringBuilder _log;
        private object _logLockObject;

        private int _progress;
        private object _progressLockObject;

        private string _status;
        private object _statusLockObject;

        private bool _isProcessing;
        private object _isProcessingLockObject;

        #endregion Fields

        #region Constructors

        public TwitchVideoDownload(DownloadParameters downloadParams)
        {
            _id = Guid.NewGuid().ToString();
            _downloadParams = downloadParams ?? throw new ArgumentNullException(nameof(downloadParams));

            _log = new StringBuilder();

            _downloadStateLockObject = new object();
            _logLockObject = new object();
            _progressLockObject = new object();
            _statusLockObject = new object();
            _isProcessingLockObject = new object();
        }

        #endregion Constructors

        #region Properties

        public string Id
        {
            get
            {
                return _id;
            }
        }

        public DownloadParameters DownloadParams
        {
            get
            {
                return _downloadParams;
            }
        }

        public DownloadState DownloadState
        {
            get
            {
                return _downloadState;
            }
            private set
            {
                _downloadState = value;
                FirePropertyChanged(nameof(DownloadState));
                FirePropertyChanged(nameof(CanRetry));
                FirePropertyChanged(nameof(Status));
            }
        }

        public bool CanRetry
        {
            get
            {
                return DownloadState == DownloadState.Canceled || DownloadState == DownloadState.Error;
            }
        }

        public string Log
        {
            get
            {
                lock (_logLockObject)
                {
                    return _log.ToString();
                }
            }
        }

        public int Progress
        {
            get
            {
                return _progress;
            }
            private set
            {
                SetProperty(ref _progress, value);
            }
        }

        public string Status
        {
            get
            {
                if (_downloadState != DownloadState.Downloading)
                {
                    return _downloadState.ToString();
                }

                return _status;
            }
            private set
            {
                SetProperty(ref _status, value);
            }
        }

        public bool IsProcessing
        {
            get
            {
                return _isProcessing;
            }
            private set
            {
                SetProperty(ref _isProcessing, value);
            }
        }

        #endregion Properties

        #region Methods

        public void SetDownloadState(DownloadState downloadState)
        {
            lock (_downloadStateLockObject)
            {
                DownloadState = downloadState;
            }
        }

        public void AppendLog(string text)
        {
            lock (_logLockObject)
            {
                _log.Append(text);
                FirePropertyChanged(nameof(Log));
            }
        }

        public void ResetLog()
        {
            lock (_logLockObject)
            {
                _log.Clear();
                FirePropertyChanged(nameof(Log));
            }
        }

        public void SetProgress(int progress)
        {
            lock (_progressLockObject)
            {
                Progress = progress;
            }
        }

        public void SetStatus(string status)
        {
            lock (_statusLockObject)
            {
                Status = status;
            }
        }

        public void SetIsProcessing(bool isProcessing)
        {
            lock (_isProcessingLockObject)
            {
                IsProcessing = isProcessing;
            }
        }

        #endregion Methods
    }
}