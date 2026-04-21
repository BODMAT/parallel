## Build & Run Commands

### Java
Run from `src/` with UTF-8 encoding:
```powershell
# Option A — package
javac -encoding UTF-8 com/company/*.java
if ($?) { java "-Dfile.encoding=UTF-8" com.company.Main }

# Option B — flat files
javac -encoding UTF-8 Main.java Philosopher.java Table.java
if ($?) { java "-Dfile.encoding=UTF-8" Main }
```

---

### C#
Run from the directory containing `.csproj`:
```bash
dotnet run
```

---

### Node.js
```bash
npm install
npm run build
npm start
```

---

### C++ (via Docker)
Run from the directory containing `.cpp`:
```powershell
docker run --rm `
  -v "C:\Users\BOHDAN\Desktop\unik\Parallel\lab5\openMpDemoCPP\openMpDemo:/app" `
  -w /app gcc:latest bash -c `
  "g++ -fopenmp -O2 -o lab5 Source.cpp && ./lab5"
```