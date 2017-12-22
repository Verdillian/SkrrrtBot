using System;
using System.Collections.Generic;
using System.Text;

namespace KnightBot.Nsfw
{
    class Images
    {
        private static Image placeholderImage;

        public void Initialize()
        {
            placeholderImage = new Image("placeholder", "", 0, ImageType.placeholder);
        }
    }
}
