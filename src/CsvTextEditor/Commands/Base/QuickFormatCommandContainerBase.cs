﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="QuickFormatCommandContainerBase.cs" company="WildGums">
//   Copyright (c) 2008 - 2017 WildGums. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


namespace CsvTextEditor
{
    using System;
    using System.Diagnostics;
    using Catel;
    using Catel.IoC;
    using Catel.MVVM;
    using Orc.Notifications;
    using Orc.ProjectManagement;

    public abstract class QuickFormatCommandContainerBase : EditProjectCommandContainerBase
    {
        #region Fields
        private readonly Notification _notification;
        private readonly INotificationService _notificationService;
        private readonly Stopwatch _stopwatch;
        #endregion

        #region Constructors
        protected QuickFormatCommandContainerBase(string commandName, ICommandManager commandManager, IProjectManager projectManager,
            IServiceLocator serviceLocator, INotificationService notificationService)
            : base(commandName, commandManager, projectManager, serviceLocator)
        {
            Argument.IsNotNull(() => notificationService);

            _notificationService = notificationService;

            _notification = new Notification
            {
                Title = "Quick format",
                ShowTime = TimeSpan.FromSeconds(3)
            };

            _stopwatch = new Stopwatch();
        }
        #endregion

        #region Methods
        protected sealed override void Execute(object parameter)
        {
            _stopwatch.Restart();

            EcecuteOperation();

            _stopwatch.Stop();

            var operationDescription = GetOperationDescription();

            ShowNotification($"Finished {operationDescription} in {_stopwatch.ElapsedMilliseconds} ms");

            base.Execute(parameter);
        }

        protected abstract void EcecuteOperation();
        protected abstract string GetOperationDescription();

        private void ShowNotification(string message)
        {
            _notification.Message = message;

            _notificationService.ShowNotification(_notification);
        }
        #endregion
    }
}