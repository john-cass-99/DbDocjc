# DbDocjc - Database Documenter
C# Project to create HTML documentation for MySql databases

## Table of Contents
* [General info](#general-info)
* [Technologies](#technologies)
* [Setup](#setup)
* [Project Status](#project-status)
* [Possible Enhancements](#possible-enhancements)

## General info
I wanted to find an open source project to create database documentation that was easy to use and generated html output that was clear and uncluttered.
Having failed to find one I decided to write one myself and put it on GitHub where it might be useful to others and might also inspire improvements and enhancements.

## Technologies
* C# on Windows is my favourite way to produce quality projects quickly. This Windows Forms project is built using Visual Studio 2019 and .NET Framework 4.7.2.
* HTML output is flexible and can be styled in many ways using CSS.
* PDF output can be obtained using many different, readily available solutions. I have used the excellent [PDF995](http://www.pdf995.com/) for many years and as well as producing very compact, accurate output it does provide a solution for one intractable problem with html - printed page numbering.

## Setup
A click-once installation is planned when the project has reached a sufficiently advanced stage.

## Project Status
MySql supports comments for tables (but only when creating) and individual fields; there is no database comment facility. DbDocjc prompts for a description which appears on the title page and stores this in an XML file for future reference.
Currently the project will document Tables, Indexes, Foreign Keys, Triggers, Create SQL and will list Stored Procedures and Functions. It produces html output which can be used as it stands or printed to PDF using (recommended) [PDF995](http://www.pdf995.com/). Events are not yet supported.

Current work is on a branch which collects all the database interaction in a single class. This will facilitate supporting other types of database.

## Possible enhancements
* Extend to other types of database e.g. SQL Server.
* Provide themes via CSS to give a variety of output styles.
* Support database events.
