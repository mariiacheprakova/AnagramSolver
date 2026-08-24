# Week 7, Day 3 — MCP Exercises

## Task 1 — Filesystem MCP server

Claude Code has built-in functions such as Bash, Grep, Read, and Glob. In this case, MCP adds extra functionality that is not really needed and may require additional tokens and MCP schema loading, which makes it less efficient. MCP servers are more useful in cases where extra functionality or external logic is required.

## Task 2 — Visma weather MCP server

The Weather MCP could only provide information for US locations because the MCP server used the US National Weather Service API, while European weather data was not supported.

Tools are described using `@mcp.tool()` and have a name, description, and parameters. Claude mainly relies on the description to determine whether a tool is applicable to the client's query. After changing the description to "does stuff", Claude stopped calling the MCP tool and used the built-in WebSearch instead, because the MCP tool description no longer matched the query well enough.

## Task 3 — Real scenario (Playwright)

Playwright MCP capabilities included opening a browser, opening the AnagramSolver WebApp, entering input, pressing Search, reading the result, and taking a screenshot. In contrast, WebFetch could only retrieve and read the HTML page and could not interact with the application.

## Task 4 — Token research

Connecting MCP servers does not necessarily mean that a large amount of context will be consumed because Claude uses tool search and loads full schemas only when they are needed. After connecting more servers, the number of tokens in the context did not increase significantly. However, after setting "alwaysLoad": true for one server, its schemas were loaded immediately, resulting in approximately 3.2k additional tokens being used.

## Task 5 — Own MCP server

A Semantic Kernel plugin is a function that is directly integrated into a Semantic Kernel agent, while an MCP server is a separate, standardised tool server that can be used by different MCP clients. MCP provides greater independence and reusability, but at the same time requires more sophisticated infrastructure, such as a separate process, transport, configuration, and permissions.
