# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## 0.3.0
### Changed
- Follow phoenix convention for project / solution / folder naming

## 0.2.1
### Added
- Reference to `Evs.Phoenix.Utils.Analyzers`
- packageSourceMapping property added to `NuGet.config` in order to speed restore
- Markdown files in template (from submodule)
- Coverage exclusion for migrations

### Changed
- Bump into dotnet 7
- Dependency updates\
  MSTest.TestAdapter: 2.2.10 -> 3.0.2\
  MSTest.TestFramework: 2.2.10 -> 3.0.2\
  Microsoft.NET.Test.Sdk: 17.4.0 -> 17.5.0\
  Evs.Phoenix.Utils.Analyzers: 1.2.0 -> 1.3.0

## 0.1.3 - 2021-01-26
### Changed
- Constant casing setting changed to PascalCase as voted by brotherhood .NET (was erroneously set to ALL_UPPER_CAMEL_CASE ;))

## 0.1.2 - 2021-01-19
### Changed
- MSTest.TestAdapter 2.2.7 => 2.2.8
- MSTest.TestFramework 2.2.7 => 2.2.8

## 0.1.0 - 2021-01-17
### Added
- First version
