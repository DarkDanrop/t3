﻿using System;
using System.Collections.Generic;
using NAudio.Midi;
using T3.Core.Logging;

namespace T3.Gui.Interaction.LegacyVariations.Midi
{
    /// <summary>
    /// Keeps track of connected midi out devices sorted by product names 
    /// </summary>
    public static class MidiOutConnectionManager
    {
        public static void Init()
        {
            CloseMidiDevices();
            ScanAndRegisterToMidiOutDevices();
            _initialized = true;
        }

        public static MidiOut GetConnectedController(int deviceIdHash)
        {
            if (!_initialized)
            {
                Log.Error("Trying to access midiout without prior initialization.");
                return null;
            }
            MidiOutsByDeviceIdHash.TryGetValue(deviceIdHash, out var midiOut);
            return midiOut;
        }

        private static void ScanAndRegisterToMidiOutDevices(bool logInformation = false)
        {
            for (var index = 0; index < MidiOut.NumberOfDevices; index++)
            {
                var deviceInfo = MidiOut.DeviceInfo(index);

                if (logInformation)
                    Log.Debug("Scanning " + deviceInfo.ProductName);

                MidiOut newMidiOut = null;
                try
                {
                    newMidiOut = new MidiOut(index);
                }
                catch (NAudio.MmException e)
                {
                    Log.Error(" > " + e.Message + " " + MidiOut.DeviceInfo(index).ProductName);
                    continue;
                }

                var hash = deviceInfo.ProductName.GetHashCode();
                MidiOutsByDeviceIdHash[hash] = newMidiOut;
            }
        }

        private static void CloseMidiDevices()
        {
            foreach (var midiOut in MidiOutsByDeviceIdHash.Values)
            {
                try
                {
                    midiOut.Close();
                    midiOut.Dispose();
                }
                catch (Exception e)
                {
                    Log.Debug("exception: " + e);
                }
            }

            MidiOutsByDeviceIdHash.Clear();
        }

        private static readonly Dictionary<int, MidiOut> MidiOutsByDeviceIdHash = new Dictionary<int, MidiOut>();
        private static bool _initialized;
    }
}