using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DataManager.Core.Exceptions;
using DataManager.Core.Exceptions.Json;

namespace DataManager.Core.WebAPI
{
    /// <summary>
    /// Represents a relative API route that is part of the Web-API.
    /// </summary>
    readonly struct Route
    {
        /// <summary>
        /// A relative route to an endpoint.
        /// </summary>
        /// <remarks>
        /// The route must start with a forward slash <c>('/')</c>.
        /// </remarks>
        public required string RouteString { get; init; }

        /// <summary>
        /// An array containing <see cref="HttpMethod"/>-values representing the
        /// supported HTTP-methods.
        /// </summary>
        public required HttpMethod[] SupportedMethods { get; init; }

        /// <summary>
        /// A Dictionary containing a <see cref="JsonContract.Contract"/>
        /// describing the requirements of which the request JSON-body needs to
        /// fulfill to be accepted by the servers. If no request-body is required, the
        /// <see cref="JsonContract.Contract"/> will be <see langword="null"/>.
        /// </summary>
        public required Dictionary<HttpMethod, JsonContract.Contract?> JsonContracts { get; init; }

        public required Dictionary<HttpMethod, HeaderContract.Contract?> HeaderContracts { get; init; }

        public required Dictionary<HttpMethod, Dictionary<HttpStatusCode, Func<FailureResponsePayload, Exception>>?> PerResponseExceptions { get; init; }
    }

    /// <summary>
    /// A static collection of all relevant API-routes on the server.
    /// </summary>
    static class Routes
    {
        public static readonly Route SessionBegin = new Route
        {
            RouteString = "/api/auth/session/begin",
            SupportedMethods = [HttpMethod.Post],
            JsonContracts = new Dictionary<HttpMethod, JsonContract.Contract?>
            {
                [HttpMethod.Post] = new JsonContract.Contract
                {
                    FieldRequirements = [new JsonContract.FieldRequirement {
                        FieldName = "master",
                        FieldType = JsonContract.FieldType.String,
                        OptionalGroup = 1
                    }],
                    FieldRequirementGroups = [new JsonContract.FieldRequirementGroup {
                        FieldRequirements = [new JsonContract.FieldRequirement {
                            FieldName = "username",
                            FieldType = JsonContract.FieldType.String
                        },
                        new JsonContract.FieldRequirement {
                            FieldName = "password",
                            FieldType = JsonContract.FieldType.String
                        }],
                        OptionalGroup = 1
                    }]
                }
            },
            HeaderContracts = new Dictionary<HttpMethod, HeaderContract.Contract?>
            {
                [HttpMethod.Post] = null
            },
            PerResponseExceptions = new Dictionary<HttpMethod, Dictionary<HttpStatusCode, Func<FailureResponsePayload, Exception>>?>
            {
                [HttpMethod.Post] = new Dictionary<HttpStatusCode, Func<FailureResponsePayload, Exception>>
                {
                    [HttpStatusCode.BadRequest] = rp => new BadRequestException() { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.Unauthorized] = rp => new AuthenticationException() { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.BadGateway] = rp => new ServerException() { ServerExceptionMessage = rp.ServerExceptionMessage }
                }
            }
        };

        public static readonly Route SessionEnd = new Route
        {
            RouteString = "/api/auth/session/end",
            SupportedMethods = [HttpMethod.Post],
            JsonContracts = new Dictionary<HttpMethod, JsonContract.Contract?>
            {
                [HttpMethod.Post] = new JsonContract.Contract
                {
                    FieldRequirements = [new JsonContract.FieldRequirement {
                        FieldName = "username",
                        FieldType = JsonContract.FieldType.String,
                        IsRequired = false
                    }],
                    FieldRequirementGroups = []
                }
            },
            HeaderContracts = new Dictionary<HttpMethod, HeaderContract.Contract?>
            {
                [HttpMethod.Post] = null
            },
            PerResponseExceptions = new Dictionary<HttpMethod, Dictionary<HttpStatusCode, Func<FailureResponsePayload, Exception>>?>
            {
                [HttpMethod.Post] = null
            }
        };

