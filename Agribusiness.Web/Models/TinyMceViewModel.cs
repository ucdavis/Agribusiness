namespace Agribusiness.Web.Models
{
    public class TinyMceViewModel
    {
        public TinyMceViewModel(string inputIds = null, string height = "300", string width = "100%", bool enableTokenInsert = false)
        {
            InputIds = inputIds;
            Height = height;
            Width = width;

            EnableTokenInsert = enableTokenInsert;
        }

        public string InputIds { get; set; }

        public string Height { get; set; }
        public string Width { get; set; }

        public bool EnableTokenInsert { get; set; }
    }
}