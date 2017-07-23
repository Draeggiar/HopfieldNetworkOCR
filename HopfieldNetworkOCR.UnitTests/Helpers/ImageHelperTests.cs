using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Drawing;
using System.IO;

namespace HopfieldNetworkOCR.UnitTests.Helpers
{
    [TestClass]
    public class ImageHelperTests
    {
        private const string _imageFolder = "../../img/";
        private const string _imageName = "A.tiff";

        [TestMethod]
        [TestCategory("Helpers")]
        public void LoadImageTest()
        {
            Image img = Image.FromFile(_imageFolder + _imageName);
            byte[] good;
            using (var  ms = new MemoryStream())
            {
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                good = ms.ToArray();
            }

            byte [] test = HopfieldNetworkOCR.Helpers.ImageHelper.LoadImage(_imageFolder + _imageName);

            Assert.AreEqual(good.Length, test.Length);

            for (int i=0; i<test.Length; i++)
            {
                Assert.AreEqual(good[i], test[i]);
            }
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void SaveImageTest()
        {           
            byte[] test;
            using (var ms = new MemoryStream())
            {
                var img = Image.FromFile(_imageFolder + _imageName);

                img.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                test = ms.ToArray();
            }

            HopfieldNetworkOCR.Helpers.ImageHelper.SaveImage(test, _imageFolder + "save_test.tiff");

            byte[] result;
            using (var ms = new MemoryStream())
            {
                var img = Image.FromFile(_imageFolder + "save_test.tiff");

                img.Save(ms, System.Drawing.Imaging.ImageFormat.Tiff);
                result = ms.ToArray();
            }

            Assert.AreEqual(test.Length, result.Length);

            for (int i = 0; i < test.Length; i++)
                Assert.AreEqual(test[i], result[i]);           
        }

        [TestMethod]
        [TestCategory("Helpers")]
        public void LoadAllFromCatalogTest()
        {
            Assert.Fail();
        }
    }
}