using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace TestsProfiler;
/// <summary>
/// This class is used to mock the UpdateResult class from MongoDB.Driver.
/// It is created for 1 propose only: to test the UpdateProfileAsync method from ProfileService class.
/// The main idea is to test if service checks ModifiedCount property of UpdateResult class.
/// </summary>
internal class UpdateResultAdapter : UpdateResult
{
    public override bool IsAcknowledged => throw new NotImplementedException();

    public override bool IsModifiedCountAvailable => throw new NotImplementedException();

    public override long MatchedCount => throw new NotImplementedException();

    public override long ModifiedCount => 1;

    public override BsonValue UpsertedId => throw new NotImplementedException();
}