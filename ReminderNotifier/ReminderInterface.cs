﻿using System;
using HidLibrary;
using System.Linq;
using System.Text;
using System.Threading;

public class ReminderInterface
{
    private const int VendorId = 0x1987;
    private const int ProductId = 0x1100;
    private const int ReportLength = 42;

    private static HidDevice device;

    private static void DeviceAttachedHandler()
    {
    }

    private static void DeviceRemovedHandler()
    {
    }

    public static void Connect()
    {
        device = HidDevices.Enumerate(VendorId, ProductId).FirstOrDefault();
        if (device != null)
        {
            device.OpenDevice();
            device.Inserted += DeviceAttachedHandler;
            device.Removed += DeviceRemovedHandler;
            device.MonitorDeviceEvents = true;
            device.ReadReport(ReadReportCallback);
        }
    }

    public static void SendMessage(String line1, String line2)
    {
        if (device == null)
        {
            Connect();
        }

        if (device == null)
        {

        }
        else
        {
            byte[] header = { 0, 1 };
            byte[] message = Encoding.ASCII.GetBytes("AHello".PadRight(40, '0'));

            device.WriteReport(new HidReport(ReportLength, new HidDeviceData(header.Concat(message).ToArray(), HidDeviceData.ReadStatus.Success)));
        }
    }

    private static void ReadReportCallback(HidReport report)
    {
        Console.WriteLine("recv: {0}", string.Join(", ", report.Data.Select(b => b.ToString("X2"))));
        device.ReadReport(ReadReportCallback);
    }

    public ReminderInterface()
	{
        device = null;
    }

    ~ReminderInterface()
    {
        if (device != null)
        {
            device.CloseDevice();
        }
    }
}