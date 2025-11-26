# Copilot Coding Agent Instructions for Transfer.Me

## Project Overview
- **Transfer.Me** is an end-to-end encrypted file transfer web app built with Blazor WebAssembly (Client) and ASP.NET Core (.NET 6) (Server).
- The app enables users to upload, encrypt, and share files securely. Recipients use a link and key to download and decrypt files.
- The project is split into three main folders:
  - `Client/`: Blazor WebAssembly frontend (C#)
  - `Server/`: ASP.NET Core backend (C#)
  - `TransferMeTests/`: xUnit test projects for both client and server logic

## Architecture & Data Flow
- **Encryption**: All files are encrypted client-side using the BouncyCastle cryptography library (`Client/Crypto/AES.cs`).
- **File Transfer**: Upload/download logic is handled via API endpoints in `Server/API/` and consumed by Blazor pages/components in `Client/Pages/` and `Client/Components/`.
- **Database**: Azure Cosmos DB is used for storage, with data access via `Server/Services/CosmosDbService.cs`.
- **Authentication**: User authentication and account management are handled in `Server/API/UserAuth.cs` and `Client/Services/AccountService.cs`.
- **UI**: Uses MudBlazor for UI components and theming (`Client/`), with custom themes in `Client/Themes/Theme.cs`.

## Developer Workflows
- **Build**: Use Visual Studio or `dotnet build Transfer.Me.sln` from the root.
- **Run**: Use Visual Studio F5 or `dotnet run --project Server/Server.csproj` (server) and `dotnet run --project Client/Client.csproj` (client, for SSR/debug; normally served as static files).
- **Test**: Run `dotnet test TransferMeTests/TransferMeTests.csproj`.
- **Debug**: Debug via Visual Studio launch profiles in `Properties/launchSettings.json` (both Client and Server).
- **Deployment**: Azure App Service deployment profiles in `Properties/ServiceDependencies/`.

## Project-Specific Patterns & Conventions
- **End-to-End Encryption**: Never send unencrypted file data to the server. All encryption/decryption is performed in the browser (`Client/Crypto/AES.cs`).
- **API Communication**: Use `Client/Services/HttpService.cs` for all HTTP calls; do not call `HttpClient` directly in components.
- **ViewModels**: Use `Client/ViewModels/` for Blazor page state and logic separation.
- **Component Structure**: UI logic is split into `.razor` (markup) and `.razor.cs` (code-behind) files.
- **File Metadata**: File info and transfer metadata are modeled in `Client/Models/EncFile.cs` and `Client/Models/FileDescriptor.cs`.
- **Testing**: Place all tests in `TransferMeTests/`, using xUnit. Test both encryption logic and UI components.

## External Dependencies
- **BouncyCastle**: For cryptography (see `Client/Crypto/AES.cs`).
- **MudBlazor**: For UI components (see `Client/` usage).
- **Azure Cosmos DB**: For NoSQL storage (see `Server/Services/CosmosDbService.cs`).

## Examples
- To add a new API endpoint: Implement in `Server/API/`, register in `Server/Program.cs`, and consume via `Client/Services/HttpService.cs`.
- To add a new UI feature: Create a `.razor` component in `Client/Components/`, add logic in `.razor.cs`, and update state via a ViewModel in `Client/ViewModels/`.

## References
- See `README.md` for technology choices and rationale.
- See `Client/Crypto/AES.cs` for encryption logic and patterns.
- See `Server/Services/` for backend service patterns.

---

If you are unsure about a workflow or pattern, check the referenced files or ask for clarification before proceeding with major changes.
