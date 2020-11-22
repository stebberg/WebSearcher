using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace UnitTests.Project_SearchService
{
    [TestClass]
    public class FormattingHelper
    {

        [TestMethod]
        public void TestUrlEncoding_ShouldReturn_Formatted_String1()
        {
            string actualValue = "123 åäö abc";
            string shouldBe = "123+%c3%a5%c3%a4%c3%b6+abc";

            string formattedValue = SearchService.FormattingHelper.UrlEncode(actualValue);
            Assert.AreEqual(shouldBe, formattedValue);
        }

        [TestMethod]
        public void TestUrlEncoding_ShouldReturn_Formatted_String2()
        {
            string actualValue = null;
            string shouldBe = null;

            string formattedValue = SearchService.FormattingHelper.UrlEncode(actualValue);
            Assert.AreEqual(shouldBe, formattedValue);
        }



        [TestMethod]
        public void TestNumberFormatter_ShouldReturn_Formatted_String_1()
        {
            testFormattedNum(12_450_000, "12,5M");
        }
        [TestMethod]
        public void TestNumberFormatter_ShouldReturn_Formatted_String_2()
        {
            testFormattedNum(12_449_000, "12,4M");
        }
        [TestMethod]
        public void TestNumberFormatter_ShouldReturn_Formatted_String_3()
        {
            testFormattedNum(12, "12,0");
        }
        [TestMethod]
        public void TestNumberFormatter_ShouldReturn_Formatted_String_4()
        {
            testFormattedNum(7_500, "7,5k");
        }
        [TestMethod]
        public void TestNumberFormatter_ShouldReturn_Zero()
        {
            testFormattedNum(0, "0,0");
        }
        [TestMethod]
        public void TestNumberFormatter_ShouldTrowError()
        {
            var ex = Assert.ThrowsException<FormatException>(() => SearchService.FormattingHelper.NumAsString(-1));
            Assert.AreEqual("Can't handle negative numbers", ex.Message);
        }

        void testFormattedNum(long number, string shouldBe)
        {
            string formattedValue = SearchService.FormattingHelper.NumAsString(number);
            Assert.AreEqual(shouldBe, formattedValue);
        }

    }
}
            
            