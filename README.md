# probe

Probe processes C# projects or files and overwrites concrete method implementations with placeholders while retaining the original method signatures. All replacements get saved in-place so beware.

## Example

**Before**

```C#
public void AMethod()
{
    var x = 0;
    var y = x + 1;
    var z = y + 1;
}
```

**After**

```C#
public void AMethod()
{
    // removed
    // removed
    throw new NotImplementedException();
}
```

## Usage

Pass a file system path to Probe containing the C# code you want to overwrite (can be both a file and a folder).

```shell
dotnet run -- /path/to/project
dotnet run -- /path/to/project/SpecificFile.cs
```

