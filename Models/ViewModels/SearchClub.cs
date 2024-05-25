namespace FinalPro.Models.ViewModels
{
    public class SearchClub
    {
        public String ClubName { get; set; }

        public List<Club>? Clubs { get; set; } = new List<Club>();
    }
}
