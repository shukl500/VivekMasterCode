using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieWEBAPI
{
    public class YoutubeTrailerResult
    {
        public String kind { get; set; }
        public string etag { get; set; }
        public string nextPageToken { get; set; }
        public string regionCode { get; set; }
        public PageInfo pageInfo { get; set; }
        public List<SearchItem> items = new List<SearchItem>();

    }

    public class PageInfo
    {
        public string totalResults { get; set; }
        public string resultsPerPage { get; set; }
    }

    public class ID
    {

        public string kind { get; set; }
        public string videoId { get; set; }
    }

    public class SearchItem
    {
        public string kind { get; set; }
        public string etag { get; set; }
        public ID id = new ID();
        public Snippet snippet = new Snippet();
    }

    public class Snippet
    {
        public string publishedAt { get; set; }
        public string channelId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public Thumbnails thumbnails = new Thumbnails();
        public string channelTitle { get; set; }
        public string liveBroadcastContent { get; set; }
    }

    public class Thumbnails
    {
        public Details medium = new Details();
        public Details high = new Details();

    }

    public class Details
    {
        public string url { get; set; }
        public string width { get; set; }
        public string height { get; set; }
    }
}
