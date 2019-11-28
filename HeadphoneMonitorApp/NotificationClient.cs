using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Runtime.InteropServices;

using NAudio;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;
using System.Windows.Threading;
using System.Windows;

namespace HeadphoneMonitorApp
{
    public class MMNotificationClient : IMMNotificationClient
    {
        public event EventHandler DeviceStateChanged;
        public event EventHandler DeviceAdded;
        public event EventHandler DeviceRemoved;
        public event EventHandler DefaultDeviceChanged;
        public event EventHandler PropertyValueChanged;

        void IMMNotificationClient.OnDeviceStateChanged(string deviceId, DeviceState newState)
        {
            //Console.WriteLine("OnDeviceStateChanged");
            //Console.WriteLine("Device Id: {0}", deviceId);
            //Console.WriteLine("Device State: {0}", newState);
            DeviceStateChanged.Invoke(this, EventArgs.Empty);
        }

        void IMMNotificationClient.OnDeviceAdded(string deviceId)
        {
            //Console.WriteLine("OnDeviceAdded");
            //Console.WriteLine("Device Id: {0}", deviceId);
            DeviceAdded.Invoke(this, EventArgs.Empty);
        }

        void IMMNotificationClient.OnDeviceRemoved(string deviceId)
        {
            //Console.WriteLine("OnDeviceRemoved");
            //Console.WriteLine("Device Id: {0}", deviceId);
            DeviceRemoved.Invoke(this, EventArgs.Empty);
        }

        void IMMNotificationClient.OnDefaultDeviceChanged(DataFlow flow, Role role, string defaultDeviceId)
        {
            //Console.WriteLine("OnDefaultDeviceChanged");
            //Console.WriteLine("Device Id: {0}", defaultDeviceId);
            DefaultDeviceChanged.Invoke(this, EventArgs.Empty);
        }

        void IMMNotificationClient.OnPropertyValueChanged(string deviceId, PropertyKey key)
        {
            PropertyValueChanged?.Invoke(this ,EventArgs.Empty);
        }
    }


}
