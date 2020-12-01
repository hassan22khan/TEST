using IRepository;
using NUnit.Framework;
using System;
using System.IO;
using System.Net;
using TechTalk.SpecFlow;

namespace RestApiTests
{
    [Binding]
    public class DeleteFeatureSteps
    {
        int studentId = 0;
        string ResponseString;
        
        [Given(@"id of student to delete")]
        public void GivenIdOfStudentToDelete()
        {
            studentId = 60; 
        }
        
        [When(@"delete button is clicked request to delete sent")]
        public void WhenDeleteButtonIsClickedRequestToDeleteSent()
        {
            if(studentId != 0)
            {
                Client client = new Client();
                ResponseString = client.Request("api/student/" + studentId, "", "DELETE", false, false); 
            }
        }
        
        [Then(@"confirmation response recieved of delete")]
        public void ThenConfirmationResponseRecievedOfDelete()
        {
            if (ResponseString == "\"Deleted student\"")
                Assert.AreEqual("\"Deleted student\"", ResponseString);
            else
                Assert.AreEqual("\"Already deleted\"",ResponseString);
        } 
    }
}
