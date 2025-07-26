
<div align="center">

  <img src="../ToolSharp/Core/logo-transparente.png" alt="logo" width="200" height="auto" />
  <h1 style="border: none; padding-bottom: 0; margin-bottom: 0;">ToolSharp</h1>
  <br>

  <p>
    Simplifying code complexity!
  </p>

<p>
  <a href="#"><img src="https://img.shields.io/badge/.NET-9.0-purple.svg?logo=dotnet" alt=".NET 9" /></a>
  <a href="https://www.nuget.org/packages/ToolSharp"><img src="https://img.shields.io/nuget/v/ToolSharp.svg?label=nuget" alt="NuGet" /></a>
  <a href="https://creativecommons.org/licenses/by-nc/4.0/" target="_blank"><img src="https://img.shields.io/badge/license-CC%20BY--NC%204.0-lightgrey.svg?logo=creativecommons" alt="License CC BY-NC 4.0" /></a>
  <a href="https://github.com/KaueSMoraes/ToolSharp/commits"><img src="https://img.shields.io/github/last-commit/KaueSMoraes/ToolSharp.svg" alt="Last Commit" /></a>
  <a href="https://github.com/KaueSMoraes/ToolSharp/stargazers"><img src="https://img.shields.io/github/stars/KaueSMoraes/ToolSharp.svg" alt="Stars" /></a>
  <a href="https://github.com/KaueSMoraes/ToolSharp/issues"><img src="https://img.shields.io/github/issues/KaueSMoraes/ToolSharp.svg" alt="Open Issues" /></a>
</p>
  <h4>
    <a href="https://github.com/KaueSMoraes/ToolSharp/issues/new?template=bug-report.md">Report a Bug</a>
    <span> · </span>
    <a href="https://github.com/KaueSMoraes/ToolSharp/issues/new?template=feature-request.md">Suggest a Feature</a>
  </h4>
</div>

<br />

# Table of Contents

- [About the Project](#about-the-project)
- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [Documentation](#documentation)
- [License](#license)

## About the Project

**ToolSharp** is a lightweight library of extension methods for `Task` in .NET, designed to simplify the use of asynchronous tasks with more safety, clarity, and less boilerplate code.

It allows you to execute tasks in a decoupled (fire-and-forget) way with integrated error handling, enabling cleaner and more robust asynchronous applications.

## Getting Started

To start using ToolSharp in your project, follow these simple steps.

### Prerequisites

* .NET 9.0 or higher.

### Installation

You can install ToolSharp via NuGet Package Manager or .NET CLI.

**.NET CLI:**
```bash
dotnet add package ToolSharp
```

**Package Manager:**
```powershell
Install-Package ToolSharp
```

## Usage

After installation, import the `toolsharp.Extensions` namespace to start using the fluent extension methods.

```csharp
using toolsharp.Extensions;

// Example: Fetch a user, process it, and handle possible errors.
var user = await GetUserFromApiAsync(userId)
    .Tap(u => Console.WriteLine($"User {u.Name} found."))
    .Then(u => ProcessUserDataAsync(u))
    .Catch(ex => 
    {
        Console.WriteLine($"Failed to fetch user: {ex.Message}");
        return new User { Name = "Default User" }; // Fallback value
    });
```

## Documentation

For a complete and detailed list of all available extension methods with usage examples, check out the documentation file:  
[**Click here to access the Documentation**](./Core/Docs/README.NUGET.md)

## License

Distributed under the CC BY-NC 4.0 License. See [**LICENSE**](./LICENSE.txt) for more information.
