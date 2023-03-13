using SharpTwitch.Core.Extensions;

namespace SharpTwitch.Core.Tests.Extensions
{
    public class StringExtensionsTests
    {
        private const string CAMEL_CASE = "userName";
        private const string PASCAL_CASE = "UserName";
        private const string SNAKE_CASE = "user_name";

        [Theory]
        [InlineData(PASCAL_CASE, SNAKE_CASE)]
        [InlineData(CAMEL_CASE, SNAKE_CASE)]
        public void StringExtensions_ToSnakeCase(string key1, string key2)
        {
            var snakeCase = key1.ToSnakeCase();
            Assert.Equal(key2, snakeCase);
        }
    }
}
