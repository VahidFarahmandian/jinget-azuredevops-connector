# Jinget Azure DevOps Connector
By using Jinget Azure DevOps Connector, the communication between your software application and Azure DevOps is facilitated and you can easily connect to Azure DevOps through this package.


### How to Use:

Download the package from NuGet using Package Manager:
`Install-Package Jinget.AzureDevOps.Connector`
You can also use other methods supported by NuGet. Check [Here](https://www.nuget.org/packages/Jinget.AzureDevOps.Connector "Here") for more information.

####Consuming Process APIs
**Create an object from `ProcessConnector` class:**
Note that if `url` is not specified then `https://dev.azure.com` will be used by default.
```csharp
var connector = new ProcessConnector(pat, organization, apiVersion: "7.0");
```
Where `pat` is your *Personal Access Token*. `organization` is the name of your organization/collection. `7.0` is the Azure DevOps api version. 

**Get list of processes:**
```csharp
var result = await connector.ListAsync();

```
**Get specific process details:**
```csharp
varr result = await connector.GetAsync(Guid.Parse("<process id>"));

```

------------

####Consuming Project APIs
**Create object from `ProjectConnector` class:**
Note that if `url` is not specified then `https://dev.azure.com` will be used by default.
```csharp
connector = new ProjectConnector(pat, organization, apiVersion: "7.0");
```
**Create new project:**
To create new project, create an object of type `NewProjectModel` and pass it to the `CreateAsync` method:
```csharp
NewProjectModel newProject = new()
{
    name = "<project name>",
    capabilities = new NewProjectModel.Capabilities
    {
        versioncontrol = new NewProjectModel.Versioncontrol
        {
            sourceControlType = "Git"
        },
        processTemplate = new NewProjectModel.Processtemplate
        {
            templateTypeId = "b8a3a935-7e91-48b8-a94c-606d37c3e9f2"
        }
    },
    description = "<description>"
};
await connector.CreateAsync(newProject);
```
**Get List of projects:**
```csharp
await connector.ListAsync();
```
**Get first `n` projects:**
```csharp
Dictionary<string, string> urlParameters = new()
{
            { "$top","n"}
};
ProjectsListViewModel result = await connector.ListAsync(urlParameters);
```
**Get specific project details:**
```csharp
await connector.GetAsync("<project name>")
```
**Get specific projects properties:**
Note that to use this api, you need to pass `7.0-preview.1` as api version
```csharp
await projectConnector.GetPropertiesAsync(Guid.Parse(<project id>))
```
**Delete Specific project:**
```csharp
await connector.DeleteAsync(Guid.Parse(<project id>))
```

------------

####Consuming Team APIs
**Create object from `TeamConnector` class:**
Note that if `apiVersion` is not specified while creating the connector, by default version `7.0-preview.3` will be selected. If `url` is not specified then `https://dev.azure.com` will be used by default.
```csharp
connector = new TeamConnector(pat, organization);
```
**Get list of all teams:**
```csharp
await connector.GetAllTeamsAsync();
```

**Get list of all teams in specific project:**

```csharp
await connector.GetTeamsAsync(Guid.Parse("<project id>"));
```
**Get specific team details inside a specific project:**
```csharp
await connector.GetAsync(Guid.Parse("<project id>"), "<team name>");
```

------------

####Consuming WorkItem APIs
**Create object from `WorkItemConnector` class:**
Note that if `apiVersion` is not specified while creating the connector, by default version `7.0` will be selected. If `url` is not specified then `https://dev.azure.com` will be used by default.
```csharp
connector = new WorkItemConnector(pat, organization, "<project name>");
```
**Get list of work items:**
```csharp
GetWorkItemBatchModel request = new GetWorkItemBatchModel
{
            fields = new string[]
            {
                        "System.Id",
                        "System.Title",
                        "System.WorkItemType",
                        "Microsoft.VSTS.Scheduling.RemainingWork"
            },
            ids = new string[] { "96", "97", "98" }
};
var result = await connector.ListBatchAsync<WorkItemBatchViewModel>(request);
```
In the above code we are going to select only `System.Id`, `System.Title`, `System.WorkItemType` and `Microsoft.VSTS.Scheduling.RemainingWork` fields.

**Get list of work items using Work Item Query Language(WIQL):**
```csharp
string query = "SELECT System.Id, System.Title, System.WorkItemType, Microsoft.VSTS.Scheduling.RemainingWork FROM WorkItems";
await connector.ListWIQLAsync<WorkItemBatchViewModel>(query);
```
**Create new work item:**
```csharp
List<NewWorkItemModel> properties = new List<NewWorkItemModel>()
{
	new NewWorkItemModel()
	{
		path="/fields/System.Title",
		value="<work item title>"
	},
	new NewWorkItemModel()
	{
		path="/fields/System.Description",
		value="<work item description>"
	},
	new NewWorkItemModel()
	{
		path="/fields/System.History",
		value="<work item comment>"
	},
	new NewWorkItemModel()
	{
		path="/fields/System.AssignedTo",
		value="<assign the task to specified user>"
	},
	new NewWorkItemModel()
	{
		path="/fields/System.AreaPath",
		value="<put the work item in given area>"
	}
};
var result = await connector.CreateAsync("$<work item type>", properties);
```

------------

####Working with OData
**Create object from `ODataConnector` class:**
```csharp
new ODataConnector(pat, "https://analytics.dev.azure.com", organization, project: "<project name>")
```
You can pass any url which is suitable for you instead of `https://analytics.dev.azure.com`.

**Get work items from one project:**
```csharp
var queries = new Dictionary<string, string>()
{
	{"$filter","WorkItemType eq 'Task' and State eq 'To Do'"},
	{"$select","WorkItemId,Title,AssignedTo,State" }
};
ODataResultViewModel result = await connector.QueryAsync("WorkItems", queries);
```
In the above code we are going to select work items which are `Task` and also are in `To Do` state. Also we are going to select only `WorkItemId`, `Title`, `AssignedTo` and `State` fields.

**Get work items from one project and deserial it to custom type:**
```csharp
var queries = new Dictionary<string, string>()
{
	{"$filter","WorkItemType eq 'Task' and State eq 'To Do'"},
	{"$select","WorkItemId,Title,AssignedTo,State" }
};
MyCustomType result = await connector.QueryAsync<MyCustomType>("WorkItems", queries);
```

**Get work items from multiple projects:**
```csharp
var otherConnector = new ODataConnector(pat, "https://analytics.dev.azure.com", organization);
var queries = new Dictionary<string, string>()
{
	{"$filter","(Project/ProjectName eq '<project one>' or Project/ProjectName eq '<project two>') and WorkItemType eq 'Task' and State eq 'To Do'"},
	{"$select","WorkItemId,Title,AssignedTo,State" }
};
var result = await otherConnector.QueryAsync("WorkItems", queries);
```
For cross-project queries, In line 1, while creating an object of `ODataConnector` type project name should **not** specified. Also in line number 4, in `$filter` the desired projects are specified.

------------
# How to install
In order to install Jinget Azure DevOps Connector please refer to [nuget.org](https://www.nuget.org/packages/Jinget.AzureDevOps.Connector "nuget.org")

# Further Information
Sample codes are available via Unit Test projects which are provided beside the main source codes.

# Contact Me
👨‍💻 Twitter: https://twitter.com/_jinget

📧 Email: farahmandian2011@gmail.com

📣 Instagram: https://www.instagram.com/vahidfarahmandian
