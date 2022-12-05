using StringFormatter;

namespace TestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        [ExpectedException(typeof(Exception), "Brackets excepion")]
        public void NoClosedBracketException()
        {
            var user = new User("����", "������");
            string result;

            result = StringFormatter.StringFormatter.Shared.Format(
                "������, {FirstName} {LastName!", user);

        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Brackets excepion")]
        public void UnbalancedBracketsException()
        {
            var user = new User("����", "������");
            string result;

            result = StringFormatter.StringFormatter.Shared.Format(
                "������, {{FirstName} {LastName}}", user);
        }

        [TestMethod]
        [ExpectedException(typeof(Exception), "Brackets excepion")]
        public void NoOpenedBracketsException()
        {
            var user = new User("����", "������");
            string result;

            result = StringFormatter.StringFormatter.Shared.Format(
                "������, FirstName}}} LastName!}", user);
        }


        [TestMethod]
        [ExpectedException(typeof(Exception), "Brackets excepion")]
        public void WrongBracketsFormatException()
        {
            var user = new User("����", "������");
            string result;

            result = StringFormatter.StringFormatter.Shared.Format(
                  "{i {{i } i }}", user);

        }
        [TestMethod]
        public void FormatTest1()
        {
            var user = new User("����", "������");
            string result;

            result = StringFormatter.StringFormatter.Shared.Format(
                "������, {FirstName} {LastName}!", user);

            Assert.AreEqual("������, ���� ������!", result);
        }

        [TestMethod]
        public void FormatTest2()
        {
            var user = new User("����", "������");
            string result;

            result = StringFormatter.StringFormatter.Shared.Format(
                  "{{FirstName}} ������������� � {FirstName}", user);

            Assert.AreEqual("{FirstName} ������������� � ����", result);
        }

        [TestMethod]
        public void FormatTest3()
        {
            var user = new User("����", "������");
            string result;

            result = StringFormatter.StringFormatter.Shared.Format(
                  "FirstName}} {{LastName", user);

            Assert.AreEqual("FirstName} {LastName", result);
        }

        [TestMethod]
        public void FormatTest4()
        {
            var user = new User("����", "������");
            string result;

            result = StringFormatter.StringFormatter.Shared.Format(
                  "{FirstName} {LastName} i:{i}", user);

            Assert.AreEqual("���� ������ i:1", result);
        }

    }
}