using System.Collections.Generic;
using System.Threading.Tasks;
using MongoEntities;
using NUnit.Framework;
using Services;

namespace Test.API.Services
{
    [TestFixture]
    [Culture(Exclude = "en,de")]
    public class FileToMongoServiceTest : MongoSetting
    {
        private FileToMongoService _service;

        private static readonly object[] _sourceMedia =
        {
            new object[] {new Media()
            {
                OriginName = "test"
            }},   //case 1
                new object[] {new Media()
            {
                OriginName = "test123"
            }} //case 2
        };


        public FileToMongoServiceTest()
        {
            _service = new FileToMongoService(_appSettings);
        }

        [Test]
        [Culture("test"),Explicit]
        public void ShouldInsertAsync()
        {
            Assert.DoesNotThrowAsync(() => new FileToMongoService(_appSettings).InsertAsync(new Media()
            {
                OriginName = "test"
            }));
        }

        // [Test]
        [TestCaseSource(nameof(_sourceMedia))]
        [Parallelizable(scope: ParallelScope.All)]
        [Culture("de")]
        public void ShouldInsertMediaListToMongo(Media media)
        {
            Assert.DoesNotThrowAsync(() => new FileToMongoService(_appSettings).InsertMediaListToMongo(new List<Media>(){
               media
            }));
        }

        [Test]
        [Culture("de")]
        public async Task ShouldGetAsync()
        {
            var media = await _service.InsertAsync(new Media()
            {
                OriginName = "test"
            });
            var result = await _service.GetAsync(media.Id);

            Assert.IsInstanceOf<Media>(result);
        }

        [Test]
        [Culture("de")]
        public async Task ShouldUpdateToSoftDelete()
        {
            var media = await _service.InsertAsync(new Media()
            {
                OriginName = "test"
            });

            Assert.DoesNotThrowAsync(async () =>
                await _service.UpdateToSoftDelete(media.Id)
            );
        }

        [Test]
        [Culture("de")]
        public async Task RemoveAsync()
        {
            var media = await _service.InsertAsync(new Media()
            {
                OriginName = "test"
            });

            Assert.DoesNotThrowAsync(async () =>
                await _service.RemoveAsync(media.Id)
            );
        }
    }
}