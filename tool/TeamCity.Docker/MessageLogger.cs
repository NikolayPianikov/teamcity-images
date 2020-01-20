﻿using System.Text;
using Docker.DotNet.Models;
using Newtonsoft.Json;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker
{
    internal class MessageLogger : IMessageLogger
    {
        private readonly ILogger _logger;

        public MessageLogger(ILogger logger) => _logger = logger;

        public Result Log(JSONMessage message)
        {
            if (message.Error != null && !string.IsNullOrWhiteSpace(message.Error.Message))
            {
                _logger.Log(message.Error.Message.Trim(), Result.Error);
                return Result.Error;
            }

            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(message.ID))
            {
                sb.Append(message.ID);
                sb.Append(": ");
            }

            if (!string.IsNullOrWhiteSpace(message.Status))
            {
                sb.Append(message.Status.Trim());
            }

            if (!string.IsNullOrWhiteSpace(message.Stream))
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }

                sb.Append(message.Stream.Trim());
            }

            if (!string.IsNullOrWhiteSpace(message.ProgressMessage))
            {
                if (sb.Length > 0)
                {
                    sb.Append(' ');
                }

                sb.Append(message.ProgressMessage);
            }
            else
            {
                if (message.Progress != null && message.Progress.Total > 0)
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(' ');
                    }

                    var percent = message.Progress.Current * 100.0 / message.Progress.Total;
                    if (percent <= 100)
                    {
                        sb.Append($"{percent: 000}%");
                    }
                    else
                    {
                        sb.Append(message.Progress.Current);
                    }
                }
            }

            _logger.Log(sb.ToString());
            return Result.Success;
        }

        public Result Log(string jsonMessage)
        {
            var message = JsonConvert.DeserializeObject<JSONMessage>(jsonMessage);
            return Log(message);
        }
    }
}