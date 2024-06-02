using Spark.Library.Extensions;


namespace BlazorSpark.Tests.Extensions
{
	[TestClass]
	public class ExtensionTests
	{
		[TestMethod]
		public void ShouldSlugifyString()
		{
			var str = "THis IS somE * String";

			var slug = str.ToSlug();

			Assert.AreEqual("this-is-some-string", slug);
        }

        [TestMethod]
        public void ShouldClampString()
        {
            var str = "This is a string";

            var clampedStr = str.Clamp(4);

            Assert.AreEqual("This...", clampedStr);
        }
    }
}
