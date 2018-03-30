﻿using Sitecore.Diagnostics;
using Sitecore.EmailCampaign.Cm.Factories;
using Sitecore.EmailCampaign.Model.Dispatch;
using Sitecore.EmailCampaign.Model.Web;
using Sitecore.ExM.Framework.Diagnostics;
using System;

namespace Sitecore.EmailCampaign.Cm.Pipelines.DispatchNewsletter
{
  public class NotifyDispatchFinished
  {
    private readonly INotificationFactory _notificationFactory;

    private readonly ILogger logger;

    public NotifyDispatchFinished(INotificationFactory notificationFactory, ILogger logger)
    {
      Assert.ArgumentNotNull(notificationFactory, "notificationFactory");
      Assert.ArgumentNotNull(logger, "logger");
      this._notificationFactory = notificationFactory;
      this.logger = logger;
    }

    public void Process(DispatchNewsletterArgs args)
    {
      if (!args.RequireFinalMovement || args.IsTestSend || !args.AllowNotifications || !args.Message.EnableNotifications || args.DispatchInterruptRequest == DispatchInterruptSignal.Pause)
      {
        return;
      }
      try
      {
        string text = args.SendingAborted ? EcmTexts.Localize("Aborted", Array.Empty<object>()) : EcmTexts.Localize("Completed", Array.Empty<object>());
        this._notificationFactory.GetNotification(args.Message).SendDispatchFinished(text.ToLower());
      }
      catch (Exception e)
      {
        this.logger.LogError(e);
      }
    }
  }
}