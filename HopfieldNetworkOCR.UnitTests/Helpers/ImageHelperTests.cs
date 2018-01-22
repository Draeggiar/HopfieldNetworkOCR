using System.Drawing;
using System.Linq;
using HopfieldNetworkOCR.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace HopfieldNetworkOCR.UnitTests.Helpers
{
    [TestClass]
    public class ImageHelperTests
    {
        private const string ImageFolder = "../../img/";
        private const string ImageName = "A.tiff";
        private const string ImageContent = "000000000000001000000001110000000111000000110110000011011000011101110001111111001110001110110000011010000000100000000000";

        [TestMethod]
        [TestCategory("Helpers")]
        public void LoadImageTest()
        {
            var test = ImageHelper.LoadImage(ImageFolder + ImageName);

            Assert.AreEqual(ImageContent.Length, test.Length);

            for (int i = 0; i < test.Length; i++)
            {
                Assert.AreEqual(ImageContent[i], test[i]);
            }
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void ConvertVectorToImageTest()
        {
            var test = ImageHelper.ConvertVectorToImage(ImageContent);

            Assert.AreEqual(typeof(Bitmap), test.GetType());
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void LoadAllFromCatalogTest()
        {
            var image = ImageHelper.LoadAllFromCatalog(ImageFolder).First();

            Assert.AreEqual(ImageName.Split('.')[0], image.Key);
            Assert.AreEqual(ImageContent, image.Value);
        }
    }
}