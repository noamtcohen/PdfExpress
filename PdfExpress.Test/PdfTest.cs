using System.IO;
using NUnit.Framework;

namespace PdfExpress.Test
{
    [TestFixture]
    class PdfTest
    {
        private string TEST_URL = FixtureSetup.URI;

        protected readonly string PDF_OUT = @"C:\DELETE\test.pdf";

        PdfCreator _creator = new PdfCreator();
        [SetUp]
        public void Setup()
        {
            if (!Directory.Exists(Path.GetDirectoryName(PDF_OUT)))
                Directory.CreateDirectory(Path.GetDirectoryName(PDF_OUT));

            if (File.Exists(PDF_OUT))
                File.Delete(PDF_OUT);

            _creator = new PdfCreator();
            _creator.SetJsRunDelayInMsec(0);
        }

        [Test]
        public void ConvertUrlToPdf()
        {
            _creator.SetPaths(TEST_URL, PDF_OUT);
            _creator.Create();

            Assert.IsTrue(File.Exists(PDF_OUT), "Pdf was not created");
        }

        [Test]
        public void ConvertUrlToPdfWithAuthentication()
        {
            _creator.SetPaths(TEST_URL + "/auth", PDF_OUT);
            _creator.AddCookie("i-am-authenticated","hi");

            _creator.Create();
            Assert.IsTrue(File.Exists(PDF_OUT), "Pdf was not created");
        }

        [Test]
        public void ConvertUrlToPdfWithPost()
        {
            _creator.SetPaths(TEST_URL + "/post", PDF_OUT);
            _creator.AddPostParams("Hi","Hello");
            _creator.Create();

            Assert.IsTrue(File.Exists(PDF_OUT), "Pdf was not created");
        }

        [Test]
        public void CreatePdfAndEmailIt()
        {
            var server = "13.13.13.13";

            var username = "noreply@here.there.and.where";
            var password = "quantumcomputing";
            var from = "me@...";
            var to = "you@?";

            _creator.SetPaths(TEST_URL + "/post", PDF_OUT);

            _creator.AddPostParams("Hi", "<h1 style='color:#45aede'>Every thing is ok, and you?</h1>");
            _creator.Create();

            Assert.IsTrue(File.Exists(PDF_OUT), "Pdf was not created");

            var emailer = new Emailer();


            emailer.SetSmtpParams(server, 25, username, password);
            emailer.AddAttachment(PDF_OUT);

            var sent = emailer.SendEmail(from, to, "Hi!", "<h1>What's new?</h1>");
            Assert.True(sent, "Failed to send Email");
        }
    }
}