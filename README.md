# Profiler
Profiler is 3-layer microservice which allows to save user profile in MongoDB cluster, update it and upload a profile picture

### Setup
1. Change [connection string](https://github.com/LexKutler/Profiler/blob/538f6daac69e12993f79c7204f1bbb309e9b2af7/ProfilerWebAPI/appsettings.json#LL10C6-L10C22) from `blank` to anything meaningful
```
"ProfilerDB": {
	"ConnectionString": "blank",
	"DatabaseName": "profilerDB"
},
```
2. Build and run

### WebAPI Endpoints
- `/profiles` - POST method to create profile
- `/profiles/{id}` - GET method to find profile by id
- `/profiles/{id}` - PATCH method to update profile, generates update event
- `/profiles/{id}/picture`[^1] - POST method to upload profile picture, generates update event

[^1]: content-type: `multipart/form-data`.

> Test with GitHub Actions: [.NET](https://github.com/LexKutler/Profiler/blob/master/.github/workflows/dotnet.yml) & [CodeQL](https://github.com/LexKutler/Profiler/blob/master/.github/workflows/codeql.yml)
