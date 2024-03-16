using System.Net;
using Models;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using Services;
using Services.Exceptions;
using Xunit;
using Assert = Xunit.Assert;

namespace Tests
{
    public class GitServiceTests
    {
        private readonly GitService _gitService;
        private readonly Mock<IHttpClientFactory> _clientFactoryMock = new Mock<IHttpClientFactory>();
        private readonly Mock<HttpMessageHandler> _handlerMock = new Mock<HttpMessageHandler>();

        public GitServiceTests()
        {
            _handlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(req =>
                    req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("/tags")),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new List<Tag> { new Tag { Name = "v2.0" }, new Tag { Name = "v1.0" } }))
            });

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains("/compare/")), 
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(new GitHubApiResponse
                    {
                        Commits = new List<CommitInfo>
                        {
                        new CommitInfo { Commit = new Commit { Message = "Initial commit" } },
                        new CommitInfo { Commit = new Commit { Message = "Add new feature" } }
                        }
                    }))
                });

            var client = new HttpClient(_handlerMock.Object) { BaseAddress = new Uri("http://test.com"), };
            _clientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(client);

            _gitService = new GitService(_clientFactoryMock.Object);
        }


        [Fact]
        public async Task GetCommitMessages_ReturnsCommitMessages()
        {

            string repoUrl = "https://github.com/owner/repo";
            string newVersionTag = "v2.0";
            string previousVersionTag = "v1.0";

            var tagsResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new List<Tag> { new Tag { Name = "v2.0" }, new Tag { Name = "v1.0" } }))
            };

            var commitsResponseMessage = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(JsonConvert.SerializeObject(new GitHubApiResponse
                {
                    Commits = new List<CommitInfo>
                {
                    new CommitInfo { Commit = new Commit { Message = "Initial commit" } },
                    new CommitInfo { Commit = new Commit { Message = "Add new feature" } }
                }
                }))
            };

            _handlerMock.Protected().SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsolutePath.Contains("/tags")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(tagsResponseMessage)
                .ReturnsAsync(commitsResponseMessage);

            var result = await _gitService.GetCommitMessages(repoUrl, newVersionTag, previousVersionTag);

            Assert.NotNull(result);
            Assert.Contains("Initial commit", result.commitMessages);
            Assert.Contains("Add new feature", result.commitMessages);
            var indexOfInitialCommit = result.commitMessages.IndexOf("Initial commit");
            var indexOfNewFeatureCommit = result.commitMessages.IndexOf("Add new feature");
            Assert.True(indexOfInitialCommit < indexOfNewFeatureCommit, "Commits should be in the expected order.");
        }

        [Fact]
        public async Task GetCommitMessages_ThrowsGitException_OnFailure()
        {
            string repoUrl = "https://github.com/owner/repo";
            string newVersionTag = "v2.0";
            string previousVersionTag = "v1.0";

            var tags = new List<Tag> { new Tag { Name = "v2.0" }, new Tag { Name = "v1.0" } };

            _handlerMock.Protected()
                .SetupSequence<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.AbsoluteUri.Contains(GitService.GitHubUrl)),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonConvert.SerializeObject(tags)) 
                })
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest, 
                });

            await Assert.ThrowsAsync<GitException>(async () => await _gitService.GetCommitMessages(repoUrl, newVersionTag, previousVersionTag));
        }

    }
}
