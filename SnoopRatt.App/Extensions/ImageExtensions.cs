using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnoopRatt.App.Extensions
{
    public static class ImageExtensions
    {
        public static async Task<Discord.FileAttachment> GetPngAttachmentAsync(this Image image)
        {
            var stream = new MemoryStream();
            await image.SaveAsPngAsync(stream);

            var attachment = new Discord.FileAttachment(stream, $"{Guid.NewGuid()}.png");
            return attachment;
        }
    }
}
