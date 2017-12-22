using System;
using System.Collections.Generic;
using System.Text;

namespace KnightBot.Nsfw
{
    class Image
    {
        private string name;
        private string directory;
        private int id;
        private ImageType type;

        public Image(string name, string directory, int id, ImageType type)
        {
            this.name = name;
            this.directory = directory;
            this.id = id;
            this.type = type;
        }

        public string GetName() { return this.name; }
        public string GetDirectory() { return this.directory; }
        public int GetId() { return this.id; }
        public ImageType GetImageType() { return this.type; }
    }
}
