namespace Redbud.BL.DL
{
    public partial class vwMyFollowup
    {
        public string StarRatingGraphic
        {
            get
            {
                string stars = string.Empty;
                if (StarRating.HasValue)
                {
                    for (int x = 0; x < StarRating.Value; x++)
                    {
                        stars += "<i class='fa fa-star'></i> ";
                    }
                    for (int x = 0; x < (5 - StarRating.Value); x++)
                    {
                        stars += "<i class='fa fa-star-o'></i> ";
                    }
                }
                else
                {
                    stars += "N/A";
                }
                return stars.Trim();
            }
        }
    }
}
