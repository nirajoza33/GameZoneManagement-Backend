//namespace GameZoneManagementApi.DTOs
//{
//    public class ReviewDto
//    {
//        public int GameId { get; set; }
//        public int UserId { get; set; }
//        public int Rating { get; set; }
//        public string ReviewText { get; set; }
//        public string Title { get; set; }

//    }

//    public class ReviewResponseDto
//    {
//        public int ReviewId { get; set; }
//        public int GameId { get; set; }
//        public string GameTitle { get; set; }
//        public int UserId { get; set; }
//        public string UserName { get; set; }
//        public string UserImage { get; set; }
//        public int Rating { get; set; }
//        public string Title { get; set; }
//        public string ReviewText { get; set; }
//        public DateTime CreatedDate { get; set; }
//        public int Likes { get; set; }
//        public int Replies { get; set; }
//        public string Sentiment { get; set; }
//        public bool Verified { get; set; } = true;
//    }

//    // For liking a review
//    public class ReviewLikeDto
//    {
//        public int ReviewId { get; set; }
//    }
//}




namespace GameZoneManagementApi.DTOs
{
    public class ReviewDto
    {
        public int GameId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string ReviewText { get; set; }
        public string Title { get; set; }
        // NEW: Support for multiple images
        public List<string> ImageUrls { get; set; } = new List<string>();
    }

    public class ReviewResponseDto
    {
        public int ReviewId { get; set; }
        public int GameId { get; set; }
        public string GameTitle { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public int Rating { get; set; }
        public string Title { get; set; }
        public string ReviewText { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Likes { get; set; }
        public int Replies { get; set; }
        public string Sentiment { get; set; }
        public bool Verified { get; set; } = true;
        // NEW: Support for multiple images
        public List<string> ImageUrls { get; set; } = new List<string>();
    }

    // For liking a review
    public class ReviewLikeDto
    {
        public int ReviewId { get; set; }
        public int UserId { get; set; }
    }


    public class ReviewReplyResponseDto
    {
        public int ReplyId { get; set; }
        public int ReviewId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UserImage { get; set; }
        public string ReplyText { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class DeleteReplyDto
    {
        public int UserId { get; set; }
    }
}