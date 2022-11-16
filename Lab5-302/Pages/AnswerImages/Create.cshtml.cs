using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Lab5_302.Data;
using Lab5_302.Models;
using Azure.Storage.Blobs;
using Azure;
using Microsoft.CodeAnalysis.Options;
using static System.Net.WebRequestMethods;

namespace Lab5_302.Pages.AnswerImages
{
    public class CreateModel : PageModel
    {
        private readonly Lab5_302.Data.AnswerImageDataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";
        private string[] permittedExtensions = { ".png", ".jpg", "jpeg" };

        public CreateModel(Lab5_302.Data.AnswerImageDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public AnswerImage AnswerImage { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync(IFormFile file)
        {
            var q = AnswerImage.Question.ToString();
            BlobContainerClient containerClient;
            string option;
            if (q == "Earth")
            {
                option = earthContainerName;
            }
            else {
                option = computerContainerName;

            }
            try
            {

                containerClient = await _blobServiceClient.CreateBlobContainerAsync(option);   
                containerClient.SetAccessPolicy(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);
            }
            catch (RequestFailedException)
            {
                containerClient = _blobServiceClient.GetBlobContainerClient(option);
            }
            string randomFileName = Path.GetRandomFileName();

            try
            {
                var blockBlob = containerClient.GetBlobClient(randomFileName);
                if (await blockBlob.ExistsAsync())
                {
                    await blockBlob.DeleteAsync();
                }

                using (var memoryStream = new MemoryStream())
                {  
                    await file.CopyToAsync(memoryStream);
                    memoryStream.Position = 0;

                    await blockBlob.UploadAsync(memoryStream);
                    memoryStream.Close();
                }
            }
            catch (RequestFailedException)
            {
                Page();
            }
            
            AnswerImage.FileName = randomFileName;
            AnswerImage.Url = "https://aymanlab5.blob.core.windows.net/" + option + "/" + randomFileName;
            if (option == "earthimages")
            {
                AnswerImage.Question = Question.Earth;
            }
            else {
                AnswerImage.Question = Question.Computer;
            }

            _context.AnswerImages.Add(AnswerImage);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
