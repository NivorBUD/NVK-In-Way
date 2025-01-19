using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Formats.Tga;

namespace NvkInWayWebApi.Application.ImageService.Interfaces
{
    public interface IImageProfile
    {
        TgaImageType ImageType { get; }

        public string Folder { get; }

        public int Width { get; }

        public int Height { get; }

        public int MaxSizeBytes { get; }

        public HashSet<string> AllowedExtensions { get; }
    }
}
