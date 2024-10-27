# GitHubUserActivity
A simple command line interface (CLI) to fetch the recent activity of a GitHub user and display it in the terminal.

## Project Overview
This CLI tool interacts with the [GitHub API](https://api.github.com) to retrieve recent user activities and display them in a user-friendly format in the terminal. Activities include events such as pushing commits, opening issues, starring repositories, and more.

You can find the full project description and roadmap [here on Roadmap.sh](https://roadmap.sh/projects/github-user-activity).

## Features
- Fetches recent activities of any GitHub user.
- Displays each event in a simple and formatted text output in the terminal.
- Provides graceful error handling for invalid usernames or API errors.
- Implements caching to improve performance, reducing redundant API calls for the same user.

## Requirements
- .NET SDK 9.0 or higher
- A GitHub account (to generate a personal access token if required, depending on API usage)

## Installation
1. **Clone the repository**:
   ```bash
   git clone https://github.com/Rabee-Qabaha/GitHubUserActivity.git
   cd GitHubUserActivity