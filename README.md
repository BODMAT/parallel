For Java in src run (with encoding UTF-8):
javac -encoding UTF-8 com/company/*.java; if ($?) { java "-Dfile.encoding=UTF-8" com.company.Main }

For C# in directory with csproj run:
dotnet run

For Node.js run:
npm install
npm run build
npm start