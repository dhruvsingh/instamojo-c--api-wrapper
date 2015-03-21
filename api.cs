#region Usings

using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

#endregion

namespace instamojo
{
    public class Instamojo
    {
        #region Global and Private Variables

        string _appId = string.Empty;
        string _authToken = string.Empty;
        string _endpoint = string.Empty;

        #endregion

        #region Public Properties

        /// <summary>
        /// It is being used to Hold the StatusCode from Request or Post.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }


        #endregion

        public Instamojo(string api_key, string auth_token, string endpoint="https://www.instamojo.com/api/1.1/")
        {
            string api_key = api_key;
            _authToken = auth_token;
            _endpoint = endpoint;

        }

        public string auth(string strUsername, string strPassword)
        {
            try
            {
                //ExceptionUtility.LogRequestResponseData(string.Format("Request - {0}", UpdateServerUrl + strUsername + strPassword));
                var request = (HttpWebRequest)WebRequest.Create(updateServerUrl);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;

                request.ProtocolVersion = HttpVersion.Version10;
                request.Timeout = Timeout.Infinite;
                request.ReadWriteTimeout = Timeout.Infinite;

                // set the method property to POST
                request.Method = "POST";

                // create the post data and 
                string postData = "email=" + strUsername + "&password=" + strPassword;

                int i = postData.Length;
                //convert it to a byte array
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);

                // set the content type of the web request                
                request.ContentType = "application/x-www-form-urlencoded";

                // set the content length
                request.ContentLength = byteArray.Length;

                // get the request stream
                Stream dataStream = request.GetRequestStream();

                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);

                // Close the Stream object.
                dataStream.Close();

                // Get the response.
                WebResponse response = request.GetResponse();

                // get the status code
                StatusCode = ((HttpWebResponse)response).StatusCode;


                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();

                // Open the stream using a StreamReader for easy access.
                if (dataStream != null)
                {
                    var reader = new StreamReader(dataStream);

                    string responseFromServer = "";

                    // Read the content.            
                    responseFromServer = reader.ReadToEnd();

                    // Clean up the streams.
                    reader.Close();
                    dataStream.Close();
                    response.Close();

                    //ExceptionUtility.LogRequestResponseData(string.Format("Response - {0}", responseFromServer));
                    return responseFromServer;
                }
            }
            catch (Exception exceptionMessage)
            {
                throw exceptionMessage;
            }
            return null;
        }
    }
}
