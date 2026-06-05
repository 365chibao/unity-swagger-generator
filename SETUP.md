# Swagger DTO Generator - Development & Integration Guide

This document records the steps taken during development, the results of the verification tests, and the integration guide for installing the tool in Unity.

---

## 1. Development Process & Milestones

### Phase 1: Environment & Requirements Definition
* Created folder structures for Gemini Skills: `.gemini/` and `.gemini/skills/`.
* Wrote skill files defining objectives, validations, and parameters:
  * `GEMINI.md` - Rules and workflow.
  * `create-editor-window.md` - Editor Window specifications.
  * `parse-swagger.md` - Types parsing guidelines.
  * `generate-dto.md` - DTO structure instructions.
  * `validate-project.md` - Compilation checks.

### Phase 2: Implementation of Core C# Modules
1. **`SwaggerParser.cs`**:
   - Parses `components.schemas` from Swagger/OpenAPI JSON using `Newtonsoft.Json`.
   - Supports primitive types mapping (`string`, `int`, `long`, `bool`, `float`, `decimal`) and nested/referenced objects (`$ref`, `array`).
   - Includes a standalone compatibility compilation layer so it can be compiled outside Unity.
2. **`DTOGenerator.cs`**:
   - Implements property conversion to `PascalCase`.
   - Adds `[JsonProperty("original_name")]` tag attributes to properties.
   - Generates and writes one clean `.cs` class file per schema definition.
   - Prevents conflicts by appending `"Value"` if a property name matches its declaring class name.
3. **`SwaggerGeneratorWindow.cs`**:
   - Exposes the GUI via `Tools -> Swagger DTO Generator` in Unity.
   - Supports directory/file selection with path text fields and persistent user state using `EditorPrefs`.
   - Runs generation and automatically forces a compiler refresh using `AssetDatabase.Refresh()`.

### Phase 3: Standalone Verification Test
* Initiated a mock .NET console program (`TempTest`) to isolate the generator logic from the Unity Engine dependencies.
* Imported `Newtonsoft.Json` package and verified that the compiled project successfully parsed the schema types and generated class structures.
* The test compiled with 0 errors, outputted correct C# DTOs, and logged verification outputs (printed in `walkthrough.md`).
* Cleaned up the `TempTest` workspace files.

### Phase 4: UPM Package Restructure
* Transformed the codebase into a official **Unity Package Manager (UPM)** package.
* Added **`package.json`** containing package metadata and `com.unity.nuget.newtonsoft-json` dependency.
* Created **`Editor/SwaggerGenerator.Editor.asmdef`** to define the Editor compilation assembly.
* Moved C# files to the package-compliant root `Editor/` folder and cleaned up obsolete directories.
* Run `npm pack` to compress the package into a distributable offline zip archive: `com.gemini.swagger-dto-generator-1.0.0.tgz`.

---

## 2. Package Installation Instructions

### Method A: Install via Offline Archive (`.tgz`) - Easiest
1. Send the file **`com.gemini.swagger-dto-generator-1.0.0.tgz`** to your target PC.
2. Open your Unity Project.
3. Open the Package Manager via **`Window -> Package Manager`**.
4. Click the **`+`** icon (top-left) and select **`Add package from tarball...`**.
5. Select the `.tgz` file and click **Open**.

### Method B: Install via Local Folder
1. Open Unity Package Manager via **`Window -> Package Manager`**.
2. Click **`+`** and select **`Add package from disk...`**.
3. Select the **`package.json`** located in the root of this workspace.

### Method C: Download & Add via Remote Git URL
1. Push this folder to your Git repository (GitHub/GitLab).
2. Inside Unity Package Manager, click **`+`** -> **`Add package from git URL...`**.
3. Paste your repository link (e.g. `https://github.com/your-username/your-repo-name.git`).

---

## 3. How to Use the Tool

1. **Standard DTO Generation:**
   - Go to top menu: **`Tools -> Swagger DTO Generator`**.
   - Select your API Swagger JSON file path.
   - Select your destination output folder in your assets.
   - Set your preferred namespace (default: `UnitySwaggerGenerator.DTOs`).
   - Click **Generate DTOs**.
