# .NET Basic Project

## Game logic parameters
|        |Building time   ||Building cost       ||HP     |Effect                                                         |
|--------|-------|---------|--------|------------|-------|---------------------------------------------------------------|
|        |Level 1|Level n  |Level 1 |Level n     |Level n|Level n                                                        |
|Townhall|2:00   |n * 1:00 |200 gold|n * 200 gold|n * 200|can build level n buildings                                    |
|Farm    |1:00   |n * 1:00 |100 gold|n * 100 gold|n * 100|+(n * 5) + 5 food / minute                                     |
|Mine    |1:00   |n * 1:00 |100 gold|n * 100 gold|n * 100|+(n * 5) + 5 gold / minute                                     |
|Academy |1:30   |n * 1:00 |150 gold|n * 100 gold|n * 150|can build level n troops                                       |
|Troop   |0:30   |n * 0:30 |25 gold |n * 25 gold |n * 20 |-(n * 5) food / minute<br>+(n * 10) attack<br>+(n * 5) defense |


## Development rules:

- For testing use this syntax wherever possible: 
  - [MethodName_StateUnderTest_ExpectedBehavior]
  - like: `checkCredentialValidity_WithValidCredentials_ReturnsTrue()`
  - in case of endpoints, instead of method name, use descriptive name for the endopint
- Use `this` keyword only to avoid variable name conflicts
- After global exception handler is present, create and throw custom exceptions in error scenarios
- Create branch names including very short description of the task
- Create (final) commits including story ID and name
- Use plurals for database table names
- Have at least 80% test coverage regarding services (unit test) and controllers (integration tests)
- Push only when all tests and style checks have passed

## Useful links


Jira board:

- <Link comming soon>


Contribution:

- https://github.com/green-fox-academy/Microtis-Iberis/blob/master/CONTRIBUTING.md

Commit messages:

- https://chris.beams.io/posts/git-commit/

`If applied, this commit will ... [commit message]`

Git cheat sheet

https://docs.google.com/spreadsheets/d/1Y6ylJLSbkUqLzn9kN_rQSgDlssRpR-ZFresF46apFWY/edit?usp=sharing

## Git Workflow

### Day Start

Use `git fetch` in order to retrieve the most recent commits from GitHub.

### Start New Feature/Bugfix

In order to minimize merge conflicts later always open a new feature branch from the most recent state of the `development` branch on GitHub.

- `git fetch`
- `git checkout -b <branch_name> origin/development`

### Update Feature Branch

While you're working on your own feature/bugfix other developers make changes on `development` and it's required to update your branch to keep consistency of the codebase. You can do this in 2 ways.

[`git merge` vs `git rebase`](https://www.atlassian.com/git/tutorials/merging-vs-rebasing)

#### Rebase

[`git rebase`](https://www.atlassian.com/git/tutorials/rewriting-history/git-rebase)

Rebase rewrites commit history; therefore, do not use rebase on the `master` and `development` branches.
On the other hand feel free to use rebase on your own branches.

Use `git rebase development` while on your branch.

#### Merge

[`git merge`](https://www.atlassian.com/git/tutorials/using-branches/git-merge)

This creates a new commit (so called merge commit) containing changes from both your branch and the development branch.

Use `git merge development` while on your branch.

### Commit and Push

You can work on your feature/bugfix separately but sometimes you may need to merge another branch into your branch (i.e. to try out your feature). In order to have clean workflow (and pull requests) always commit only feature related modifications. This is harder to reset files or hunks later.
