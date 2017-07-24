using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;

namespace HopfieldNetworkOCR.UnitTests.Helpers
{
    [TestClass]
    public class ImageHelperTests
    {
        private const string ImageFolder = "../../img/";
        private const string ImageName = "A.tiff";
        private const string ImageContent = "001000001100010100010100110110100010000000";

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
        public void SaveImageTest()
        {
            HopfieldNetworkOCR.Helpers.ImageHelper.SaveImage(ImageContent, ImageFolder + "save_test.tiff", 6, 7);

            var test = HopfieldNetworkOCR.Helpers.ImageHelper.LoadImage(ImageFolder + "save_test.tiff");

            Assert.AreEqual(ImageContent.Length, test.Length);

            for (int i = 0; i < test.Length; i++)
                Assert.AreEqual(test[i], ImageContent[i]);
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void LoadAllFromCatalogTest()
        {
            Assert.Fail();
        }
    }
}