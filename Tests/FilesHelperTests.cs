using Xunit;

namespace Contemplation.Tests
{
    public class FilesHelperTests
    {
        [Fact]
        public void RestrictFileToFolderNormal()
        {
            var folder = @"c:\temp";
            var file = "image.jpg";

            var result = FilesHelper.RestrictFileToFolder(folder, file);

            Assert.Equal(@"c:\temp\image.jpg", result);
        }

        [Fact]
        public void RestrictFileToFolderRelative()
        {
            var folder = @"c:\temp";
            var file = @"..\folder\image.jpg";

            var result = FilesHelper.RestrictFileToFolder(folder, file);

            Assert.Equal(@"c:\temp\image.jpg", result);
        }
    }
}
