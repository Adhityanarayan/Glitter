﻿using Microsoft.Extensions.Logging;

namespace Glitter.Commands.OpenSource.Baseline;

/// <summary>
/// Represents a <see cref="SlashCommand"/> for getting the current version of the bot.
/// </summary>
public sealed class VersionCommand : SlashCommand
{
    /// <summary>
    /// Creates a new <see cref="VersionCommand"/> instance.
    /// </summary>
    /// <param name="logger">The logger for the command to use.</param>
    public VersionCommand(ILogger<VersionCommand> logger) :
        base("version", "Version", "Gets the current version of the bot.", logger)
    { }
    protected override async Task<CommandResponse> Work(CancellationToken cancellationToken)
    {
        var response = new CommandResponse($"🐣 Pre-Release");
        return await Task.FromResult(response);
    }
}
