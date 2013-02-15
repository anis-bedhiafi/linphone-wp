﻿using Linphone.Core;
using Linphone.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace Linphone
{
    /// <summary>
    /// Model view for each page implementing the call controller listener to adjust displayed page depending on call events.
    /// </summary>
    public class BaseModel : CallControllerListener
    {
        /// <summary>
        /// Public constructor.
        /// </summary>
        public BaseModel()
        {

        }

        /// <summary>
        /// Page currently displayed.
        /// </summary>
        public BasePage Page { get; set; }

        /// <summary>
        /// Called when a call is starting.
        /// Displays the InCall.xaml page.
        /// </summary>
        /// <param name="callerNumber"></param>
        public void NewCallStarted(string callerNumber)
        {
            this.Page.Dispatcher.BeginInvoke(() =>
                {
                    this.Page.NavigationService.Navigate(new Uri("/Views/InCall.xaml?sip=" + callerNumber, UriKind.RelativeOrAbsolute));
                });
        }

        /// <summary>
        /// Called when a call is finished.
        /// Goes back to the last page if possible, else displays Dialer.xaml.
        /// </summary>
        public void CallEnded()
        {
            this.Page.Dispatcher.BeginInvoke(() =>
                {
                    if (this.Page.NavigationService.CanGoBack)
                        this.Page.NavigationService.GoBack();
                    else
                    {
                        //If incall view directly accessed from home page, backstack is empty
                        //If so, instead of keeping the incall view, launch the Dialer and remove the incall view from the backstack
                        this.Page.NavigationService.Navigate(new Uri("/Views/Dialer.xaml", UriKind.RelativeOrAbsolute));
                        this.Page.NavigationService.RemoveBackEntry();
                    }
                });
        }

        /// <summary>
        /// Actualises the listener when the pages changes.
        /// </summary>
        /// <param name="nea"></param>
        public virtual void OnNavigatedTo(NavigationEventArgs nea)
        {
            LinphoneManager.Instance.CallController.SetCallControllerListener(this);
        }

        /// <summary>
        /// Actualises the listener when the pages changes.
        /// </summary>
        /// <param name="nea"></param>
        public virtual void OnNavigatedFrom(NavigationEventArgs nea)
        {
            LinphoneManager.Instance.CallController.SetCallControllerListener(null);
        } 
    }
}