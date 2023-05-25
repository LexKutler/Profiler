using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.Internal;
using MediatR;
using MongoDB.Bson;
using ProfilerIntegration.Entities;

namespace ProfilerCQRS.Queries;
public class GetProfileQuery: IRequest<UserProfile>
{
    public ObjectId Id { get; set; }
}
