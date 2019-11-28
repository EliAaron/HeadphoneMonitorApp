using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

using NAudio;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace HeadphoneMonitorApp
{
    public class HeadphoneMonitor
    {
        //const string SpeakersFriendlyName = "Speakers (Realtek High Definition Audio)";
        //const string DigitalOutFriendlyName = "Realtek Digital Output (Realtek High Definition Audio)";

        const int HeadphoneConnectedVal = 3;
        const int HeadphoneNotConnectedVal = 1;
        static readonly PropertyKey HeadphoneProperty = new PropertyKey(new Guid(0xdd0df94b, 0x53a2, 0x4a92, 0x85, 0x4e, 0x91, 0xf4, 0x2e, 0x28, 0xfd, 0x7b), 0);
        static readonly List<VarEnum> VarEnumList = Enum.GetValues(typeof(VarEnum)).Cast<VarEnum>().ToList();

        bool _isNotificationsRegistered = false;

        bool? _lastIsHeadphonesConnected = null;

        VolumeAction _headphonesConnectedAction = VolumeAction.None;

        VolumeAction _headphonesNotConnectedAction = VolumeAction.None;

        bool _hasHeadphoneProperty = false;

        object _locker = new object();

        private ThreadPriority _notificationsThreadPriority = ThreadPriority.Highest;



        public MMDeviceEnumerator MMDeviceEnumerator { get; private set; }

        public MMNotificationClient MMNotificationClient { get; private set; }

        public MMDeviceCollection MMDevices { get; private set; }

        public MMDevice SpeakersDevice { get; private set; }

        public VolumeAction HeadphonesConnectedAction
        {
            get { lock (_locker) return _headphonesConnectedAction; }
            set
            {
                lock (_locker)
                {
                    if (value != _headphonesConnectedAction)
                    {
                        _headphonesConnectedAction = value;
                        Monitor();
                    }
                }
            }
        }

        public VolumeAction HeadphonesNotConnectedAction
        {
            get { lock (_locker) return _headphonesNotConnectedAction; }
            set
            {
                
                lock (_locker)
                {
                    if (value != _headphonesNotConnectedAction)
                    {
                        _headphonesNotConnectedAction = value;
                        Monitor();
                    }
                }
            }
        }

        public bool IsNotificationsRegistered
        {
            get { return _isNotificationsRegistered; }
            private set { _isNotificationsRegistered = value; }
        }

        public bool Mute
        {
            get { lock (_locker) return SpeakersDevice.AudioEndpointVolume.Mute; }
            set { lock (_locker) SpeakersDevice.AudioEndpointVolume.Mute = value; }

        }

        public ThreadPriority NotificationsThreadPriority
        {
            get { lock (_locker) return _notificationsThreadPriority; }
            set { lock (_locker) _notificationsThreadPriority = value; }
        }

        public bool HasHeadphoneProperty
        {
            get { return _hasHeadphoneProperty; }
            private set { _hasHeadphoneProperty = value; }
        }

        public event EventHandler SpeakersVolumeNotification;

        public event EventHandler HeadphonesConnectionChanged;



        public HeadphoneMonitor()
        {
            MMDeviceEnumerator = new MMDeviceEnumerator();

            MMDevices = MMDeviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Unplugged | DeviceState.Active);
            SpeakersDevice = FindSpeakersDevice(MMDevices);

            if (SpeakersDevice != null)
            {
                SpeakersDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
                HasHeadphoneProperty = SpeakersDevice.Properties.Contains(HeadphoneProperty);
            }

            //PrintDevice(MMDevices, true);

            MMNotificationClient = new MMNotificationClient();
            MMNotificationClient.PropertyValueChanged += MMNotificationClient_PropertyValueChanged;
            MMNotificationClient.DeviceStateChanged += MMNotificationClient_DeviceStateChanged;
            MMNotificationClient.DeviceAdded += MMNotificationClient_DeviceAdded;
            MMNotificationClient.DeviceRemoved += MMNotificationClient_DeviceRemoved;
            MMNotificationClient.DefaultDeviceChanged += MMNotificationClient_DefaultDeviceChanged;
        }

        ~HeadphoneMonitor()
        {
            UnregisterNotifications();
        }



        public static MMDevice FindSpeakersDevice(IEnumerable<MMDevice> MMDevices)
        {
            foreach (MMDevice dev in MMDevices)
            {
                EndpointFormFactor? endpointFormFactor = GetEndpointFormFactor(dev);
                if (endpointFormFactor != null &&
                    endpointFormFactor == EndpointFormFactor.Speakers)
                {
                    return dev;
                }
            }

            return null;
        }

        public static EndpointFormFactor? GetEndpointFormFactor(MMDevice d)
        {
            PropertyKey propkey = PropertyKeys.PKEY_AudioEndpoint_FormFactor;
            if (d.Properties.Contains(propkey))
            {
                return (EndpointFormFactor)(uint)(d.Properties[propkey].Value);
            }
            else
            {
                return null;
            }
        }

        public static void PrintDevice(IEnumerable<MMDevice> MMDevices, bool printProperties)
        {
            foreach (MMDevice d in MMDevices)
            {
                Console.WriteLine("=========================================");
                Console.WriteLine("Device FriendlyName: {0}", d.FriendlyName);

                PropertyKey propkey = PropertyKeys.PKEY_AudioEndpoint_FormFactor;
                bool hasFormFactorProperty = d.Properties.Contains(propkey);

                if (hasFormFactorProperty)
                {
                    Console.WriteLine("EndpointFormFactor: {0}", (EndpointFormFactor)(uint)(d.Properties[propkey].Value));
                }
                else
                {
                    Console.WriteLine("EndpointFormFactor: Not found");
                }

                bool hasHeadphoneProperty = d.Properties.Contains(HeadphoneProperty);

                if (hasHeadphoneProperty)
                {
                    Console.WriteLine("HeadphoneProperty: {0}", (int)(uint)(d.Properties[HeadphoneProperty].Value));
                    Console.WriteLine("Headphone Connected Value: HeadphoneProperty == {0}", HeadphoneConnectedVal);
                    Console.WriteLine("Headphone Not Connected Value: HeadphoneProperty == {0}", HeadphoneNotConnectedVal);
                }
                else
                {
                    Console.WriteLine("HeadphoneProperty: Not found");
                }
                Console.WriteLine();
                

                PrintAllProperties(d);

                Console.WriteLine();
            }
        }

        public static void PrintAllProperties(MMDevice d)
        {
            for (int i = 0; i < d.Properties.Count; i++)
            {
                PrintProperty(d, i);
            }
        }

        public static void PrintProperty(MMDevice dev, int i)
        {
            PropertyKey pk = dev.Properties.Get(i);
            PropVariant pv = dev.Properties.GetValue(i);
            if (VarEnumList.Contains(pv.DataType))
            {
                Console.WriteLine("{0:00}:", i);
                Console.WriteLine("Value: {0}", pv.Value);
                Console.WriteLine("propertyId: {0}", pk.propertyId);
                Console.WriteLine("propertyId: {0}", pk.formatId.ToString("X"));
                Console.WriteLine();
            }
        }

        public void RegisterNotifications()
        {
            lock (_locker)
            {
                if (IsNotificationsRegistered)
                {
                    UnregisterNotifications();
                }

                MMDeviceEnumerator.RegisterEndpointNotificationCallback(MMNotificationClient);
                
                IsNotificationsRegistered = true;

                Monitor();
            }
        }

        public void UnregisterNotifications()
        {
            if (IsNotificationsRegistered)
            {
                lock (_locker)
                {
                    MMDeviceEnumerator.UnregisterEndpointNotificationCallback(MMNotificationClient);

                    IsNotificationsRegistered = false;
                }
            }
        }

        public void Monitor()
        {
            bool connectionChanged = false;

            lock (_locker)
            {
                if (SpeakersDevice != null)
                {
                    bool isHeadphonesConnected = CheckIfHeadphonesConnected(out connectionChanged);

                    VolumeAction volumeAction = isHeadphonesConnected ? HeadphonesConnectedAction : HeadphonesNotConnectedAction;

                    if (volumeAction == VolumeAction.Unmute && SpeakersDevice.AudioEndpointVolume.Mute)
                    {
                        SpeakersDevice.AudioEndpointVolume.Mute = false;
                    }
                    else if (volumeAction == VolumeAction.Mute && !SpeakersDevice.AudioEndpointVolume.Mute)
                    {
                        SpeakersDevice.AudioEndpointVolume.Mute = true;
                    }
                }
            }

            if (connectionChanged)
            {
                HeadphonesConnectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CheckIfHeadphonesConnected(bool thriggerChangedEvent = true)
        {
            bool connectionChanged;
            bool connected = CheckIfHeadphonesConnected(out connectionChanged);
            if(connectionChanged && thriggerChangedEvent)
            {
                HeadphonesConnectionChanged?.Invoke(this, EventArgs.Empty);
            }

            return connected;
        }

        public bool CheckIfHeadphonesConnected(out bool connectionChanged)
        {
            connectionChanged = false;
            lock (_locker)
            {
                if (!HasHeadphoneProperty) return false;

                int val = (int)(uint)SpeakersDevice.Properties[HeadphoneProperty].Value;
                bool isHeadphonesConnected = val == HeadphoneConnectedVal;

                if (_lastIsHeadphonesConnected == null ||
                    _lastIsHeadphonesConnected.Value != isHeadphonesConnected)
                {
                    _lastIsHeadphonesConnected = isHeadphonesConnected;
                    connectionChanged = true;
                }

                return isHeadphonesConnected;
            }
        }

        private void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            SpeakersVolumeNotification?.Invoke(this, EventArgs.Empty);
        }

        private void MMNotificationClient_DeviceStateChanged(object sender, EventArgs e)
        {
            Monitor();

            if (Thread.CurrentThread.Priority != NotificationsThreadPriority)
            {
                Thread.CurrentThread.Priority = NotificationsThreadPriority;
            }
        }

        private void MMNotificationClient_DeviceAdded(object sender, EventArgs e)
        {
            Monitor();

            if (Thread.CurrentThread.Priority != NotificationsThreadPriority)
            {
                Thread.CurrentThread.Priority = NotificationsThreadPriority;
            }
        }

        private void MMNotificationClient_DeviceRemoved(object sender, EventArgs e)
        {
            Monitor();

            if (Thread.CurrentThread.Priority != NotificationsThreadPriority)
            {
                Thread.CurrentThread.Priority = NotificationsThreadPriority;
            }
        }

        private void MMNotificationClient_DefaultDeviceChanged(object sender, EventArgs e)
        {
            Monitor();

            if (Thread.CurrentThread.Priority != NotificationsThreadPriority)
            {
                Thread.CurrentThread.Priority = NotificationsThreadPriority;
            }
        }

        private void MMNotificationClient_PropertyValueChanged(object sender, EventArgs e)
        {
            Monitor();

            if (Thread.CurrentThread.Priority != NotificationsThreadPriority)
            {
                Thread.CurrentThread.Priority = NotificationsThreadPriority;
            }
        }
    }
}
