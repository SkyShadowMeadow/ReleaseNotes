# GitHub Release Notes Generator

## 🌟 Project Overview

Welcome to the **GitHub Release Notes Generator**! This tool transforms the way developers and teams create release notes for their projects. Using ChatGPT, this tool analyzes commit messages between specified tags or between last two pushed tags in a GitHub repository and automatically generates concise, informative release notes. No more manual compilation. Ideal for maintaining up-to-date documentation and enhancing project transparency.
## 🔧 How to Set Up

### Step 1: Obtain Your API Key

To use the GitHub Release Notes Generator, you need an API key for ChatGPT. You can obtain this key by following these steps:

1. Visit [OpenAI's API page](https://openai.com/api/).
2. Sign up or log in to your OpenAI account.
3. Navigate to the API section and generate a new API key.

### Step 2: Configure the API Key

Once you have your API key, you need to add it to the project. This ensures secure and authenticated requests to ChatGPT for analyzing commit messages.

- Open appsettings.Development.json 
- Copy your key into the ApiKey currently empty string

```env
"OpenAI": {
    "ApiKey": "COPY YOUR KEY HERE"
```
![Apikey](https://github.com/SkyShadowMeadow/ReleaseNotes/assets/65484187/e2019be0-cba8-4378-8b5b-d0b3817a4cf9)

## 🚀 Get the result!
### Run, Request and Get the result

- Run the app. Once started, the app will open a Swagger UI where you can input the parameters for your request.
### Request Format

To generate release notes, you'll need to make a request to the tool with the following parameters:

- **GitHub Repository URL**: The URL of the GitHub repository(public repository).
- **(optional)Tag 1 and Tag 2**: The two tags between which commit messages will be analyzed.

![SendReq](https://github.com/SkyShadowMeadow/ReleaseNotes/assets/65484187/10afca96-6a36-4082-a8b8-212841ba99f5)

- Get the result

<img width="1052" alt="image" src="https://github.com/SkyShadowMeadow/ReleaseNotes/assets/65484187/65a70c66-8c9b-4fa3-acf9-8f3945c25d2c">

## 🛠️ Future Improvements

I'm committed to enhancing the GitHub Release Notes Generator with the following improvements:

- **Authentication Support**: Implement authentication mechanisms to access commit messages from private repositories.
- **Extended Test Coverage**: Currently, only the GitService is thoroughly tested. I aim to expand tests to cover all components and functionalities.
- **Support for More Platforms**: Extend the tool's compatibility to include GitLab and BitBucket repositories.
- **Customization Features**: Enable users to specify additional details about their projects for more tailored and precise release notes.
