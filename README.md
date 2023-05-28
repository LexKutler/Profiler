# Profiler
Profiler is 3-layer microservice which allows to save user profile in MongoDB cluster, update it and upload a profile picture. Check [wiki](https://github.com/LexKutler/Profiler/wiki/About) for walkthrough

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

### Postman
Postman collection created for testing Profiler can be add via 
```
https://api.postman.com/collections/24270585-25439c17-2235-40e1-900b-80a58f06a3a5?access_key=PMAT-01H1HXZC525BSCJXEGAAYMB2DJ
``` 
No sensitive information, just ✂️ & 📋 it to your Postman agent. Autofill tests use scripts that let user create, update profile and upload a picture to it with as few clicks as possible if 👇 environment is configured. Don't be lazy, it saves a lot of effort 🙂
>
![image](https://github.com/LexKutler/Profiler/assets/68227124/f1527635-9f65-4499-a1fd-b198838a4e25)


[^1]: content-type: `multipart/form-data`.

> Test with GitHub Actions: [.NET](https://github.com/LexKutler/Profiler/blob/master/.github/workflows/dotnet.yml) & [CodeQL](https://github.com/LexKutler/Profiler/blob/master/.github/workflows/codeql.yml)
