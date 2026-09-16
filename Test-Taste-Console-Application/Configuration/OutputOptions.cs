namespace Test_Taste_Console_Application.Configuration;

public sealed class OutputOptions
{
    public const string OutputDirectoryPath = "./Files/";

    public string DirectoryPath { get; init; } = string.Empty;

    /// <summary>
    /// Creates output settings by resolving the configured relative path to an absolute path.
    /// </summary>
    /// <returns>The default CSV output settings.</returns>
    public static OutputOptions CreateDefault()
    {
        return new OutputOptions { DirectoryPath = Path.GetFullPath(OutputDirectoryPath) };
    }
}