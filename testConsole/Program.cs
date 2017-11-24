﻿using System;
using System.Management;
using System.Diagnostics;
using System.Threading;

public class Example
{
    public static void Main()
    {
        Console.WriteLine(AutodetectArduinoPort());
        Console.ReadKey();
    }
    public static string AutodetectArduinoPort()
    {
        ManagementScope connectionScope = new ManagementScope();
        SelectQuery serialQuery = new SelectQuery("SELECT * FROM Win32_SerialPort");
        ManagementObjectSearcher searcher = new ManagementObjectSearcher(connectionScope, serialQuery);

        try
        {
            foreach (ManagementObject item in searcher.Get())
            {
                string desc = item["Description"].ToString();
                string deviceId = item["DeviceID"].ToString();

                if (desc.Contains("Arduino"))
                {
                    return deviceId;
                }
            }
        }
        catch (ManagementException e)
        {
            /* Do Nothing */
        }
        return null;
    }
}