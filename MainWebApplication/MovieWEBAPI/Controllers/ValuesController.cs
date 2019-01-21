using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MovieWEBAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        ResponseClass responseClass = new ResponseClass();
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "This is default page.", " Please use other action for values" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<IEnumerable<MovieClass>> Get(string id)
        {
            MovieFastCall(id);
            if (responseClass.Search.Count > 0)
                return responseClass.Search.ToArray();
            else
            {
                NotFoundResult();
                return responseClass.Search.ToArray();
            }
            
        }

        /// <summary>
        /// This method is for if IMDB doesn't found any movies. To avoid exception in UI we send blank details
        /// </summary>
        private void NotFoundResult()
        {
            try
            {
                MovieClass defaultmovie = new MovieClass();
                defaultmovie.Poster = "https://media.istockphoto.com/vectors/error-page-or-file-not-found-icon-vector-id924949200";
                defaultmovie.Title = "Moive not found on IMDB";
                defaultmovie.Year = System.DateTime.Now.Year.ToString();
                defaultmovie.videoId = "00000";
                defaultmovie.snippet.channelId = "0000000";
                defaultmovie.snippet.channelTitle = "Not Found due to Limit crossed";
                defaultmovie.snippet.description = "This generally happen after 10K request per day by you.";
                defaultmovie.snippet.publishedAt = System.DateTime.Now.ToString();
                defaultmovie.snippet.thumbnails.high.url = "https://media.istockphoto.com/vectors/error-page-or-file-not-found-icon-vector-id924949200";
                responseClass.Search.Add(defaultmovie);
            }
            catch (Exception)
            {

                throw new Exception ("Error in NotFound Result");
            }
        }

        /// <summary>
        /// Youtube trailer searched in youtube 
        /// </summary>
        /// <param name="responseClass"></param>
        private void YoutubeTrailer(ResponseClass responseClass)
        {
            try
            {
                foreach (MovieClass movie in responseClass.Search)
                {
                    YoutubeTrailerResult youtubeTrailerResult = new YoutubeTrailerResult();
                    //string url = "https://www.googleapis.com/youtube/v3/search?part=snippet&q="+movie.Title+" trailer&type=video&videoCaption=closedCaption&key=AIzaSyB3kNya-9burZ-jaRq2-QxK0oUv9mV3raM";
                    string url = "https://www.googleapis.com/youtube/v3/search?part=snippet&q=" + movie.Title + " trailer&type=video&videoCaption=closedCaption&key=AIzaSyDN6nrlUzcSS2ifsBt6fDOH4GTiK2_Yd0M";
                    HttpClient client = new HttpClient();
                    var response = client.GetAsync(url);
                    response.Wait();
                    if (response.IsCompleted)
                    {
                        var result = response.Result.Content.ReadAsStringAsync().Result;

                        youtubeTrailerResult = JsonConvert.DeserializeObject<YoutubeTrailerResult>(result);
                        if (youtubeTrailerResult.etag != null)
                        {
                            responseClass.Search.Find(item => item.Title == movie.Title).snippet =
                              youtubeTrailerResult.items[0].snippet;
                            responseClass.Search.Find(item => item.Title == movie.Title).videoId =
                              youtubeTrailerResult.items[0].id.videoId;
                        }
                        else
                        {
                            LimitReached(youtubeTrailerResult);
                            responseClass.Search.Find(item => item.Title == movie.Title).snippet =
                             youtubeTrailerResult.items[0].snippet;
                            responseClass.Search.Find(item => item.Title == movie.Title).videoId =
                              youtubeTrailerResult.items[0].id.videoId;
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw new Exception ("Error in Youtube Trailer");
            }
        }

        /// <summary>
        /// The method when you have limit reached.
        /// </summary>
        /// <param name="youtubeTrailerResult"></param>
        private void LimitReached(YoutubeTrailerResult youtubeTrailerResult)
        {
            try
            {
                SearchItem searchItem = new SearchItem();
                searchItem.etag = "000";
                searchItem.kind = "0000";
                searchItem.snippet.channelId = "0000000";
                searchItem.snippet.channelTitle = "Not Found due to Limit crossed";
                searchItem.id.videoId = "00000";
                searchItem.id.kind = "000";
                searchItem.snippet.description = "This generally happen after 10K request per day by you.";
                searchItem.snippet.publishedAt = System.DateTime.Now.ToString();
                searchItem.snippet.thumbnails.high.url = "https://media.istockphoto.com/vectors/error-page-or-file-not-found-icon-vector-id924949200";
                youtubeTrailerResult.items.Add(searchItem);
            }
            catch (Exception)
            {

                throw new Exception("Error in LimitReached function");
            }
        }

        /// <summary>
        /// IMDB api call for the movie search.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private ResponseClass MovieFastCall(string name)
        {
            HttpClient client = new HttpClient();
            var response = client.GetAsync("http://www.omdbapi.com?s=" + name + "&apikey=thewdb");
            response.Wait();
            if( response.IsCompleted)
            {
                var result = response.Result.Content.ReadAsStringAsync().Result;
                responseClass = JsonConvert.DeserializeObject<ResponseClass>(result);
            }

            if (responseClass.Search.Count > 0)
                YoutubeTrailer(responseClass);

            return responseClass;
        }

        
    }
}
