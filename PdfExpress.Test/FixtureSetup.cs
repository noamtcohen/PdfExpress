using ExpressCS.Test;
using NUnit.Framework;

namespace PdfExpress.Test
{
    [SetUpFixture]
    class FixtureSetup : FixtureSetupWithHost
    {
        public const string URI = "http://localhost:770";
        
        public override string GetUri()
        {
            return URI;
        }
    }
}
