using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

using NAudio;
using NAudio.CoreAudioApi;
using NAudio.CoreAudioApi.Interfaces;

namespace HeadphoneMonitorApp
{
    public static class PropertyKeyCollection
    {
        public static List<PropertyKey> PropertyKeyList { get; private set; }
        public static Dictionary<PropertyKey, string> PropertyKeyNamesDictionary { get; private set; }

        static PropertyKeyCollection()
        {
            PropertyKeyList = new List<PropertyKey>();
            PropertyKeyNamesDictionary = new Dictionary<PropertyKey, string>();

            Type type = typeof(PropertyKeyCollection); // MyClass is static class with static properties
            foreach (FieldInfo f in type.GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                PropertyKey propKey = (PropertyKey)f.GetValue(null);
                PropertyKeyList.Add(propKey);
                PropertyKeyNamesDictionary.Add(propKey, f.Name);

            }
        }

        //
        // _NAME
        //
        public static readonly PropertyKey PKEY_NAME = new PropertyKey(new Guid(0xb725f130, 0x47ef, 0x101a, 0xa5, 0xf1, 0x02, 0x60, 0x8c, 0x9e, 0xeb, 0xac), 10);    // DEVPROP_TYPE_STRING

        //
        // Device properties
        // These PKEYs correspond to the old setupapi SPDRP_XXX properties
        //
        public static readonly PropertyKey PKEY_Device_DeviceDesc = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 2);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_HardwareIds = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 3);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_CompatibleIds = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 4);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_Service = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 6);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_Class = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 9);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_ClassGuid = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 10);    // DEVPROP_TYPE_GUID
        public static readonly PropertyKey PKEY_Device_Driver = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 11);    // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_ConfigFlags = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 12);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_Manufacturer = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 13);    // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_FriendlyName = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 14);    // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_LocationInfo = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 15);    // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_PDOName = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 16);    // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_Capabilities = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 17);    // DEVPROP_TYPE_UNINT32
        public static readonly PropertyKey PKEY_Device_UINumber = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 18);    // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_UpperFilters = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 19);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_LowerFilters = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 20);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_BusTypeGuid = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 21);    // DEVPROP_TYPE_GUID
        public static readonly PropertyKey PKEY_Device_LegacyBusType = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 22);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_BusNumber = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 23);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_EnumeratorName = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 24);    // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_Security = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 25);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public static readonly PropertyKey PKEY_Device_SecuritySDS = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 26);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public static readonly PropertyKey PKEY_Device_DevType = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 27);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_Exclusive = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 28);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_Characteristics = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 29);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_Address = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 30);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_UINumberDescFormat = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 31);    // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_PowerData = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 32);    // DEVPROP_TYPE_BINARY
        public static readonly PropertyKey PKEY_Device_RemovalPolicy = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 33);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_RemovalPolicyDefault = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 34);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_RemovalPolicyOverride = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 35);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_InstallState = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 36);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_LocationPaths = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 37);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_BaseContainerId = new PropertyKey(new Guid(0xa45c254e, 0xdf1c, 0x4efd, 0x80, 0x20, 0x67, 0xd1, 0x46, 0xa8, 0x50, 0xe0), 38);    // DEVPROP_TYPE_GUID

        //
        // Device properties
        // These PKEYs correspond to a device's status and problem code
        //
        public static readonly PropertyKey PKEY_Device_DevNodeStatus = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 2);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_ProblemCode = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 3);     // DEVPROP_TYPE_UINT32

        //
        // Device properties
        // These PKEYs correspond to device relations
        //
        public static readonly PropertyKey PKEY_Device_EjectionRelations = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 4);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_RemovalRelations = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 5);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_PowerRelations = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 6);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_BusRelations = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 7);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_Parent = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 8);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_Children = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 9);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_Siblings = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 10);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_TransportRelations = new PropertyKey(new Guid(0x4340a6c5, 0x93fa, 0x4706, 0x97, 0x2c, 0x7b, 0x64, 0x80, 0x08, 0xa5, 0xa7), 11);    // DEVPROP_TYPE_STRING_LIST

        //
        // Other Device properties
        //
        public static readonly PropertyKey PKEY_Device_Reported = new PropertyKey(new Guid(0x80497100, 0x8c73, 0x48b9, 0xaa, 0xd9, 0xce, 0x38, 0x7e, 0x19, 0xc5, 0x6e), 2);     // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_Device_Legacy = new PropertyKey(new Guid(0x80497100, 0x8c73, 0x48b9, 0xaa, 0xd9, 0xce, 0x38, 0x7e, 0x19, 0xc5, 0x6e), 3);     // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_Device_InstanceId = new PropertyKey(new Guid(0x78c34fc8, 0x104a, 0x4aca, 0x9e, 0xa4, 0x52, 0x4d, 0x52, 0x99, 0x6e, 0x57), 256);   // DEVPROP_TYPE_STRING

        public static readonly PropertyKey PKEY_Device_ContainerId = new PropertyKey(new Guid(0x8c7ed206, 0x3f8a, 0x4827, 0xb3, 0xab, 0xae, 0x9e, 0x1f, 0xae, 0xfc, 0x6c), 2);     // DEVPROP_TYPE_GUID

        public static readonly PropertyKey PKEY_Device_ModelId = new PropertyKey(new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 2);     // DEVPROP_TYPE_GUID

        public static readonly PropertyKey PKEY_Device_FriendlyNameAttributes = new PropertyKey(new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 3);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_ManufacturerAttributes = new PropertyKey(new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 4);     // DEVPROP_TYPE_UINT32

        public static readonly PropertyKey PKEY_Device_PresenceNotForDevice = new PropertyKey(new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 5);     // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_Device_SignalStrength = new PropertyKey(new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 6);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_IsAssociateableByUserAction = new PropertyKey(new Guid(0x80d81ea6, 0x7473, 0x4b0c, 0x82, 0x16, 0xef, 0xc1, 0x1a, 0x2c, 0x4c, 0x8b), 7);// DEVPROP_TYPE_BOOLEAN



        public static readonly PropertyKey PKEY_Numa_Proximity_Domain = new PropertyKey(new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), 1);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_DHP_Rebalance_Policy = new PropertyKey(new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), 2);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_Numa_Node = new PropertyKey(new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), 3);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_BusReportedDeviceDesc = new PropertyKey(new Guid(0x540b947e, 0x8b40, 0x45bc, 0xa8, 0xa2, 0x6a, 0x0b, 0x89, 0x4c, 0xbd, 0xa2), 4);     // DEVPROP_TYPE_STRING

        public static readonly PropertyKey PKEY_Device_InstallInProgress = new PropertyKey(new Guid(0x83da6326, 0x97a6, 0x4088, 0x94, 0x53, 0xa1, 0x92, 0x3f, 0x57, 0x3b, 0x29), 9);     // DEVPROP_TYPE_BOOLEAN

        //
        // Device driver properties
        //
        public static readonly PropertyKey PKEY_Device_DriverDate = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 2);      // DEVPROP_TYPE_FILETIME
        public static readonly PropertyKey PKEY_Device_DriverVersion = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 3);      // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_DriverDesc = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 4);      // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_DriverInfPath = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 5);      // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_DriverInfSection = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 6);      // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_DriverInfSectionExt = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 7);      // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_MatchingDeviceId = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 8);      // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_DriverProvider = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 9);      // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_DriverPropPageProvider = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 10);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_DriverCoInstallers = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 11);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_Device_ResourcePickerTags = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 12);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_ResourcePickerExceptions = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 13); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_Device_DriverRank = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 14);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_DriverLogoLevel = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 15);     // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_Device_NoConnectSound = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 17);     // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_Device_GenericDriverInstalled = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 18);     // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_Device_AdditionalSoftwareRequested = new PropertyKey(new Guid(0xa8b865dd, 0x2e3d, 0x4094, 0xad, 0x97, 0xe5, 0x93, 0xa7, 0xc, 0x75, 0xd6), 19);// DEVPROP_TYPE_BOOLEAN

        //
        // Device safe-removal properties
        //
        public static readonly PropertyKey PKEY_Device_SafeRemovalRequired = new PropertyKey(new Guid(0xafd97640, 0x86a3, 0x4210, 0xb6, 0x7c, 0x28, 0x9c, 0x41, 0xaa, 0xbe, 0x55), 2);    // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_Device_SafeRemovalRequiredOverride = new PropertyKey(new Guid(0xafd97640, 0x86a3, 0x4210, 0xb6, 0x7c, 0x28, 0x9c, 0x41, 0xaa, 0xbe, 0x55), 3);// DEVPROP_TYPE_BOOLEAN


        //
        // Device properties that were set by the driver package that was installed
        // on the device.
        //
        public static readonly PropertyKey PKEY_DrvPkg_Model = new PropertyKey(new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 2);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DrvPkg_VendorWebSite = new PropertyKey(new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 3);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DrvPkg_DetailedDescription = new PropertyKey(new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 4);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DrvPkg_DocumentationLink = new PropertyKey(new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 5);     // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DrvPkg_Icon = new PropertyKey(new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 6);     // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_DrvPkg_BrandingIcon = new PropertyKey(new Guid(0xcf73bb51, 0x3abf, 0x44a2, 0x85, 0xe0, 0x9a, 0x3d, 0xc7, 0xa1, 0x21, 0x32), 7);     // DEVPROP_TYPE_STRING_LIST

        //
        // Device setup class properties
        // These PKEYs correspond to the old setupapi SPCRP_XXX properties
        //
        public static readonly PropertyKey PKEY_DeviceClass_UpperFilters = new PropertyKey(new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 19);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_DeviceClass_LowerFilters = new PropertyKey(new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 20);    // DEVPROP_TYPE_STRING_LIST
        public static readonly PropertyKey PKEY_DeviceClass_Security = new PropertyKey(new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 25);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR
        public static readonly PropertyKey PKEY_DeviceClass_SecuritySDS = new PropertyKey(new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 26);    // DEVPROP_TYPE_SECURITY_DESCRIPTOR_STRING
        public static readonly PropertyKey PKEY_DeviceClass_DevType = new PropertyKey(new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 27);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_DeviceClass_Exclusive = new PropertyKey(new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 28);    // DEVPROP_TYPE_UINT32
        public static readonly PropertyKey PKEY_DeviceClass_Characteristics = new PropertyKey(new Guid(0x4321918b, 0xf69e, 0x470d, 0xa5, 0xde, 0x4d, 0x88, 0xc7, 0x5a, 0xd2, 0x4b), 29);    // DEVPROP_TYPE_UINT32

        //
        // Device setup class properties
        // These PKEYs correspond to registry values under the device class GUID key
        //
        public static readonly PropertyKey PKEY_DeviceClass_Name = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 2);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DeviceClass_ClassName = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 3);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DeviceClass_Icon = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 4);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DeviceClass_ClassInstaller = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 5);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DeviceClass_PropPageProvider = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 6);  // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DeviceClass_NoInstallClass = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 7);  // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_DeviceClass_NoDisplayClass = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 8);  // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_DeviceClass_SilentInstall = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 9);  // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_DeviceClass_NoUseClass = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 10); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_DeviceClass_DefaultService = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 11); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DeviceClass_IconPath = new PropertyKey(new Guid(0x259abffc, 0x50a7, 0x47ce, 0xaf, 0x8, 0x68, 0xc9, 0xa7, 0xd7, 0x33, 0x66), 12); // DEVPROP_TYPE_STRING_LIST

        //
        // Other Device setup class properties
        //
        public static readonly PropertyKey PKEY_DeviceClass_ClassCoInstallers = new PropertyKey(new Guid(0x713d1703, 0xa2e2, 0x49f5, 0x92, 0x14, 0x56, 0x47, 0x2e, 0xf3, 0xda, 0x5c), 2); // DEVPROP_TYPE_STRING_LIST

        //
        // Device interface properties
        //
        public static readonly PropertyKey PKEY_DeviceInterface_FriendlyName = new PropertyKey(new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), 2); // DEVPROP_TYPE_STRING
        public static readonly PropertyKey PKEY_DeviceInterface_Enabled = new PropertyKey(new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), 3); // DEVPROP_TYPE_BOOLEAN
        public static readonly PropertyKey PKEY_DeviceInterface_ClassGuid = new PropertyKey(new Guid(0x026e516e, 0xb814, 0x414b, 0x83, 0xcd, 0x85, 0x6d, 0x6f, 0xef, 0x48, 0x22), 4); // DEVPROP_TYPE_GUID

        //
        // Device interface class properties
        //
        public static readonly PropertyKey PKEY_DeviceInterfaceClass_DefaultInterface = new PropertyKey(new Guid(0x14c83a99, 0x0b3f, 0x44b7, 0xbe, 0x4c, 0xa1, 0x78, 0xd3, 0x99, 0x05, 0x64), 2); // DEVPROP_TYPE_STRING
    }
}
