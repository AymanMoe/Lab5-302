using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Lab5_302.Data;
using Lab5_302.Models;
using Azure.Storage.Blobs;

namespace Lab5_302.Pages.AnswerImages
{
    public class DeleteModel : PageModel
    {
        private readonly Lab5_302.Data.AnswerImageDataContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string earthContainerName = "earthimages";
        private readonly string computerContainerName = "computerimages";


        public DeleteModel(Lab5_302.Data.AnswerImageDataContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        [BindProperty]
      public AnswerImage AnswerImage { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.AnswerImages == null)
            {
                return NotFound();
            }

            var answerimage = await _context.AnswerImages.FirstOrDefaultAsync(m => m.AnswerImageId == id);

            if (answerimage == null)
            {
                return NotFound();
            }
            else 
            {
                AnswerImage = answerimage;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.AnswerImages == null)
            {
                return NotFound();
            }
            var answerimage = await _context.AnswerImages.FindAsync(id);

            if (answerimage != null)
            {
                try
                {
                    BlobContainerClient bcc = _blobServiceClient.GetBlobContainerClient(answerimage.Question == Question.Earth ? earthContainerName : computerContainerName);
                    await bcc.DeleteBlobAsync(answerimage.FileName);
                }
                catch
                {
                    // if manually deleted or something just ignore and continue
                }

                AnswerImage = answerimage;
                _context.AnswerImages.Remove(AnswerImage);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
