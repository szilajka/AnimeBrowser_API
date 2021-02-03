using AnimeBrowser.Data.Entities;
using JsonApiDotNetCore.Configuration;
using JsonApiDotNetCore.Controllers;
using JsonApiDotNetCore.Resources;
using JsonApiDotNetCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AnimeBrowser.API.Controllers
{
    public class AnimeInfoController : JsonApiController<AnimeInfo, long>
    {
        public AnimeInfoController(IJsonApiOptions options,
            ILoggerFactory loggerFactory,
            IResourceService<AnimeInfo, long> resourceService) : base(options, loggerFactory, resourceService)
        {
        }

        [HttpPost]
        [Authorize(Policy = "AnimeInfoAdmin")]
        public override Task<IActionResult> PostAsync([FromBody] AnimeInfo resource, CancellationToken cancellationToken)
        {
            return base.PostAsync(resource, cancellationToken);
        }

        [HttpPost("{id}/relationships/{relationshipName}")]
        [Authorize(Policy = "AnimeInfoAdmin")]
        public override Task<IActionResult> PostRelationshipAsync(long id, string relationshipName, [FromBody] ISet<IIdentifiable> secondaryResourceIds, CancellationToken cancellationToken)
        {
            return base.PostRelationshipAsync(id, relationshipName, secondaryResourceIds, cancellationToken);
        }

        [HttpPatch("{id}")]
        [Authorize(Policy = "AnimeInfoAdmin")]
        public override Task<IActionResult> PatchAsync(long id, [FromBody] AnimeInfo resource, CancellationToken cancellationToken)
        {
            return base.PatchAsync(id, resource, cancellationToken);
        }

        [HttpPatch("{id}/relationships/{relationshipName}")]
        [Authorize(Policy = "AnimeInfoAdmin")]
        public override Task<IActionResult> PatchRelationshipAsync(long id, string relationshipName, [FromBody] object secondaryResourceIds, CancellationToken cancellationToken)
        {
            return base.PatchRelationshipAsync(id, relationshipName, secondaryResourceIds, cancellationToken);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AnimeInfoAdmin")]
        public override Task<IActionResult> DeleteAsync(long id, CancellationToken cancellationToken)
        {
            return base.DeleteAsync(id, cancellationToken);
        }

        [HttpDelete("{id}/relationships/{relationshipName}")]
        [Authorize(Policy = "AnimeInfoAdmin")]
        public override Task<IActionResult> DeleteRelationshipAsync(long id, string relationshipName, [FromBody] ISet<IIdentifiable> secondaryResourceIds, CancellationToken cancellationToken)
        {
            return base.DeleteRelationshipAsync(id, relationshipName, secondaryResourceIds, cancellationToken);
        }

        [HttpGet]
        public override Task<IActionResult> GetAsync(CancellationToken cancellationToken)
        {
            return base.GetAsync(cancellationToken);
        }

        [HttpGet("{id}")]
        public override Task<IActionResult> GetAsync(long id, CancellationToken cancellationToken)
        {
            return base.GetAsync(id, cancellationToken);
        }

        [HttpGet("{id}/relationships/{relationshipName}")]
        public override Task<IActionResult> GetRelationshipAsync(long id, string relationshipName, CancellationToken cancellationToken)
        {
            return base.GetRelationshipAsync(id, relationshipName, cancellationToken);
        }
    }
}
