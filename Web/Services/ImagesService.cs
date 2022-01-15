using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Options;

namespace Contemplation.Services
{
    public class ImagesService : IImagesService
    {
        public ImagesService(IOptions<AppConfiguration> options)
        {
            _appConfiguration = options.Value;
        }

        private AppConfiguration _appConfiguration;

        public string[] GetAll()
        {
            return Directory.GetFiles(_appConfiguration.ImagesFolder, "*.jpg")
                .Select(file => Path.GetFileName(file))
                .ToArray();
        }

        public byte[] GetFull(string name, int? width = null, int? height = null)
        {
            var filePath = FilesHelper.RestrictFileToFolder(_appConfiguration.ImagesFolder, name);

            if (width == null || height == null)
            {
                return File.ReadAllBytes(filePath);
            }

            using var bitmap = new Bitmap(width.Value, height.Value);

            // Main part - resized image
            using var image = Image.FromFile(filePath);
            var sizeContain = ProportionsHelper.Contain(new Point(image.Width, image.Height), new Point(width.Value, height.Value));
            var sizeCover = ProportionsHelper.Cover(new Point(image.Width, image.Height), new Point(width.Value, height.Value));
            using var resizedImage = ResizeImage(image, sizeContain.X, sizeContain.Y);
            var resizedImageX = (bitmap.Width - resizedImage.Width) / 2;
            var resizedImageY = (bitmap.Height - resizedImage.Height) / 2;

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawImage(resizedImage, resizedImageX, resizedImageY, resizedImage.Width, resizedImage.Height);
            }

            var scaleCover = (double)sizeCover.X / image.Width;

            var matrix = GetSaturationMatrix(_appConfiguration.PartsSaturation);
            matrix.Matrix33 = _appConfiguration.PartsOpacity;
            using var attributes = new ImageAttributes();
            attributes.SetColorMatrix(matrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            var partsBackgroundColor = ColorTranslator.FromHtml(_appConfiguration.PartsBackgroundColor);

            // Left and right parts
            if (bitmap.Width != resizedImage.Width)
            {
                var partWidthCover = (sizeCover.X - resizedImage.Width) / 2;
                var partWidth = (int)(partWidthCover / scaleCover);

                var partYCover = (sizeCover.Y - resizedImage.Height) / 2;
                var partY = (int)(partYCover / scaleCover);
                var partHeight = (int)(resizedImage.Height / scaleCover);

                // Left part
                using var leftPart = new Bitmap(partWidthCover, bitmap.Height);

                using (var partGraphics = Graphics.FromImage(leftPart))
                {
                    partGraphics.Clear(partsBackgroundColor);
                    var destRect = new Rectangle(0, 0, leftPart.Width, leftPart.Height);
                    partGraphics.DrawImage(image, destRect, 0, partY, partWidth, partHeight, GraphicsUnit.Pixel, attributes);
                }

                using var leftPartBlured = new GaussianBlur(leftPart).Process(_appConfiguration.PartsBlurRadius);

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawImage(leftPartBlured, 0, 0);
                }

                // Right part
                using var rightPart = new Bitmap(bitmap.Width - partWidthCover - resizedImage.Width, bitmap.Height);

                using (var partGraphics = Graphics.FromImage(rightPart))
                {
                    partGraphics.Clear(partsBackgroundColor);
                    var destRect = new Rectangle(0, 0, rightPart.Width, rightPart.Height);
                    partGraphics.DrawImage(image, destRect, image.Width - partWidth, partY, partWidth, partHeight, GraphicsUnit.Pixel, attributes);
                }

                using var rightPartBlured = new GaussianBlur(rightPart).Process(_appConfiguration.PartsBlurRadius);

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawImage(rightPartBlured, partWidthCover + resizedImage.Width, 0);
                }
            }

            // Top and bottom parts
            if (bitmap.Height != resizedImage.Height)
            {
                var partHeightCover = (sizeCover.Y - resizedImage.Height) / 2;
                var partHeight = (int)(partHeightCover / scaleCover);

                var partXCover = (sizeCover.X - resizedImage.Width) / 2;
                var partX = (int)(partXCover / scaleCover);
                var partWidth = (int)(resizedImage.Width / scaleCover);

                // Top part
                using var topPart = new Bitmap(bitmap.Width, partHeightCover);

                using (var partGraphics = Graphics.FromImage(topPart))
                {
                    partGraphics.Clear(partsBackgroundColor);
                    var destRect = new Rectangle(0, 0, topPart.Width, topPart.Height);
                    partGraphics.DrawImage(image, destRect, partX, 0, partWidth, partHeight, GraphicsUnit.Pixel, attributes);
                }

                using var topPartBlured = new GaussianBlur(topPart).Process(_appConfiguration.PartsBlurRadius);

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawImage(topPartBlured, 0, 0);
                }

                // Bottom part
                using var bottomPart = new Bitmap(bitmap.Width, bitmap.Height - partHeightCover - resizedImage.Height);

                using (var partGraphics = Graphics.FromImage(bottomPart))
                {
                    partGraphics.Clear(partsBackgroundColor);
                    var destRect = new Rectangle(0, 0, bottomPart.Width, bottomPart.Height);
                    partGraphics.DrawImage(image, destRect, partX, image.Height - partHeight, partWidth, partHeight, GraphicsUnit.Pixel, attributes);
                }

                using var bottomPartBlured = new GaussianBlur(bottomPart).Process(_appConfiguration.PartsBlurRadius);

                using (var graphics = Graphics.FromImage(bitmap))
                {
                    graphics.DrawImage(bottomPartBlured, 0, partHeightCover + resizedImage.Height);
                }
            }

            using (var jpegStream = BitmapToJpeg(bitmap))
            {
                return jpegStream.ToArray();
            }
        }

        private Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, null);
            }

            return destImage;
        }

        private MemoryStream BitmapToJpeg(Bitmap bitmap)
        {
            EncoderParameters encoderParameters = new EncoderParameters(1);
            encoderParameters.Param[0] = new EncoderParameter(Encoder.Quality, 98L);
            var encoder = ImageCodecInfo.GetImageDecoders().First(t => t.FormatID == ImageFormat.Jpeg.Guid);
            var stream = new MemoryStream();
            bitmap.Save(stream, encoder, encoderParameters);
            return stream;
        }

        private ColorMatrix GetSaturationMatrix(float s)
        {
            // From http://www.graficaobscura.com/matrix/index.html

            var rwgt = 0.3086F;
            var gwgt = 0.6094F;
            var bwgt = 0.0820F;

            var result = new ColorMatrix();

            result.Matrix00 = (1.0F - s) * rwgt + s;
            result.Matrix01 = (1.0F - s) * rwgt;
            result.Matrix02 = (1.0F - s) * rwgt;

            result.Matrix10 = (1.0F - s) * gwgt;
            result.Matrix11 = (1.0F - s) * gwgt + s;
            result.Matrix12 = (1.0F - s) * gwgt;

            result.Matrix20 = (1.0F - s) * bwgt;
            result.Matrix21 = (1.0F - s) * bwgt;
            result.Matrix22 = (1.0F - s) * bwgt + s;

            return result;
        }

    }
}
