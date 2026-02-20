# sharp-twitch
A .NET 6 C# client for Twitch APIs — providing structured access to Twitch Auth, Helix, and EventSub services using strongly-typed models and async patterns.

Designed for integrations or services that rely on Twitch’s authenticated APIs. Supports core Twitch functionality with modular packages for API surface areas.

## Features

* Helix API client — access Twitch REST endpoints for users, stream subscriptions, etc.
* Authentication support — OAuth flows and token handling helpers.
* EventSub integration — subscribe to real-time Twitch events.
* Unit test projects included for core components.
* Built for .NET 6 / .NET Standard / modern C#.

## Repository Structure

SharpTwitch.sln
├── SharpTwitch                 # Helix and Auth wrapper
├── SharpTwitch.Auth            # OAuth and token handling
├── SharpTwitch.Helix           # Helix API wrappers
├── SharpTwitch.EventSub        # EventSub subscription support
├── *.Tests                     # Unit tests for each module

## Getting Started

Prerequisites

1. .NET 6 SDK or newer
2. A Twitch Developer account with registered app credentials
    * Client ID
    * Client Secret

### Install

Since the library is not yet published to NuGet, you can:

```
git clone https://github.com/exriba/sharp-twitch.git
cd sharp-twitch
```
Add the desired projects as references in your solution, or convert to NuGet packages for reuse.

Run all tests 
```
dotnet test
```

Run specific tests
```
dotnet test SharpTwitch.Auth.Tests
dotnet test SharpTwitch.Core.Tests
dotnet test SharpTwitch.Helix.Tests
```

## Documentation

Reference Twitch official docs for more information regarding Helix and EventSub APIs.
* https://dev.twitch.tv/docs/api/
* https://dev.twitch.tv/docs/eventsub/