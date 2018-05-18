using System;
using System.Collections.Generic;
using System.Text;

namespace SkrrrtBot.Nsfw
{
    class NsfwImage
    {
        private string name;
        private string directory;
        private int id;
        private ImageType type;

        public NsfwImage(string name, string directory, ImageType type)
        {
            this.name = name;
            this.directory = directory;
            this.type = type;
        }

        public string GetName() { return this.name; }
        public string GetDirectory() { return this.directory; }
        public int GetId() { return this.id; }
        public ImageType GetImageType() { return this.type; }
    }
}
