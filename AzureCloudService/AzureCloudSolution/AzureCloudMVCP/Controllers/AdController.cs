using AdsCommonLibrary;
using Microsoft.Azure;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Queue;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using System;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AzureCloudMVCP.Controllers
{
    public class AdController : Controller
    {
        private DatabaseContext db = new DatabaseContext();
        private CloudQueue imagesQueue;
        private static CloudBlobContainer imagesBlobContainer;

        public AdController()
        {

        }
        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            InitializeStorage();
        }
        private void InitializeStorage()
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString"));
                var blobClient = storageAccount.CreateCloudBlobClient();
                blobClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

                imagesBlobContainer = blobClient.GetContainerReference("images");

                CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                queueClient.DefaultRequestOptions.RetryPolicy = new LinearRetry(TimeSpan.FromSeconds(3), 3);

                imagesQueue = queueClient.GetQueueReference("images");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET: Ad
        public async Task<ActionResult> Index(int? category)
        {
            var res = db.Ads.AsQueryable();
            if (category != null)
            {
                res = db.Ads.Where(a => a.Category == (Category)category);
            }
            return View(await res.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Bind(Include = "Title,Price,Description,Category,Phone")]Ad ad, HttpPostedFileBase imageFile)
        {
            CloudBlockBlob imageBlob = null;
            // A production app would implement more robust input validation.
            // For example, validate that the image file size is not too large.
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength != 0)
                {
                    imageBlob = await UploadAndSaveBlobAsync(imageFile);
                    ad.ImageURL = imageBlob.Uri.ToString();
                }
                ad.PostedDate = DateTime.Now;
                db.Ads.Add(ad);
                await db.SaveChangesAsync();
                Trace.TraceInformation("Created AdId {0} in database", ad.AdId);

                if (imageBlob != null)
                {
                    var queueMessage = new CloudQueueMessage(ad.AdId.ToString());
                    await imagesQueue.AddMessageAsync(queueMessage);
                    Trace.TraceInformation("Created queue message for AdId {0}", ad.AdId);
                }
                return RedirectToAction("Index");
            }

            return View(ad);
        }

        private async Task<CloudBlockBlob> UploadAndSaveBlobAsync(HttpPostedFileBase imageFile)
        {
            Trace.TraceInformation($"Uploading image file " + imageFile.FileName);

            string blobName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            // Retrieve reference to a blob.
            CloudBlockBlob imageBlob = imagesBlobContainer.GetBlockBlobReference(blobName);
            // Create the blob by uploading a local file.
            using (var fileStream = imageFile.InputStream)
            {
                await imageBlob.UploadFromStreamAsync(fileStream);
            }

            Trace.TraceInformation("Uploaded image file to {0}", imageBlob.Uri.ToString());

            return imageBlob;
        }
    }
}