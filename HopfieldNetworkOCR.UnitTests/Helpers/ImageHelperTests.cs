using System.Drawing;
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
            var test = HopfieldNetworkOCR.Helpers.ImageHelper.LoadImage(ImageFolder + ImageName);

            Assert.AreEqual(ImageContent.Length, test.Length);

            for (int i=0; i<test.Length; i++)
            {
                Assert.AreEqual(ImageContent[i], test[i]);
            }
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void ConvertVectorToImageTest()
        {
            var test = HopfieldNetworkOCR.Helpers.ImageHelper.ConvertVectorToImage(ImageContent);

            Assert.AreEqual(typeof(Bitmap), test.GetType());
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void SaveImageTest()
        {
            HopfieldNetworkOCR.Helpers.ImageHelper.SaveImage(ImageContent, ImageFolder + "save_test.tiff");

            var test = HopfieldNetworkOCR.Helpers.ImageHelper.LoadImage(ImageFolder + "save_test.tiff");

            Assert.AreEqual(ImageContent.Length, test.Length);

            for (int i = 0; i < test.Length; i++)
                Assert.AreEqual(test[i], ImageContent[i]);
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void LoadAllFromCatalogTest()
        {
            var images = HopfieldNetworkOCR.Helpers.ImageHelper.LoadAllFromCatalog(ImageFolder);

            foreach (string image in images)
            {
                Assert.AreEqual(ImageContent, image);
            }
        }
    }
}