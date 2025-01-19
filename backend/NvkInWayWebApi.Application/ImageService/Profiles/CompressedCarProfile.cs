using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;
using NvkInWayWebApi.Application.ImageService.Interfaces;
using SixLabors.ImageSharp.Formats.Tga;

namespace NvkInWayWebApi.Application.ImageService.Profiles
{
    public class CompressedCarProfile : IImageProfile
    {
        private const int mb = 1048576;

        public TgaImageType ImageType => TgaImageType.RleColorMapped;

        public string Folder => "driver_cars";

        public int Width => 400;

        public int Height => 400;

        public int MaxSizeBytes => 10 * mb;

        public HashSet<string> AllowedExtensions => [".bmp", ".gif", ".jpeg", ".jpg", ".pbm", ".png", ".tiff", ".tga", ".webp"];
    }
}
