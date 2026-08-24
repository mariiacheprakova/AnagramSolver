Sample weather service for e.g. GitHub Copilot

Go to Copilot, set the .vscode/mcp.json file to

{
    "servers": {
        "my-weather-server": {
            "type": "stdio",
            "command": "uv",
            "args": [
                "--directory",
                "/Users/marcus/repos/Visma-Tech-Cloud-and-VCDM/example-mcp-servers/weather_mcp",
                "run",
                "weather.py"
            ]
        }
    }
}

Then use it in agent mode, e.g., "What is the weather in NY?"
