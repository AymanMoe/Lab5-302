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
using Azure;

namespace Lab5_302.Pages.AnswerImages
{
    public class IndexModel : PageModel
    {
        private readonly Lab5_302.Data.AnswerImageDataContext _context;

        public IndexModel(Lab5_302.Data.AnswerImageDataContext context)
        {
            _context = context;
        }

        public IList<AnswerImage> AnswerImage { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.AnswerImages != null)
            {
                AnswerImage = await _context.AnswerImages.ToListAsync();
            }
        }
    }
}