        public static readonly Route Entity = new Route
        {
            RouteString = "/api/auth/entity",
            SupportedMethods = [HttpMethod.Get, HttpMethod.Post, HttpMethod.Patch],
            JsonContracts = new Dictionary<HttpMethod, JsonContract.Contract?>
            {
                [HttpMethod.Get] = null,
                [HttpMethod.Post] = new JsonContract.Contract
                {
                    FieldRequirements = [new JsonContract.FieldRequirement {
                        FieldName = "username",
                        FieldType = JsonContract.FieldType.String
                    },
                    new JsonContract.FieldRequirement {
                        FieldName = "password",
                        FieldType = JsonContract.FieldType.String,
                        IsRequired = false
                    }],
                    FieldRequirementGroups = []
                },
                [HttpMethod.Patch] = new JsonContract.Contract
                {
                    FieldRequirements = [new JsonContract.FieldRequirement {
                        FieldName = "username",
                        FieldType = JsonContract.FieldType.String
                    },
                    new JsonContract.FieldRequirement {
                        FieldName = "password",
                        FieldType = JsonContract.FieldType.String,
                        IsRequired = false
                    }],
                    FieldRequirementGroups = []
                }
            },
            HeaderContracts = new Dictionary<HttpMethod, HeaderContract.Contract?>
            {
                [HttpMethod.Get] = new HeaderContract.Contract
                {
                    HeaderRequirements = [new HeaderContract.HeaderRequirement {
                        HeaderName = "Entity-Username",
                        IsRequired = false
                    }]
                },
                [HttpMethod.Post] = null,
                [HttpMethod.Patch] = null
            },
            PerResponseExceptions = new Dictionary<HttpMethod, Dictionary<HttpStatusCode, Func<FailureResponsePayload, Exception>>?>
            {
                [HttpMethod.Get] = new Dictionary<HttpStatusCode, Func<FailureResponsePayload, Exception>>
                {
                    [HttpStatusCode.Unauthorized]   = rp => new AuthenticationException()   { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.Forbidden]      = rp => new AuthorizationException()    { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.NotFound]       = rp => new NotFoundException()         { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.BadGateway]     = rp => new ServerException()           { ServerExceptionMessage = rp.ServerExceptionMessage }
                },
                [HttpMethod.Post] = new Dictionary<HttpStatusCode, Func<FailureResponsePayload, Exception>>
                {
                    [HttpStatusCode.BadRequest]     = rp => new BadRequestException()       { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.Unauthorized]   = rp => new AuthenticationException()   { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.NotFound]       = rp => new NotFoundException()         { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.BadGateway]     = rp => new ServerException()           { ServerExceptionMessage = rp.ServerExceptionMessage }
                },
                [HttpMethod.Patch] = new Dictionary<HttpStatusCode, Func<FailureResponsePayload, Exception>>
                {
                    [HttpStatusCode.BadRequest]     = rp => new BadRequestException()       { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.Unauthorized]   = rp => new AuthenticationException()   { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.Forbidden]      = rp => new AuthorizationException()    { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.NotFound]       = rp => new NotFoundException()         { ServerExceptionMessage = rp.ServerExceptionMessage },
                    [HttpStatusCode.BadGateway]     = rp => new ServerException()           { ServerExceptionMessage = rp.ServerExceptionMessage }
                }
            }
        };

        public static readonly Route Upload = new Route
        {
            RouteString = "/api/auth/upload",
            SupportedMethods = [HttpMethod.Post],
            JsonContracts = new Dictionary<HttpMethod, JsonContract.Contract?>
            {
                [HttpMethod.Post] = new JsonContract.Contract
                {
                    FieldRequirements = [new JsonContract.FieldRequirement {
                        FieldName = "group_target",
                        FieldType = JsonContract.FieldType.String
                    },
                    new JsonContract.FieldRequirement {
                        FieldName = "layout_target",
                        FieldType = JsonContract.FieldType.String
                    },
                    new JsonContract.FieldRequirement {
                        FieldName = "quantity",
                        FieldType = JsonContract.FieldType.Number
                    }],
                    FieldRequirementGroups = []
                }
            },
            HeaderContracts = new Dictionary<HttpMethod, HeaderContract.Contract?>
            {
                [HttpMethod.Post] = null
            },
            PerResponseExceptions = new Dictionary<HttpMethod, Dictionary<HttpStatusCode, Func<FailureResponsePayload, Exception>>?>
            {
                [HttpMethod.Post] = null
            }
        };
    }
}
