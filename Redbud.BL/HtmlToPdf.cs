using System.Collections.Generic;

public class HtmlToPdf
{
    /// <summary>
    ///     Redbud API License to use the WkHtmlToPdf service portal
    /// </summary>
    public string License
    {
        get
        {
            return "09d8bf9d-1ce3-4d59-9b99-dcd3c283c563";
        }
    }

    /// <summary>
    ///     Html content
    /// </summary>
    public string Html { get; set; }


    /// <summary>
    ///     Orientation of the PDF generated
    /// </summary>
    public PdfOrientation Orientation { get; set; }

    public Dictionary<string, string> Arguments
    {
        get
        {
            return new Dictionary<string, string>()
            {
                {"orientation", null},
                {"viewport-size", null},
                {"margin-bottom", null},
                {"margin-right", null},
                {"javascript-delay", null},
                {"margin-top", null},
                {"margin-left", null},
                {"dpi", null},
                {"print-media-type", null}
            };
        }
    }
}
public enum PdfOrientation
{
    Portrait = 1,
    Landscape = 2
}