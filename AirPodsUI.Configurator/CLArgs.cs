﻿using AirPodsUI.Configurator.Cards;
using AirPodsUI.Configurator.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AirPodsUI.Configurator
{
    public class CLArgs
    {
        public static void Parse(string[] args)
        {
            Log.Information($"Got args {args[0]} and {args[1]}");
            string ext = Path.GetExtension(args[0]).ToLower();

            if (ext == ".pencil")
            {
                PencilConfig config = ConfigParser.ParseP(args[0]);
                if (string.IsNullOrEmpty(config.StaticName))
                    config.StaticName = args[1];
                new Pencil(config).Show();
            }
            else if (ext == ".notif")
            {
                NotificationConfig config = ConfigParser.ParseN(args[0]);
                if (string.IsNullOrEmpty(config.StaticName))
                    config.StaticName = args[1];
                new Banner(config).Show();
            }
            else if (ext == ".card")
            {
                CardConfig config = ConfigParser.ParseC(args[0]);
                if (string.IsNullOrEmpty(config.StaticName))
                    config.StaticName = args[1];
                new Card(config).Show();
            }
            else
            {
                new MainWindow();
            }
        }
    }
}
