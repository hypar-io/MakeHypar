<img src="https://github.com/hypar-io/sdk/blob/master/hypar_logo.svg" width="300px" style="display: block;margin-left: auto;margin-right: auto;width: 50%;">

# dotnet-starter
A .NET starter project for Hypar. You can see it running [here](https://explore.hypar.io/functions/hypar-dotnet-starter).

The `dotnet-starter` project is a dotnet core 2.0 library project which references the [Hypar SDK](https://github.com/hypar-io/sdk) and uses the [Hypar CLI](https://github.com/hypar-io/sdk/tree/master/src/cli) to bootstrap your project.

## Prerequisites
- Install [.NET](https://www.microsoft.com/net/)

## Getting Started
- Fork this repository.
- Clone your fork of the repository locally.
- Edit the `hypar.json` to describe your function.
- Add your business logic to `/src/Function.cs`.
- `dotnet build`

## Testing
```
cd test
dotnet test
```
test
