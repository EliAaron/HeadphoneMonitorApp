using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeadphoneMonitorApp
{
    /// <summary>
    /// The EndpointFormFactor enumeration defines constants that indicate the general physical attributes of an audio endpoint device
    /// </summary>
    public enum EndpointFormFactor
    {
        /// <summary>
        /// An audio endpoint device that the user accesses remotely through a network.
        /// </summary>
        RemoteNetworkDevice = 0,

        /// <summary>
        /// A set of speakers.
        /// </summary>
        Speakers = (RemoteNetworkDevice + 1),

        /// <summary>
        /// An audio endpoint device that sends a line-level analog signal to a line-input jack on an audio adapter or that receives a line-level analog signal from a line-output jack on the adapter.
        /// </summary>
        LineLevel = (Speakers + 1),

        /// <summary>
        /// A set of headphones.
        /// </summary>
        Headphones = (LineLevel + 1),

        /// <summary>
        /// A microphone.
        /// </summary>
        Microphone = (Headphones + 1),

        /// <summary>
        /// An earphone or a pair of earphones with an attached mouthpiece for two-way communication.
        /// </summary>
        Headset = (Microphone + 1),

        /// <summary>
        /// The part of a telephone that is held in the hand and that contains a speaker and a microphone for two-way communication.
        /// </summary>
        Handset = (Headset + 1),

        /// <summary>
        /// An audio endpoint device that connects to an audio adapter through a connector for a digital interface of unknown type that transmits non-PCM data in digital pass-through mode. For more information, see Remarks.
        /// </summary>
        UnknownDigitalPassthrough = (Handset + 1),

        /// <summary>
        /// An audio endpoint device that connects to an audio adapter through a Sony/Philips Digital Interface (S/PDIF) connector.
        /// </summary>
        SPDIF = (UnknownDigitalPassthrough + 1),

        /// <summary>
        /// An audio endpoint device that connects to an audio adapter through a High-Definition Multimedia Interface (HDMI) connector or a display port.
        /// </summary>
        DigitalAudioDisplayDevice = (SPDIF + 1),

        /// <summary>
        /// An audio endpoint device with unknown physical attributes.
        /// </summary>
        UnknownFormFactor = (DigitalAudioDisplayDevice + 1),

        /// <summary>
        /// Windows 7: Maximum number of endpoint form factors.
        /// </summary>
        EndpointFormFactor_enum_count = (UnknownFormFactor + 1)
    }

}
