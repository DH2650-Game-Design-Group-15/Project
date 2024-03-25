# Structure
This documents the structure of commits to the project and the overall structure of code.

## Issues and Branches
For each feature to be added or bug to be solved a `issue` should be created. When working on a new issue, a new `branch` should be created and connected to the issue. When the issue is solved, create a pull request (PR) to main. After successful PR delete the branch locally and remotely.

## Commit Structure
Commits should be atomic and have a commit that begins with defining the type of commit e.g. `feat:`.

### Commit Types
* `feat` - a new feature is introduced with the changes
* `fix`- a bug fix has occurred
* `chore` - changes that do not relate to a fix or feature and don't modify src or test files (for example updating dependencies)
* `refactor` - refactored code that neither fixes a bug nor adds a feature
* `docs` - updates to documentation such as a the README or other markdown files
* `style` - changes that do not affect the meaning of the code, likely related to code formatting such as white-space, missing semi-colons, and so on.
* `test` - including new or correcting previous tests
* `perf` - performance improvements
* `ci` - continuous integration related
* `build` - changes that affect the build system or external dependencies
* `revert` - reverts a previous commit

## Code Structure

### Function and Variable Names
