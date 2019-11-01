# Ramses
Lifecycle hooks for EF Core.
The project is licensed under the MIT license.

## Getting started
1. Install the package from [NuGet](link)
2. Make your `DbContext` extend from `LifecycleDbContext`
3. Call the `SaveWithLifecycles` or async variant to allow hooking into the lifecycle
4. Make your model(s) implement one of the provided lifecycle interfaces

## Possible lifecycle hooks
| Lifecycle hook        | Interface           |
| ------------- |-------------|
| Before adding the model to the database     | `Com.Setarit.Ramses.LifecycleListener.IBeforeAddingListener` |
| Before deleting the model from the database     | `Com.Setarit.Ramses.LifecycleListener.IBeforeDeletionListener` |
| Before updating the model in the database     | `Com.Setarit.Ramses.LifecycleListener.IBeforeUpdateListener` |
| After adding the model to the database     | `Com.Setarit.Ramses.LifecycleListener.IAfterAddingListener` |
| After deleting the model from the database     | `Com.Setarit.Ramses.LifecycleListener.IAfterDeletionListener` |
| After updating the model in the database     | `Com.Setarit.Ramses.LifecycleListener.IAfterUpdateListener` |

## :point_up: Caveats
### Lifecycle hooks after saving to the database
When implementing one of the `IAfter*` interfaces, beware that the object will be in the same state as before saving to the database.
If you change a property in for example `AfterUpdate`, this change will **not** be present in the database. 
To make sure the update is saved in the database, call `SaveChanges()`.

### Calling `SaveWithLifecycles` within the lifecycle hook
This may result in unexpected changes.
Currently you cannot call `SaveWithLifecycles` or `SaveWithLifecyclesAsync` within the lifecycle hook. If you want to save the changes you made to the model within the lifecycle hook, call `SaveChanges()`.
