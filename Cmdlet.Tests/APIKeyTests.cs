using OpenAICmdlet;
using Moq;
namespace Cmdlet.Tests;

[TestClass]
public class APIKeyTest
{
    private readonly string localAPIKeyPath = System.IO.Path.Join(
          Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
          "OpenAICmdlet/API.key");

    [TestCleanup]
    public void ClearLocalFiles()
    {
        if (File.Exists(localAPIKeyPath))
        {
            File.Delete(localAPIKeyPath);
            if (Directory.GetParent(localAPIKeyPath)?.FullName is { } parentDir)
                Directory.Delete(parentDir);
        }
    }

    [TestMethod]
    public void T1_CanSetNewAPIKey()
    {
        var mock = new Mock<MockSetAPIKeyCommand>() { CallBase = true };
        mock.Setup(x => x.ReadConsoleLine(It.IsAny<string>())).Returns("abcd1234").Verifiable();
        mock.Setup(x => x.ShouldProcess(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        var setKeyCmd = mock.Object;
        setKeyCmd.TestEndProcessing();
        Assert.IsTrue(File.Exists(localAPIKeyPath));
        mock.Verify();
    }


    public class MockSetAPIKeyCommand : SetOpenAIAPIKeyCommand
    {
        public void TestEndProcessing()
        {
            base.EndProcessing();
        }
    }
}
