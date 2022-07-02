# Make Libraries Not Apllications

## Background
A lot of the time, developers make everything into an application. This is fine, but the better way is to alway make libraries that an application implements.

## Premise
1. Imagine an application for registering purchases by a customer, it has a few things, (and a lot more):
    - Login for the operator
    - Product selection
    - Payment type selection
    - Price over-rides (Like, maybe a small scratch in the product triggers a 10% discount)
    - A "Save" -button
2. The "backend" for this application is stored in a remote system that is accessed by REST -calls, but a local Sqlite -databasefile exist to handle that the remote server goes offline or network connection fails

## Questions
- How should the project be structured?
- Where should it be librarified?
- Is there something called "too many libraries"?

### How should the project be structured?
A good way to structure a project is to begin with a class library that is assumed to "live" just under the "Application". Application in this context is an executable project that is an "entry-point", which might be a Console, WPF -GUI, or similar.

### Where should it be librarified?
The easy answer is: Everywhere, the more correct answer is that anything that can be group into a piece of logic should be in it's own project. In relation to the premise set above there would be a natural separation of logic accessing data from REST and for local DB, where they have a shared resource for data-models and maybe some shared abstractions and business logic like data-validation.

### Is there something called "too many libraries"?
Yes. If you are splitting into libraries that are called from one place, and there is no way that it'll be needed somewhere else, (except for data-models), should not be put into its own library

# Demo
1. Create a new WPF without XAML -solution
2. Add classlib -projects for accessing data, data-models and similar
3. 