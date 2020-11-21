using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Project_SearchService
{
    [TestClass]
    public class SearchEngineBase
    {

        class TestSearchEngine : SearchService.SearchEngineBase
        {
            public override string ProviderName { get; } = "test";
            public override string HitCountRegEx { get; } = "";
            public override string GetUrl(string query) => "www.google.se";
        }


        [TestMethod]
        public void Return_Only_Digits()
        {
            var sut = new TestSearchEngine();

            var hitText = "found 123.45 matching pages";
            var shouldBeValue = 12345L;
            var actualValue = sut.GetNumberFromHitText(hitText);
            Assert.AreEqual(shouldBeValue, actualValue);
        }


    }
}
