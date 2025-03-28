# C#.NET File Storage API Template

A containerized clean-architecture, cloud-agnostic file storage API template
- Built with
	- .NET 8
	- MediatR
	- PostgreSQL + Entity Framework Core
	- AWS SDK for .NET
	- Azure Blob Storage SDK
	- Docker + Docker Compose

Supports both AWS S3 and Azure Blob Storage.

---
## Features

- Upload files to either AWS S3 or Azure Blob Storage
- Record file metadata in PostgreSQL
	- General and easily expanded for metadata tracking
	- Queued background task to be async to be non-blocking
- Stream-based uploads for large file support
- MediatR for CQRS pattern
- Dockerized environment with Compose support
- Modular infrastructure for easy provider swapping
---
## TODOs
- Presigned URLs from storage provider
- File versioning and expanded metadata
- Auth and credential management
---
## Architecture
```bash
src/ 
├── Api/ # Web API entry point 
│ └── Controllers/ 
├── Application/ # Interfaces & command/query contracts 
├── Domain/ # Entities, Enums, and Abstractions 
├── Infrastructure/ # Blob/S3 storage, DbContext, Repositories 
│ └── Storage/ 
docker/ 
└── docker-compose.yml
```
---
## Endpoints

##### `POST /api/files`
Upload a file with metadata:
- `multipart/form-data`
  - `file`: the file
  - `metadata`: JSON string
    ```json
    {
      "fileName": "example.txt",
      "provider": "aws", // or "azure"
      "uploadedBy": "your-name"
    }
    ```

##### `GET /api/files/{id}`
Download a file by its metadata record ID.

---
## Environment Setup

### 1. Configure `appsettings.json`
**Note:** This is for local development, you should setup AWS Secret Manager or Azure KeyVault for credential management. But this is a template and I'm not that committed to it. 
```json
"Storage": {
  "Aws": {
    "AccessKey": "your-key",
    "SecretKey": "your-secret",
    "Region": "us-east-1",
    "BucketName": "your-bucket"
  },
  "Azure": {
    "Connection": "your-blob-connection-string",
    "Container": "files"
  }
},
"ConnectionStrings": {
  "DefaultConnection": "Host=postgres;Port=5432;Database=recordsdb;Username=postgres;Password=postgres"
}
```
### 2. Run Docker Compose
```bash
docker-compose up --build
```

## Testing

Can test with Swagger UI at `http://localhost:8080/swagger`

Or you can do what I like to do and just use python
```python
import requests, json

metadata = {
    "fileName": "example.txt",
    "provider": "aws",
}

files = {
    'file': ('example.txt', open('example.txt', 'rb'), 'text/plain'),
    'metadata': (None, json.dumps(metadata))
}

r = requests.post("http://localhost:8080/api/files", files=files)
print(r.status_code, r.json())
```