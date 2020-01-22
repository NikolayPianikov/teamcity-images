﻿using System;
using System.Threading.Tasks;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Push
{
    internal class Command: ICommand<IOptions>
    {
        private readonly ILogger _logger;
        private readonly IOptions _options;
        private readonly IImageFetcher _imageFetcher;
        private readonly IImagePublisher _imagePublisher;
        private readonly IImageCleaner _imageCleaner;

        public Command(
            [NotNull] ILogger logger,
            [NotNull] IOptions options,
            [NotNull] IImageFetcher imageFetcher,
            [NotNull] IImagePublisher imagePublisher,
            [NotNull] IImageCleaner imageCleaner)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _imageFetcher = imageFetcher ?? throw new ArgumentNullException(nameof(imageFetcher));
            _imagePublisher = imagePublisher ?? throw new ArgumentNullException(nameof(imagePublisher));
            _imageCleaner = imageCleaner ?? throw new ArgumentNullException(nameof(imageCleaner));
        }

        public async Task<Result> Run()
        {
            var imagesResult = await _imageFetcher.GetImages();
            if (imagesResult.State == Result.Error)
            {
                _logger.Log("Cannot get docker images to push for session \"{_options.SessionId}\".", Result.Error);
                return Result.Error;
            }

            if (imagesResult.Value.Count == 0)
            {
                _logger.Log($"Docker images were not found for session \"{_options.SessionId}\". Make sure you've made a build before and check the session id.", Result.Error);
                return Result.Error;
            }

            var pushResult = await _imagePublisher.PushImages(imagesResult.Value);

            if (_options.Clean)
            {
                imagesResult = await _imageFetcher.GetImages();
                if (imagesResult.State != Result.Error)
                {
                    var cleanResult =  await _imageCleaner.CleanImages(imagesResult.Value);
                    if (cleanResult == Result.Error)
                    {
                        _logger.Log("Cannot clean images for session \"{_options.SessionId}\".", Result.Warning);
                    }
                }
                else
                {
                    _logger.Log("Cannot get docker images to clean for session \"{_options.SessionId}\".", Result.Warning);
                }
            }

            return pushResult;
        }
    }
}
