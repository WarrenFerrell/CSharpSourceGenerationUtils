# CSharp.SourceGenerationUtils.
Collection of Helper classes to make Source Generation for csharp easier.

### Prerequisites

Visual Studio version 16.8 and above is required as its first version to support source generators.

```xml
<PropertyGroup>
  <CompilerGeneratedFilesOutputPath>$(MSBuildProjectDirectory)/generated</CompilerGeneratedFilesOutputPath>
  <EmitCompilerGeneratedFiles>true</EmitCompilerGeneratedFiles>
</PropertyGroup>

<Target Name="ExcludeGenerated" BeforeTargets="AssignTargetPaths">
  <ItemGroup>
    <Generated Include="generated/**/*.g.cs" />
    <Compile Remove="@(Generated)" />
  </ItemGroup>
  <Delete Files="@(Generated)" />
</Target>
```

### notes 
Some of the foundational code was based on code from https://github.com/chaowlert/PrimaryConstructor 