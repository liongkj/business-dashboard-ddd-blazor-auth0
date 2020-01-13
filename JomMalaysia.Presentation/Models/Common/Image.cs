namespace JomMalaysia.Presentation.Models.Common
{
    public class Image
    {
        public Image(string url)
        {
            Url = url;
        }

        public Image()
        {
            
        }
        public string Url { get; set; } =
            "https://res.cloudinary.com/jomn9-com/image/upload/c_scale,w_200/v1575257964/placeholder_xtcpy8.jpg";
    }
}