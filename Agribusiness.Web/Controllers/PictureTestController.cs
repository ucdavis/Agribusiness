using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Agribusiness.Core.Domain;
using UCDArch.Core.PersistanceSupport;
using UCDArch.Web.Controller;
using UCDArch.Web.Helpers;
using UCDArch.Core.Utils;

using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Agribusiness.Web.Controllers
{
    /// <summary>
    /// Controller for the PictureTest class
    /// </summary>
    public class PictureTestController : ApplicationController
    {
        private readonly IRepository<Person> _personRepository;

        public PictureTestController(IRepository<Person> personRepository)
        {
            _personRepository = personRepository;
        }

        //
        // GET: /PictureTest/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(int x, int y, int x2, int y2, int w, int h)
        {
            // load the file from the directory
            var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder.jpg"), FileMode.Open, FileAccess.Read);
            var img = new byte[fs.Length];
            fs.Read(img, 0, img.Length);
            fs.Close();

            // resize the sucker
            //var cropped = Crop(img, w, h, x, y);
            //return File(cropped, "image/jpeg");

            var resized = Resize(img, 80, 100);
            return File(resized, "image/jpeg");
        }

        private byte[] Crop(byte[] img, int w, int h, int x, int y)
        {
            try
            {
                var s = new MemoryStream(img);
                var origImg = Image.FromStream(s);
                
                var bmp = new Bitmap(w, h);
                bmp.SetResolution(origImg.HorizontalResolution, origImg.VerticalResolution);

                var graphic = Graphics.FromImage(bmp);
                graphic.SmoothingMode = SmoothingMode.AntiAlias;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphic.DrawImage(origImg, new Rectangle(0,0,w,h), x,y, w, h, GraphicsUnit.Pixel);

                var ms = new MemoryStream();
                bmp.Save(ms, origImg.RawFormat);
                return ms.GetBuffer();

            }
            catch (Exception)
            {
                throw;
            }

        }

        private byte[] Resize(byte[] img, int w, int h)
        {
            try
            {
                var s = new MemoryStream(img);
                var origImg = Image.FromStream(s);

                var newImg = new Bitmap(w, h);

                var rectangle = new Rectangle(0, 0, w, h);

                var graphic = Graphics.FromImage(newImg);
                graphic.CompositingQuality = CompositingQuality.HighQuality;
                graphic.SmoothingMode = SmoothingMode.HighQuality;
                graphic.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphic.DrawImage(origImg, rectangle);

                var ms = new MemoryStream();
                newImg.Save(ms, origImg.RawFormat);
                return ms.GetBuffer();
            }
            catch (Exception)
            {
                throw;
            }
        }


        //http://www.mikesdotnetting.com/Article/95/Upload-and-Crop-Images-with-jQuery-JCrop-and-ASP.NET



        //private const string url = "http://www.picnik.com/service/";
        //private const string apiKey = "6d16a7d2069c5ea033815fcb7f9a518d";

        //public ActionResult TestCall()
        //{
        //    var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder.jpg"), FileMode.Open, FileAccess.Read);
        //    var img = new Byte[fs.Length];
        //    fs.Read(img, 0, img.Length);
        //    fs.Close();

        //    var postParameters = new Dictionary<string, object>();
        //    postParameters.Add("_apikey", apiKey);
        //    postParameters.Add("_import", "imagedata");
        //    postParameters.Add("imagedata", new FileParameter(img, "profileplaceholder.jpg", "image/jpeg"));

        //    //string userAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15";
        //    string userAgent = string.Empty;
        //    var response = MultipartFormDataPost(url, userAgent, postParameters);

        //    return File(response.GetResponseStream(), "application/xhtml+xml");
        //}

        ///// <summary>
        ///// http://www.tech-archive.net/Archive/DotNet/microsoft.public.dotnet.languages.csharp/2008-02/msg03264.html
        ///// </summary>
        //private void Test1()
        //{
        //    long length = 0;
        //    var boundry = "-----------------------------236392951527621";

        //    var request = (HttpWebRequest)WebRequest.Create(url);
        //    request.ContentType = "multipart/form-data; boundry=" + boundry;
        //    request.Method = "POST";
        //    request.Credentials = System.Net.CredentialCache.DefaultCredentials;
        //    request.KeepAlive = true;
        //    request.Referer = "http://localhost:3071/picturetest";

        //    var header = string.Format("{0}", boundry);
        //    var footer = string.Format("{0}--", boundry);

        //    var contents = new StringBuilder();

        //    // add the image
        //    var person = _personRepository.Queryable.FirstOrDefault();

        //    var contentDesc = "Content-Disposition: form-data; name=\"{0}\"";

        //    contents.AppendLine(header);
        //    contents.AppendLine(string.Format(contentDesc, "_apikey"));
        //    contents.AppendLine();
        //    contents.AppendLine("6d16a7d2069c5ea033815fcb7f9a518d");

        //    contents.AppendLine(header);
        //    contents.AppendLine(string.Format(contentDesc, "_import"));
        //    contents.AppendLine();
        //    contents.AppendLine("imagedata");

        //    var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder.jpg"), FileMode.Open, FileAccess.Read);
        //    var br = new BinaryReader(fs);

        //    var img = br.ReadBytes((int)br.BaseStream.Length);

        //    br.Close();
        //    fs.Close();

        //    contents.AppendLine(header);
        //    contents.AppendLine(string.Format("Content-Disposition: form-data; name=\"imagedata\"; filename=\"{0}\"", "profileplaceholder.jpg"));
        //    contents.AppendLine("Content-Type: image/jpeg");
        //    contents.AppendLine();
        //    contents.AppendLine(Encoding.UTF8.GetString(img));
        //    //contents.AppendLine(Encoding.UTF8.GetString(person.OriginalPicture));

        //    contents.AppendLine(footer);

        //    var bytes = Encoding.UTF8.GetBytes(contents.ToString());

        //    request.ContentLength = bytes.Length;

        //    using (Stream requestStream = request.GetRequestStream())
        //    {
        //        requestStream.Write(bytes, 0, bytes.Length);
        //        requestStream.Flush();
        //        requestStream.Close();

        //        using (WebResponse response = request.GetResponse())
        //        {
        //            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
        //            {
        //                var result = reader.ReadToEnd();
        //                TempData["Result"] = result;
        //            }
        //        }
        //    }            
        //}

        ///// <summary>
        ///// http://stackoverflow.com/questions/219827/multipart-forms-from-c-client
        ///// </summary>
        //private void Test2()
        //{
        //    var fs = new FileStream(Server.MapPath("~/Images/profileplaceholder.jpg"), FileMode.Open, FileAccess.Read);
        //    var img = new Byte[fs.Length];
        //    fs.Read(img, 0, img.Length);
        //    fs.Close();

        //    var postParameters = new Dictionary<string, object>();
        //    postParameters.Add("_apikey", apiKey);
        //    postParameters.Add("_import", "imagedata");
        //    postParameters.Add("imagedata", new FileParameter(img, "profileplaceholder.jpg", "image/jpeg"));

        //    //string userAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.15) Gecko/20110303 Firefox/3.6.15";
        //    string userAgent = string.Empty;
        //    var response = MultipartFormDataPost(url, userAgent, postParameters);

            

        //    //var sr = new StreamReader(response.GetResponseStream());
        //    //var fullResponse = sr.ReadToEnd();
        //    //response.Close();

        //    //TempData["Result"] = fullResponse;
        //}

        ///// <summary>
        ///// http://www.briangrinstead.com/blog/multipart-form-post-in-c
        ///// </summary>
        //private static readonly Encoding encoding = Encoding.UTF8;
        //public static HttpWebResponse MultipartFormDataPost(string postUrl, string userAgent, Dictionary<string, object> postParameters)
        //{
        //    string formDataBoundary = "-----------------------------28947758029299";
        //    string contentType = "multipart/form-data; boundary=" + formDataBoundary;

        //    byte[] formData = GetMultipartFormData(postParameters, formDataBoundary);

        //    return PostForm(postUrl, userAgent, contentType, formData);
        //}
        //private static HttpWebResponse PostForm(string postUrl, string userAgent, string contentType, byte[] formData)
        //{
        //    HttpWebRequest request = WebRequest.Create(postUrl) as HttpWebRequest;

        //    if (request == null)
        //    {
        //        throw new NullReferenceException("request is not a http request");
        //    }

        //    // Set up the request properties
        //    request.Method = "POST";
        //    request.ContentType = contentType;
        //    request.UserAgent = userAgent;
        //    request.CookieContainer = new CookieContainer();
        //    request.ContentLength = formData.Length;  // We need to count how many bytes we're sending. 

        //    using (Stream requestStream = request.GetRequestStream())
        //    {
        //        // Push it out there
        //        requestStream.Write(formData, 0, formData.Length);
        //        requestStream.Close();
        //    }

        //    return request.GetResponse() as HttpWebResponse;
        //}

        //private static byte[] GetMultipartFormData(Dictionary<string, object> postParameters, string boundary)
        //{
        //    Stream formDataStream = new System.IO.MemoryStream();

        //    foreach (var param in postParameters)
        //    {
        //        if (param.Value is FileParameter)
        //        {
        //            FileParameter fileToUpload = (FileParameter)param.Value;

        //            // Add just the first part of this param, since we will write the file data directly to the Stream
        //            string header = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"; filename=\"{2}\";\r\nContent-Type: {3}\r\n\r\n",
        //                boundary,
        //                param.Key,
        //                fileToUpload.FileName ?? param.Key,
        //                fileToUpload.ContentType ?? "application/octet-stream");

        //            formDataStream.Write(encoding.GetBytes(header), 0, header.Length);

        //            // Write the file data directly to the Stream, rather than serializing it to a string.
        //            formDataStream.Write(fileToUpload.File, 0, fileToUpload.File.Length);
        //            // Thanks to feedback from commenters, add a CRLF to allow multiple files to be uploaded
        //            formDataStream.Write(encoding.GetBytes("\r\n"), 0, 2);
        //        }
        //        else
        //        {
        //            string postData = string.Format("--{0}\r\nContent-Disposition: form-data; name=\"{1}\"\r\n\r\n{2}\r\n",
        //                boundary,
        //                param.Key,
        //                param.Value);
        //            formDataStream.Write(encoding.GetBytes(postData), 0, postData.Length);
        //        }
        //    }

        //    // Add the end of the request
        //    string footer = "\r\n--" + boundary + "--\r\n";
        //    formDataStream.Write(encoding.GetBytes(footer), 0, footer.Length);

        //    // Dump the Stream into a byte[]
        //    formDataStream.Position = 0;
        //    byte[] formData = new byte[formDataStream.Length];
        //    formDataStream.Read(formData, 0, formData.Length);
        //    formDataStream.Close();

        //    return formData;
        //}
    }


    //public class FileParameter
    //{
    //    public byte[] File { get; set; }
    //    public string FileName { get; set; }
    //    public string ContentType { get; set; }
    //    public FileParameter(byte[] file) : this(file, null) { }
    //    public FileParameter(byte[] file, string filename) : this(file, filename, null) { }
    //    public FileParameter(byte[] file, string filename, string contenttype)
    //    {
    //        File = file;
    //        FileName = filename;
    //        ContentType = contenttype;
    //    }
    //}


}
