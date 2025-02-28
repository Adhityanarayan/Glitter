﻿using Glitter.Commands;

using MediatR;

using Microsoft.Extensions.Logging;

using SystemConsole = System.Console;

namespace Glitter.Ai;

/// <summary>
/// Represents a test service for testing within the deployed console.
/// </summary>
internal sealed class ConsoleChatbot<T> : Chatbot<T> where T : ChatbotOptions
{
    private readonly RequestParser _parser;
    /// <summary>
    /// Creates a new <see cref="ConsoleChatbot"/> instance.
    /// </summary>
    /// <param name="parser">A <see cref="RequestParser"/> for parsing requests.</param>
    /// <param name="mediator">The mediator for handling <see cref="Command"/> requests.</param>
    /// <param name="logger">The logger for the <see cref="Chatbot"/>.</param>
    public ConsoleChatbot(
        T options,
        RequestParser parser,
        IMediator mediator,
        ILogger<ConsoleChatbot<T>> logger) :
        base("Testing Bot", options, mediator, logger) =>
        _parser = parser;
    /// <inheritdoc/>
    protected override async Task Run(CancellationToken cancellationToken)
    {
        // Cancel if requested, otherwise start the service.
        cancellationToken.ThrowIfCancellationRequested();
        Logger.LogInformation("Connected to console.");

        do
        {
            // Cancel if requested, otherwise wait for input.
            cancellationToken.ThrowIfCancellationRequested();
            string? input = SystemConsole.ReadLine();

            // Validate any input received.
            if (string.IsNullOrWhiteSpace(input) ||
                !_parser.TryParse(input, out CommandRequest? commandRequest) ||
                commandRequest is null)
                continue;

            // Capture the command and validate.
            Command? command = await Mediator.Send(commandRequest, cancellationToken);
            if (command is null)
            {
                Logger.LogWarning($"Unable to locate a command that satisfies the given request.");
                continue;
            }

            // Execute the command and handle the response.
            CommandResponse? response = await command.Execute(cancellationToken);
            if (!string.IsNullOrWhiteSpace(response?.Message))
                SystemConsole.WriteLine($"Freya: {response.Message}");
        } while (!cancellationToken.IsCancellationRequested);
    }
}
