using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using TechTalk.SpecFlow;
using ViewModels;
using Utf8Json;
using Newtonsoft.Json;
using Models;
using NUnit.Framework;

namespace RestApiTests
{
    [Binding]
    public class GetStudentFeatureSteps
    {
        string url;
        List<StudentViewModel> studentViewModels;
        string ResponseString;
        [Given(@"url for api")]
        public void GivenUrlForApi()
        {
            url = "https://localhost:44380/api/student";
        }
        
        [When(@"api called")]
        public void WhenApiCalled()
        {
            StudentApiFeatureSteps steps = new StudentApiFeatureSteps();
            steps.GivenUsernameIs("Kamran");
            steps.GivenPasswordIs("12345");
            steps.WhenLoginRequestIsCalled();
            DataPlusToken dataPlusToken = steps.ThenRecieveAccessToken();
            Client client = new Client();
            ResponseString = client.Request("api/student/" + dataPlusToken.UserId, "", "GET", false, false);
            }
        
        [Then(@"list of students returned")]
        public void ThenListOfStudentsReturned()
        {
            List<StudentViewModel> studentViewModels = JsonConvert.DeserializeObject<List<StudentViewModel>>(ResponseString);
            Assert.AreEqual(studentViewModels.Count > 0,true);
        }
    }
}
