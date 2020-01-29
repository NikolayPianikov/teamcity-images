using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Docker.DotNet;
using IoC;

// ReSharper disable ClassNeverInstantiated.Global

namespace TeamCity.Docker.Push
{
    internal class Command: ICommand<IOptions>
    {
        private readonly IOptions _options;
        private readonly IImageFetcher _imageFetcher;
        private readonly IImagePublisher _imagePublisher;
        private readonly IImageCleaner _imageCleaner;

        public Command(
            [NotNull] IActiveObject[] activeObjects,
            [NotNull] IOptions options,
            [NotNull] IImageFetcher imageFetcher,
            [NotNull] IImagePublisher imagePublisher,
            [NotNull] IImageCleaner imageCleaner)
        {
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
                return Result.Error;
            }

            var pushResult = await _imagePublisher.PushImages(imagesResult.Value);

            if (_options.Clean)
            {
                imagesResult = await _imageFetcher.GetImages();
                if (imagesResult.State != Result.Error)
                {
                    await _imageCleaner.CleanImages(imagesResult.Value);
                }
            }

            return pushResult;
        }
    }
}
