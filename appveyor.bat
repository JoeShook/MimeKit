SET NUNITRUNNER=%USERPROFILE%\.nuget\packages\nunit.consolerunner\3.11.1\tools\nunit3-console.exe
SET COVERALLS=%USERPROFILE%\.nuget\packages\coveralls.net\tools
SET OPENCOVER=%USERPROFILE%\.nuget\packages\opencover\4.7.922\tools
SET OPENCOVER32=%OPENCOVER%\x86
SET OPENCOVER64=%OPENCOVER%\x64

regsvr32 %OPENCOVER32%\OpenCover.Profiler.dll
regsvr32 %OPENCOVER64%\OpenCover.Profiler.dll

%OPENCOVER64%\OpenCover.Console.exe -filter:"+[MimeKit]* -[UnitTests]* -[submodules]* -[Mono.Data.Sqlite]*" -target:"%NUNITRUNNER%" -targetargs:"/domain:single UnitTests\bin\Debug\UnitTests.dll" -output:opencover.xml

%COVERALLS%\csmacnz.Coveralls.exe --opencover -i opencover.xml --repoToken %COVERALLS_REPO_TOKEN% --useRelativePaths --basePath .\UnitTests\bin\Debug --commitId %APPVEYOR_REPO_COMMIT% --commitBranch %APPVEYOR_REPO_BRANCH% --commitAuthor %APPVEYOR_REPO_COMMIT_AUTHOR% --commitEmail %APPVEYOR_REPO_COMMIT_AUTHOR_EMAIL% --commitMessage %APPVEYOR_REPO_COMMIT_MESSAGE% --jobId %APPVEYOR_BUILD_NUMBER% --serviceName appveyor