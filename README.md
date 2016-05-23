# Pdf Express

### Convert web pages to pdf and email them.


#### How to use:

```csharp
var creator = new PdfCreator();

creator.SetPaths("http://url.com",@"c:\path\to.pdf");
creator.SetTitle("My Cool Pdf!");
creator.SetOrientation(PdfCreator.Orientation.Landscape);

creator.SetJsRunDelayInMsec(1000);

creator.AddCookie("ASP-AUTH","ASDKJWERIU2348234");
creator.AddPostParams("username","shmolik");
creator.AddPostParams("password","bazari");
                
creator.SetCookieJarFile(@"c:\file\to\save\cookies\set\by\post");

creator.SetExternalStyleSheetUrl("http://some.style.css");
creator.SetRunJsAfterLoad("alert('Hi!')");

creator.SetDpi(300);
creator.Create();
Assert.True(creator.WasCreated);

var emailer = new Emailer();

emailer.SetSmtpParams("SERVER", PORT, "USER_NAME", "PASSWORD");
emailer.AddAttachment(creator.SavePdfPath);

var sent = emailer.SendEmail("from@this.address", "to@this.address", "Hi!", "<h1>What's new?</h1>");
Assert.True(sent, "Failed to send Email");
```