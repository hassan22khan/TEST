﻿using Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;
using TechTalk.SpecFlow;
using ViewModels;

namespace RestApiTests
{
    [Binding]
    public class EditFeatureSteps
    {
        StudentViewModel studentViewModel;
        HttpWebResponse response;
        string ResponseString;
        [Given(@"student object to edit")]
        public void GivenStudentObjectToEdit()
        {
            StudentApiFeatureSteps steps = new StudentApiFeatureSteps();
            steps.GivenUsernameIs("Kamran");
            steps.GivenPasswordIs("12345");
            steps.WhenLoginRequestIsCalled();
            DataPlusToken dataPlusToken = steps.ThenRecieveAccessToken();
            if(dataPlusToken != null)
            {
                studentViewModel = new StudentViewModel
                {
                    Student = new Student
                    {
                        Id= 63,
                        Name = "Shadaab Khan",
                        Email = "shadaab123@yahoo.com",
                        Dob = new DateTime(2010, 6, 12),
                        Phone = "03160578748",
                        Password = "Shadaab1",
                        ConfirmPassword = "Shadaab1",
                        UserId = Convert.ToInt32(dataPlusToken.UserId)
                    },
                    Courses = new List<string>() { "3", "4", "5" },
                    ImageFile = new string[1] { "iVBORw0KGgoAAAANSUhEUgAAANQAAAB4CAYAAACKANyNAAAACXBIWXMAAAsTAAALEwEAmpwYAAA7jGlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4KPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxMzIgNzkuMTU5Mjg0LCAyMDE2LzA0LzE5LTEzOjEzOjQwICAgICAgICAiPgogICA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPgogICAgICA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIgogICAgICAgICAgICB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIKICAgICAgICAgICAgeG1sbnM6c3RSZWY9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZVJlZiMiCiAgICAgICAgICAgIHhtbG5zOnN0RXZ0PSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VFdmVudCMiCiAgICAgICAgICAgIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIKICAgICAgICAgICAgeG1sbnM6ZGM9Imh0dHA6Ly9wdXJsLm9yZy9kYy9lbGVtZW50cy8xLjEvIgogICAgICAgICAgICB4bWxuczpwaG90b3Nob3A9Imh0dHA6Ly9ucy5hZG9iZS5jb20vcGhvdG9zaG9wLzEuMC8iCiAgICAgICAgICAgIHhtbG5zOnRpZmY9Imh0dHA6Ly9ucy5hZG9iZS5jb20vdGlmZi8xLjAvIgogICAgICAgICAgICB4bWxuczpleGlmPSJodHRwOi8vbnMuYWRvYmUuY29tL2V4aWYvMS4wLyI+CiAgICAgICAgIDx4bXBNTTpPcmlnaW5hbERvY3VtZW50SUQ+eG1wLmRpZDplYTIxZDNiMi03OTI3LTRhNmQtODA2Zi02M2FmY2I5NTJmMzk8L3htcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkRvY3VtZW50SUQ+YWRvYmU6ZG9jaWQ6cGhvdG9zaG9wOjY0NWM4ZjFlLWY1ZDItMTFlNy1hMGQzLWU3MzlkZGYxYjU1NDwveG1wTU06RG9jdW1lbnRJRD4KICAgICAgICAgPHhtcE1NOkluc3RhbmNlSUQ+eG1wLmlpZDoyYTk5NzQxYi1lNDNiLTEyNDktYmM0OC04OWM0M2JlNzRmZGY8L3htcE1NOkluc3RhbmNlSUQ+CiAgICAgICAgIDx4bXBNTTpEZXJpdmVkRnJvbSByZGY6cGFyc2VUeXBlPSJSZXNvdXJjZSI+CiAgICAgICAgICAgIDxzdFJlZjppbnN0YW5jZUlEPnhtcC5paWQ6NWJiNjI4MmMtNzA0Yy00NDA3LWE0ZjctM2M1NzgwZWYyNzIyPC9zdFJlZjppbnN0YW5jZUlEPgogICAgICAgICAgICA8c3RSZWY6ZG9jdW1lbnRJRD5hZG9iZTpkb2NpZDpwaG90b3Nob3A6NDE5ZGJkYTktNTk1My0xMTc5LThkZmYtYTBkYWQ0ZWQwN2MzPC9zdFJlZjpkb2N1bWVudElEPgogICAgICAgICA8L3htcE1NOkRlcml2ZWRGcm9tPgogICAgICAgICA8eG1wTU06SGlzdG9yeT4KICAgICAgICAgICAgPHJkZjpTZXE+CiAgICAgICAgICAgICAgIDxyZGY6bGkgcmRmOnBhcnNlVHlwZT0iUmVzb3VyY2UiPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6YWN0aW9uPnNhdmVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6ZmUzZTBkYTItMGNiNy1jMTQ4LTg3NzUtNzgxZmJhMWE3MjRiPC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE4LTAxLTEwVDExOjQ5OjIzKzA1OjAwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNS41IChXaW5kb3dzKTwvc3RFdnQ6c29mdHdhcmVBZ2VudD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmNoYW5nZWQ+Lzwvc3RFdnQ6Y2hhbmdlZD4KICAgICAgICAgICAgICAgPC9yZGY6bGk+CiAgICAgICAgICAgICAgIDxyZGY6bGkgcmRmOnBhcnNlVHlwZT0iUmVzb3VyY2UiPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6YWN0aW9uPnNhdmVkPC9zdEV2dDphY3Rpb24+CiAgICAgICAgICAgICAgICAgIDxzdEV2dDppbnN0YW5jZUlEPnhtcC5paWQ6MmE5OTc0MWItZTQzYi0xMjQ5LWJjNDgtODljNDNiZTc0ZmRmPC9zdEV2dDppbnN0YW5jZUlEPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6d2hlbj4yMDE4LTAxLTEwVDExOjQ5OjIzKzA1OjAwPC9zdEV2dDp3aGVuPgogICAgICAgICAgICAgICAgICA8c3RFdnQ6c29mdHdhcmVBZ2VudD5BZG9iZSBQaG90b3Nob3AgQ0MgMjAxNS41IChXaW5kb3dzKTwvc3RFdnQ6c29mdHdhcmVBZ2VudD4KICAgICAgICAgICAgICAgICAgPHN0RXZ0OmNoYW5nZWQ+Lzwvc3RFdnQ6Y2hhbmdlZD4KICAgICAgICAgICAgICAgPC9yZGY6bGk+CiAgICAgICAgICAgIDwvcmRmOlNlcT4KICAgICAgICAgPC94bXBNTTpIaXN0b3J5PgogICAgICAgICA8eG1wOkNyZWF0b3JUb29sPkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE1LjUgKFdpbmRvd3MpPC94bXA6Q3JlYXRvclRvb2w+CiAgICAgICAgIDx4bXA6Q3JlYXRlRGF0ZT4yMDE4LTAxLTEwVDExOjM5OjU3KzA1OjAwPC94bXA6Q3JlYXRlRGF0ZT4KICAgICAgICAgPHhtcDpNb2RpZnlEYXRlPjIwMTgtMDEtMTBUMTE6NDk6MjMrMDU6MDA8L3htcDpNb2RpZnlEYXRlPgogICAgICAgICA8eG1wOk1ldGFkYXRhRGF0ZT4yMDE4LTAxLTEwVDExOjQ5OjIzKzA1OjAwPC94bXA6TWV0YWRhdGFEYXRlPgogICAgICAgICA8ZGM6Zm9ybWF0PmltYWdlL3BuZzwvZGM6Zm9ybWF0PgogICAgICAgICA8cGhvdG9zaG9wOkNvbG9yTW9kZT4zPC9waG90b3Nob3A6Q29sb3JNb2RlPgogICAgICAgICA8dGlmZjpPcmllbnRhdGlvbj4xPC90aWZmOk9yaWVudGF0aW9uPgogICAgICAgICA8dGlmZjpYUmVzb2x1dGlvbj43MjAwMDAvMTAwMDA8L3RpZmY6WFJlc29sdXRpb24+CiAgICAgICAgIDx0aWZmOllSZXNvbHV0aW9uPjcyMDAwMC8xMDAwMDwvdGlmZjpZUmVzb2x1dGlvbj4KICAgICAgICAgPHRpZmY6UmVzb2x1dGlvblVuaXQ+MjwvdGlmZjpSZXNvbHV0aW9uVW5pdD4KICAgICAgICAgPGV4aWY6Q29sb3JTcGFjZT42NTUzNTwvZXhpZjpDb2xvclNwYWNlPgogICAgICAgICA8ZXhpZjpQaXhlbFhEaW1lbnNpb24+MjEyPC9leGlmOlBpeGVsWERpbWVuc2lvbj4KICAgICAgICAgPGV4aWY6UGl4ZWxZRGltZW5zaW9uPjEyMDwvZXhpZjpQaXhlbFlEaW1lbnNpb24+CiAgICAgIDwvcmRmOkRlc2NyaXB0aW9uPgogICA8L3JkZjpSREY+CjwveDp4bXBtZXRhPgogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgIAogICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgCiAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAgICAKICAgICAgICAgICAgICAgICAgICAgICAgICAgIAo8P3hwYWNrZXQgZW5k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" }
                };
            }
        }
        
        [When(@"edit form is submitted edit request is sent")]
        public void WhenEditFormIsSubmittedEditRequestIsSent()
        {
            JavaScriptSerializer jss = new JavaScriptSerializer();
            var myContent = jss.Serialize(studentViewModel);
            Client client = new Client();
           ResponseString = client.Request("api/student", myContent, "PUT", false, false);
            
        }
        
        [Then(@"result of request is recieved")]
        public void ThenResultOfRequestIsRecieved()
        {
            Assert.AreEqual("\"Updated the student\"",ResponseString); 
        }
    }
}
