# ToolSharp - Extension Methods

This document details the extension methods available in the **ToolSharp** package, designed to simplify coding.

---

## `TaskExtensions`


These extensions for `System.Threading.Tasks.Task` allow composing asynchronous operations in a functional way.

### `Then`

Chains a synchronous or asynchronous operation to be executed after the completion of the previous task.

**Usage:**
To compose complex asynchronous workflows in a readable way, passing the result from one step to the next.

**Example:**
```csharp
using toolsharp.Extensions;

var userEmail = await GetUserIdAsync("test")
                          .Then(id => GetUserFromDbAsync(id))
                          .Then(user => user.Email);
```

---

### `Catch`

Catches an exception thrown by the task and returns a fallback value or executes a fallback task.

**Usage:**
For error recovery, allowing you to continue with a default value or an alternative result in case of failure.

**Example:**
```csharp
using toolsharp.Extensions;

var user = await GetUserAsync(id)
                   .Catch(ex => new User { Id = id, Name = "Guest User" });
```

---

### `Try`

Executes the task safely and encapsulates the result in a `Result<T>` object, which contains the value in case of success or the exception in case of failure.

**Usage:**
For functional-style error handling, avoiding `try-catch` blocks and allowing to inspect the result of the operation explicitly.

**Example:**
```csharp
using toolsharp.Extensions;
using Core.Records;

Result<User> result = await GetUserAsync(id).Try();

if (result.IsSuccess)
{
    Console.WriteLine($"User found: {result.Value.Name}");
}
else
{
    Console.WriteLine($"An error occurred: {result.Error.Message}");
}

```

---

### `Finally`

Executes an action (`Action`) upon completion of the task, regardless of whether it succeeds or fails.

**Use:**
For cleanup operations that must always occur, such as closing connections, freeing resources or recording the end of an operation.

**Example:**
```csharp
using toolsharp.Extensions;

var data = await FetchDataAsync()
                 .Finally(() => Console.WriteLine("Fetch operation completed."));
```

---

### `Tap`

Executes a "secondary" action (side-effect) with the task result without modifying it, returning the original result.

**Use:**
Ideal for logging, debugging or monitoring in the middle of a task pipeline, without interrupting the data flow.

**Example:**
```csharp
using toolsharp.Extensions;

var user = await GetUserAsync(id)
                 .Tap(u => Log.Information($"User {u.Name} was loaded."))
                 .Then(u => ProcessUser(u));
```

---

### `If`

Conditionally transforms the result of a task if a predicate is true.

**Use:**
To apply conditional logic within a fluent pipeline without the need for external `if-else` blocks.

**Example:**
```csharp
using toolsharp.Extensions;

var order = await GetOrderAsync(orderId)
                  .If(o => o.IsPriority, o => ApplyPriorityShipping(o));
```

---

### `ThrowIf`

Throws a custom exception if a condition on the task result is met.

**Use:**
For validating business rules and fail-fast with specific exceptions, making the control flow clearer.

**Example:**
```csharp
using toolsharp.Extensions;

var account = await GetAccountAsync(accountId)
                    .ThrowIf(acc => acc.IsLocked, acc => new AccountLockedException(acc.Id));
```

---

### `Ensure`

Ensures that a condition on the task result is true. Otherwise, throw an `InvalidOperationException`.

**Use:**
For simple assertions and validations within the pipeline, ensuring results are in an expected state before proceeding.

**Example:**
```csharp
using toolsharp.Extensions;

var product = await GetProductAsync(productId)
                    .Ensure(p => p.IsAvailable, "Product is not available.");
```

---

### `Retry`

Performs an asynchronous operation with retry logic on failure. **Note:** This is a static helper method, not an extension method on a `Task`.

**Use:**
To handle transient failures, such as network issues or temporary unavailability of an external service.

**Example:**
```csharp
using toolsharp.Extensions;

var response = await TaskExtensions.Retry(
    operation: () => httpClient.GetAsync("api/data"),
    retries: 3,
    delayBetweenRetries: 1000 // 1 second
);
```
