# DealerAgent

DealerAgent is an AI-powered car dealership assistant. You talk to it, and it manages the store for you: your cars, accessories, carts, orders, and complaints — through natural conversation (text or images).

<img width="959" height="437" alt="Screenshot 2026-10-28 205604" src="https://github.com/user-attachments/assets/1788b475-1a09-4b26-91fc-4d2ce073c608" />


## 1. What DealerAgent Does

- Browses the store inventory: cars, their makes, prices, quantities, and model years.
- Manages accessories alongside vehicles.
- Handles the cart and its items.
- Creates, tracks, and updates orders and their line items.
- Registers and follows up on customer complaints.
- Lets you manage everything by simply chatting with it.
- Gives personalized buying advice and semantic product search through specialized sub-agents.

### From Message to Response

1. The user sends a message (text and/or an image) to the chat endpoint.
2. The message is stored as a chat row, and the request is routed to the right agent.
3. The request is queued into an in-memory channel and the API responds `202 Accepted` immediately.
4. A background worker picks up the request, marks the notification as `Running`, and runs the selected agent.
5. The agent reasons with the AI model, calls tools when it needs data, and saves every step in the chat history.
6. When it finishes, the agent creates a notification: `Completed`, `WaitingForUser` (asked the user a question), or `Failed`.
7. The user reads the final answer from the chat history and the notification feed (the client polls the notification endpoint).

## 2. The AI Engine (BaseAgent) and Sub-Agents

`BaseAgent` is the single engine that powers all agents — there is one shared engine, not one per agent. An agent is just a configuration of it: an agent type plus its own system prompt.

At a high level, the engine loop:

- Loads the chat context for the conversation (filtered to that agent's own rows only).
- Sends the context to the AI model (Gemini) with the agent's system prompt.
- Parses the model's JSON decision: answer, ask the user, use a tool, or hand over to another agent.
- Executes a tool if requested, saves the result, and loops again.
- Finishes with `IsFinished`, `AskUser`, or an error (with a retry limit).

`AgentOrchestratorService` coordinates the agents. It starts with the primary agent and can delegate work to a sub-agent using `TargetAgentName`; when the sub-agent finishes or asks the user a question, control returns to the primary agent so the customer always gets one coherent answer.

Current agents:

- **DealerAgent** — the primary, customer-facing agent. Handles the overall conversation: inventory browsing, carts, orders, complaints, and delegating work to the right specialist.
- **AdvisorAgent** — personalized car-buying advice: comparisons, recommendations, and guidance based on the customer's needs.
- **ProductsSearchAgent** — semantic product search over the catalog (cars and accessories) using the vector database.
- **ComplaintAgent** — creates and manages customer complaints, and answers questions about them.

## 3. Tools & Services

Tools are the small operations the AI agents decide to call. Each tool calls a service, and the services do the actual work (database access, external providers, search).

Tool groups:

- **Database tools** — create, read, update, and delete cars, car accessories, carts, cart items, orders, order items, stores, and complaints.
- **Semantic search tool** — `QdrantProductSearchTool` embeds the query with Gemini and searches the Qdrant vector collection for the closest matching products.
- **External tools (e.g. web search)** — planned for the future.

Tool calls and their results are stored in the chat history as `Tool` rows, so the agent can follow its own reasoning on the next turn.

## 4. Configuration

The backend reads its configuration from `Backend/CarDealer/appsettings.Development.json` (kept out of version control next to `appsettings.json`).

Create the file yourself and fill in your own values. It defines:

- `SQL_CONNECTION_STRING` — PostgreSQL connection string.
- `geminiApi` — Gemini API key used by the agents and the embedding service.
- `aiRetryCount` — how many times the AI call is retried before failing.
- `OpenRouter:ApiKey` — key used by the agent routing decision service.
- `Qdrant:Url`, `Qdrant:ApiKey`, `Qdrant:CollectionName` — vector database used by the product search agent.
- Image/blob settings (`blobconnectionstring`, `Image_Host`, `Image_Container`, `Image_Cdn`) — used for product images.

Never commit real keys, connection strings, or secrets — use placeholders in shared files and keep your local file private.

## 5. Running the Backend

From the `Backend/` folder:

Create the database schema with EF Core migrations:

```bash
dotnet ef migrations add <MigrationName> --project CarDealer/CarDealer.csproj --startup-project CarDealer/CarDealer.csproj

dotnet ef database update --project CarDealer/CarDealer.csproj --startup-project CarDealer/CarDealer.csproj
```

Run the API:

```bash
dotnet run --project CarDealer/CarDealer.csproj
```

The API runs on `http://localhost:5199` under the `api/v1/stores` routes.

## 6. Running the Frontend

The frontend is a Vite + React + TypeScript app styled with Tailwind CSS.

Create `Frontend/my-app/.env` (there is a `.env.example` to copy) and set:

```env
VITE_API_BASE_URL=http://localhost:5199/api/v1
VITE_STORE_ID=<your-store-guid>
```

Then:

```bash
cd Frontend/my-app
npm install
npm run dev
```

The app includes:

- Home page with the store's cars and accessories sections.
- Product pages, cart page, and orders page.
- A chat with the DealerAgent: messages are rendered as Markdown, and any cars or accessories returned by the agent are shown as small product cards inside the conversation.
- A notification feed that follows the agent's progress (`Queued`, `Running`, `WaitingForUser`, `Completed`, `Failed`).

## 7. Contributing

Contributions are welcome! Feel free to open a pull request: fork the repo, create a feature branch, commit your changes, and describe your PR clearly.

Author: Amro Aladghem
